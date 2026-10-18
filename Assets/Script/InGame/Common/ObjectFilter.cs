using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.Common
{

    public class ObjectFilter<T>
    {
        private List<T> _onFilterObject = new(); 
        private ObjectDetector _detector;
        private Func<T, bool> _filterAction;

        public IReadOnlyList<T> Fillters => _onFilterObject;

        public ObjectFilter(ObjectDetector detector,Func<T,bool> filterAction = null)
        {
            if (detector == null)
            {
                Debug.LogError("");
                return;
            }

            _detector = detector;
            _detector.OnEnter += OnEnter;
            _detector.OnExit += OnExit;

            _filterAction = filterAction;
        }

        private void OnEnter(GameObject target)
        {
            //T Component‚ðŽ‚Á‚Ä‚¢‚Ä2d“o˜^‚ð–hŽ~
            if (!target.TryGetComponent(out T component) || _onFilterObject.Contains(component))
                return;

            //ƒtƒBƒ‹ƒ^ƒŠƒ“ƒO
            if (!_filterAction?.Invoke(component) ?? false)
                return;

            _onFilterObject.Add(component);
        }

        private void OnExit(GameObject target)
        {
            _onFilterObject.RemoveAll(x => x.Equals(target));
        }
    }
}
