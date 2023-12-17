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
    public AudioSource currentAudioSource, nextAnterior;
    public bool isGotico;
    private void Start()
    {
        instance = this;
        if(!isGotico)
        {
            int l = Random.Range(0, normalAudios.Length);
            currentAudioSource = normalAudios[l];
            currentPlaylist = normalAudios;
            Invoke("ChangeSong", currentAudioSource.clip.length - 3f);
        }

        else if (isGotico)
        {
            int l = Random.Range(0, goticoAudios.Length);
            currentAudioSource = goticoAudios[l];
            currentPlaylist = goticoAudios;
            Invoke("ChangeSong", currentAudioSource.clip.length - 3f);
        }

        currentAudioSource.Play();
    }
    public void ChangeSong()
    {
        int i = 0;
        do
        {
            i = Random.Range(0, currentPlaylist.Length);
        } while (currentAudioSource == currentPlaylist[i]);
        StartCoroutine(Fade(currentAudioSource, currentPlaylist[i]));
    }
    IEnumerator Fade(AudioSource currentAudio, AudioSource nextAudio)
    {
        float t = 0;
        float maxt = 2f;
        nextAudio.Play();
        if(nextAnterior != null)
        {
            nextAnterior.Stop();
        }
        while(t < maxt)
        {
            nextAudio.volume = Mathf.Lerp(0, 1, t/maxt);
            currentAudio.volume = Mathf.Lerp(1, 0, t/maxt);
            t += Time.deltaTime;
        }
        nextAnterior = nextAudio;
        currentAudio.Stop();
        currentAudio = nextAudio;
        Invoke("ChangeSong", currentAudioSource.clip.length - 5f);
        yield return null;
    }
}
