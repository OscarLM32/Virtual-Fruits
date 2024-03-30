
using System;
using UnityEngine;

namespace Enemies.Bunny
{
    [Serializable]
    public struct BunnyPatrolAction
    {
        public BunnyPatrolActionType action;
        public Transform nextPatrolPoint;
    }

}