using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
        Instantiate(particles, gameObject.transform.position, quaternion.identity);
        EnemyProjectilePool.I.DeleteProjectile(gameObject);
    }
}
