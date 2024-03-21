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
                    GameObject newObject = new GameObject("NotfoundSingleton");
                    i = newObject.AddComponent<T>();
                    newObject.name = typeof(T).Name;
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