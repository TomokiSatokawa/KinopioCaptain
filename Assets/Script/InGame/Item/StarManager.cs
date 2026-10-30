using DG.Tweening;
using UnityEngine;

namespace InGame.Item
{
    public class StarManager : MonoBehaviour , ICollectible
    {
        [SerializeField] private int _rotNum = 3;
        [SerializeField] private float _endPosY = 5;
        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _maxDistance = 50;

        private float _angle = 0;
        public string Name => "•óÎ";

        void Start()
        {
            _angle = 360 * _rotNum;
        }

        void Update()
        {

        }

        public void AnimateGem()
        {
            float moveDistance = CheckRay();

            DOTween.Sequence()
                    .Join(this.transform.DORotate(new Vector3(0, _angle, 0), _moveSpeed, RotateMode.FastBeyond360).SetEase(Ease.InOutSine));

            DOTween.Sequence()
                    .Append(this.transform.DOMoveY(moveDistance, _moveSpeed / 2)
                    .SetEase(Ease.InOutSine)
                    .SetRelative())
                    .Append(this.transform.DOMoveY(-moveDistance, _moveSpeed / 2)
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
             
            return _endPosY;
        }

        public void OnPlayerTouch()
        {
            AnimateGem();
        }
    }
}
