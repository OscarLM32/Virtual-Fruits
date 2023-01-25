using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Cave : MonoBehaviour
{
    private Light2D globalLight;
    private float initialLight;
    private float desiredLight = 0.7f;
    private float lightDifference;

    protected bool zoneChange = false; 
    protected float speed = 2.5f;
    protected bool entering = false;

    public Light2D[] lanterns;

    protected void Start()
    {
        foreach (var lantern in lanterns)
        {
            lantern.enabled = false;
        }
        globalLight = GameObject.Find("Global light").GetComponent<Light2D>();
        initialLight = globalLight.intensity;
        lightDifference = initialLight - desiredLight;
    }

    private void Update()
    {
        if (!zoneChange)
            return;

        if (entering)
            LightDown();
        else
            LightUp();

    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        zoneChange = true;
        entering = !entering;
        foreach (var lantern in lanterns)
        {
            lantern.enabled = !lantern.enabled;
        }
    }
    
    protected void LightUp()
    {
        if (globalLight.intensity < initialLight)
            globalLight.intensity += lightDifference * speed * Time.deltaTime;
        else
            zoneChange = false;

    }

    protected void LightDown()
    {
        if (globalLight.intensity > desiredLight)
            globalLight.intensity -= lightDifference * speed * Time.deltaTime;
        else
            zoneChange = false;
    }
}
