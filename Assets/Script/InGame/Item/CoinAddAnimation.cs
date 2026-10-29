using DG.Tweening;
using UnityEngine;

public class CoinAddAnimation : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _upAmount;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _upEase;
    [SerializeField] private Ease _downEase;
    [SerializeField] private Ease _rotateEase;
    [SerializeField] private bool _isCompleteDestroy;

    private Sequence _sequence;
    private Vector3 _startPosition;

    public void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        _sequence?.Kill();

        _startPosition = this.transform.position;

        _sequence = DOTween.Sequence();

        //“§–¾‚É‚·‚é
        var material = _renderer.material;
        SetAlpha(material, 0);

        //ƒJƒƒ‰‚ÉŒü‚¯‚é
        var cameraPos = Camera.main.transform.position;
        cameraPos.y= _startPosition.y;
        var rotation = Quaternion.LookRotation(cameraPos - transform.position);
        transform.rotation = rotation;

        _sequence.Append(material.DOFade(1, _duration).SetEase(_upEase));
        _sequence.Join(transform.DOMoveY(_startPosition.y + _upAmount, _duration / 2).SetEase(_upEase));
        _sequence.Join(transform.DORotate(Vector3.up * 360, _duration, RotateMode.FastBeyond360).SetRelative().SetEase(_rotateEase));
        _sequence.Join(transform.DOMoveY(_startPosition.y, _duration / 2).SetEase(_downEase).SetDelay(_duration / 2));
        _sequence.AppendCallback(() => { if (_isCompleteDestroy) Destroy(this.gameObject); });
    }

    private void SetAlpha(Material material, float alpha)
    {
        var color = material.color;
        color.a = alpha;
        material.color = color;
    }
}
