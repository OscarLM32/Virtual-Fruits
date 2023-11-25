using UnityEngine;

public class PreInstantiationModTest : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        var aux = player;
        aux.GetComponent<UnityInstantiateFlow>().enabled = false;
        Instantiate(aux);
        Debug.Log("Calling after instantiating");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
