using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    public AudioClip startupClip;
    public AudioClip idleClip;
    public AudioClip lowOnClip;
    public AudioClip lowOffClip;

    private AudioSource audioSource;
    private bool isMoving;
    private bool wasMoving;
    private bool hasStarted = false;
    private Coroutine fadeCoroutine;
    private Coroutine transitionCoroutine;

    public float moveThreshold = 0.1f;
    public float fadeDuration = 0.5f; // smooth fades

    private float moveInput;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f; // start muted
        PlayStartupSound();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        isMoving = Mathf.Abs(moveInput) > moveThreshold;

        if (!hasStarted) return;

        HandleSound();
    }

    void HandleSound()
    {
        if (isMoving && !wasMoving)
        {
            PlayLowOn();
        }
        else if (!isMoving && wasMoving)
        {
            PlayLowOff();
        }
        else if (!isMoving && !audioSource.isPlaying && audioSource.clip != idleClip)
        {
            PlayIdle();
        }

        wasMoving = isMoving;
    }

    void PlayStartupSound()
    {
        PlaySound(startupClip, false);
        StartCoroutine(StartupDelay());
    }

    IEnumerator StartupDelay()
    {
        yield return new WaitForSeconds(2f);
        hasStarted = true;
        PlayIdle();
    }

    void PlayIdle()
    {
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(PlayAfterCurrent(idleClip, true));
    }

    void PlayLowOn()
    {
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(PlayAfterCurrent(lowOnClip, false));
    }

    void PlayLowOff()
    {
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(PlayAfterCurrent(lowOffClip, false));
    }

    IEnumerator PlayAfterCurrent(AudioClip clipToPlay, bool loop)
    {
        if (audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeOut(fadeDuration));
        }

        PlaySound(clipToPlay, loop);
        yield return StartCoroutine(FadeIn(fadeDuration));

        if (!loop)
        {
            yield return new WaitForSeconds(clipToPlay.length);

            if (!isMoving)
            {
                PlayIdle();
            }
            else
            {
                PlayLowOn(); // restart LowOn if still moving
            }
        }
    }

    void PlaySound(AudioClip clip, bool loop)
    {
        audioSource.volume = 0.1f; // Lowered volume for all engine sounds
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    IEnumerator FadeOut(float duration)
    {
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    IEnumerator FadeIn(float duration)
    {
        float t = 0f;
        float targetVolume = 0.1f;
        audioSource.volume = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
