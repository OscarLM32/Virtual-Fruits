using Enemies.ShootingEnemyLogic;
using Unity.Mathematics;
using UnityEngine;

public class Stinger : MonoBehaviour
{
    public GameObject particles;

    void OnEnable()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -5f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        InstantiateParticles();
    }

    private void InstantiateParticles()
    {
        Instantiate(particles, gameObject.transform.position, quaternion.identity);
        EnemyProjectilePool.I.DeleteProjectile(gameObject);
    }



}
