using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destino {
    public Endereco endereco;
    public Destino proximo;
    public Missao missao;

    public Destino(Endereco endereco, Destino proximo = null) {
        this.endereco = endereco;
        this.proximo = proximo;
    }

    // Handle genérico de um Destino do tipo Vazio, que irá para o próximo caso jogador entre em trigger
    public virtual void HandleDestinoTrigger(bool estado) {
        Finalizar();
    }

    // Chamada quando o destino se torna ativo, ou seja, é o próximo destino do jogador
    public void Iniciar(Missao missao) {
        this.missao = missao;
        endereco.DefinirComoDestino(this);
    }

    public virtual void Concluir() {
        Finalizar();
    }

    // Deve ser chamada quando destino for concluído, seja chegando no destino, recebendo/entregando carga, etc
    public void Finalizar() {
        endereco.RemoverDestino();
        missao.ProximoDestino();
    }

    // Deve ser chamada caso a missão seja interrompida
    public void Interromper() {
        endereco.RemoverDestino();
    }
}

public class DestinoRecebimento : Destino {
    public List<Carga> cargas;

    public DestinoRecebimento(Endereco endereco, List<Carga> cargas, Destino proximo = null) : base(endereco, proximo) {
        this.cargas = cargas;
    }

    public override void HandleDestinoTrigger(bool estado) {
        UIController.instance.PlayerNaAreaDeReceber(this, estado);
    }

    // Chamada ao clicar no botão de receber carga, adiciona a carga ao player e finaliza o destino
    public override void Concluir() {
        Player.instance.AdicionarCarga(cargas);
        Finalizar();
    }
}

public class DestinoEntrega : Destino {
    // Construtor vazio porque aparentemente a questão de herança e construtor em C# é algo bem feio mesmo (mapeando os parametros para a base)
    public DestinoEntrega(Endereco endereco, Destino proximo = null) : base(endereco, proximo) { }

    public override void HandleDestinoTrigger(bool estado) {
        UIController.instance.PlayerNaAreaDeEntrega(this, estado);
    }

    // Chamada ao clicar no botão de entregar carga, remove do player e finaliza o destino
    public override void Concluir() {
        List<Carga> entregues = Player.instance.RemoverCarga(endereco);
        // ... Valor da carga
        Finalizar();
    }
}

public class DestinoComecar : Destino {
    public DestinoComecar(Endereco endereco) : base(endereco, null) { }

    public override void HandleDestinoTrigger(bool estado) {
        UIController.instance.PlayerNaAreaDeIniciarMissao(this, estado);
    }

    // Chamada ao clicar no botão de iniciar missão
    public override void Concluir() {
        missao.Iniciar();
        endereco.RemoverDestino();
    }
}