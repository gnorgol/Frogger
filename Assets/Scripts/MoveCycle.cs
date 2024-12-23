using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public Vector2 direction = Vector2.right;

    public float speed = 1.0f;
    public int size = 1;

    private Vector3 leftEdge;
    private Vector3 rightEdge;

    private void Start()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
    }
    private void Update()
    {
        // Check if the object is past the right edge of the screen
        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x)
        {
            transform.position = new Vector3(leftEdge.x - size, transform.position.y, transform.position.z);
        }
        // Check if the object is past the left edge of the screen
        else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            transform.position = new Vector3(rightEdge.x + size, transform.position.y, transform.position.z);
        }
        // Move the object
        else
        {
            transform.Translate(speed * Time.deltaTime * direction);
        }
    
    }
}
