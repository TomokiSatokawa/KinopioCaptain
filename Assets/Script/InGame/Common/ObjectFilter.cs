using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.Common
{
    /// <summary>
    /// 検出イベントを利用して、条件に一致するコンポーネントを保持するクラス
    /// </summary>
    /// <typeparam name="T">検出対象のコンポーネント型</typeparam>
    public class ObjectFilter<T>
    {
        private readonly List<T> _filteredObjects  = new(); 
        private readonly ObjectDetector _detector;
        private readonly Func<T, bool> _filterPredicate;

        /// <summary> フィルタリング済みオブジェクトの読み取り専用リスト </summary>
        public IReadOnlyList<T> FilteredObjects  => _filteredObjects ;

        /// <summary>
        /// 検出イベントを購読し、フィルタリングを開始する
        /// </summary>
        public ObjectFilter(ObjectDetector detector,Func<T,bool> filterPredicate  = null)
        {
            if (detector == null)
            {
                Debug.LogError("[ObjectFilter] ObjectDetector is null.");
                return;
            }

            _detector = detector;
            _detector.OnEnter += OnEnter;
            _detector.OnExit += OnExit;

            _filterPredicate = filterPredicate ;
        }

        private void OnEnter(GameObject target)
        {
            if (target == null)
                return;

            // 対象のコンポーネントを取得できなければ登録しない
            if (!target.TryGetComponent(out T component))
                return;

            // 重複登録を防止する
            if (_filteredObjects.Contains(component))
                return;

            // フィルター条件を満たさない場合は登録しない
            if (_filterPredicate != null && !_filterPredicate(component))
                return;

            _filteredObjects .Add(component);
        }

        private void OnExit(GameObject target)
        {
            if (target == null)
                return;

            _filteredObjects.RemoveAll(x => x.Equals(target));
        }

        /// <summary>
        /// 検出イベントの購読を解除し、保持している対象をクリアする
        /// </summary>
        public void Dispose()
        {
            if (_detector != null)
            {
                _detector.OnEnter -= OnEnter;
                _detector.OnExit -= OnExit;
            }

            _filteredObjects.Clear();
        }
    }
}
