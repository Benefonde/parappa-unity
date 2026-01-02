using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.clip = rankMusic[1];
    }

    private void Update()
    {
        if (aud.time >= rankMusic[4].length && aud.clip == rankMusic[4])
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void ChangeMusic(int a)
    {
        if (a == 4)
        {
            aud.clip = rankMusic[a];
            aud.Play();
            return;
        }
        if (!aud.isPlaying)
        {
            return;
        }
        float tim = aud.time;
        aud.clip = rankMusic[a];
        aud.Play();
        aud.time = tim;
    }

    public AudioClip[] rankMusic; // 0 cool 1 good 2 bad 3 awful 4 fail
    public AudioSource aud;
}
