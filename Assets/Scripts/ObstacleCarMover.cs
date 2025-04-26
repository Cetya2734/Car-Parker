using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCarMover : MonoBehaviour
{
    public Transform targetPosition; // Set this in the inspector
    public float moveSpeed = 5f;      // Movement speed
    private bool shouldMove = false;  // Control moving state

    private void Update()
    {
        if (shouldMove && targetPosition != null)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // Check if arrived
            if (transform.position == targetPosition.position)
            {
                shouldMove = false; // Stop moving
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shouldMove = true;
        }
    }
}
