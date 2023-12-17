using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;
    public static HUDController HUD;
    public static EncaixeUIController encaixe;
    public static PauseUIController pause;
    public static OficinaUI oficina;
    public static DiretrizUI diretriz;
    public static DinheiroUI dinheiro;
    public static CutsceneUI cutscene;

    void Awake() {
        instance = this;
        HUD = transform.GetComponentInChildren<HUDController>(true);
        encaixe = transform.GetComponentInChildren<EncaixeUIController>(true);
        pause = transform.GetComponentInChildren<PauseUIController>(true);
        oficina = transform.GetComponentInChildren<OficinaUI>(true);
        diretriz = transform.GetComponentInChildren<DiretrizUI>(true);
        dinheiro = transform.GetComponentInChildren<DinheiroUI>(true);
        cutscene = transform.GetComponentInChildren<CutsceneUI>(true);
    }

    public void ShowCutscene(PersonagemObject personagem, FalaPersonagens fala, System.Action next) {
        cutscene.Mostrar();
        cutscene.ShowFala(personagem, fala, next);
    }

    public void ShowCutscene(Cutscene cutscene, System.Action next) {
        UIController.cutscene.Mostrar();
        UIController.cutscene.ShowCutscene(cutscene, next);
    }

    public void ShowCheatsIcon() {
        transform.GetChild(0).Find("CheatStatus").gameObject.SetActive(true);
    }

    public void HideCheatsIcon() {
        transform.GetChild(0).Find("CheatStatus").gameObject.SetActive(false);
    }


    public void ToggleDebug() {
        GameObject debug = transform.GetChild(0).Find("Debug").gameObject;
        debug.SetActive(!debug.activeSelf);
    }
    
}
