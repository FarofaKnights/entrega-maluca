using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missao {
    public Destino destinoInicial, destinoAtual;

    // Destino para chegar onde começa a missão
    public DestinoComecar destinoComecar; 

    bool disponivel = false;
    // Missao[] liberaMissoes;

    public Missao(Destino destinoInicial) {
        this.destinoInicial = destinoInicial;
        destinoComecar = new DestinoComecar(destinoInicial.endereco);
    }

    public void DefinirDisponibilidade(bool disponivel) {
        // Caso não houve alterações na disponibilidade
        if (this.disponivel == disponivel)
            return;

        if (disponivel) {
            destinoComecar.Iniciar(this);
        } else {
            destinoComecar.Interromper();
        }

        this.disponivel = disponivel;
    }

    public bool Iniciar() {
        Debug.Log("chegou aq");
        if (!disponivel) return false;
        
        Player.instance.ComecarMissao(this);

        // O primeiro destino sempre será o mesmo que o destino de inicio da missão, então é automaticamente concluido
        destinoAtual = destinoInicial;
        destinoAtual.Iniciar(this);
        destinoAtual.Concluir();
        Debug.Log("concluiu destino inicial");
        return true;
    }

    public void Interromper() {
        if (Player.instance.missaoAtual == this) {
            Player.instance.missaoAtual = null;
            destinoAtual.Interromper();
        }
    }

    public void ProximoDestino() {
        Debug.Log("proximo!!");
        destinoAtual = destinoAtual.proximo;

        // Quando chegou no final 
        if (destinoAtual == null) {
            Concluir();
            return;
        }

        destinoAtual.Iniciar(this);
    }

    void Concluir() {
        Debug.Log("ACABOU!");
        Player.instance.dinheiro += 100;
        Player.instance.missoesDisponiveis.Remove(this.destinoComecar);
        Player.instance.FinalizarMissao();

        Missao novaMissao = GerarMissaoAleatoria();
        Player.instance.AdicionarMissao(novaMissao);
    }

    public static Missao GerarMissaoAleatoria() {
        int a, b;
        a = Random.Range(1,6); // de 1 a 5

        // Gera numero aleatorio diferente de a
        do {
            b = Random.Range(1,6);
        } while (a == b);

        // Pega objetos do tipo Endereco
        Endereco remetente = Endereco.ListaEnderecos["Predio " + a];
        Endereco destinatario = Endereco.ListaEnderecos["Predio " + b];

        // Gera quantidade aleatoria de cargas
        List<Carga> cargas = new List<Carga>();
        int quant = Random.Range(1, 4);
        for (int i = 0; i < quant; i++) {
            Carga carga = new Carga(1, 1, remetente);
            cargas.Add(carga);
        }

        // Define padrão de destino de A a B
        Destino final = new DestinoEntrega(destinatario);
        Destino inicio = new DestinoRecebimento(remetente, cargas, final);

        return new Missao(inicio);
    }
}
