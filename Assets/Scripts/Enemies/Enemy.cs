using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        //This is going to be called in GameManager Start method

        private void OnEnable()
        {
            GameManager.LevelStart += OnLevelStart;
        }

        private void OnDisable()
        {
            GameManager.LevelStart -= OnLevelStart;
        }

        protected abstract void OnLevelStart();
    }
}