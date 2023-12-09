using Enemies;
using Level.DynamicDifficulty.Modifiers.Parameters;
using System;
using UnityEngine;

namespace Level.DynamicDifficulty.Modifiers
{
    [Serializable]
    public class Modifier : ISerializationCallbackReceiver
    {
        public ModifierAction action;
        [SerializeField] public ModifierParameters parameters = new AddEnemyParameters();

        public void OnAfterDeserialize()
        {
            switch (action)
            {
                case ModifierAction.CUSTOM:
                    break;
                case ModifierAction.CHANGE_TERRAIN:
                    break;
                case ModifierAction.ADD_ENEMY:
                    parameters = new AddEnemyParameters();
                    break;
                case ModifierAction.REMOVE_ENEMY:
                    parameters = new RemoveEnemyParameters();
                    break;
            }
        }

        public void OnBeforeSerialize()
        {
            
        }




    }
}

/*public GameObject target;

public EnemyType enemyType;
public Vector2 position;
//TODO: i'm not a fan of this so, I should give it a whirl later on
public Transform parentObject;
//TODO: add patrolling values
//TODO: add enemy transform parameters*/