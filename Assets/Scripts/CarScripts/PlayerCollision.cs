using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public GameObject crashText;  // UI crash text
    public AudioClip crashSound; // Sound effect for collision
    private AudioSource audioSource;
    private bool hasCrashed = false;

    private void Start()
    {
        // Get the AudioSource component attached to the player
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCrashed && collision.collider.CompareTag("ObstacleCar"))
        {
            hasCrashed = true;

            // Freeze time
            Time.timeScale = 0f;

            // Show crash text immediately
            if (crashText != null)
                crashText.SetActive(true);

            // Play crash sound
            if (audioSource != null && crashSound != null)
            {
                float originalVolume = audioSource.volume;
                audioSource.volume = 1f; // Boost volume for crash
                audioSource.PlayOneShot(crashSound);
                audioSource.volume = originalVolume; // Reset volume after playing
            }

            // Start real-time delay coroutine for scene restart
            StartCoroutine(RestartAfterDelay());
        }
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f); // Delay before restarting

        // Reset time scale before restarting
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
