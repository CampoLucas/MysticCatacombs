using Unity.VisualScripting;
using UnityEngine;

namespace _Main.Scripts.General
{
    public struct NullChecker<T>
    {
        private bool _notNull;
        private bool _logError;

        public NullChecker(T value, bool logError = false)
        {
            _notNull = value != null;
            _logError = logError;
        }

        public bool HasValue()
        {
#if UNITY_EDITOR
            if (!_notNull && _logError)
                Debug.LogError($"NullChecker: found null reference from type ({typeof(T).ToString()})");
#endif
            return _notNull;
        }
        public void Set(T value) => _notNull = value != null;

        public static implicit operator bool(NullChecker<T> checker) => checker.HasValue();
    }
}