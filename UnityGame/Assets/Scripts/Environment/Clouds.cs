using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Clouds : MonoBehaviour
{
    public float Speed;
    private static float distance;
    private static Camera cam;

    private float deltaY;

    void Start()
    {
        cam = Camera.main;
        deltaY = cam.transform.position.y - transform.position.y;
        distance = GetComponent<SpriteRenderer>().size.x;
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x + Speed, cam.transform.position.y - deltaY);
        if (transform.position.x - cam.transform.position.x >= distance)
        {
            transform.position = new Vector2(cam.transform.position.x - distance, cam.transform.position.y - deltaY);
        }
        else if (transform.position.x - cam.transform.position.x <= -distance)
        {
            transform.position = new Vector2(cam.transform.position.x + distance, cam.transform.position.y - deltaY);
        }
    }
}