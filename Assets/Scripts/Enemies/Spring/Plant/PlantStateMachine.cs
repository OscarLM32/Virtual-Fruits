using DevSystems.StateMachine;
using UnityEngine;

namespace Enemies.Plant
{
    public class PlantStateMachine : BaseStateMachine<PlantStateMachine>
    { 
        public Animator animator { get; private set; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
    }
}