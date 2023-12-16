using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusica : MonoBehaviour
{
    public enum Tipo {Normal, Gotico, Praia, Parque };
    public Tipo thisTipo;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (thisTipo == Tipo.Gotico)
            {
                MusicManager.instance.currentPlaylist = MusicManager.instance.goticoAudios;
                MusicManager.instance.CancelInvoke();
                MusicManager.instance.ChangeSong();
            }
            else if (thisTipo == Tipo.Normal)
            {
                MusicManager.instance.currentPlaylist = MusicManager.instance.normalAudios;
                MusicManager.instance.CancelInvoke();
                MusicManager.instance.ChangeSong();
            }
            else if (thisTipo == Tipo.Praia)
            {
                MusicManager.instance.currentPlaylist = MusicManager.instance.praiaAudios;
                MusicManager.instance.CancelInvoke();
                MusicManager.instance.ChangeSong();
            }
            else if (thisTipo == Tipo.Parque)
            {
                MusicManager.instance.currentPlaylist = MusicManager.instance.parqueAudios;
                MusicManager.instance.CancelInvoke();
                MusicManager.instance.ChangeSong();
            }
        }
    }
}
