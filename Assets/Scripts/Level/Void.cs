
using DevSystems.CombatSystem;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Collider2D))]
    public class LevelVoid : MonoBehaviour
    {
        private const int _attackPower = 99999;

        private void Awake()
        {
            CheckTrigger();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IKillable killable = collision.GetComponent<IKillable>();
            killable?.Kill(new KillContext(_attackPower));
        }

        private void CheckTrigger()
        {
            var trigger = GetComponent<Collider2D>();
            trigger.isTrigger = true;
        }
    }
}