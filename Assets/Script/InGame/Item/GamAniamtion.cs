using DG.Tweening;
using UnityEngine;

namespace InGame.Item
{
    public class GamAnimation : MonoBehaviour, ICollectible
    {
        [SerializeField] private float _rotateCount = 1.5f;
        [SerializeField] private float _maxPosY = 1;
        [SerializeField] private float _duration = 1;
        [SerializeField] private float _maxDistance = 50;
        public string Name => "•óÎ";

        private Sequence _rotateSequence;
        private Sequence _positionSequence;
        private bool _isCollected = false;

        public void OnPlayerTouch()
        {
            if (_isCollected) return;

            _isCollected = true;

            AnimateGem();
            GameManager.Instance.Status.AddJewelry();
        }

        private void AnimateGem()
        {
            if (_rotateSequence?.IsPlaying() == true)
                return;

            if (_positionSequence?.IsPlaying() == true)
                return;


            float moveDistance = CheckRay();

            _rotateSequence = DOTween.Sequence()
                      .Append(this.transform.DORotate(new Vector3(0, _rotateCount * 360f, 0), _duration, RotateMode.FastBeyond360).SetEase(Ease.InOutSine));

            _positionSequence = DOTween.Sequence()
                         .Append(this.transform.DOMoveY(moveDistance, _duration / 2)
                         .SetEase(Ease.InOutSine)
                         .SetRelative())
                         .Append(this.transform.DOMoveY(-moveDistance, _duration / 2)
                         .SetEase(Ease.InOutSine)
                         .SetRelative())
                         .OnComplete(() => { Destroy(this.gameObject); });
        }

        private float CheckRay()
        {
            if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, _maxDistance))
            {
                return hit.distance / 2;
            }

            return _maxPosY;
        }
    }
}
