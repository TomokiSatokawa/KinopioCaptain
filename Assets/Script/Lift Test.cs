using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LiftTest : MonoBehaviour, IMovingFloor
{
    public enum MoveAxis
    {
        Vertical,
        Horizontal
    }

    [SerializeField] private MoveAxis axis = MoveAxis.Vertical;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float duration = 3f;
    [SerializeField] private AnimationCurve easing = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool pingPong = false;
    [SerializeField] private float waitTime = 1f;

    private Rigidbody _rigidbody;
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 _lastPos;
    private Coroutine moveRoutine;

    /// <summary>IMovingFloor: リフトの現在の移動力(ワールド座標, m/s)</summary>
    public Vector3 Power { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Start()
    {
        startPos = _rigidbody.position;
        _lastPos = startPos;
        targetPos = startPos + GetDirection() * distance;

        if (playOnStart)
        {
            StartMove();
        }
    }

    private Vector3 GetDirection()
    {
        return axis == MoveAxis.Vertical ? Vector3.up : Vector3.right;
    }

    public void StartMove()
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveRoutine());
    }

    public void StopMove()
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = null;
        Power = Vector3.zero;
    }

    private IEnumerator MoveRoutine()
    {
        do
        {
            yield return MoveTo(targetPos);

            if (!pingPong) break;

            yield return Wait(waitTime);
            yield return MoveTo(startPos);
            yield return Wait(waitTime);
        }
        while (pingPong);

        moveRoutine = null;
    }

    private IEnumerator Wait(float seconds)
    {
        Power = Vector3.zero;
        yield return new WaitForSeconds(seconds);
    }

    private IEnumerator MoveTo(Vector3 to)
    {
        Vector3 from = _lastPos;
        float time = Mathf.Max(0.01f, duration);
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            Move(Vector3.LerpUnclamped(from, to, easing.Evaluate(t)));
            yield return new WaitForFixedUpdate();
        }

        Move(to); // 最終位置のズレ防止
        yield return new WaitForFixedUpdate();
        Power = Vector3.zero;
    }

    /// <summary>Rigidbodyで移動し、1ステップ分の移動量から速度を求める</summary>
    private void Move(Vector3 next)
    {
        Power = (next - _lastPos) / Time.fixedDeltaTime;
        _lastPos = next;
        _rigidbody.MovePosition(next);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = Application.isPlaying ? startPos : transform.position;
        Vector3 dir = axis == MoveAxis.Vertical ? Vector3.up : Vector3.right;
        Vector3 target = origin + dir * distance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, target);
        Gizmos.DrawWireSphere(target, 0.3f);
    }
}

/// <summary>うごく床用インターフェース。Power = 床の現在の移動力(ワールド座標, m/s)</summary>
public interface IMovingFloor
{
    Vector3 Power { get; }
}