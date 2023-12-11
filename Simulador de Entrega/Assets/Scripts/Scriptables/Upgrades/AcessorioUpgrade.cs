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

    public override bool CalculoDeExclusividade(UpgradeObject outro) {
        if (outro is AcessorioUpgrade) {
            AcessorioUpgrade outroAcessorio = (AcessorioUpgrade)outro;
            return outroAcessorio.localizacao == localizacao;
        }

        return false;
    }
}
