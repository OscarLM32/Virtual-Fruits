
using Enemies;
using Extensions.Serializables;
using System;
using UnityEngine;

namespace DynamicDifficulty.DynamicParametersScriptables
{
    public abstract class DynamicParameters<T> : ScriptableObject, ISerializationCallbackReceiver where T : struct
    {
        public EnemyType type;

        public SerializableDictionary<Difficulty, T> parameters;

        public void OnAfterDeserialize()
        {
            if (parameters.Count < Enum.GetValues(typeof(Difficulty)).Length)
            {
                AddMissingFields();
            }
        }

        public void OnBeforeSerialize()
        {
            
        }

        private void AddMissingFields()
        {
            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                parameters.TryAdd(difficulty, new T());
            }
        }
    }
}