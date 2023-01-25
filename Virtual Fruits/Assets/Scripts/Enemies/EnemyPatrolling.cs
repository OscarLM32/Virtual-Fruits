using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyPatrolling : MonoBehaviour
{

    public Vector3[] patrolPoints;
    public float totalPatrolTime;
    public PathType pathType = PathType.CubicBezier;
    public string patrolId;
    public Ease easeType = Ease.Linear;

    void Start()
    {
        transform.DOLocalPath(
            patrolPoints, 
            totalPatrolTime, 
            pathType, 
            PathMode.Sidescroller2D, 
            5)
            .SetId(patrolId)
            .SetEase(easeType)
            .SetLoops(-1);
    }
}
