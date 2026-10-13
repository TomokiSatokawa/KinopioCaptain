using InGame.Player;
using R3;
using UnityEngine;

namespace InGame.UI
{

    public class InGameUIPresenter : MonoBehaviour
    {
        [SerializeField] private PlayerStatus _playerStatus;
        [SerializeField] private CoinView _coinView;
        [SerializeField] private JewelryView _jewelryView;
        [SerializeField] private RemainingLivesView _remainingLivesView;

        private void Start()
        {
            _playerStatus.Coin.Subscribe(_coinView.UpdateCoinCount).RegisterTo(destroyCancellationToken);
            _playerStatus.RemainingLives.Subscribe(_remainingLivesView.UpdateLivesCount).RegisterTo(destroyCancellationToken);
            _playerStatus.Jewelry.Subscribe(_jewelryView.UpdateCount).RegisterTo(destroyCancellationToken);
        }
    }
}
