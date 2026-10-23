using UnityEngine;

namespace InGame.Item
{
    public class ItemData : MonoBehaviour, ICollectible
    {
        [SerializeField] private string _name = "defaultItemName";

        public string Name => _name;

        public void OnPlayerTouch()
        {
            Debug.Log($"{Name}‚ğˆø‚Á‚±”²‚¢‚½I");
        }
    }
}
