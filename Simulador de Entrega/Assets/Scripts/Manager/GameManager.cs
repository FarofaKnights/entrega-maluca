using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public MissaoObject[] missoesIniciais;

    public Missao visualizarMissaoAtual;

    public List<Missao> missoesDisponiveis = new List<Missao>();
    bool mostrandoMissoes = true;

    void Awake() {
        instance = this;
    }

    void Update() {
        // CHEAT CODE
        if (Input.GetKeyDown(KeyCode.P)) {
            StartDrag.sd.Confirm();
        }
    }

    public void CarregarMissaoInicial() {
        foreach (MissaoObject missaoObject in missoesIniciais) {
            Missao missao = missaoObject.Convert();
            AdicionarMissao(missao);
        }
    }

    public void VisualizarMissao(Missao missao) {
        visualizarMissaoAtual = missao;
    }

    public void AdicionarMissao(Missao missao) {
        missoesDisponiveis.Add(missao);

        if (mostrandoMissoes)
            missao.objetivoInicial.Iniciar();
    }

    public void RemoverMissao(Missao missao) {
        missoesDisponiveis.Remove(missao);

        if (mostrandoMissoes)
            missao.objetivoInicial.Interromper();
    }

    public void AlterarDisponibilidadeDeMissoes(bool disponiveis) {
        mostrandoMissoes = disponiveis;
        // Define se os chamados de missão estarao disponiveis para o jogador
        foreach (Missao missao in missoesDisponiveis) {
            if (mostrandoMissoes) missao.objetivoInicial.Iniciar();
            else missao.objetivoInicial.Interromper();
        }
    }
}
