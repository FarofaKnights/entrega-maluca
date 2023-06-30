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

    bool ativo = false;


    public Objetivo(Endereco endereco, Conjunto pai = null) {
        this.endereco = endereco;
        this.pai = pai;
    }

    public Objetivo(Endereco endereco, List<Carga> cargas, bool permiteReceber, Diretriz diretriz = null, Conjunto pai = null) {
        this.endereco = endereco;
        this.pai = pai;
        this.cargas = cargas;
        this.permiteReceber = permiteReceber;
        this.diretriz = diretriz;
    }

    // Chamada quando o Player entrou/saiu do trigger do endereço
    public virtual void HandleObjetivoTrigger(bool estado) {
        if ((cargas != null && cargas.Count > 0) || permiteReceber) // Se houver carga para receber, chama o Handle de recebimento
            UIController.instance.PlayerNaAreaDeAcao(this, estado);
        else
            Finalizar();
    }

    // Chamada quando o destino se torna ativo, ou seja, é o próximo destino do jogador
    public virtual void Iniciar() {
        if (ativo) return;

        ativo = true;
        endereco.DefinirComoObjetivo(this);
        MissaoManager.instance.AddObjetivoAtivo(this);

        if (diretriz != null) diretriz.Iniciar();
    }

    public virtual void Concluir() {
        if (permiteReceber){
            foreach (Carga carga in MissaoManager.instance.RemoverCarga(endereco)) {
                pai.missao.CargaEntregue(carga);
            }
        }
            
        
        if (cargas != null && cargas.Count > 0)
            MissaoManager.instance.AdicionarCarga(cargas);
        
        Finalizar();
    }

    // Deve ser chamada quando destino for concluido, seja chegando no destino, recebendo/entregando carga, etc
    public void Finalizar() {
        if (!ativo) return;

        ativo = false;
        endereco.RemoverObjetivo();
        MissaoManager.instance.RemoveObjetivoAtivo(this);

        if (diretriz != null) diretriz.Interromper();
        if (pai != null) pai.ObjetivoConcluido(this);
    }

    // Deve ser chamada caso a missão seja interrompida
    public void Interromper() {
        if (!ativo) return;

        ativo = false;
        endereco.RemoverObjetivo();

        if (diretriz != null) diretriz.Interromper();
        if (pai != null) pai.Interromper();
    }
}

public class ObjetivoInicial : Objetivo {
    public Missao missao;

    public ObjetivoInicial(Objetivo objetivo, Missao missao = null) : base(objetivo.endereco, objetivo.cargas, objetivo.permiteReceber, null) {
        this.missao = missao;
    }

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
        MissaoManager.instance.ComecarMissao(missao);

        MissaoManager.instance.RemoveObjetivoAtivo(this);

        if (cargas != null && cargas.Count > 0) 
            MissaoManager.instance.AdicionarCarga(cargas);
        
        if (diretriz != null) diretriz.Interromper();
    }
}