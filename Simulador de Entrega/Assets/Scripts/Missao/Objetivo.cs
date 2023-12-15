using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objetivo: Iniciavel {
    public Endereco endereco;
    public List<Carga> cargas;
    public bool permiteReceber = false;
    public Diretriz diretriz = null;

    protected bool ativo = false, concluido = false;


    public Objetivo(Endereco endereco) {
        this.endereco = endereco;
    }

    public Objetivo(Endereco endereco, List<Carga> cargas, bool permiteReceber, Diretriz diretriz = null) {
        this.endereco = endereco;
        this.cargas = cargas;
        this.permiteReceber = permiteReceber;
        this.diretriz = diretriz;
    }

    // Chamada quando o Player entrou/saiu do trigger do endereço
    public virtual void HandleObjetivoTrigger(bool estado) {
        if ((cargas != null && cargas.Count > 0) || permiteReceber) // Se houver carga para receber, chama o Handle de recebimento
            UIController.HUD.MostrarBotaoAcao(this, estado);
        else
            Concluir();
    }

    // Chamada quando o destino se torna ativo, ou seja, é o próximo destino do jogador
    public virtual void Iniciar() {
        if (ativo) return;

        ativo = true;
        concluido = false;
        endereco.DefinirComoObjetivo(this);
        MissaoManager.instance.AddObjetivoAtivo(this);

        if (diretriz != null) diretriz.Iniciar();
    }

    public virtual void Concluir() {
        if (!ativo) return;
        ativo = false;
        concluido = true;

        if (permiteReceber){
            foreach (Carga carga in Player.instance.RemoverCargaDeEndereco(endereco)) {
                MissaoManager.instance.missaoAtual.AddCargaEntregue(carga);
            }
        }
        
        if (cargas != null && cargas.Count > 0)
            Player.instance.AdicionarCarga(cargas);
        

        endereco.RemoverObjetivo();
        MissaoManager.instance.RemoveObjetivoAtivo(this);
        MissaoManager.instance.missaoAtual.Next();
        
        if (diretriz != null) diretriz.Parar();
    }

    // Deve ser chamada caso a missão seja interrompida
    public virtual void Parar() {
        if (!ativo) return;

        ativo = false;
        concluido = false;
        endereco.RemoverObjetivo();
        MissaoManager.instance.RemoveObjetivoAtivo(this);

        if (diretriz != null) diretriz.Parar();
    }

    public bool IsConcluido() { return concluido; }
    public bool IsIniciada() { return ativo; }

    // Gambiarras
    public List<Carga> RemoveCargas() {
        List<Carga> cargas = this.cargas;
        this.cargas = null;
        return cargas;
    }

    public void SetCargas(List<Carga> cargas) {
        this.cargas = cargas;
    }
}

public class ObjetivoInicial : Objetivo {
    public Missao missao;

    public ObjetivoInicial(Objetivo objetivo, Missao missao = null) : base(objetivo.endereco, objetivo.cargas, objetivo.permiteReceber, null) {
        this.missao = missao;
    }

    public ObjetivoInicial(Endereco endereco, Missao missao = null) : base(endereco) {
        this.missao = missao;
    }

    public ObjetivoInicial(Endereco endereco, List<Carga> cargas, Missao missao = null) : base(endereco) {
        this.missao = missao;
        this.cargas = cargas;
    }

    public override void HandleObjetivoTrigger(bool estado) {
        UIController.HUD.MostrarMissaoInfo(this, estado);
    }

    // Chamada ao clicar no botão de iniciar missão
    public override void Concluir() {
        MissaoManager.instance.RemoveObjetivoAtivo(this);
        ativo = false;
        concluido = true;
        endereco.RemoverObjetivo();

        IniciarMissao(); //IniciarMissaoComCutscene();
        
        if (diretriz != null) diretriz.Parar();
    }

    public void IniciarMissaoComCutscene() {
        if (missao.info.dialogo == null) IniciarMissao();
        else UIController.instance.ShowCutscene(missao.info.personagem, missao.info.dialogo.falaInicial, IniciarMissao);
    }

    void IniciarMissao() {
        MissaoManager.instance.IniciarMissao(missao);

        if (cargas != null && cargas.Count > 0) 
            Player.instance.AdicionarCarga(cargas);
    }
}