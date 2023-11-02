using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Cave : MonoBehaviour
{
    private Light2D _globalLight;
    private float _initialLight;
    private float _desiredLight = 0.7f;
    private float _lightDifference;

    protected bool zoneChange = false;
    protected float speed = 2.5f;
    protected bool lightDown = false;



    protected void Start()
    {
        _globalLight = GameObject.Find("Global light").GetComponent<Light2D>();
        _initialLight = _globalLight.intensity;
        _lightDifference = _initialLight - _desiredLight;
    }

    private void Update()
    {
        if (!zoneChange)
            return;

        if (lightDown)
            LightDown();
        else
            LightUp();
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        zoneChange = true;
        lightDown = true;
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        zoneChange = true;
        lightDown = false;
    }

    protected void LightUp()
    {
        if (_globalLight.intensity < _initialLight)
            _globalLight.intensity += _lightDifference * speed * Time.deltaTime;
        else
            zoneChange = false;

    }

    protected void LightDown()
    {
        if (_globalLight.intensity > _desiredLight)
            _globalLight.intensity -= _lightDifference * speed * Time.deltaTime;
        else
            zoneChange = false;
    }
}
