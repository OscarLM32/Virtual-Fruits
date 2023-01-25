using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public Animator animator;
    public GameObject bean;

    private float _attackCycleTime = 3f;
    private float _timeElapsed = 0f;

    private void Update()
    {
        if (_timeElapsed > _attackCycleTime)
        {
            StartCoroutine(AttackBehaviour()); 
            _timeElapsed = 0f;
        }
        _timeElapsed += Time.deltaTime;
    }

    private IEnumerator AttackBehaviour()
    {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.5f);
        Vector2 beanSpawnPosition = new Vector2(transform.position.x+0.5f, transform.position.y+0.1f);
        Instantiate(bean, beanSpawnPosition, Quaternion.identity);
        animator.SetBool("Attack", false);
    }
}
