using System.Collections;
using System.Collections.Generic;
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
    // > ***Set a random size for the asteroid.
    // > Set the asteroid's mass in the rigidbody based on the new size.

    void Start()
    {
        asteroidSpriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        transform.localScale = Vector3.one * size;

        asteroidRB.mass = size;
    }

}
