using System;

namespace Extensions.Serializables
{
    [Serializable]
    public class SerializableDictionaryField<TKey, TValue>
    {
        public TKey key;
        public TValue value;

        public SerializableDictionaryField(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }
}