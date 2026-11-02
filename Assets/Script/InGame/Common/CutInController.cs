using Input;
using UnityEngine;
using UnityEngine.Playables;

namespace InGame.Common
{
    public class CutInController : MonoBehaviour
    {
        [SerializeField] private Camera _cutInCamera;
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private PlayableAsset _startTimeline;

        private Transform _cutInCameraPosition;
        private PlayableAsset _playAsset;
        private float _startTime;

        public void StartFollowCamera(Transform target)
        {
            if (target == null)
            {
                Debug.LogError("[CutInPlayer] í«è]ëŒè€Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
                return;
            }

            if (_cutInCamera == null || _gameCamera == null)
            {
                Debug.LogError("[CutInPlayer] ÉJÉÅÉâÇ™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
                return;
            }

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
            _playAsset = null;
            InputManager.SetEnabled(true);
        }

        public void LookDirection(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.0001f) return;

            transform.forward = direction.normalized;
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
                return;
            }

            if (_cutInCameraPosition != null)
            {
                _cutInCamera.transform.position = _cutInCameraPosition.position;
                _cutInCamera.transform.rotation = _cutInCameraPosition.rotation;
            }

        }
    }
}