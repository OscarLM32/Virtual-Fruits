
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretZone : Cave
{
    private Tilemap tilemap;
    private Color color;

    void Start()
    {
        base.Start();
        tilemap = GetComponent<Tilemap>();
        color = GetComponent<Tilemap>().color;
    }

    private void Update()
    {
        if (!zoneChange)
            return;

        if (entering)
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
    }

    private void FadeIn()
    {
        float fadeInAmount;
        fadeInAmount = color.a + (speed * Time.deltaTime);
        color = new Color(color.r, color.g, color.b, fadeInAmount);
        tilemap.color = color;
        if (color.a >= 1)
        {
            zoneChange = false;
        }
    }

    private void FadeOut()
    {
        float fadeOutAmount;
        fadeOutAmount = color.a - (speed * Time.deltaTime);
        color = new Color(color.r, color.g, color.b, fadeOutAmount);
        tilemap.color = color;

        if (color.a <= 0)
        {
            zoneChange = false;
        }
    }
}