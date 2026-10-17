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
        [SerializeField] private float _gravityPower;
        [SerializeField] private Rigidbody _playerRigidbody;
        private Vector3 _velocity;
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

        }

        private void FixedUpdate()
        {
            GroundCheck();
            AddGravity();
            ApplyVelocity();
        }

        private void GroundCheck()
        {
            Vector3 bottomPosition = _capsuleCollider.transform.TransformPoint(
                _capsuleCollider.center -
                Vector3.up * (_capsuleCollider.height * 0.5f));

            bottomPosition += Vector3.up * 0.01f;

            float radius = _capsuleCollider.radius * 0.9f;

            Vector3 point1 = bottomPosition + transform.up * radius;
            Vector3 point2 = point1 + transform.up * 0.01f;

            _isGround = Physics.CapsuleCast(
                point1,
                point2,
                radius,
                -transform.up,
                _groundDistance,
                _groundLayer
            );
        }

        private void MovePlayer(Vector3 direction)
        {
            Vector3 move = new Vector3(direction.x, 0, direction.z) * _moveSpeed * Time.deltaTime;
            _velocity = move;
        }

        private void AddGravity()
        {
            if (!_isGround)
            {
                _velocity = -Vector3.up * _gravityPower;
            }
        }
        
        private void ApplyVelocity()
        {
            _playerRigidbody.linearVelocity = _velocity;
        }

        private void LookPlayer(Vector3 direction)
        {
            Quaternion from = transform.rotation;
            Quaternion to = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(from, to, _lookSpeed * Time.deltaTime);
        }
    }
}