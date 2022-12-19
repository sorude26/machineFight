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

    /// <summary>
    /// Jsonからデータをロードする
    /// 中身がある場合true, ない場合falseを返す
    /// </summary>
    public static bool JsonLoad()
    {
        string jsonData;
        StreamReader reader = new StreamReader(Application.dataPath + "/StreamingAssets/" + _getsPartsJsonFileName);
        jsonData = reader.ReadToEnd();
        reader.Close();
        reader = new StreamReader(Application.dataPath + "/StreamingAssets/" + _partsBuildJsonFileName);
        string buildData = reader.ReadToEnd();
        reader.Close();
        Debug.Log("DATALOAD");
        JsonUtility.FromJsonOverwrite(jsonData, PlayerData.instance);
        PartsBuildParam Data = JsonUtility.FromJson<PartsBuildParam>(buildData);
        PlayerData.instance.BuildPreset = Data;
        if (jsonData == "")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static void JsonSave(PlayerData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/" + _getsPartsJsonFileName, jsonData);
        string buildData = JsonUtility.ToJson(playerData.BuildPreset);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/" + _partsBuildJsonFileName, buildData);
        Debug.Log("DATASAVE");
    }

    public static void JsonDelete()
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "/StreamingAssets/" + _getsPartsJsonFileName);
        writer.Write("");
        writer.Flush();
        writer.Close();
        writer = new StreamWriter(Application.dataPath + "/StreamingAssets/" + _partsBuildJsonFileName);
        writer.Write("");
        writer.Flush();
        writer.Close();
    }
}
