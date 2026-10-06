using UnityEngine;
using Input;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _lookSpeed = 5f;

    private Vector2 _direction;
    private Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookDirection = InputManager.Look;
        float lookMagnitude = lookDirection.magnitude;
        if(Mathf.Approximately(lookMagnitude, 0f) == false)
        {
            LookPlayer(new Vector3(lookDirection.x, 0, lookDirection.y));
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        _direction = InputManager.Move.normalized;

        Vector3 move = new Vector3(_direction.x, 0, _direction.y) * _moveSpeed * Time.deltaTime;
        move = move.x * transform.right + move.z * transform.forward;

        _rb.linearVelocity = move;
    }

    private void LookPlayer(Vector3 direction)
    {
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(from, to, _lookSpeed * Time.deltaTime);
    }
}
