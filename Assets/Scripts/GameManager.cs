using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Reference to the player object.
    public Player player;
    // Reference to the explosion particle system object.
    public ParticleSystem explosionEffect;
    // Reference to the Score UI object.
    public TextMeshProUGUI scoreUI;
    // Reference to the Final Score UI object.
    public TextMeshProUGUI finalScoreUI;
    // Reference to the Hi-Score UI object.
    public TextMeshProUGUI hiScoreUI;
    // Reference to the Game Over UI objects parent.
    public GameObject GameOverUI;
    // Reference to the Lives Icon UI images.
    public Image[] livesUI;

    // Int value representing the number of lives the player has.
    public int lives = 3;
    // Int value representing the player's current score.
    public int score = 0;
    // Int value representing the player's Hi-Score.
    public int hiScore = 0;
    // Float value representing how long it takes for the player to respawn after dying.
    public float respawnTime = 3.0f;
    // Float value representing how long the player remains invincible after respawning.
    public float invincibleTime = 3.0f;

    // Bool value representing if the game is in the Game Over state.
    bool isGameOver;

    // OnEnable needs to:
    // > Subscribe to the player's HasDied action, to call PlayerDied.
    // > Subscribe to the asteroids' HasBeenDestroyed action, to call AsteroidDestroyed.
    void OnEnable()
    {
        Player.HasDied += PlayerDied;
        Asteroid.HasBeenDestoryed += AsteroidDestroyed;
    }

    // OnEnable needs to:
    // > Unsubscribe to the player's HasDied action.
    // > Subscribe to the asteroids' HasBeenDestroyed action.
    void OnDisable()
    {
        Player.HasDied -= PlayerDied;
        Asteroid.HasBeenDestoryed -= AsteroidDestroyed;
    }

    // Start needs to:
    // > Get hiScore from PlayerPrefs if it is present.
    void Start()
    {
        if (PlayerPrefs.HasKey("hiScore"))
        {
            hiScore = PlayerPrefs.GetInt("hiScore");
        }
    }

    // Update needs to:
    // > Get input when the game is over to reset the game.
    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        
    }

    // AsteroidDestroyed needs to:
    // > Pass in the asteroid being destroyed.
    // > Move and call the explosion effect object to play the explosion where the asteroid was destroyed.
    // > Award points based on Asteroid size.
    //   > If between min-0.75: 100 points (hard to hit)
    //   > If between 0.75-1.2: 50 points (about average)
    //   > if between 1.2-max: 25 points (easy target)
    // > Update the score UI with the new score.
    public void AsteroidDestroyed(Asteroid asteroid)
    {
        explosionEffect.transform.position = asteroid.transform.position;
        explosionEffect.Play();

        if (asteroid.size < 0.75f)
        {
            score += 100;
        }
        else if (asteroid.size < 1.2f)
        {
            score += 50;
        }
        else
        {
            score += 25;
        }

        UpdateScoreUI();
    }

    // PlayerDied needs to:
    // > Move and call the explosion effect to play where the player was.
    // > Decrement the lives count.
    // > Update the Lives UI using the new lives count.
    // > When lives are at 0 or less, call the Game Over function.
    // > If not, respawn the player after respawnTime.
    public void PlayerDied()
    {
        explosionEffect.transform.position = player.transform.position;
        explosionEffect.Play();
        lives--;
        UpdateLivesUI(lives);

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    // Respawn needs to:
    // > Put the player back at the origin.
    // > Put the player on the Invincibility layer (to not die immediately if you respawn on an asteroid).
    // > Re-activate the player object.
    // > Put the player back on the normal Player layer after invincibileTime.
    void Respawn()
    {
        //Debug.Log("I died!");
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer("Invincibility");
        player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollisions), invincibleTime);
    }

    // TurnOnCollisions needs to:
    // > Put the player back on the normal Player layer.
    void TurnOnCollisions()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    // GameOver needs to:
    // > Activate the isGameOver bool.
    // > Hide the normal Score UI.
    // > Activate the Game Over UI.
    // > Save the Hi-Score.
    // > Update the Final Score UI.
    // > Update the Hi-Score UI.
    void GameOver()
    {
        isGameOver = true;
        scoreUI.enabled = false;
        GameOverUI.SetActive(true);
        SaveHiScore();
        UpdateFinalScoreUI();
        UpdateHiScoreUI();
    }

    // UpdateScoreUI needs to:
    // > Update the score UI text with the current score.
    void UpdateScoreUI()
    {
        scoreUI.SetText(score.ToString());
    }

    // UpdateLivesUI needs to:
    // > Pass in the new lives count.
    // > Use that new count to access the index of the icon that needs to be disabled, and disable it.
    void UpdateLivesUI(int newLives)
    {
        livesUI[newLives].enabled = false;
    }

    // UpdateFinalScoreUI needs to:
    // > Update the final score UI with the current score.
    void UpdateFinalScoreUI()
    {
        finalScoreUI.SetText("Final Score: " + score.ToString());
    }

    // UpdateHiScoreUI needs to:
    // > Update the hi-score UI with the current hi-score.
    void UpdateHiScoreUI()
    {
        hiScoreUI.SetText("Hi-Score: " + hiScore.ToString());
    }

    // SaveHiScore needs to:
    // > Check if score is greater than hi-score.
    // > If so:
    //   > Set Hi-Score to be current score.
    //   > Save Hi-Score to PlayerPrefs under hiScore.
    void SaveHiScore()
    {
        if (score > hiScore)
        {
            hiScore = score;
            PlayerPrefs.SetInt("hiScore", hiScore);
            PlayerPrefs.Save();
        }
    }
}
