using UnityEngine;

namespace GameSystems.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T i = null;

        public static T I
        {
            get
            {
                if (i == null)
                {
                    Debug.LogError("The singleton trying to be accessed cannot be found");
                }
                return i;
            }
            private set
            {
                i = value;
            }
        }

        protected void Awake()
        {
            if (i != null)
            {
                Destroy(gameObject);
            }
            else
            {
                I = this as T;
            }
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }

        protected abstract void OnAwake();

    }
}