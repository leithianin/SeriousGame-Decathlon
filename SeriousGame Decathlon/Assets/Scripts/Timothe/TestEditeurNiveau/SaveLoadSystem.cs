﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem instance;

    //private Colis colisToSave;

    //https://www.youtube.com/watch?v=SNwPq01yHds

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/game_save");
    }

    public void SaveColis(Colis colisToSave)
    {
        if(!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }

        if (!Directory.Exists(Application.persistentDataPath + "/game_save/colis_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/colis_data");
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/game_save/colis_data" + colisToSave.name + ".txt");
        var json = JsonUtility.ToJson(colisToSave);
        bf.Serialize(file, json);
        file.Close();
    }

    public void LoadColis(Colis colisToLoad)
    {
        if (!IsSaveFile())
        {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/game_save/colis_data" + colisToLoad.name + ".txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_save/colis_data" + colisToLoad.name + ".txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), colisToLoad);
        }
    }

    public void SaveLevel(LevelScriptable levelToSave, List<Colis> colisDuLevel)
    {
        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }

        if (!Directory.Exists(Application.persistentDataPath + "/game_save/level_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/level_data");
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/game_save/level_data/Level" + levelToSave.nbLevel.ToString() + ".txt");
        var json = JsonUtility.ToJson(levelToSave);
        bf.Serialize(file, json);
        file.Close();

        foreach(Colis lisco in colisDuLevel)
        {
            SaveColis(lisco);
        }
    }

    public void LoadLevel(LevelScriptable levelToSave)
    {
        if (!IsSaveFile())
        {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/game_save/level_data/Level" + levelToSave.nbLevel.ToString() + ".txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_save/level_data/Level" + levelToSave.nbLevel.ToString() + ".txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), levelToSave);
        }
    }
}
