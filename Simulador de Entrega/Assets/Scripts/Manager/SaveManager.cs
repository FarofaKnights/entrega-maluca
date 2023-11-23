using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SceneData
{
    public MissaoData missaoData;
    public PlayerData playerData;
}
public class SaveManager : MonoBehaviour
{
    string path;
    public static SaveManager instance;
    private void Awake()
    {
        instance = this;
        path = Application.dataPath + "save.txt";
    }

    public void Save()
    {
        SceneData data = new SceneData();
        data.missaoData = MissaoManager.instance.GetMissaoData();
        data.playerData = Player.instance.GetPlayerData();
        string save = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, save);

    }
    public void Load ()
    {
        string load = File.ReadAllText(path);
        SceneData data = JsonUtility.FromJson<SceneData>(load);
        Player.instance.SetPlayerData(data.playerData);
        MissaoManager.instance.SetMissaoData(data.missaoData);

    }
}
