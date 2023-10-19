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

        if (carga.fragilidadeInicial == 0) {
            Debug.LogError("Porra de carga não ta com os valores certos onde devia tá arruma isso ai Henri");
            // Se você veio aqui descobrir o que é esse erro, bem, o valor de fragilidade inicial da carga é 0. Se o problema for do prefab, arrume
            // Se o problema não for do prefab (provavelmente é o caso), é porque a carga que está no cargasEntregues não é a mesma que está na caixa!
            porcentagem = 0.75f;
        }
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
        cargas = new StatusCarga[missao.cargasEntregues.Count];
        dinheiro = 0;
        
        float porcentagem = 0;

        for (int i = 0; i < missao.cargasEntregues.Count; i++) {
            cargas[i] = new StatusCarga(missao.cargasEntregues[i]);
            porcentagem += cargas[i].porcentagem;
            dinheiro += cargas[i].valor;
        }

        porcentagem /= missao.cargasEntregues.Count; // Valor de 0 a 1

        avaliacao = (int) Math.Round(porcentagem * 4, MidpointRounding.AwayFromZero) + 1; // Valor de 1 a 5
        tempo = 1;
    }
}
