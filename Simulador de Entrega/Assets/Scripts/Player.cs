using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public static Player instance;

    public float dinheiro;
    public List<Carga> cargaAtual = new List<Carga>(); // Sistema temporario

    [System.NonSerialized]
    public Missao missaoAtual = null;
    public List<Missao> missoesDisponiveis = new List<Missao>();


    void Start() {
        instance = this;

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

        cargaAtual = new List<Carga>();
        missaoAtual = missao;

        GameManager.instance.VisualizarMissao(missaoAtual);
        AlterarDisponibilidadeDeMissoes(false);
    }

    public void InterromperMissao() {
        if (missaoAtual != null) {
            missaoAtual.Interromper();
        }

        missaoAtual = null;
        cargaAtual = new List<Carga>();

        AlterarDisponibilidadeDeMissoes(true);
    }

    // Chamada pela propria missao ao ser finalizada
    public void FinalizarMissao() {
        RemoverMissao(missaoAtual);

        missaoAtual = null;
        cargaAtual = new List<Carga>();

        AlterarDisponibilidadeDeMissoes(true);
    }

    public void AdicionarMissao(Missao missao) {
        missoesDisponiveis.Add(missao);

        if (missaoAtual == null)
            missao.destinoComecar.Iniciar();
    }

    public void RemoverMissao(Missao missao) {
        missoesDisponiveis.Remove(missao);
    }

    void AlterarDisponibilidadeDeMissoes(bool disponiveis) {
        // Define se os chamados de missão estarao disponiveis para o jogador
        foreach (Missao missao in missoesDisponiveis) {
            if (disponiveis) missao.destinoComecar.Iniciar();
            else missao.destinoComecar.Interromper();
        }
    }

    #endregion


    #region SistemaDeCarga
    public void AdicionarCarga(List<Carga> cargas) {
        cargaAtual.AddRange(cargas);
    }

    public List<Carga> RemoverCarga(Endereco endereco) {
        List<Carga> cargas = new List<Carga>();

        // Poderia ser substituido por um Where ?
        for (int i = cargaAtual.Count-1; i >= 0; i--) {
            Carga carga = cargaAtual[i];
            if (carga.destinatario == endereco) {
                cargas.Add(carga);
                cargaAtual.Remove(carga);
            }
        }

        return cargas;
    }
    #endregion
}
