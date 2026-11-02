using UnityEngine;
using UnityEngine.Playables;

namespace InGame.Player
{
    public class CutInPlayer : MonoBehaviour
    {
        [SerializeField] private PlayableAsset _startTimeline;
        [SerializeField] private PlayableDirector _playableDirector;

        public void PlayStarClip()
        {
            _playableDirector.Play(_startTimeline);
        }
    }
}
