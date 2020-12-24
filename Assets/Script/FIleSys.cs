using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class FIleSys
{
    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        return Resources.LoadAll<T>("ScriptableObject");
    }

    public static void SaveFile(string path, object contente)
    {
        //Directory.CreateDirectory(Application.dataPath + path);
        FileStream fileStream = new FileStream(Application.persistentDataPath + path, FileMode.OpenOrCreate);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, contente);
        fileStream.Close();
    }

    public static T OpenFile<T>(string path)
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            BinaryFormatter binaryFormater = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + path, FileMode.Open);
            T contente = (T)binaryFormater.Deserialize(fileStream);
            fileStream.Close();
            return contente;
        }
        else
        {
            Debug.LogError($"{Application.persistentDataPath + path} not found");
        }
        return default;
    }
}
