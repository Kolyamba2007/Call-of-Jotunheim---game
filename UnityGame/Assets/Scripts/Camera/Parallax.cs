using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float ParallaxEffect;

    private float length, startPos;
    private float dist;
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player.Hero.IsDead)
        {
            float temp = Camera.main.transform.position.x * (1 - ParallaxEffect);
            dist = Camera.main.transform.position.x * ParallaxEffect;

            transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

            if (temp > startPos + length) startPos += length;
            else if (temp < startPos - length) startPos -= length;
        }
    }
}
