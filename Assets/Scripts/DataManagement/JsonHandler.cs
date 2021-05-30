using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonHandler : MonoBehaviour
{
    private static JsonHandler instance;
    private JsonHandler() { }
    public static JsonHandler getInstance()
    {
        if (instance == null)
        {
            instance = new JsonHandler();
        }
        return instance;
    }

    private string jsonPath = Application.dataPath + "/data.json";
    public DataCollection data;
    public void LoadField()
    {
        data = JsonUtility.FromJson<DataCollection>(File.ReadAllText(jsonPath));
        PlayerPrefs.SetInt("type", data.type);
        PlayerPrefs.SetInt("height", data.gammaHeight);
        PlayerPrefs.SetInt("width", data.gammaWidth);
        PlayerPrefs.SetInt("length", data.deltaLength);
        PlayerPrefs.SetInt("radius", data.thetaRadius);
    }
    public void SaveField()
    {
        File.WriteAllText(jsonPath, JsonUtility.ToJson(data));
    }

    [System.Serializable]
    public class DataCollection
    {
        public int type;
        public int gammaWidth;
        public int gammaHeight;
        public int deltaLength;
        public int thetaRadius;
    }
}
