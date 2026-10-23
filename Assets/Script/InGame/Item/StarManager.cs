using DG.Tweening;
using UnityEngine;

namespace InGame.Item
{
    public class StarManager : MonoBehaviour
    {
        [SerializeField] private int _rotNum = 3;
        [SerializeField] private float _endPosY = 5;
        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _maxDistance = 50;

        private float _angle = 0;

        void Start()
        {
            _angle = 360 * _rotNum;
            AnimateGem();
        }

        void Update()
        {

        }

        public void AnimateGem()
        {
            DOTween.Sequence()
                    .Join(this.transform.DOMoveY(CheckRay(), _moveSpeed))
                    .SetRelative()
                    .Join(this.transform.DORotate(new Vector3(0, _angle, 0), _moveSpeed, RotateMode.FastBeyond360))
                    .SetRelative()
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
    }
}
