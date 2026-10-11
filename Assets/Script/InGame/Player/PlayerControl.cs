using Input;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _lookSpeed = 5f;
    [SerializeField] private Rigidbody _playerRigidbody;

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
