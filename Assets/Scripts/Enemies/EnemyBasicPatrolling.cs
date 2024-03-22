using DG.Tweening;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Enemies
{

    /*
     * This is a very simple patrol script meant to be used when the enemy has some really basic
     * patrolling like no rotation, linear or simple shapes, no stop...
     */
    public class EnemyBasicPatrolling : MonoBehaviour
    {

        private const int _defaultResolution = 5;

        //These variables may be handy in enemy placement workflow
        [SerializeField]private Vector3[] _patrolPoints;
        [SerializeField]private PathType _pathType = PathType.CubicBezier;
        [SerializeField]private Ease _easeType = Ease.Linear;
        [SerializeField] private LoopType _loopType = LoopType.Restart;

        private Tween _tween;

        private void OnDisable()
        {
            PausePatrol();
            _tween.Kill();
            _tween = null;
        }

        public void SetUpPatrol(float speed)
        {
            _tween = transform.DOLocalPath(_patrolPoints, speed, _pathType, PathMode.Sidescroller2D, _defaultResolution)
                              .SetEase(_easeType)
                              .SetSpeedBased(true)
                              .SetLoops(-1, _loopType);
            PausePatrol();
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
}