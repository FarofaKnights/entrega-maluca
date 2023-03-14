using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Destino {
    public Endereco endereco;
    public List<Carga> cargas;
    public bool permiteReceber = false;

    [System.NonSerialized]
    public SubMissao pai;

    public Destino(Endereco endereco, SubMissao pai = null) {
        this.endereco = endereco;
        this.pai = pai;
    }

    public Destino(Endereco endereco, List<Carga> cargas, SubMissao pai = null) {
        this.endereco = endereco;
        this.pai = pai;
        this.cargas = cargas;
    }

    // Chamada quando o Player entrou/saiu do trigger do endereço
    public virtual void HandleDestinoTrigger(bool estado) {
        if ((cargas != null && cargas.Count > 0) || permiteReceber) // Se houver carga para receber, chama o Handle de recebimento
            UIController.instance.PlayerNaAreaDeAcao(this, estado);
        else
            Finalizar();
    }

    // Chamada quando o destino se torna ativo, ou seja, é o próximo destino do jogador
    public void Iniciar() {
        endereco.DefinirComoDestino(this);
    }

    public virtual void Concluir() {
        if (permiteReceber)
            Player.instance.RemoverCarga(endereco);
        
        if (cargas != null)
            Player.instance.AdicionarCarga(cargas);
        
        Finalizar();
    }

    // Deve ser chamada quando destino for concluido, seja chegando no destino, recebendo/entregando carga, etc
    public void Finalizar() {
        endereco.RemoverDestino();

        if (pai != null)
            pai.DestinoConcluido(this);
    }

    // Deve ser chamada caso a missão seja interrompida
    public void Interromper() {
        endereco.RemoverDestino();
    }
}

public class DestinoComecar : Destino {
    public Missao missao;

    public DestinoComecar(Endereco endereco, Missao missao = null) : base(endereco, null) {
        this.missao = missao;
    }

    public DestinoComecar(Endereco endereco, List<Carga> cargas, Missao missao = null) : base(endereco, null) {
        this.missao = missao;
        this.cargas = cargas;
    }

    public override void HandleDestinoTrigger(bool estado) {
        UIController.instance.PlayerNaAreaDeIniciarMissao(this, estado);
    }

    // Chamada ao clicar no botão de iniciar missão
    public override void Concluir() {
        missao.Iniciar();

        if (cargas != null) 
            Player.instance.AdicionarCarga(cargas);
    
        endereco.RemoverDestino();
    }
}