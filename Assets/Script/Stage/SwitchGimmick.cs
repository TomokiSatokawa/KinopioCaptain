using DG.Tweening;
using InGame.Gimmick;
using InGame.Player;
using InGame.Stage;
using UnityEngine;
using UnityEngine.Events;

public class SwitchGimmick : MonoBehaviour, IInteractable
{
    [Header("引っこ抜き(上昇アニメーション完了後に実行)")]
    [SerializeField] private GameObject _switchEraseObject;   // 引っこ抜いたら消すオブジェクト
    [SerializeField] private Renderer _switchBaseRenderer;
    [SerializeField] private Material _switchOnMaterial;      // 使用済み(全Elementに適用)

    [Header("上昇アニメーション (DOTween)")]
    [SerializeField] private Transform _raiseTarget;          // 上げるオブジェクト
    [SerializeField] private float _raiseHeight = 0.5f;
    [SerializeField] private float _raiseDuration = 0.5f;
    [SerializeField] private Ease _raiseEase = Ease.OutQuad;

    [Header("連動")]
    [SerializeField] private MovingPlatform _movingPlatform;  // 引っこ抜き後に動かすリフト
    [SerializeField] private UnityEvent _onSwitchOn;
    [SerializeField] private CameraControl _cameraControl;  // カメラシェイク用(未設定でも動作します)

    private bool _isUsed;
    private Sequence _sequence;

    private void Awake()
    {
        if (_switchEraseObject == null || _switchBaseRenderer == null ||
            _switchOnMaterial == null || _raiseTarget == null || _movingPlatform == null)
        {
            Debug.LogError("[SwitchGimmick] 必須のInspector参照が設定されていません。", this);
            enabled = false;
        }
    }

    private void Start()
    {
        // 使用可能な状態
        _isUsed = false;
        _switchEraseObject.SetActive(true);
    }

    public bool CanInteract()
    {
        return enabled && !_isUsed;
    }

    public void Interact()
    {
        // すでに使用済みの場合は処理しない
        if (!CanInteract())
            return;

        _isUsed = true;

        // 引っこ抜き(上昇アニメーション)と同時にカメラを揺らす
        if (_cameraControl != null) _cameraControl.Shake();

        _sequence?.Kill();
        _sequence = DOTween.Sequence()
                    .Append(_raiseTarget
                .DOLocalMoveY(_raiseTarget.localPosition.y + _raiseHeight, _raiseDuration)
                .SetEase(_raiseEase))
            .OnComplete(() =>
            {
                SetUsedState();
                _movingPlatform.StartMove();
                _onSwitchOn?.Invoke();
            });
    }

    private void SetUsedState()
    {
        // すべてのElementを使用済みマテリアルに差し替える
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