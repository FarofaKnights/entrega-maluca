using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusica : MonoBehaviour
{
    public bool gotico;

    private void OnTriggerEnter(Collider other)
    {
        if(gotico)
        {
            MusicManager.instance.currentPlaylist = MusicManager.instance.goticoAudios;
            MusicManager.instance.CancelInvoke();
            MusicManager.instance.isGotico = true;
            MusicManager.instance.ChangeSong();
        }
        else
        {
            MusicManager.instance.currentPlaylist = MusicManager.instance.normalAudios;
            MusicManager.instance.CancelInvoke();
            MusicManager.instance.isGotico = false;
            MusicManager.instance.ChangeSong();
        }
    }
}
