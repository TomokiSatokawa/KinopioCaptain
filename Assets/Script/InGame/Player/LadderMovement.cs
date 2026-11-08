using System;
using InGame.Common;
using InGame.Stage;
using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// プレイヤーのハシゴ昇降移動を担当する。
    /// 乗り始め・降り終わりはTransformの位置を補間し、昇降中は入力に応じて位置を直接更新する。
    /// </summary>
    [System.Serializable]
    public class LadderMovement
    {
        private enum State
        {
            None,
            Enter,
            Climb,
            Exit,
        }

        [SerializeField] private ObjectDetector _ladderDetector;
        [SerializeField] private float _moveAmount;
        [Tooltip("ハシゴの端に取り付くまでの時間(秒)")]
        [SerializeField] private float _enterDuration = 0.2f;
        [Tooltip("上り切り・下り切りから降り先に着くまでの合計時間(秒)。前半が垂直、後半が水平移動")]
        [SerializeField] private float _exitDuration = 0.4f;

        private Vector2 _upInputDirection;
        private float _currentHeight;
        private Transform _player;
        private LadderData _ladderData;

        private State _state;
        private Vector3 _moveStart;
        private Vector3 _moveMidPoint;
        private Vector3 _moveTarget;
        private float _moveElapsed;
        private Action _finishAction;

        public bool IsTransit => _state != State.None;

        public void Initialize(Transform player,Action onFinish)
        {
            _player = player;

            _ladderDetector.OnContactEnter += LadderFilter;

            _finishAction = onFinish;
        }

        public bool FindLadder()
        {
            return _ladderData != null;
        }

        public void OnStart()
        {
            if (_ladderData == null)
            {
                Debug.LogError("[LadderMovement] ハシゴが検出されていない状態でOnStartが呼ばれました");
                return;
            }

            float topSqr = (_player.position - _ladderData.TopLadderPos).sqrMagnitude;
            float bottomSqr = (_player.position - _ladderData.BottomLadderPos).sqrMagnitude;

            bool isTop = bottomSqr > topSqr;
            _currentHeight = isTop ? 1f : 0f;

            Vector2 move = InputService.Player.Move.Value;
            if (move.sqrMagnitude > 0f)
            {
                _upInputDirection = isTop ? -move : move;
                _upInputDirection.Normalize();
            }
            else
            {
                _upInputDirection = Vector2.up;
            }

            _moveStart = _player.position;
            _moveTarget = _ladderData.GetPosition(_currentHeight);
            _moveElapsed = 0f;
            _state = State.Enter;
        }

        public void OnUpdate()
        {
            switch (_state)
            {
                case State.Enter:
                    UpdateEnter();
                    break;
                case State.Climb:
                    UpdateClimb();
                    break;
                case State.Exit:
                    UpdateExit();
                    break;
                case State.None:
                default:
                    break;
            }
        }

        public Vector3 GetLookDirection()
        {
            switch (_state)
            {
                case State.Enter:
                case State.Climb:
                    return _ladderData.transform.up;
                case State.Exit:
                    return _ladderData.transform.up *( _currentHeight >= 1 ? 0 : -1);
                case State.None:
                default:
                    return Vector3.zero;
            }
        }

        private void UpdateEnter()
        {
            _moveElapsed += Time.deltaTime;

            float t = _enterDuration > 0f ? Mathf.Clamp01(_moveElapsed / _enterDuration) : 1f;
            _player.position = Vector3.Lerp(_moveStart, _moveTarget, t);

            if (t >= 1f)
                _state = State.Climb;
        }

        private void UpdateClimb()
        {
            Vector2 move = InputService.Player.Move.Value;

            if (_upInputDirection != Vector2.up && move.sqrMagnitude == 0f)
                _upInputDirection = Vector2.up;

            float inputAmount = Vector2.Dot(move, _upInputDirection);
            _currentHeight += inputAmount * _moveAmount * Time.deltaTime;

            if (_currentHeight > 1f || _currentHeight < 0f)
            {
                BeginExit(_currentHeight > 1f);
                return;
            }

            _player.position = _ladderData.GetPosition(_currentHeight);
        }

        private void BeginExit(bool isTop)
        {
            _currentHeight = Mathf.Clamp01(_currentHeight);
            _player.position = _ladderData.GetPosition(_currentHeight);

            _moveStart = _player.position;
            _moveTarget = isTop ? _ladderData.TopPlayerPos : _ladderData.BottomPlayerPos;
            _moveMidPoint = new Vector3(_moveStart.x, _moveTarget.y, _moveStart.z);

            _moveElapsed = 0f;
            _state = State.Exit;
        }

        private void UpdateExit()
        {
            _moveElapsed += Time.deltaTime;

            float t = _exitDuration > 0f ? Mathf.Clamp01(_moveElapsed / _exitDuration) : 1f;

            // 前半(0~0.5)は垂直移動、後半(0.5~1)は水平移動
            _player.position = t < 0.5f
                ? Vector3.Lerp(_moveStart, _moveMidPoint, t * 2f)
                : Vector3.Lerp(_moveMidPoint, _moveTarget, (t - 0.5f) * 2f);

            if (t >= 1f)
                FinishExit();
        }

        private void FinishExit()
        {
            _player.position = _moveTarget;

            _ladderData = null;
            _state = State.None;

            _finishAction?.Invoke();
        }

        private void LadderFilter(GameObject target)
        {
            //はしご移動中は無視
            if (IsTransit)
                return;

            if (!target.CompareTag("Ladder")) return;

            if (target.TryGetComponent(out LadderData ladderData))
                _ladderData = ladderData;
        }
    }
}