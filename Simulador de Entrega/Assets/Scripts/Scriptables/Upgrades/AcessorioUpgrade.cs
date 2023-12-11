using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aquele acessorio lá", menuName = "Entrega Maluca/Upgrade/Acessorio"), System.Serializable]
public class AcessorioUpgrade : UpgradeObject {

    public GameObject acessorioPrefab;
    public VisualPlayer.Localizacao localizacao;

    protected override void _Ativar() {
        VisualPlayer.instance.SetAcessorio(acessorioPrefab, localizacao);
    }

    protected override void _Desativar() {
        VisualPlayer.instance.RemoveAcessorio(localizacao);
    }

    public override Dictionary<string, (string, float)> GetInfo() {
        return null;
    }
}
