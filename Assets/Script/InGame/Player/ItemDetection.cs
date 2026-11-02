using InGame.Item;
using UnityEngine;

namespace InGame.Player
{

    public class ItemDetection : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out ICollectible itemData))
            {
                itemData.OnPlayerTouch();
            }
        }
    }
}