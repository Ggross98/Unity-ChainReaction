using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonReader
{

    public static T GetDataFromJson<T>(string _path, bool asset = false)
    {   
        string prefix = asset ? Application.streamingAssetsPath : System.Environment.CurrentDirectory;
        string path = prefix + "/" + _path;
        Debug.Log(path);

        if (File.Exists(path))
        {
            using (StreamReader reader = /*File.OpenText(_path)*/ new StreamReader(path))
            {
                string readdata = reader.ReadToEnd();

                if (readdata.Length > 0)
                {
                    // Debug.Log(readdata);
                    T data = JsonUtility.FromJson<T>(readdata);

                    return data;
                }
            }
        }
        Debug.Log("Not read");
        return default(T);

    }
}

