using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    // Reference to bullet object through it's script.
    public Bullet bulletPrefab;
    // Float value that the movement force is multiplied by to increase move speed.
    public float moveSpeed = 1.0f;
    // Float value that the turn direction is multiplied by to get the torque applied to the rigidbody.
    public float turnSpeed = 1.0f;

    // An Action that is fired when the player has died. Listened to by the game manager.
    public static event Action HasDied;

    // Bool for if you are boosting (moving forward by pressing W or Up Arrow)
    bool boosting;
    // Float value that will be applied to turn the player ship.
    float turnDirection;

    // Reference to player rigidbody. Grabbed on Awake.
    Rigidbody2D playerRB;

    private ObjectPool<Bullet> bulletPool;

    // Awake needs to:
    // > Get player Rigidbody component.
    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        SimplePoolFactory<Bullet> factory = new();
        bulletPool = factory.CreatePool(bulletPrefab, null, 10);
    }

    // Update needs to:
    // > Get Input
    // > Apply changes to boosting and turn direction using input
    // > Shoot when pressing shoot button.
    void Update()
    {
        boosting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        }
        else
        {
            turnDirection = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    // Fixed Update needs to:
    // > Use status of boosting bool and moveSpeed value to apply force to "up" direction.
    // > Use turnDirection value and turnSpeed value to apply torque to player rigidbody.
    void FixedUpdate()
    {
        if (boosting)
        {
            playerRB.AddForce(this.transform.up * moveSpeed);
        }
        if (turnDirection != 0.0f)
        {
            playerRB.AddTorque(turnDirection * turnSpeed);
        }
    }

    // Shoot needs to:
    // Instantiate bullet prefab at center of player with player's current rotation.
    // "Project" the bullet (fire it with the current direction the player is facing).
    void Shoot()
    {
        Bullet bullet = bulletPool.Get();
        bullet.PoolParent = bulletPool;
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;

        bullet.Project(this.transform.up);
    }

    // OnCollisionEnter2D needs to:
    // > Look if the collision was with an asteroid.
    // > If so, stop player movement (velocity and angular velocity), deactivate the object, and fire the HasDied event.
    void OnCollisionEnter2D(Collision2D collision)
    {
        playerRB.velocity = Vector3.zero;
        playerRB.angularVelocity = 0.0f;

        this.gameObject.SetActive(false);

        HasDied();
    }
}
