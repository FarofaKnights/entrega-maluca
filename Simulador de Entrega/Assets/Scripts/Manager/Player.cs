using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player instance;

    public float dinheiro = 300;
    public List<Carga> cargaAtual = new List<Carga>();
    List<Carga> cargasCaidasProximas = new List<Carga>(), cargasCaidas = new List<Carga>();
    public float areaRecuperarCaixa = 15f;
    IState estadoAtual;

    Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();
    }

    public void SetState(IState estado) {
        estadoAtual?.Exit();
        estadoAtual = estado;
        estadoAtual.Enter();

        onStateChange?.Invoke(estadoAtual);
    }

    public PlayerData GetPlayerData()
    {
        PlayerData pd = new PlayerData(transform.position, transform.rotation.eulerAngles, dinheiro);
        return pd;
    }

    public void SetPlayerData(PlayerData p)
    {
        transform.position = p.position;
        transform.rotation = Quaternion.Euler(p.rotation);
        dinheiro = p.dinheiro;
        rb.velocity = Vector3.zero;
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

    public void PararCompletamente() {
        GetComponent<Rigidbody>().isKinematic = true;
    }
    public void Retomar() {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    #region SistemaDeDinheiro

    public void AdicionarDinheiro(float valor) {
        dinheiro += valor;

        UIController.dinheiro.AtualizarDinheiro();
    }

    public bool RemoverDinheiro(float valor) {
        if (dinheiro >= valor) {
            dinheiro -= valor;
            UIController.dinheiro.AtualizarDinheiro();

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
                
                // cargasCaidas.Remove(carga);
            }
        }

        return cargasRemovidas;
    }

    public void RemoverCarga(Carga carga) {
        cargaAtual.Remove(carga);

        if (!cargasCaidas.Contains(carga))
            cargasCaidas.Add(carga);
    }

    public void ZerarCargas() {
        foreach (Carga carga in cargaAtual) {
            Destroy(carga.cx.gameObject);
            carga.cx = null;
        }

        foreach (Carga carga in cargasCaidas) {
            if (carga.cx == null) continue;
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
        // Pega caixas na região do player e que não estão como caixas caídas por algum motivo
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, areaRecuperarCaixa, Vector3.up, 0f, GameManager.instance.caixaLayer);
        foreach (RaycastHit hit in hits) {
            Caixa caixa = hit.collider.GetComponent<Caixa>();
            if (caixa == null) continue;
            if (caixa.GetState() is CaixaCaidaState) {
                Carga carga = caixa.carga;
                if (!cargasCaidasProximas.Contains(carga))
                    cargasCaidasProximas.Add(carga);
            }
        }

        return cargasCaidasProximas.ToArray();
    }

    public List<Carga> GetCargasCaidas() {
        return cargasCaidas;
    }

    public void CargaDestruida(Carga carga) {
        if (cargaAtual.Contains(carga)) {
            cargaAtual.Remove(carga);
            if (!cargasCaidas.Contains(carga))
                cargasCaidas.Add(carga);
        }

       ChecarSeCargasDestruidas(carga);
    }

    void ChecarSeCargasDestruidas(Carga carga) {
        if (cargaAtual.Contains(carga)) {
            Debug.Log("Não falha: caixa ainda tá com o player");
            return;
        }

        // Se uma caixa quebra checa se todas as caixas com o mesmo remetente foram destruídas também
        bool todasCargasDestruidas = true;
        foreach (Carga cargaAtual in cargaAtual) {
            if (cargaAtual.destinatario == carga.destinatario) {
                Debug.Log("Não falha: ainda tem caixa com o mesmo remetente");
                Debug.Log(cargaAtual.nome);
                todasCargasDestruidas = false;
                break;
            }
        }

        foreach (Carga cargaAtual in cargasCaidas) {
            if (cargaAtual.destinatario == carga.destinatario && cargaAtual.cx != null) {
                Debug.Log("Não falha: ainda tem caixa com o mesmo remetente (caída)");
                Debug.Log(cargaAtual.nome);
                todasCargasDestruidas = false;
                break;
            }
        }
        if (!todasCargasDestruidas) {
            return;
        }

        // Se sim, e se o endereço for um objetivo
        Objetivo objetivoDoEndereco = null;

        foreach (Objetivo objetivo in MissaoManager.instance.GetObjetivosAtivos()) {
            if (objetivo.endereco == carga.destinatario) {
                objetivoDoEndereco = objetivo;
                break;
            }
        }

        string motivo = "Eita! Você destruiu todas as caixas!";

        if (objetivoDoEndereco == null) {
            // Caso não seja um objetivo, falha mesmo assim
            Debug.Log("Falha destruição: não acho objetivo com o endereço");
            MissaoManager.instance.FalharMissao(motivo);
            return;
        }

        // checa se há mais objetivo. 
        int objetivosAtivos = MissaoManager.instance.GetObjetivosAtivos().Length;
        if (objetivosAtivos > 1) {
            // Se sim, conclui o objetivo
            Debug.Log("Falha destruição: conclui um dos multiplos");
            objetivoDoEndereco.Concluir();
        } else {
            // Se não, falha a missão
            Debug.Log("Falha destruição: unico objetivo");
            MissaoManager.instance.FalharMissao(motivo);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaRecuperarCaixa);
    }

    public void RecuperarCargasProximas() {
        SetEncaixando(GetCargasProximas());
        cargasCaidasProximas.Clear();

        foreach (Carga cargaCaida in cargasCaidas) {
            if (cargaCaida.cx == null) continue;
            if (cargaAtual.Contains(cargaCaida)) {
                cargasCaidas.Remove(cargaCaida);
            }
        }
    }
    #endregion
}
