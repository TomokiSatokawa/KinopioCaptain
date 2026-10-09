using UnityEngine;
using Input;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _lookSpeed = 5f;

    private Vector2 _direction;
    private Vector3 _forward;
    private Vector3 _right;
    private Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector2 lookDirection = InputManager.Move.normalized;
        float lookMagnitude = lookDirection.magnitude;
        if(Mathf.Approximately(lookMagnitude, 0f) == false)
        {
            LookPlayer(new Vector3(lookDirection.x, 0, lookDirection.y));
        }*/

        _forward = _cameraTransform.forward;
        _right = _cameraTransform.right;

        _forward.y = 0f;
        _right.y = 0f;

        _direction = InputManager.Move.normalized;
        _direction = _direction.x * _right + _direction.y * _forward;

        MovePlayer(_direction);
        LookPlayer(_direction);
    }

    private void MovePlayer(Vector3 direction)
    {
        /*var forward = _cameraTransform.forward;
        var right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;*/

        //_direction = InputManager.Move.normalized;
        Vector3 move = new Vector3(direction.x, 0, direction.y) * _moveSpeed * Time.deltaTime;
        //move = move.x * right + move.z * forward;

        _rb.linearVelocity = move;
    }

    private void LookPlayer(Vector3 direction)
    {
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(from, to, _lookSpeed * Time.deltaTime);
    }
}
