using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParkingSpot : MonoBehaviour
{
    public GameObject successMessageUI;  // UI with text + buttons
    public AudioClip parkingSound;       // Sound effect for parking success
    private AudioSource audioSource;
    private bool hasParked = false;

    private void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Make the audio ignore time scale pause
        audioSource.ignoreListenerPause = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasParked && collision.CompareTag("Player"))
        {
            hasParked = true;

            // Show UI
            if (successMessageUI != null)
                successMessageUI.SetActive(true);

            // Play sound
            if (parkingSound != null)
                audioSource.PlayOneShot(parkingSound);

            // Freeze time (after small delay to let sound play properly)
            StartCoroutine(PauseAfterSound());
            Debug.Log("Parked Successfully!");
        }
    }

    private IEnumerator PauseAfterSound()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 0f;
    }

    // Buttons will call these
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
