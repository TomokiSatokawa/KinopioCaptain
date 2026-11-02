using Input;
using UnityEngine;
using UnityEngine.Playables;

namespace InGame.Common
{
    public class CutInPlayer : MonoBehaviour
    {
        [SerializeField] private Camera _cutInCamera;
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private PlayableAsset _startTimeline;

        private Transform _cutInCameraPosition;
        private PlayableAsset _playAsset;
        private float _startTime;

        public void StartFollowCamera(Transform target)
        {
            _cutInCamera.gameObject.SetActive(true);
            _gameCamera.gameObject.SetActive(false);
            _cutInCameraPosition = target;
            InputManager.SetEnabled(false);
        }

        private void End()
        {
            _cutInCamera.gameObject.SetActive(false);
            _gameCamera.gameObject.SetActive(true);
            _cutInCameraPosition = null;
            InputManager.SetEnabled(true);
        }

        public void LookDirection(Vector3 direction)
        {
            direction.y = 0;
            this.transform.forward = direction;
        }

        public void SetPosition(Vector3 pos)
        {
            pos.y = this.transform.position .y;
            this.transform.position = pos;
        }
        public void PlayCutIn(PlayableAsset playable)
        {
            _playAsset = playable;
            _startTime = Time.time;
        }

        private void Update()
        {
            if (_playAsset == null) return;

            if (Time.time - _startTime >= _playAsset.duration)
            {
                End();
            }

            if (_cutInCameraPosition != null)
            {
                _cutInCamera.transform.position = _cutInCameraPosition.position;
                _cutInCamera.transform.rotation = _cutInCameraPosition.rotation;
            }

        }
    }
}