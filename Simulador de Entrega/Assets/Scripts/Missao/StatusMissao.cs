using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCarga {
    public string nome;
    public float porcentagem;
    public float valor;
    public bool entregue = true;

    public StatusCarga(string nome, float porcentagem, float valor, bool entregue) {
        this.nome = nome;
        this.porcentagem = porcentagem;
        this.valor = valor;
        this.entregue = entregue;
    }

    public StatusCarga(Carga carga, bool entregue) {
        nome = carga.nome;
        porcentagem = carga.fragilidade / carga.fragilidadeInicial;
        valor = carga.GetValor();
        this.entregue = entregue;
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
        List<StatusCarga> cargasStatus = new List<StatusCarga>();
        dinheiro = missao.info.valorFixo;
        
        float porcentagem = 0;

        for (int i = 0; i < cargasEntregues.Length; i++) {
            StatusCarga carga = new StatusCarga(cargasEntregues[i], true);
            porcentagem += carga.porcentagem;
            dinheiro += carga.valor;

            cargasStatus.Add(carga);

            if (caixasCaidas.Contains(cargasEntregues[i])) caixasCaidas.Remove(cargasEntregues[i]);
        }

        foreach (Carga carga in caixasCaidas) {
            StatusCarga carga1;

            if (carga.EstaDestruida()) carga1 = new StatusCarga(carga.nome, 0, 0, false);
            else carga1 = new StatusCarga(carga, false);
            cargasStatus.Add(carga1);
        }

        cargas = cargasStatus.ToArray();

        porcentagem /= cargas.Length;

        avaliacao = (int) Math.Round(porcentagem * 4, MidpointRounding.AwayFromZero) + 1; // Valor de 1 a 5
        tempo = MissaoManager.instance.GetTempoMissao();

        if (cargas.Length == 0) avaliacao = 5;
    }
}
