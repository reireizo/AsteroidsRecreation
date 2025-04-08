using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Array of sprites that the asteroid can potentially use.
    public Sprite[] sprites;
    // Float value representing the current set size of the asteroid.
    public float size = 1.0f;
    // Float value representing the minimum size an asteroid can randomly be set to.
    public float minSize = 0.5f;
    // Float value representing the maximum size an asteroid can randomly be set to.
    public float maxSize = 1.5f;
    // Float value the asteroid trajectory force is multiplied by to increase move speed.
    public float asteroidSpeed = 50.0f;
    // Float value representing amount of time before asteroid deletes itself.
    public float asteroidLifetime = 30.0f;

    // An Action that is fired whenever an asteroid has been destroyed. Listened to by the Game Manager. Takes in the destroyed asteroid.
    public static event Action<Asteroid> HasBeenDestoryed;

    // Reference to the asteroid's sprite renderer, to change out the sprite. Grabbed on Awake.
    SpriteRenderer asteroidSpriteRenderer;
    // Reference to the asteroid's rigidbody, to change it's mass by it's size. Grabbed on Awake.
    Rigidbody2D asteroidRB;

    // Awake needs to:
    // > Get sprite renderer and rigidbody for asteroid.
    void Awake()
    {
        asteroidSpriteRenderer = GetComponent<SpriteRenderer>();
        asteroidRB = GetComponent<Rigidbody2D>();
    }

    // Start needs to:
    // > Set a random sprite from the array of sprites to the sprite renderer.
    // > Set a random rotation on the asteroid.
    // > Set asteroid size.
    // > Set the asteroid's mass in the rigidbody based on the new size.
    void Start()
    {
        asteroidSpriteRenderer.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];

        transform.eulerAngles = new Vector3(0.0f, 0.0f, UnityEngine.Random.value * 360.0f);
        transform.localScale = Vector3.one * size;

        asteroidRB.mass = size;
    }

    // SetTrajectory needs to:
    // > Add a force to the asteroid rigid body, based on the passed in direction, and speed of the asteroid.
    // > Set the object to destroy after enough time has passed, according to asteroidLifetime.
    public void SetTrajectory(Vector2 direction)
    {
        asteroidRB.AddForce(direction * asteroidSpeed);
        Destroy(this.gameObject, asteroidLifetime);
    }

    // OnCollisionEnter2D needs to:
    // > Check if collision is a bullet.
    //   > Destroy the object.
    //   > Create 2 splits if size of current would create asteroids above or equal to minSize.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if((this.size * 0.5f) >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            Destroy (this.gameObject);
            HasBeenDestoryed(this);
        }
    }

    // CreateSplit needs to:
    // > Get current position and add randomization within a half unit radius.
    // > Instantiate a "half" asteroid with new position and current rotation.
    // > Set size to half of current size.
    // > Set trajectory via outer-edge of a 1 unit radius circle, multiplied by asteroid speed.
    void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += UnityEngine.Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;
        half.SetTrajectory(UnityEngine.Random.insideUnitCircle.normalized * asteroidSpeed);
    }

}
