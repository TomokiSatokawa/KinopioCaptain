using InGame;
using InGame.Gimmick;
using UnityEngine;

public class CoinPlant : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _leavesObject;
    [SerializeField] private Renderer _plantRenderer;
    [SerializeField] private Material _availableMaterial;
    [SerializeField] private Material _usedMaterial;
    [SerializeField] private int _initialCoinCount;

    private int _remainingCoinCount;

    private void Start()
    {
        //1以上の数値にする
        _initialCoinCount = Mathf.Clamp(_initialCoinCount, 1, int.MaxValue);
        // 初期コイン数を設定する
        _remainingCoinCount = _initialCoinCount;

        // 使用可能な状態
        _plantRenderer.material = _availableMaterial;
        _leavesObject.SetActive(true);
    }

    public bool CanInteract()
    {
        return _remainingCoinCount > 0;
    }

    public void Interact()
    {
        // コインが残っていない場合は処理しない
        if (!CanInteract())
            return;

        // コインを獲得して残数を減らす
        GameManager.Instance.Status.AddCoin();
        _remainingCoinCount --;

        // コインがなくなったら使用済みの状態にする
        if (_remainingCoinCount <= 0)
            SetUsedState();
    }

    private void SetUsedState()
    {
        _plantRenderer.material = _usedMaterial;
        _leavesObject.SetActive(false);
    }
}
