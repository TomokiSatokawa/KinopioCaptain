using System;
using UnityEngine;

namespace InGame.Common
{
    /// <summary>
    /// CollisionまたはTriggerによる接触・退出イベントを通知する
    /// </summary>
    public class ObjectDetector : MonoBehaviour
    {
        [SerializeField] private bool _isTrigger;

        public event Action<GameObject> OnContactEnter;
        public event Action<GameObject> OnContactExit;

        private void OnCollisionEnter(Collision collision)
        {
            if (_isTrigger) return;

            OnContactEnter?.Invoke(collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (_isTrigger) return;

            OnContactExit?.Invoke(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_isTrigger) return;

            OnContactEnter?.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_isTrigger) return;

            OnContactExit?.Invoke(other.gameObject);
        }
    }
}
