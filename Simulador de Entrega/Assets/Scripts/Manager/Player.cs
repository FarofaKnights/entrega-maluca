using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player instance;

    public float dinheiro = 300;
    public List<Carga> cargaAtual = new List<Carga>();
    IState estadoAtual;

    public System.Action<IState> onStateChange;


    // temp
    public GameObject gizmos, subirPos;
    public float velocidade, velocidadeRotacao;
    public Trigger cacambaTrigger;

    public GameObject [] cameras;
    public Transform[] pontosCaixa;


    void Awake() {
        instance = this;
    }

    void Start() {
        SetState(new DirigindoState(this));
    }

    public void SetState(IState estado) {
        estadoAtual?.Exit();
        estadoAtual = estado;
        estadoAtual.Enter();

        onStateChange?.Invoke(estadoAtual);
    }

    public IState GetState() {
        return estadoAtual;
    }

    void FixedUpdate() {
        estadoAtual?.Execute(Time.fixedDeltaTime);
    }

    public void SetDirigindo() {
        SetState(new DirigindoState(this));
    }

    public void SetEncaixando(Carga[] cargas) {
        SetState(new EncaixeState(this, cargas));
    }

    #region SistemaDeDinheiro

    public void AdicionarDinheiro(float valor) {
        dinheiro += valor;

        UIController.HUD.AtualizarDinheiro();
    }

    public bool RemoverDinheiro(float valor) {
        if (dinheiro >= valor) {
            dinheiro -= valor;
            UIController.HUD.AtualizarDinheiro();

            return true;
        }

        return false;
    }

    public float GetDinheiro() {
        return dinheiro;
    }

    #endregion

    #region SistemaDeCargas
    public void AdicionarCarga(List<Carga> cargas) {
        cargaAtual.AddRange(cargas);

        SetState(new EncaixeState(this, cargas.ToArray()));
    }

    public List<Carga> RemoverCarga(Endereco endereco) {
        List<Carga> cargasRemovidas = new List<Carga>();

        // Poderia ser substituido por um Where ?
        for (int i = cargaAtual.Count-1; i >= 0; i--) {
            Carga carga = cargaAtual[i];
            if (carga.destinatario == endereco) {
                carga.cx.Destruir();
                carga.cx = null;
                cargasRemovidas.Add(carga);
                cargaAtual.Remove(carga);
            }
        }

        return cargasRemovidas;
    }

    public void ZerarCargas() {
        foreach (Carga carga in cargaAtual) {
            carga.cx.Destruir();
            carga.cx = null;
        }

        cargaAtual = new List<Carga>();
    }

    #endregion
}
