using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioSource[] normalAudios;
    public AudioSource[] goticoAudios;
    public AudioSource[] praiaAudios;
    public AudioSource[] parqueAudios;

    public AudioSource[] currentPlaylist;
    public AudioSource currentAudioSource;
    public bool isGotico;
    private void Start()
    {
        instance = this;
        if(!isGotico)
        {
            int l = Random.Range(0, normalAudios.Length);
            currentAudioSource = normalAudios[l];
            currentPlaylist = normalAudios;
            Invoke("ChangeSong", currentAudioSource.clip.length - 1.25f);
        }

        else if (isGotico)
        {
            int l = Random.Range(0, goticoAudios.Length);
            currentAudioSource = goticoAudios[l];
            currentPlaylist = goticoAudios;
            Invoke("ChangeSong", currentAudioSource.clip.length - 1.25f);
        }

        currentAudioSource.Play();
    }
    public void ChangeSong()
    {
        int i = 0;
        do
        {
            i = Random.Range(0, currentPlaylist.Length);
        } while (currentPlaylist[i] == currentAudioSource);
        Fade(currentAudioSource, currentPlaylist[i]);
        Invoke("ChangeSong", currentAudioSource.clip.length - 1.25f);
    }
    IEnumerator Fade(AudioSource currentAudio, AudioSource nextAudio)
    {
        float timeToFade = 1.25f;
        float timeElapsed = 0;
        nextAudio.Play();
        nextAudio.volume = 0;
        while(timeElapsed < timeToFade)
        {
            currentAudio.volume = Mathf.Lerp(1, 0, timeElapsed/timeToFade);
            nextAudio.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
        }
        currentAudio = nextAudio;
        return null;
    }
}
