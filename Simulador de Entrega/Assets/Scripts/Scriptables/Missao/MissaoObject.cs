using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Missao", menuName = "Entrega Maluca/Missao"), System.Serializable]
public class MissaoObject : ScriptableObject {
    public string nome;
    public string descricao;
    public string descricaoGrande;
    public float valorFixo = 0f;
    public PersonagemObject personagem;
    public DialogoPersonagens dialogo;

    public ObjetivoObject objetivoInicial;

    public ConjuntoObject[] conjuntos;

    public MissaoObject[] missoesDesbloqueadas;

    public bool gerarAleatoriaNoFinal = false;

    public Carga[] GetAllCargas() {
        List<CargaObject> cargas = new List<CargaObject>();

        if (objetivoInicial.cargas != null)
            cargas.AddRange(objetivoInicial.cargas);

        foreach (ConjuntoObject conjunto in conjuntos) {
            foreach (ObjetivoObject objetivo in conjunto.objetivos) {
                if (objetivo.cargas != null)
                    cargas.AddRange(objetivo.cargas);
            }
        }

        return cargas.ConvertAll(c => c.Convert()).ToArray();
    }

    public Endereco GetEnderecoInicial() {
        return Endereco.GetEndereco(objetivoInicial.endereco);
    }

    public Endereco GetEnderecoFinal() {
        ConjuntoObject ultimoConjunto = conjuntos[conjuntos.Length - 1];
        ObjetivoObject ultimoObjetivo = ultimoConjunto.objetivos[ultimoConjunto.objetivos.Length - 1];
        string endereco = ultimoObjetivo.endereco;
        return Endereco.GetEndereco(endereco);
    }

    public Cutscene GetCutsceneInicial() {
        if (dialogo == null) return null;

        return new Cutscene(dialogo.falaInicial, personagem);
    }

    public Cutscene GetCutsceneFalha() {
        if (dialogo == null) return null;

        return new Cutscene(personagem.falaFalha, personagem);
    }

    public Cutscene GetCutsceneConclusao() {
        if (dialogo == null) return null;

        return new Cutscene(dialogo.falaConclusao, personagem);
    }
}
