using InGame;
using InGame.Gimmick;
using UnityEngine;

public class CoinPlant : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _leafObject;
    [SerializeField] private Renderer _baseRenderer;
    [SerializeField] private Material _availableMaterial;
    [SerializeField] private Material _usedMaterial;
    [SerializeField] private int _maxCoinCount;

    private int _currentCoinCount;

    private void Start()
    {
        _baseRenderer.material = _availableMaterial;

        //1ˆÈã‚Ì”’l‚É‚·‚é
        _maxCoinCount = Mathf.Clamp(_maxCoinCount, 1, int.MaxValue);

        _currentCoinCount = _maxCoinCount;
        _leafObject.SetActive(true);
    }

    public bool CanInteract()
    {
        return _currentCoinCount > 0;
    }

    public void Interact()
    {
        GameManager.Instance.Status.AddCoin();
        _currentCoinCount --;

        if (_currentCoinCount <= 0)
        {
            _baseRenderer.material = _usedMaterial;
            _leafObject.SetActive(false);
        }
    }
}
