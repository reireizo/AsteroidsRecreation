using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosionEffect;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI finalScoreUI;
    public TextMeshProUGUI hiScoreUI;
    public GameObject GameOverUI;
    public Image[] livesUI;
    public int lives = 3;
    public int score = 0;
    public int hiScore = 0;
    public float respawnTime = 3.0f;
    public float invincibleTime = 3.0f;

    bool isGameOver;

    void OnEnable()
    {
        Player.HasDied += PlayerDied;
        Asteroid.HasBeenDestoryed += AsteroidDestroyed;
    }

    void OnDisable()
    {
        Player.HasDied -= PlayerDied;
        Asteroid.HasBeenDestoryed -= AsteroidDestroyed;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("hiScore"))
        {
            hiScore = PlayerPrefs.GetInt("hiScore");
        }
    }

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

    void Respawn()
    {
        //Debug.Log("I died!");
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer("Invincibility");
        player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollisions), invincibleTime);
    }

    void TurnOnCollisions()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    void GameOver()
    {
        isGameOver = true;
        scoreUI.enabled = false;
        GameOverUI.SetActive(true);
        SaveHiScore();
        UpdateFinalScoreUI();
        UpdateHiScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreUI.SetText(score.ToString());
    }
    void UpdateLivesUI(int newLives)
    {
        livesUI[newLives].enabled = false;
    }
    void UpdateFinalScoreUI()
    {
        finalScoreUI.SetText("Final Score: " + score.ToString());
    }
    void UpdateHiScoreUI()
    {
        hiScoreUI.SetText("Hi-Score: " + hiScore.ToString());
    }
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
