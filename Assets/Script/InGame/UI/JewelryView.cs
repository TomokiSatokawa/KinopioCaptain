using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{

    public class JewelryView : MonoBehaviour
    {
        [SerializeField] private Image[] _images;
        [SerializeField] private Sprite _ownedSprite;
        [SerializeField] private Sprite _emptySprite;

        public void UpdateCount(int count)
        {
            for (int i = 0; i < _images.Length; i++)
            {
                _images[i].sprite = i < count ? _ownedSprite : _emptySprite;
            }
        }

    }
}