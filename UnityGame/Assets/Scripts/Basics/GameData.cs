using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using JetBrains.Annotations;
using System.Reflection;

[Serializable]
public class SerializedSprite
{
    public int width, height;
    [SerializeField]
    public IntPtr ptr = IntPtr.Zero;

    public SerializedSprite(int _width, int _height, IntPtr _ptr)
    {
        width = _width;
        height = _height;
        ptr = _ptr;
    }    
}

[Serializable]
public class DataObject
{
    public List<Ability> Abilities { private set; get; } = new List<Ability>();
    public List<Item> Items { private set; get; } = new List<Item>();
    public int ActionTier1Index, ActionTier2Index;

    public DataObject(List<Ability> abilities, List<Item> items)
    {
        Abilities = abilities;
        Items = items;
    }
}


[ExecuteAlways]
public class GameData : MonoBehaviour
{
    public static string DataBasePath { get; } = "Assets/DataBase.data";
#if UNITY_EDITOR
    public static UnityEditor.EditorWindow BaseData { set; get; }
#endif
    public void OnEnable()
    {
        DataObject load = Load(DataBasePath);
    }

    public static void Save(string path, DataObject data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            formatter.Serialize(stream, data);
            stream.Close();
        }
    }
    public static DataObject Load(string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        DataObject data = formatter.Deserialize(stream) as DataObject;
        stream.Close();
        return data;
    }
}