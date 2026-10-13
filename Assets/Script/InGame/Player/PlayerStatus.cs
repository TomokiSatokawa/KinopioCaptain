using R3;
using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// プレイヤーのステータスを管理する
    /// </summary>
    public class PlayerStatus : MonoBehaviour
    {
        private readonly ReactiveProperty<int> _coin = new();
        private readonly ReactiveProperty<int> _jewelry = new();
        private readonly ReactiveProperty<int> _remainingLives = new();

        public ReadOnlyReactiveProperty<int> Coin => _coin;
        public ReadOnlyReactiveProperty<int> Jewelry => _jewelry;
        public ReadOnlyReactiveProperty<int> RemainingLives => _remainingLives;

        public void AddCoin(int amount = 1)
        {
            _coin.Value += amount;
        }

        public void AddJewelry(int amount = 1)
        {
            _jewelry.Value += amount;
        }

        public void AddLives(int amount = 1)
        {
            _remainingLives.Value += amount;
        }

        public void RemoveLives()
        {
            _remainingLives.Value--;
        }
    }
}
