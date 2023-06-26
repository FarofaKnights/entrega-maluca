using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscolherJogoTutorial : MonoBehaviour {
    
    public void HandleClique() {
        if (PlayerPrefs.HasKey("TerminouTutorial")) {
            // if it has, load the scene "EscolherJogo"
            SceneManager.LoadScene("Jogo");
        } else {
            // if it doesn't, load the scene "Tutorial"
            SceneManager.LoadScene("Tutorial");
        }
    }
}
