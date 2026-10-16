using InGame.Item;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    [SerializeField] private string _name = "defaultItemName";

    public string Name => _name;

    public void PullItem()
    {
            Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PullItem();
        }
    }
}
