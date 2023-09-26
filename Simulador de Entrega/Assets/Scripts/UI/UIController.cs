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

    void Awake() {
        instance = this;
        HUD = transform.GetComponentInChildren<HUDController>(true);
        encaixe = transform.GetComponentInChildren<EncaixeUIController>(true);
        pause = transform.GetComponentInChildren<PauseUIController>(true);
        oficina = transform.GetComponentInChildren<OficinaUI>(true);
    }
    
}
