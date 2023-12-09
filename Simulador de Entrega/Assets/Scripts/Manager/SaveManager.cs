using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    string pathPlayer, pathMissao, pathUpgrades;
    public static SaveManager instance;
    private void Awake()
    {
        instance = this;
        pathPlayer = Application.dataPath + "savePlayer.txt";
        pathMissao = Application.dataPath + "saveMissao.txt";
        pathUpgrades = Application.dataPath + "saveUpgrades.txt";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    public void Save()
    {
        PlayerData playerData = Player.instance.GetPlayerData();
        string savePlayer = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(pathPlayer, savePlayer);

        MissaoData missaoData = MissaoManager.instance.GetMissaoData();
        string saveMissao = JsonUtility.ToJson(missaoData, true);
        File.WriteAllText(pathMissao, saveMissao);

        UpgradeData upgradeData = OficinaController.instance.GetUpgradeData();
        string saveUpgrade = JsonUtility.ToJson(upgradeData, true);
        File.WriteAllText(pathUpgrades, saveUpgrade);

    }
    public void Load ()
    {
        string loadPlayer = File.ReadAllText(pathPlayer);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(loadPlayer);
        Player.instance.SetPlayerData(playerData);

        string loadMissao = File.ReadAllText(pathMissao);
        MissaoData missaoData = JsonUtility.FromJson<MissaoData>(loadMissao);
        MissaoManager.instance.SetMissaoData(missaoData);

        string loadUpgrade = File.ReadAllText(pathUpgrades);
        UpgradeData upgradeData = JsonUtility.FromJson<UpgradeData>(loadUpgrade);
        OficinaController.instance.SetUpgradeData(upgradeData);

    }
}
