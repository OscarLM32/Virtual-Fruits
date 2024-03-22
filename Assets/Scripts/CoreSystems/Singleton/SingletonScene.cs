using EditorSystems.Logger;
using UnityEngine;

namespace GameSystems.Singleton
{
    public abstract class SingletonScene<T> : MonoBehaviour where T : Component
    {
        protected static T i = null;

        public static T I
        {
            get
            {
                //I do not instantiate SceneSingleton like I do with generic singletons since these usually
                //have more spicific code that cannot me simply instantiated
                if (i == null)
                {
                    EditorLogger.LogWarning(LoggingSystem.SINGLETON, "The singleton trying to be accessed cannot be found: "+ typeof(T));
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
            OnAwake();
        }

        protected abstract void OnAwake();

    }
}