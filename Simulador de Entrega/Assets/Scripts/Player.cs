using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public float dinheiro;
    public Missao missaoAtual;
    public List<DestinoComecar> missoesDisponiveis = new List<DestinoComecar>();

    public List<Carga> cargaAtual = new List<Carga>(); // Sistema temporário enquanto sistema de armazenamento não foi feito

    void Start() {
        instance = this;

        foreach (KeyValuePair<string, Endereco> entry in Endereco.ListaEnderecos) {
            entry.Value.gameObject.SetActive(false);
        }

        Missao novaMissao = Missao.GerarMissaoAleatoria();
        Missao novaMissao2 = Missao.GerarMissaoAleatoria();
        Player.instance.AdicionarMissao(novaMissao);
        Player.instance.AdicionarMissao(novaMissao2);
    }

    // É chamada pela própria missão ao começar
    public void ComecarMissao(Missao missao) {
        if (missaoAtual != null) {
            missaoAtual.Interromper();
        }

        cargaAtual = new List<Carga>();
        missaoAtual = missao;

        foreach (DestinoComecar destinoMissao in missoesDisponiveis) {
            destinoMissao.Interromper();
        }
    }

    public void InterromperMissao() {
        if (missaoAtual != null) {
            missaoAtual.Interromper();
        }

        missaoAtual = null;
        cargaAtual = new List<Carga>();


        foreach (DestinoComecar destinoMissao in missoesDisponiveis) {
            destinoMissao.Iniciar(destinoMissao.missao);
        }
    }

    public void FinalizarMissao() {
        missaoAtual = null;
        cargaAtual = new List<Carga>();


        foreach (DestinoComecar destinoMissao in missoesDisponiveis)
        {
            destinoMissao.Iniciar(destinoMissao.missao);
        }
    }

    public void AdicionarMissao(Missao missao) {
        missoesDisponiveis.Add(missao.destinoComecar);
        
        if (this.missaoAtual == null) {
            missao.DefinirDisponibilidade(true);
        } 
    }

    public void AdicionarCarga(List<Carga> cargas) {
        foreach (Carga carga in cargas) {
            cargaAtual.Add(carga);
        }
    }

    public List<Carga> RemoverCarga(Endereco endereco) {
        List<Carga> cargas = new List<Carga>();

        // Poderia ser substituido por um Where ?
        foreach (Carga carga in cargaAtual) {
            if (carga.destinatario == endereco) {
                cargas.Add(carga);
                cargaAtual.Remove(carga);
            }
        }

        return cargas;
    }
}
