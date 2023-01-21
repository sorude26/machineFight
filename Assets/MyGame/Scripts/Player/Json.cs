using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Meta.Wit.LitJson;

public static class Json
{
    //入手済みのパーツのデータ
    static string _getsPartsJsonFileName = "PartsSaveData.json";

    //現在のプレセットのデータ
    static string _partsBuildJsonFileName = "BuildSaveData.json";

    static readonly string dataPath = Application.dataPath + "/StreamingAssets/";

    /// <summary>
    /// Jsonからデータをロードする
    /// 中身がある場合true, ない場合falseを返す
    /// </summary>
    public static bool JsonLoad()
    {
        string jsonData;
        StreamReader reader = new StreamReader(dataPath + _getsPartsJsonFileName);
        jsonData = reader.ReadToEnd();
        reader.Close();
        reader = new StreamReader(dataPath + _partsBuildJsonFileName);
        string buildData = reader.ReadToEnd();
        reader.Close();
        Debug.Log("DATALOAD");
        if (jsonData == "")
        {
            return false;
        }
        else
        {
            PartsData partsData = JsonUtility.FromJson<PartsData>(jsonData);
            PartsBuildParam build = JsonUtility.FromJson<PartsBuildParam>(buildData);
            PlayerData.instance.BuildPreset = build;
            SaveDataReader.SetData(partsData);
            return true;
        }
    }

    public static void JsonSave(PlayerData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(dataPath + _getsPartsJsonFileName, jsonData);
        string buildData = JsonUtility.ToJson(playerData.BuildPreset);
        File.WriteAllText(dataPath + _partsBuildJsonFileName, buildData);
        Debug.Log("DATASAVE");
    }
    public static void SavePreset(PartsBuildParam buildPreset)
    {
        string buildData = JsonUtility.ToJson(buildPreset);
        File.WriteAllText(dataPath + _partsBuildJsonFileName, buildData);
    }
    public static void SavePartsData(PartsData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(dataPath + _getsPartsJsonFileName, jsonData);
    }

    public static void JsonDelete()
    {
        StreamWriter writer = new StreamWriter(dataPath + _getsPartsJsonFileName);
        writer.Write("");
        writer.Flush();
        writer.Close();
        writer = new StreamWriter(dataPath + _partsBuildJsonFileName);
        writer.Write("");
        writer.Flush();
        writer.Close();
    }
}
