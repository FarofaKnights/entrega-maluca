using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public static Player instance;

    float dinheiro;
    public List<Carga> cargaAtual = new List<Carga>(); // Sistema temporario

    [System.NonSerialized]
    public Missao missaoAtual = null;
    

    public List<Diretriz> diretrizes = new List<Diretriz>();

    private void Awake()
    {
        instance = this;
        
    }
    void Start() {

        foreach (KeyValuePair<string, Endereco> entry in Endereco.ListaEnderecos) {
            entry.Value.gameObject.SetActive(false);
        }

        GameManager.instance.CarregarMissaoInicial();
    }

    #region SistemaDeMissao

    // Chamada pela propria missao ao ser iniciada
    public void ComecarMissao(Missao missao) {
        if (missaoAtual != null) {
            missaoAtual.Interromper();
        }
        foreach (Carga carga in cargaAtual)
        {
            carga.cx.Remover();
        }
        cargaAtual = new List<Carga>();
        missaoAtual = missao;

        GameManager.instance.VisualizarMissao(missaoAtual);
        GameManager.instance.AlterarDisponibilidadeDeMissoes(false);
    }

    public void InterromperMissao() {
        if (missaoAtual != null) {
            missaoAtual.Interromper();
        }

        missaoAtual = null;
        foreach(Carga carga in cargaAtual)
        {
            carga.cx.Remover();
        }
        cargaAtual = new List<Carga>();

        GameManager.instance.AlterarDisponibilidadeDeMissoes(true);
    }

    // Chamada pela propria missao ao ser finalizada
    public void FinalizarMissao() {
        GameManager.instance.RemoverMissao(missaoAtual);

        missaoAtual = null;
        foreach (Carga carga in cargaAtual)
        {
            carga.cx.Remover();
        }
        cargaAtual = new List<Carga>();

        GameManager.instance.AlterarDisponibilidadeDeMissoes(true);
    }
    #endregion


    #region SistemaDeCarga
    public void AdicionarCarga(List<Carga> cargas) {
        cargaAtual.AddRange(cargas);
        StartDrag.sd.changeCass();
        // StartDrag.sd.Confirm();
    }



    public List<Carga> RemoverCarga(Endereco endereco) {
        List<Carga> cargas = new List<Carga>();

        // Poderia ser substituido por um Where ?
        for (int i = cargaAtual.Count-1; i >= 0; i--) {
            Carga carga = cargaAtual[i];
            if (carga.destinatario == endereco) {
                carga.cx.Remover();
                cargas.Add(carga);
                cargaAtual.Remove(carga);
            }
        }

        return cargas;
    }
    #endregion

    #region SistemaDeDinheiro

    public void AdicionarDinheiro(float valor) {
        dinheiro += valor;
    }

    public bool RemoverDinheiro(float valor) {
        if (dinheiro >= valor) {
            dinheiro -= valor;
            return true;
        }

        return false;
    }

    public float GetDinheiro() {
        return dinheiro;
    }

    #endregion
}
