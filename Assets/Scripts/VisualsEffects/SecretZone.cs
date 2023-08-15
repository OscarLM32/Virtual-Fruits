
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class SecretZone : Cave
{
    private Tilemap _tilemap;
    private Color _color;
    
    public List<GameObject> lanterns;

    private new void Start()
    {
        base.Start();
        
        foreach (var lantern in lanterns)
        {
            lantern.GetComponent<Light2D>().enabled = false;
        }

        _tilemap = GetComponent<Tilemap>();
        _color = GetComponent<Tilemap>().color;
    }

    private void Update()
    {
        if (!zoneChange)
            return;

        if (lightDown)
        {
            FadeOut();
            LightDown(); 
        }
        else
        {
            FadeIn();
            LightUp();
        }

    }
    
    private void FadeIn()
    {
        float fadeInAmount = _color.a + (speed * Time.deltaTime);
        _color = new Color(_color.r, _color.g, _color.b, fadeInAmount);
        _tilemap.color = _color;
        if (_color.a >= 1)
        {
            zoneChange = false;
        }
    }

    private void FadeOut()
    {
        float fadeOutAmount = _color.a - (speed * Time.deltaTime);
        _color = new Color(_color.r, _color.g, _color.b, fadeOutAmount);
        _tilemap.color = _color;

        if (_color.a <= 0)
        {
            zoneChange = false;
        }
    }
    
    private new void  OnTriggerEnter2D(Collider2D col)
    {
        zoneChange = true;
        lightDown = true;
        foreach (var lantern in lanterns)
        {
            lantern.GetComponent<Light2D>().enabled = !lantern.GetComponent<Light2D>().enabled;
        }
    }
    
    protected new void OnTriggerExit2D(Collider2D other)
    {

        zoneChange = true;
        lightDown = false;
        foreach (var lantern in lanterns)
        {
            lantern.GetComponent<Light2D>().enabled = !lantern.GetComponent<Light2D>().enabled;
        }
    }
}