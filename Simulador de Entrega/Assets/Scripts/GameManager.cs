using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public MissaoObject missaoObject;

    public Missao visualizarMissaoAtual;

    void Start() {
        instance = this;
    }

    public void CarregarMissaoInicial() {
        /*
        Missao novaMissao = Missao.GerarMissaoAleatoria();
        Missao novaMissao2 = Missao.GerarMissaoAleatoria();
        Player.instance.AdicionarMissao(novaMissao);
        Player.instance.AdicionarMissao(novaMissao2);
        
        Missao missaoMultiplos = Missao.GerarMissaoMultiplosPontos();
        Player.instance.AdicionarMissao(missaoMultiplos);
        */
        
        Missao missao = Missao.GerarMissao(missaoObject);
        Player.instance.AdicionarMissao(missao);
    }

    public void VisualizarMissao(Missao missao) {
        visualizarMissaoAtual = missao;
    }
}
