using DG.Tweening;
using UnityEngine;

/*
 * This is a very simple patrol script meant to be used when the enemy has some really basic
 * patrolling like no rotation, linear or simple shapes, no stop...
 */
public class EnemyBasicPatrolling : MonoBehaviour
{

    private const int _defaultResolution = 5;

    [SerializeField]private Vector3[] _patrolPoints;
    [SerializeField]private float _totalPatrolTime;
    [SerializeField]private PathType _pathType = PathType.CubicBezier;
    [SerializeField]private Ease _easeType = Ease.Linear;

    private Tween _tween;

    private void OnDisable()
    {
        PausePatrol();
        _tween = null;
    }

    public void SetUpPatrol(Vector3[] patrolPoints, float totalPatrolTime, PathType pathType, Ease easeType)
    {
        _tween = transform.DOLocalPath(patrolPoints,totalPatrolTime, pathType,PathMode.Sidescroller2D,_defaultResolution)
                         .SetEase(easeType)
                         .SetLoops(-1);
    }

    //Maybe overkills
    public void StartPatrol()
    {
        _tween.Play();
    }

    public void PausePatrol()
    {
        _tween.Pause();
    }
}
