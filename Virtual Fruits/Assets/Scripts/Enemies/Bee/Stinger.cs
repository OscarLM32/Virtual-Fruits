using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Stinger : MonoBehaviour
{
    public ParticleSystem stingerDestroy;
    private Color invisibleColor = new Color(0,0,0,0);
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -4f);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        StartCoroutine(instantiateParticles());
    }
    
    private IEnumerator instantiateParticles()
    {
        ParticleSystem particles;
        particles = Instantiate(stingerDestroy, gameObject.transform.position, quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().color = invisibleColor;
        yield return new WaitForSeconds(1f); 
        Destroy(particles.gameObject);
        Destroy(gameObject);
    }
    


}
