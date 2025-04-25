using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private void Awake()
    {
        // Make sure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Make it persist between scenes
        DontDestroyOnLoad(gameObject);

        // Optional: Ensure music keeps playing during Time.timeScale = 0
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.ignoreListenerPause = true;
    }
}
