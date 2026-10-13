using TMPro;
using UnityEngine;

namespace InGame.UI
{
    public class RemainingLivesView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _livesCount;

        public void UpdateLivesCount(int count)
        {
            _livesCount.text = $"Å~{count}";
        }
    }
}