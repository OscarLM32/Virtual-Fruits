using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Enemies.ShootingEnemyLogic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private GameObject _onCollisionParticles;

        [SerializeField]private Rigidbody2D _rb;

        public void SetUpProjectile(Vector2 position, Vector2 direction, float speed)
        {
            transform.position = position;
            _rb.velocity = direction * speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(_onCollisionParticles != null)
            {
                Instantiate(_onCollisionParticles, gameObject.transform.position, Quaternion.identity);
            }

            //var killable = collision.gameObject.GetComponent<IKillable>();
            //if(killable) killable.Kill();

            EnemyProjectilePool.I.DeleteProjectile(gameObject);
        }
    }
}