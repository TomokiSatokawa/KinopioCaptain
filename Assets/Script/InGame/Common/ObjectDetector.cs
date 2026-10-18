using System;
using UnityEngine;

namespace InGame.Common
{
    /// <summary>
    /// Colliderに触れている検出イベントを出す
    /// </summary>
    public class ObjectDetector : MonoBehaviour
    {
        [SerializeField] private bool _isTrigger;

        public event Action<GameObject> OnEnter;
        public event Action<GameObject> OnExit;

        private void OnCollisionEnter(Collision collision)
        {
            if (_isTrigger) return;

            OnEnter?.Invoke(collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (_isTrigger) return;

            OnExit?.Invoke(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_isTrigger) return;

            OnEnter?.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_isTrigger) return;

            OnExit?.Invoke(other.gameObject);
        }
    }
}
