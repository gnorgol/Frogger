using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Frogger frogger;
    private Home[] homes;
    private int score;
    private int lives;

    private void Awake()
    {
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
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
        NewRound();
    }
    private void NewRound()
    {
        frogger.Respawn();
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
        frogger.gameObject.SetActive(false);
        if (AllHomesAreOccupied())
        {
            Invoke(nameof(NewLevel), 1f);
        }
        else
        {
            Invoke(nameof(NewRound), 1f);
        }

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
