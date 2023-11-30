using UnityEngine;

namespace Testing
{
    internal class StartFlowTest : MonoBehaviour
    {
        public GameObject prefab;

        private void Start()
        {
            Instantiate(prefab);
            Debug.Log("Calling from instantiatior");
        }
    }
}