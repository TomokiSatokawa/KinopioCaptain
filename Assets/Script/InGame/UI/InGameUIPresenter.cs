using InGame.Player;
using R3;
using UnityEngine;

namespace InGame.UI
{

    public class InGameUIPresenter : MonoBehaviour
    {
        [SerializeField] private CoinView _coinView;
        [SerializeField] private JewelryView _jewelryView;
        [SerializeField] private RemainingLivesView _remainingLivesView;

        private void Start()
        {
            var  status = GameManager.Instance.Status;
            status.Coin.Subscribe(_coinView.UpdateCoinCount).RegisterTo(destroyCancellationToken);
            status.RemainingLives.Subscribe(_remainingLivesView.UpdateLivesCount).RegisterTo(destroyCancellationToken);
            status.Jewelry.Subscribe(_jewelryView.UpdateCount).RegisterTo(destroyCancellationToken);
        }
    }
}
