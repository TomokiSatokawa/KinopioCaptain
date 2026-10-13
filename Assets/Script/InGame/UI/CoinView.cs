using TMPro;
using UnityEngine;

namespace InGame.UI
{
    public class CoinView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinCount;

        public void UpdateCoinCount(int count)
        {
            _coinCount .text = $"Å~{count}";
        }
    }
}
