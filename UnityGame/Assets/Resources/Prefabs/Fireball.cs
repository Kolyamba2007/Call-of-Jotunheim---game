using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Range(0, 1)]
    public float ProjectileSpeed;
    public byte Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            default:
                Destroy(this);
                break;
            case "Unit":
                if (collision.GetComponent<Unit>() != Player.Hero)
                {
                    collision.GetComponent<Unit>().Hit(collision.GetComponent<Unit>(), Damage);
                    Destroy(this);
                }
                break;
        }
    }
}
