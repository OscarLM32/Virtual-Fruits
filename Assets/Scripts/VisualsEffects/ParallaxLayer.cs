using UnityEngine;


public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactorX;
    public float parallaxFactorY;

    public float length;
    public float distX = 0; //The distance that the camera has travelled in relation to the layer

    private void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public void Move(float deltaX, float deltaY)
    {
        //The - sign is necessary because the sign on "deltaX" is contrary to the movement direction
        distX += -(deltaX * (1 - parallaxFactorX));

        Vector2 newPos = transform.position;
        newPos.x -= deltaX * parallaxFactorX;
        newPos.y -= deltaY * parallaxFactorY;

        transform.position = newPos;

        if (distX > length)
        {
            transform.position += new Vector3(length, 0, 0);
            distX -= length;
        }
        else if (distX < -length)
        {
            transform.position -= new Vector3(length, 0, 0);
            distX += length;
        }
    }
}