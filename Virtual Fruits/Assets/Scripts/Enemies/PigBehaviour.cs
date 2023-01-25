using System.Collections;
using UnityEngine;


public class PigBehaviour : MonoBehaviour
{
    public Animator animator;
    public float speed;
    private Vector2 _lastPosition;

    private Collider2D _frontCol;
    public bool stopBehaviourPig = false;
    public PhysicsMaterial2D ragdollMaterial;
    

    private void Start()
    {
        _lastPosition = transform.position;
        _frontCol = GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        //I know these will only move horizontally. So I will not bother calculating the speed in the Y axis.
        speed = (transform.position.x - _lastPosition.x) / Time.deltaTime;
        if (speed >= 0) 
            transform.localScale = new Vector3(-1.5f, 1.5f, 1);
        else
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
        
        _lastPosition = transform.position;
        animator.SetFloat("Speed", Mathf.Abs(speed));
    }
    

    public bool IsFrontCol(Collider2D col)
    {
        if (_frontCol == col)
        {
            return true;
        }
        return false;
    }
    
    public IEnumerator OnHitBehaviour()
    {
        stopBehaviourPig = true;
        //GetComponent<Patrol>().stopPatrolling = true;
        animator.SetBool("Hit", true);
        float launchDirection = Mathf.Abs(speed + 1) / (speed + 1);

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.sharedMaterial = ragdollMaterial;
        rb.AddForce(new Vector2(800 * launchDirection, 550));
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    

}
