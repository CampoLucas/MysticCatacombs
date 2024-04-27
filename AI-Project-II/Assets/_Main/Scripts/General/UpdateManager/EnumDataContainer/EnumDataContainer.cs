using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UpdateManager
{
    [Serializable]
    public class EnumDataContainer<T1, T2> : IDisposable, IReadOnlyCollection<T2>
        where T1 : Enum
    {
        public int Count => content.Length;
        public bool IsReadOnly => true;
        
        [SerializeField] private T2[] content = null;
        [SerializeField] private T1 enumType;

        public T2 this[int i] => content[i];

        public IEnumerator<T2> GetEnumerator()
        {
            for (var i = 0; i < content.Length; i++)
            {
                yield return content[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public void Dispose()
        {
            content = null;
        }
    }
}