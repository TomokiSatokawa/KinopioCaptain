using Input;
using UnityEngine;

namespace InGame.Player
{

    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private float _gravityPower;
        [Header("Ground")]
        [SerializeField] private CapsuleCollider _capsuleCollider;
        [SerializeField] private float _groundDistance;
        [SerializeField] private LayerMask _groundLayer;
        [Header("Move")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _lookSpeed = 5f;
        [Header("Slope")]
        [SerializeField] private float _slopeRayPos;
        [SerializeField] private float _slopeRayDistance;

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

            var moveVector = CalculateMovementVector(direction);
            MovePlayer(moveVector);

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
            Vector3 move = direction * _moveSpeed * Time.deltaTime;
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

        private Vector3 CalculateMovementVector(Vector3 vector)
        {
            Vector3 rayPosition = this.transform.position + this.transform.forward * _slopeRayPos;
            var ray = new Ray(rayPosition, -this.transform.up);

            Debug.DrawRay(ray.origin, ray.direction * _slopeRayDistance,Color.green);

            //前方から下に向けてRayを飛ばす
            if (!Physics.Raycast(ray, out var hit, _slopeRayDistance))
                return vector;//Hitしなければ移動ベクトルを返す

            //坂ベクトルを返す
            Vector3 planeVector = Vector3.ProjectOnPlane(vector, hit.normal);
            Debug.DrawLine(this.transform.position, this.transform.position + planeVector);
            return planeVector;
        }
    }
}