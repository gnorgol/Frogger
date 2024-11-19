using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frogger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    

    public Sprite idleSprite;
    public Sprite jumpSprite;
    public Sprite deathSprite;
    public Camera mainCamera;

    private Vector3 SpawnPosition;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SpawnPosition = transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Move(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            Move(Vector3.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            Move(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            Move(Vector3.right);
        }
    }
    private void Move(Vector3 direction)
    {
        Collider2D plateform = Physics2D.OverlapBox(transform.position + direction, Vector2.zero,0f, LayerMask.GetMask("Plateform"));
        Collider2D obstacle = Physics2D.OverlapBox(transform.position + direction, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));

        

        if (plateform != null) {
            transform.SetParent(plateform.transform);
        }
        else
        {
            transform.SetParent(null);
        }
        if (obstacle != null && plateform == null)
        {
            transform.position = transform.position + direction;
            Death();
        }
        else
        {
            StartCoroutine(MoveCoroutine(direction));
        }
    }

    private IEnumerator MoveCoroutine(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;
        float time = 0;
        float duration = 0.125f;

        spriteRenderer.sprite = jumpSprite;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = ClampPositionToCameraBounds(targetPosition);
        spriteRenderer.sprite = idleSprite;
    }
    private Vector3 ClampPositionToCameraBounds(Vector3 position)
    {
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.y = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);

        return position;
    }
    private void Death()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        spriteRenderer.sprite = deathSprite;
        enabled = false;

        Invoke(nameof(Respawn), 1f);
    }
    public void Respawn()
    {
        StopAllCoroutines();
        enabled = true;
        spriteRenderer.sprite = idleSprite;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.SetActive(true);
        transform.position = SpawnPosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && enabled && transform.parent == null)
        {
            Death();
        }

    }


}
