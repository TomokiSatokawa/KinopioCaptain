using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace InGame.Stage
{

    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour, IMovingFloor
    {
        public enum MoveAxis
        {
            Up,
            Right,
            forward,
        }

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MoveAxis _axis = MoveAxis.Up;
        [SerializeField] private float _distance = 5f;
        [SerializeField] private float _duration = 3f;
        [SerializeField] private AnimationCurve _easing = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private bool _playOnStart = true;
        [SerializeField] private bool _pingPong = false;
        [SerializeField] private float _waitTime = 1f;
        [SerializeField] private UnityEvent _onReachedTarget;   // 上がりきったとき(目的地到着時)に呼ばれる

        private Vector3 _startPos;
        private Vector3 _targetPos;
        private Vector3 _lastPos;
        private Coroutine _moveRoutine;

        /// <summary>IMovingFloor: リフトの現在の移動力(ワールド座標, m/s)</summary>
        public Vector3 Power { get; private set; }

        private void Awake()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        private void Start()
        {
            _startPos = _rigidbody.position;
            _lastPos = _startPos;
            _targetPos = _startPos + GetDirection() * _distance;

            if (_playOnStart)
            {
                StartMove();
            }
        }

        private Vector3 GetDirection()
        {
            return _axis switch
            {
                MoveAxis.Up => this.transform.up,
                MoveAxis.Right => this.transform.right,
                MoveAxis.forward => this.transform.forward,
                _ => Vector3.zero
            };
        }

        public void StartMove()
        {
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);
            _moveRoutine = StartCoroutine(MoveRoutine());
        }

        public void StopMove()
        {
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);
            _moveRoutine = null;
            Power = Vector3.zero;
        }

        private IEnumerator MoveRoutine()
        {
            do
            {
                yield return MoveTo(_targetPos);
                _onReachedTarget?.Invoke();   // ← 追加

                if (!_pingPong) break;

                yield return Wait(_waitTime);
                yield return MoveTo(_startPos);
                yield return Wait(_waitTime);
            }
            while (_pingPong);

            _moveRoutine = null;
        }

        private IEnumerator Wait(float seconds)
        {
            Power = Vector3.zero;
            yield return new WaitForSeconds(seconds);
        }

        private IEnumerator MoveTo(Vector3 to)
        {
            Vector3 from = _lastPos;
            float time = Mathf.Max(0.01f, _duration);
            float elapsed = 0f;

            while (elapsed < time)
            {
                elapsed += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(elapsed / time);
                UpdatePlatformPosition(Vector3.LerpUnclamped(from, to, _easing.Evaluate(t)));
                yield return new WaitForFixedUpdate();
            }

            UpdatePlatformPosition(to); // 最終位置のズレ防止
            yield return new WaitForFixedUpdate();
            Power = Vector3.zero;
        }

        /// <summary>Rigidbodyで移動し、1ステップ分の移動量から速度を求める</summary>
        private void UpdatePlatformPosition(Vector3 next)
        {
            Power = (next - _lastPos) / Time.fixedDeltaTime;
            _lastPos = next;
            _rigidbody.MovePosition(next);
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 origin = Application.isPlaying ? _startPos : transform.position;
            Vector3 dir = GetDirection();
            Vector3 target = origin + dir * _distance;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, target);
            Gizmos.DrawWireSphere(target, 0.3f);
        }
    }
}