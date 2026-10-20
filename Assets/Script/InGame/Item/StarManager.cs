using UnityEngine;

namespace InGame.Item
{
    public class StarManager : MonoBehaviour
    {
        [SerializeField] private float _endPosY = 5;
        [SerializeField] private float _speed = 5;
        [SerializeField] private float _maxDistance = 50;

        Vector3 _endPos;

        void Start()
        {
            SetMoveDistance(CheckRay());
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, _endPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    _endPos,
                    _speed * Time.deltaTime
                );
            }
        }

        private void SetMoveDistance(float _distance)
        {
            _endPos = transform.position;
            _endPos.y += _distance;
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
