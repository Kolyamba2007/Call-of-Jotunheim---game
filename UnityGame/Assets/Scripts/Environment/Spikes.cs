using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.GetComponent<Unit>() && !collision.gameObject.GetComponent<Unit>().Invulnerable && !collision.gameObject.GetComponent<Unit>().IsDead)
        {
            collision.gameObject.GetComponent<Unit>().Kill(collision.gameObject.GetComponent<Unit>());
        }
    }
}
