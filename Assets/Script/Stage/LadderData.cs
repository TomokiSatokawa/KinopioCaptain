using UnityEngine;

namespace InGame.Stage
{
    public class LadderData : MonoBehaviour
    {
        [SerializeField] private Transform _topLadder;
        [SerializeField] private Transform _bottomLadder;
        [SerializeField] private Transform _topPlayer;
        [SerializeField] private Transform _bottomPlayer;

        public Vector3 TopLadderPos => _topLadder.position;
        public Vector3 BottomLadderPos => _bottomLadder.position;
        public Vector3 TopPlayerPos => _topPlayer.position;
        public Vector3 BottomPlayerPos => _bottomPlayer.position;

        public Vector3 GetPosition(float t)
        {
            return Vector3.Lerp(BottomLadderPos, TopLadderPos, t);
        }
    }
}
