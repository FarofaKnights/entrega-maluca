using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player instance;

    public float dinheiro = 300;
    public List<Carga> cargaAtual = new List<Carga>();
    List<Carga> cargasCaidasProximas = new List<Carga>(), cargasCaidas = new List<Carga>();
    IState estadoAtual;

    public System.Action<IState> onStateChange;


    // temp
    public GameObject gizmos, subirPos;
    public float velocidade, velocidadeRotacao;
    public TriggerSubject cacambaTrigger;

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

    public void SetEncaixando(Carga[] cargasNovas) {
        SetState(new EncaixeState(this, cargasNovas, cargaAtual));
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
        SetState(new EncaixeState(this, cargas.ToArray(), cargaAtual));
    }

    public List<Carga> RemoverCargaDeEndereco(Endereco endereco) {
        List<Carga> cargasRemovidas = new List<Carga>();

        // Poderia ser substituido por um Where ?
        for (int i = cargaAtual.Count-1; i >= 0; i--) {
            Carga carga = cargaAtual[i];
            if (carga.destinatario == endereco) {
                Destroy(carga.cx.gameObject);
                carga.cx = null;
                cargasRemovidas.Add(carga);
                cargaAtual.Remove(carga);
            }
        }

        // Se o endereço onde a caixa supostamente devia ser entregue já foi concluido, não tem porque mante-la no cenário
        for (int i = cargasCaidas.Count-1; i >= 0; i--) {
            Carga carga = cargasCaidas[i];
            if (carga.destinatario == endereco) {
                if (carga.cx != null) {
                    Destroy(carga.cx.gameObject);
                    carga.cx = null;
                }
                
                cargasCaidas.Remove(carga);
            }
        }

        return cargasRemovidas;
    }

    public void RemoverCarga(Carga carga) {
        cargaAtual.Remove(carga);
        cargasCaidas.Add(carga);
    }

    public void ZerarCargas() {
        foreach (Carga carga in cargaAtual) {
            Destroy(carga.cx.gameObject);
            carga.cx = null;
        }

        foreach (Carga carga in cargasCaidas) {
            Destroy(carga.cx.gameObject);
            carga.cx = null;
        }

        cargaAtual = new List<Carga>();
        cargasCaidas = new List<Carga>();
    }

    public void AdicionarCargaProxima(Carga carga) {
        if (cargasCaidasProximas.Contains(carga)) return;
        cargasCaidasProximas.Add(carga);
    }

    public void RemoverCargaProxima(Carga carga) {
        if (!cargasCaidasProximas.Contains(carga)) return;
        cargasCaidasProximas.Remove(carga);
    }

    public Carga[] GetCargasProximas() {
        return cargasCaidasProximas.ToArray();
    }

    public void RecuperarCargasProximas() {
        SetEncaixando(GetCargasProximas());
        cargasCaidasProximas.Clear();
    }
    #endregion
}
