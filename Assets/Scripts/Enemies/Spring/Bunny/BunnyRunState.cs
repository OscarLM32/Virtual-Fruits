using UnityEngine;

namespace Enemies.Bunny
{
    public class BunnyRunState : MonoBehaviour
    {
        public float speed = 2f;
        private float _realSpeed = 2f;

        private Vector2 _moveFrom;
        private Vector2 _moveTo;
        private float distance = 0;

        private Rigidbody2D _rb;

        private float _pathPercentageDone = 0;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            //Just in case somethign goes wrong with "SetUpMove"
            _pathPercentageDone = 0;
        }

        private void Update()
        {
            var currentPosX = Mathf.Lerp(_moveFrom.x, _moveTo.x, _pathPercentageDone);
            _pathPercentageDone += _realSpeed * Time.deltaTime;
            _rb.position = new Vector2(currentPosX, _rb.position.y);
        }

        public void SetUpMove(Vector2 moveFrom, Vector2 moveTo)
        {
            _moveFrom = moveFrom;
            _moveTo = moveTo;
            distance = Mathf.Abs(_moveTo.x - _moveFrom.x);
            _realSpeed = speed / distance;
            _pathPercentageDone = 0;
        }
    }
}