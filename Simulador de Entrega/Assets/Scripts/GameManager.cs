using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public MissaoObject[] missoesIniciais;

    public Missao visualizarMissaoAtual;

    void Awake() {
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
        
        foreach (MissaoObject missaoObject in missoesIniciais) {
            Missao missao = Missao.GerarMissao(missaoObject);
            Player.instance.AdicionarMissao(missao);
        }
    }

    public void VisualizarMissao(Missao missao) {
        visualizarMissaoAtual = missao;
    }
}
