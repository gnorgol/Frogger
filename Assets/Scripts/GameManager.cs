using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Frogger frogger;
    private Home[] homes;
    private int score;
    private int lives;

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
    private void NewGame()
    {
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
        while (this.time > 0)
        {
            yield return new WaitForSeconds(1);
            this.time--;
        }
        frogger.Death();
    }
    private void SetScore(int value)
    {
        score = value;
    }
    private void SetLives(int value)
    {
        lives = value;
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
