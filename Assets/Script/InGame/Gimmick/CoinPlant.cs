using InGame;
using InGame.Gimmick;
using UnityEngine;

public class CoinPlant : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _leafObject;
    [SerializeField] private Renderer _baseRenderer;
    [SerializeField] private Material _availableMaterial;
    [SerializeField] private Material _usedMaterial;

    private bool _isUsed = false;

    private void Start()
    {
        _baseRenderer.material = _availableMaterial;
        _leafObject.SetActive(true);
    }

    public bool CanInteract()
    {
        return !_isUsed;
    }

    public void Interact()
    {
        GameManager.Instance.Status.AddCoin();
        _baseRenderer.material = _usedMaterial;
        _leafObject.SetActive(false);
        _isUsed = true;
    }
}
