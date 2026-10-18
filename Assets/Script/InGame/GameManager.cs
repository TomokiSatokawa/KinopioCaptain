using InGame.Player;
using UnityEngine;

namespace InGame
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private PlayerStatus _status;
        public PlayerStatus Status => _status;

        private void Start()
        {
            _status = new();
        }
    }
}
