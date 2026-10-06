using UnityEngine;
using Input;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

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
        MovePlayer();
    }

    private void MovePlayer()
    {
        _direction = InputManager.Move.normalized;

        Vector3 move = new Vector3(_direction.x, 0, _direction.y) * _moveSpeed * Time.deltaTime;

        _rb.linearVelocity = move;
    }
}
