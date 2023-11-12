using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Missao: Iniciavel {
    public MissaoObject info;
    public StatusMissao melhorStatus = null;

    ObjetivoInicial objetivoInicial;
    MissaoIterator iterator;
    List<Carga> cargasEntregues = new List<Carga>();
    bool concluida = false;

    public Missao(MissaoObject info) {
        this.info = info;

        objetivoInicial = new ObjetivoInicial(info.objetivoInicial.Convert(), this);
    }

    public void Iniciar() {
        if (iterator != null) return;
        cargasEntregues.Clear();

        iterator = new MissaoIterator(info.conjuntos);
        iterator.Next();
        
        concluida = false;
    }

    public void Parar() {
        iterator.Reset();
        iterator = null;
        cargasEntregues.Clear();
    }

    public void Next() {
        if (iterator == null) return;

        Objetivo[] proximosObjetivos = iterator.Next();

        if (proximosObjetivos == null) {
            Concluir();
            return;
        }
    }

    void Concluir() {
        iterator.Reset();
        iterator = null;

        concluida = true;
        CalculaStatus();
        MissaoManager.instance.HandleMissaoConcluida(this);
        cargasEntregues.Clear();
    }

    void CalculaStatus() {
        StatusMissao status = new StatusMissao(this);
        float dinheiro = status.dinheiro;

        if (melhorStatus != null) {
            if (melhorStatus.dinheiro < dinheiro) {
                dinheiro -= melhorStatus.dinheiro;
                melhorStatus = status;
            }
            else dinheiro = 0;
        } else {
            melhorStatus = status;
        }

        Player.instance.AdicionarDinheiro(dinheiro);
        UIController.HUD.ChamaVitoria(this, status);
    }


    public void ShowObjetivoInicial(bool show) {
        if (show) objetivoInicial.Iniciar();
        else objetivoInicial.Parar();
    }

    public bool IsIniciada() { return iterator == null; }
    public bool IsConcluida() { return concluida; }
    public Objetivo GetObjetivoInicial() { return objetivoInicial; }


    #region Cargas

    public void AddCargaEntregue(Carga carga) {
        cargasEntregues.Add(carga);
    }

    //NOTA: não devia ser usado, quando ocorrer refatoração, remover
    public void RemoveCargaEntregue(Carga carga) {
        cargasEntregues.Remove(carga);
    }

    public Carga[] GetCargasEntregues() {
        return cargasEntregues.ToArray();
    }
    
    #endregion
}
