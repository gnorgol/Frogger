using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Frogger frogger;
    private Home[] homes;
    private int score;
    private int lives;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timeText;

    public GameObject gameOverPanel;

    private int time;

    private void Awake()
    {
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
    }
    private void Start()
    {
        NewGame();
    }
    public void NewGame()
    {
        gameOverPanel.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewLevel();
    }
    private void NewLevel()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        Respawn();
    }
    private void Respawn()
    {
        frogger.Respawn();

        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }
    private IEnumerator Timer(int time)
    {
        this.time = time;
        timeText.text = this.time.ToString();
        while (this.time > 0)
        {
            yield return new WaitForSeconds(1);
            this.time--;
            timeText.text = this.time.ToString();
        }
        frogger.Death();
    }
    private void SetScore(int value)
    {
        score = value;
        scoreText.text = score.ToString();
    }
    private void SetLives(int value)
    {
        lives = value;
        livesText.text = lives.ToString();
    }
    public void HomeHasBeenOccupied()
    {
        int bonusPoints = time * 20;
        SetScore(score + 50 + bonusPoints);

        frogger.gameObject.SetActive(false);
        if (AllHomesAreOccupied())
        {
            SetScore(score + 1000);
            Invoke(nameof(NewLevel), 1f);
        }
        else
        {
            Invoke(nameof(Respawn), 1f);
        }

    }
    public void AdvencedRow()
    {
        SetScore(score + 10);
    }
    public void Died()
    {
        SetLives(lives - 1);
        if (lives > 0)
        {
            Invoke(nameof(Respawn), 1f);
        }
        else
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
        frogger.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(CheckForPlayAgain());

    }
    private IEnumerator CheckForPlayAgain()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        NewGame();
    }

        private bool AllHomesAreOccupied()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            if (!homes[i].enabled)
            {
                return false;
            }
        }
        return true;
    }
}
