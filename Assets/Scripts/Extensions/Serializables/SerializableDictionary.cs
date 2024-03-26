
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions.Serializables
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {

        [SerializeField]
        private List<SerializableDictionaryField<TKey, TValue>> elements = new();
        public void OnAfterDeserialize()
        {
            Clear();

            foreach (var element in elements)
            {
                this[element.key] = element.value;
            }
        }

        public void OnBeforeSerialize()
        {
            elements.Clear();

            foreach (var pair in this)
            {
                elements.Add(new SerializableDictionaryField<TKey, TValue>(pair.Key, pair.Value));
            }
        }
    }
}