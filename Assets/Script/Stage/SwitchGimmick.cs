using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace InGame.Gimmick
{
    public class SwitchGimmick : MonoBehaviour, IInteractable, SwitchInterface.IInteractPromptProvider
    {
        [Header("引っこ抜き(上昇アニメーション完了後に実行)")]
        [SerializeField] private GameObject _switchEraseObject;
        [SerializeField] private Renderer _switchBaseRenderer;
        [SerializeField] private Material _switchOnMaterial;

        [Header("上昇アニメーション (DOTween)")]
        [SerializeField] private Transform _raiseTarget;
        [SerializeField] private float _raiseHeight = 0.5f;
        [SerializeField] private float _raiseDuration = 0.5f;
        [SerializeField] private Ease _raiseEase = Ease.OutQuad;

        [Header("インタラクト案内の画像")]
        [SerializeField] private Sprite _promptSprite;
        [SerializeField] private Transform _promptAnchor;

        [Header("イベント")]
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private UnityEvent _onSwitchOn;

        private bool _isUsed;
        private Sequence _sequence;

        public Sprite PromptSprite => _promptAnchor != null ? _promptSprite : null;
        public Vector3 PromptWorldPosition =>
            _promptAnchor != null ? _promptAnchor.position : transform.position;

        private void Awake()
        {
            if (_switchEraseObject == null || _switchBaseRenderer == null ||
                _switchOnMaterial == null || _raiseTarget == null )
            {
                Debug.LogError($"[SwitchGimmick] 必須のInspector参照が設定されていません。 {this}");
                enabled = false;
            }
            if (_promptAnchor == null)
            {
                Debug.LogWarning($"[SwitchGimmick] Prompt Anchor が未設定です。吹き出しは出ません。 {name}", this);
            }
        }

        private void Start()
        {
            _isUsed = false;
            _switchEraseObject.SetActive(true);
        }

        public bool CanInteract()
        {
            return enabled && !_isUsed;
        }

        public void Interact()
        {
            if (!CanInteract())
                return;

            _isUsed = true;

            _onInteract?.Invoke();

              _sequence?.Kill();
            _sequence = DOTween.Sequence()
                .Append(_raiseTarget.DOLocalMoveY(_raiseTarget.localPosition.y + _raiseHeight, _raiseDuration)
                .SetEase(_raiseEase))
                .OnComplete(() =>
                {
                    SetUsedState();
                    _onSwitchOn?.Invoke();
                });
        }

        private void SetUsedState()
        {
            Material[] mats = _switchBaseRenderer.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = _switchOnMaterial;
            }
            _switchBaseRenderer.materials = mats;

            _switchEraseObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}