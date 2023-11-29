using Enemies.ShootingEnemyLogic;
using UnityEngine;

namespace Enemies
{
    public class Bean : MonoBehaviour
    {
        public GameObject particles;
        public int direction = 1;

        void OnEnable()
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(8 * direction, 0);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            InstantiateParticles();
        }

        private void InstantiateParticles()
        {
            Instantiate(particles, gameObject.transform.position, Quaternion.identity);
            EnemyProjectilePool.I.DeleteProjectile(gameObject);
        }
    }
}