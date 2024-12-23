using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject frog;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enabled = true;

            FindObjectOfType<GameManager>().HomeHasBeenOccupied();

        }
    }
    private void OnEnable()
    {
        frog.SetActive(true);
    }
    private void OnDisable()
    {
        frog.SetActive(false);
    }

}
