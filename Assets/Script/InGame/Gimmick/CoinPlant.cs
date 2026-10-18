using InGame.Gimmick;
using UnityEngine;

public class CoinPlant : MonoBehaviour,IInteractable
{
    private bool _isUsed = false;
    public bool CanInteract()
    {
        return !_isUsed;
    }

    public void Interact()
    {
        Debug.Log("GetCoin");
        _isUsed = true;
    }
}
