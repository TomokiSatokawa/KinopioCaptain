using Input;
using UnityEngine;

namespace InGame.Player
{

    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider _capsuleCollider;
        [SerializeField] private float _groundDistance;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _lookSpeed = 5f;
        [SerializeField] private Rigidbody _playerRigidbody;

        private bool _isGround;
        public bool IsGround => _isGround;
        void Update()
        {
            var forward = _cameraTransform.forward;
            var right = _cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            var input = InputManager.Move.normalized;
            var direction = input.x * right + input.y * forward;

            MovePlayer(direction);

            if (direction.magnitude > 0f)
            {
                LookPlayer(direction);
            }

            GroundCheck();
        }

        private void GroundCheck()
        {
            Vector3 bottomPosition = _capsuleCollider.transform.TransformPoint(_capsuleCollider.center - Vector3.up * (_capsuleCollider.height * 0.5f));
            bottomPosition += Vector3.up * 0.01f;

            Ray ray = new Ray(bottomPosition, -this.transform.up);

            Debug.DrawRay(ray.origin, ray.direction * _groundDistance);

            _isGround = Physics.Raycast(ray, _groundDistance, _groundLayer);
        }

        private void MovePlayer(Vector3 direction)
        {
            Vector3 move = new Vector3(direction.x, 0, direction.z) * _moveSpeed * Time.deltaTime;
            _playerRigidbody.linearVelocity = move;
        }

        private void LookPlayer(Vector3 direction)
        {
            Quaternion from = transform.rotation;
            Quaternion to = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(from, to, _lookSpeed * Time.deltaTime);
        }
    }
}