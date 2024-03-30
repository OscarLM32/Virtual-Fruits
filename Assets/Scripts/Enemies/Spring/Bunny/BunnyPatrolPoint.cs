using EditorSystems.Logger;
using Extensions.Serializables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Bunny
{
    [RequireComponent(typeof(Collider2D))]
    public class BunnyPatrolPoint : MonoBehaviour
    {
        [SerializeField] private List<PatrolContext> _actions = new();

        public BunnyPatrolAction GetNextAction(BunnyPatrolPoint _previousPoint)
        {
            foreach(var action in _actions)
            {
                if(action.previousPoint == _previousPoint)
                {
                    return action.action;
                }
            }

            EditorLogger.LogError(LoggingSystem.ENEMY, $"{{{EnemyType.BUNNY}}}: No action was found for the provided point in this context");
            return new BunnyPatrolAction();
        }

        //This method is called to get the initial action may not be neccessary
        public BunnyPatrolAction GetFirstAction()
        {
            if( _actions.Count == 0)
            {
                EditorLogger.LogError(LoggingSystem.ENEMY, $"{{{EnemyType.BUNNY}}}: No action was found for this patrol point");
                return new BunnyPatrolAction();
            }

            return _actions[0].action;
        }

        [Serializable]
        private struct PatrolContext
        {
            public BunnyPatrolPoint previousPoint;
            public BunnyPatrolAction action;
        }

    }
}