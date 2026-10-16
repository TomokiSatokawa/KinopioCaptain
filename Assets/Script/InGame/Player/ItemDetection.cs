using InGame.Item;
using UnityEngine;

namespace InGame.Player
{

    public class ItemDetection : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICollectible itemData))
            {
                itemData.PullItem();
            }
        }
    }
}