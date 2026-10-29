using System;
using System.Threading;
using Input;
using R3;
using UnityEngine;

namespace InGame.Gimmick
{
    public class TurnipController : MonoBehaviour
    {
        [SerializeField] private GameObject _destructionEffect;
        [SerializeField] private GameObject _coinAddEffect;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _duration;
        [SerializeField] private Vector3 _controlOffset;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _distance;
        [SerializeField] private float _deleteDuration;

        private bool _isThrew = false;
        private CancellationTokenSource _token;
        private Action _throwAction;
        private float _timer;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _controlPosition;

        //“Š‚°‚é‚Æ‚±‚ë
        private void Start()
        {
            _token = new();
            InputManager.Interact.Subscribe(_ => Throw()).RegisterTo(_token.Token);
        }

        public void Initialize(Action throwAction)
        {
            _throwAction = throwAction;
        }

        private void Update()
        {
            if (!_isThrew) return;

            _timer += Time.deltaTime;
            float t = _timer / _duration;

            if (_timer >= _deleteDuration)
            {
                BreakTurnip();
            }

            //‹Èü‚ğ•`‚¢‚Ä—‚¿‚é
            if (t <= 1f)
            {
                transform.position = CalRouteCurve(_startPosition, _endPosition, _controlPosition, t);
            }
            //‹ÈüŠO‚É‚¢‚Á‚½‚ç‚»‚Ì—¬‚ê‚Å“®‚­
            else
            {
                Vector3 velocity = 2f * (_endPosition - _controlPosition) / _duration;
                float excessTime = _timer - _duration;

                transform.position = _endPosition + velocity * excessTime;
            }

            //‰ñ“]‚·‚é
            transform.rotation *= Quaternion.Euler(_rotationSpeed * Time.deltaTime, 0f, 0f);
        }

        private void Throw()
        {
            _token?.Cancel();
            _token?.Dispose();
            _token = null;

            _isThrew = true;
            _throwAction.Invoke();
            _timer = 0;

            _startPosition = this.transform.position;
            _endPosition = this.transform.position + this.transform.forward * _distance;
            _controlPosition = _startPosition + transform.TransformVector(_controlOffset);
        }

        private Vector3 CalRouteCurve(Vector3 start, Vector3 end, Vector3 control, float t)
        {
            Vector3 v1 = Vector3.Lerp(start, control, t);
            Vector3 v2 = Vector3.Lerp(control, end, t);
            Vector3 v3 = Vector3.Lerp(v1, v2, t);

            return v3;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isThrew) return;

            if ((_groundLayer & (1 << other.gameObject.layer)) != 0)
            {
                GameManager.Instance.Status.AddCoin();
                BreakTurnip();
            }
        }

        private void BreakTurnip()
        {
            Destroy(this.gameObject);
            Instantiate(_destructionEffect, this.transform.position, Quaternion.identity);
            Instantiate(_coinAddEffect, this.transform.position + Vector3.up, Quaternion.identity);
        }

        private void OnDestroy()
        {
            if (_token != null)
            {
                _token?.Cancel();
                _token?.Dispose();
            }
        }
    }
}
