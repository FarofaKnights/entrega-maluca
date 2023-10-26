using System;
using UnityEngine;

public class StatusCarga {
    public string nome;
    public float porcentagem;
    public float valor;

    public StatusCarga(string nome, float porcentagem, float valor) {
        this.nome = nome;
        this.porcentagem = porcentagem;
        this.valor = valor;
    }

    public StatusCarga(Carga carga) {
        nome = carga.nome;
        porcentagem = carga.fragilidade / carga.fragilidadeInicial;
        valor = carga.GetValor();
    }
}

public class StatusMissao {
    public float dinheiro = 0;
    public float tempo = 0;
    public int avaliacao = 0;
    public StatusCarga[] cargas;

    public StatusMissao(int avaliacao, float dinheiro, float tempo) {
        this.dinheiro = dinheiro;
        this.tempo = tempo;
        this.avaliacao = avaliacao;
    }

    public StatusMissao(Missao missao) {
        Carga[] cargasEntregues = missao.GetCargasEntregues();
        cargas = new StatusCarga[cargasEntregues.Length];
        dinheiro = 0;
        
        float porcentagem = 0;

        for (int i = 0; i < cargasEntregues.Length; i++) {
            cargas[i] = new StatusCarga(cargasEntregues[i]);
            porcentagem += cargas[i].porcentagem;
            dinheiro += cargas[i].valor;
        }

        porcentagem /= cargasEntregues.Length; // Valor de 0 a 1

        avaliacao = (int) Math.Round(porcentagem * 4, MidpointRounding.AwayFromZero) + 1; // Valor de 1 a 5
        tempo = 1;
    }
}
