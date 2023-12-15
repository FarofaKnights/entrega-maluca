using System;
using System.Collections;
using System.Collections.Generic;
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
        List<Carga> caixasCaidas = Player.instance.GetCargasCaidas();

        Carga[] cargasEntregues = missao.GetCargasEntregues();
        cargas = new StatusCarga[cargasEntregues.Length + caixasCaidas.Count];
        dinheiro = 0;

        //Debug.Log("Cargas entregues: " + cargasEntregues.Length);
        //Debug.Log("Cargas caidas: " + caixasCaidas.Count);
        
        float porcentagem = 0;
        int i = 0;

        for (i = 0; i < cargasEntregues.Length; i++) {
            cargas[i] = new StatusCarga(cargasEntregues[i]);
            porcentagem += cargas[i].porcentagem;
            dinheiro += cargas[i].valor;

            if (caixasCaidas.Contains(cargasEntregues[i])) caixasCaidas.Remove(cargasEntregues[i]);
        }

        //Debug.Log("i: " + i);

        foreach (Carga carga in caixasCaidas) {
            cargas[i] = new StatusCarga(carga.nome, 0, 0);
            i++;
        }

        porcentagem /= cargas.Length;

        avaliacao = (int) Math.Round(porcentagem * 4, MidpointRounding.AwayFromZero) + 1; // Valor de 1 a 5
        tempo = 1;
    }
}
