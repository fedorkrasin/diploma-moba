using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Util.Misc
{
    [Serializable]
    public abstract class Library<TKey, TValue>
    {
        [SerializeField]
        private List<Mapping> _mappings;

        public Dictionary<TKey, TValue> Mappings => _mappings.ToDictionary(m => m.Key, m => m.Value);

        [Serializable]
        private class Mapping
        {
            public TKey Key;
            public TValue Value;
        }
    }
}