using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Util.Misc
{
    public class ScriptableObjectLibrary<TKey, TValue> : ScriptableObject
    {
        [SerializeField]
        private LibraryBase _library;

        public Dictionary<TKey, TValue> Library => _library.Mappings;

        [Serializable]
        protected class LibraryBase : Library<TKey, TValue>
        {
        }
    }
}