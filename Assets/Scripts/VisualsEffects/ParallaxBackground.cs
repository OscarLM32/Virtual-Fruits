using System.Collections.Generic;
using UnityEngine;


public class ParallaxBackground : MonoBehaviour
{
    public Vector2 cameraPos;
    private Vector2 oldCameraPos;
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    private float deltaX, deltaY;
    void Start()
    {
        cameraPos = Camera.main.transform.position;
        oldCameraPos = cameraPos;
        SetLayers();
        InitPosition();
    }

    private void FixedUpdate()
    {
        cameraPos = Camera.main.transform.position;
        if (cameraPos != oldCameraPos)
        {
            deltaX = oldCameraPos.x - cameraPos.x;
            deltaY = oldCameraPos.y - cameraPos.y;
            Move(deltaX, deltaY);
            oldCameraPos = cameraPos;
        }
    }

    void InitPosition()
    {
        //Not necessary, but gives some safety making sure that the player always spawns in front of the background
        deltaX = transform.position.x - cameraPos.x;
        deltaY = transform.position.y - cameraPos.y;
        //If the background is positioned higher than the character it should be perfectly aligned
        if (deltaY > 0) { deltaY = 0; }
        Move(deltaX, deltaY);
    }
    void SetLayers()
    {
        parallaxLayers.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }

    void Move(float deltaX, float deltaY)
    {
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(deltaX, deltaY);
        }
    }
}
