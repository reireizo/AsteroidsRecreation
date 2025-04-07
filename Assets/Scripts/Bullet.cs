using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Float value that the direction for the projection force is multiplied by to increase bullet speed.
    public float bulletSpeed = 500.0f;
    // Float value representing how much time in seconds the bullet prefab will exist for.
    public float bulletLifetime = 10.0f;

    // Reference to the bullet's rigidbody. Grabbed on Awake.
    Rigidbody2D bulletRB;

    // Awake needs to:
    // > Get rigidbody for bullet.
    void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>();   
    }

    // Project needs to:
    // > Apply force to bullet with given direction.
    public void Project(Vector2 direction)
    {
        bulletRB.AddForce(direction * bulletSpeed);

        Destroy(this.gameObject, bulletLifetime);
    }

    // OnCollisionEnter2D needs to:
    // Destroy bullet when it touches something.
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
