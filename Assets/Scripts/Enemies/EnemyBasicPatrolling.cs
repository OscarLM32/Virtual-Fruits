using DG.Tweening;
using UnityEngine;

/*
 * This is a very simple patrol script meant to be used when the enemy has some really basic
 * patrolling like no rotation, linear or simple shapes, no stop...
 */
public class EnemyBasicPatrolling : MonoBehaviour
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
