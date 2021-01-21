using System.Collections;
using System.Collections.Generic;
using System;
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
        if (!Directory.Exists(Path.GetDirectoryName(Application.persistentDataPath + path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Application.persistentDataPath + path));
        }
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

    public static string[] GetFolder(string path)
    {
        string[] folders = Directory.GetDirectories(Application.persistentDataPath + path);
        string[] foldersCut = new string[folders.Length];
        for (int i = 0; i < folders.Length; i++)
        {
            int indexCut = folders[i].IndexOf("\\");
            foldersCut[i] = folders[i].Substring(indexCut + 1);
        }
        return foldersCut;
    }
}
