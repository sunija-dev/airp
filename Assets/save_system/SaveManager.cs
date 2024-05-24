using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    public string m_strRelativePath = "savefiles";

    public string m_strSavePath = "";

    public void Awake()
    {
        m_strSavePath = Application.persistentDataPath + "/" + m_strRelativePath;
    }

    public void Save(object _obj, string _strName, string _strFolder)
    {
        string strJson = JsonConvert.SerializeObject(_obj, Formatting.Indented);
        string strPath = m_strSavePath + "/" + _strFolder + "/" + _strName + ".json";
        System.IO.File.WriteAllText(strPath, strJson);
        Debug.Log($"SaveManager.Save({_obj}, {_strName}, {m_strSavePath})");
    }

    public T Load<T>(string _strName, string _strFolder)
    {
        string strPath = m_strSavePath + "/" + _strFolder + "/" + _strName + ".json";
        if (System.IO.File.Exists(strPath))
        {
            string strJson = System.IO.File.ReadAllText(strPath);
            T obj = JsonConvert.DeserializeObject<T>(strJson);
            Debug.Log($"SaveManager.Load<{typeof(T)}>({strPath})");
            return obj;
        }
        else
        {
            Debug.Log($"SaveManager.Load<{typeof(T)}>({strPath}) - File not found");
            return default(T);
        }
    }
}
