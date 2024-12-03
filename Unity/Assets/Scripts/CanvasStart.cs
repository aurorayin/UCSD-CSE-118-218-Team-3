using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeCanvas : MonoBehaviour
{
    public Transform playerCamera; // Reference to the player's camera
    public float distanceFromPlayer = 2.0f; // Distance to place the Canvas in front of the player

    void Start()
    {
        // Ensure the Canvas starts in front of the player
        if (playerCamera != null)
        {
            // Set the position of the Canvas
            transform.position = playerCamera.position + playerCamera.forward * distanceFromPlayer;

            // Make the Canvas face the player
            transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);
        }
        else
        {
            Debug.LogWarning("Player Camera is not assigned!");
        }
    }
}
