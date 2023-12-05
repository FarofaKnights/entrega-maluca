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
    public PersonagemObject personagem;

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
}
