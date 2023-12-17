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
                if (MusicManager.instance.currentPlaylist != MusicManager.instance.goticoAudios)
                {
                    MusicManager.instance.currentPlaylist = MusicManager.instance.goticoAudios;
                    MusicManager.instance.CancelInvoke();
                    MusicManager.instance.StopAllCoroutines();
                    MusicManager.instance.ChangeSong();
                }
            }
            else if (thisTipo == Tipo.Normal)
            {
                if (MusicManager.instance.currentPlaylist != MusicManager.instance.normalAudios)
                {
                    MusicManager.instance.currentPlaylist = MusicManager.instance.normalAudios;
                    MusicManager.instance.CancelInvoke();
                    MusicManager.instance.StopAllCoroutines();
                    MusicManager.instance.ChangeSong();
                }
            }
            else if (thisTipo == Tipo.Praia)
            {
                if (MusicManager.instance.currentPlaylist != MusicManager.instance.praiaAudios)
                {
                    MusicManager.instance.currentPlaylist = MusicManager.instance.praiaAudios;
                    MusicManager.instance.CancelInvoke();
                    MusicManager.instance.StopAllCoroutines();
                    MusicManager.instance.ChangeSong();
                }
            }
            else if (thisTipo == Tipo.Parque)
            {
                if(MusicManager.instance.currentPlaylist != MusicManager.instance.parqueAudios)
                {
                    MusicManager.instance.currentPlaylist = MusicManager.instance.parqueAudios;
                    MusicManager.instance.CancelInvoke();
                    MusicManager.instance.StopAllCoroutines();
                    MusicManager.instance.ChangeSong();

                }
            }
        }
    }
}
