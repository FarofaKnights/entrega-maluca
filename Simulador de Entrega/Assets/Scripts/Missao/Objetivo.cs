using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objetivo: Iniciavel {
    public Endereco endereco;
    public List<Carga> cargas;
    public bool permiteReceber = false;

    [System.NonSerialized]
    public Conjunto pai;

    public Diretriz diretriz = null;


    public Objetivo(Endereco endereco, Conjunto pai = null) {
        this.endereco = endereco;
        this.pai = pai;
    }

    public Objetivo(Endereco endereco, List<Carga> cargas, Conjunto pai = null) {
        this.endereco = endereco;
        this.pai = pai;
        this.cargas = cargas;
    }

    // Chamada quando o Player entrou/saiu do trigger do endereço
    public virtual void HandleObjetivoTrigger(bool estado) {
        if ((cargas != null && cargas.Count > 0) || permiteReceber) // Se houver carga para receber, chama o Handle de recebimento
            UIController.instance.PlayerNaAreaDeAcao(this, estado);
        else
            Finalizar();
    }

    // Chamada quando o destino se torna ativo, ou seja, é o próximo destino do jogador
    public void Iniciar() {
        endereco.DefinirComoObjetivo(this);
        if (diretriz != null) diretriz.Iniciar();
    }

    public virtual void Concluir() {
        if (permiteReceber)
            Player.instance.RemoverCarga(endereco);
        
        if (cargas != null && cargas.Count > 0)
            Player.instance.AdicionarCarga(cargas);
        
        Finalizar();
    }

    // Deve ser chamada quando destino for concluido, seja chegando no destino, recebendo/entregando carga, etc
    public void Finalizar() {
        endereco.RemoverObjetivo();

        if (diretriz != null) diretriz.Finalizar();
        if (pai != null) pai.ObjetivoConcluido(this);
    }

    // Deve ser chamada caso a missão seja interrompida
    public void Interromper() {
        endereco.RemoverObjetivo();

        if (diretriz != null) diretriz.Interromper();
        if (pai != null) pai.Interromper();
    }
}

public class ObjetivoInicial : Objetivo {
    public Missao missao;

    public ObjetivoInicial(Endereco endereco, Missao missao = null) : base(endereco, null) {
        this.missao = missao;
    }

    public ObjetivoInicial(Endereco endereco, List<Carga> cargas, Missao missao = null) : base(endereco, null) {
        this.missao = missao;
        this.cargas = cargas;
    }

    public override void HandleObjetivoTrigger(bool estado) {
        UIController.instance.PlayerNaAreaDeIniciarMissao(this, estado);
    }

    // Chamada ao clicar no botão de iniciar missão
    public override void Concluir() {
        missao.Iniciar();

        if (cargas != null && cargas.Count > 0) 
            Player.instance.AdicionarCarga(cargas);
        
        if (diretriz != null) diretriz.Finalizar();
    }
}