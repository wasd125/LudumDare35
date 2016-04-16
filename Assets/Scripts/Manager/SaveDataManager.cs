using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveDataManager : MonoBehaviour {

    [SerializeField][HideInInspector]
    public MyAudioConfigData AudioConfigData;

    public static SaveDataManager Instance;

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance == null)
        {
            // If that is the case, we destroy other instances
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Speichtert alle Daten
    /// </summary>
    public void SaveAllData()
    {
        SaveSoundData();
    }
    /// <summary>
    /// Lädt alle Daten
    /// </summary>
    public void LoadAllData()
    {
        LoadSoundData();
    }
    /// <summary>
    /// Lädt die SoundConfig aus dem Pfad "AUDIO_CONFIG_PATH"
    /// </summary>
    public void LoadSoundData()
    {
        if (File.Exists(DataPaths.AUDIO_CONFIG_PATH))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(DataPaths.AUDIO_CONFIG_PATH, FileMode.Open);
            AudioConfigData = (MyAudioConfigData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            AudioConfigData = new MyAudioConfigData(1f, 1f, 1f, 1f);
        }

        SoundManager.Instance.MasterVolume = AudioConfigData.MasterVolume;
        SoundManager.Instance.MusikVolume = AudioConfigData.MusikVolume;
        SoundManager.Instance.AmbienceVolume = AudioConfigData.AmbienceVolume;
        SoundManager.Instance.SoundEffectVolume = AudioConfigData.SoundEffectVolume;
    }
    /// <summary>
    /// Speichert die aktuelle SoundConfig in dem Pfad "AUDIO_CONFIG_PATH"
    /// </summary>
    public void SaveSoundData()
    {
        AudioConfigData = new MyAudioConfigData(SoundManager.Instance.MasterVolume,
            SoundManager.Instance.MusikVolume,
            SoundManager.Instance.AmbienceVolume,
            SoundManager.Instance.SoundEffectVolume);


        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(DataPaths.AUDIO_CONFIG_PATH, FileMode.OpenOrCreate);

        bf.Serialize(file, AudioConfigData);

        file.Close();
    }
}

public static class DataPaths
{
    public static string AUDIO_CONFIG_PATH = string.Format("{0}/AudioConfig.dat", Application.persistentDataPath);
}

[Serializable]
public class MyAudioConfigData
{
    public float MasterVolume { get; private set; }
    public float MusikVolume { get; private set; }
    public float AmbienceVolume { get; private set; }
    public float SoundEffectVolume { get; private set; }

    public MyAudioConfigData(float master,float musik,float ambience,float soundEffect):this()
    {
        this.MasterVolume = master;
        this.MusikVolume = musik;
        this.AmbienceVolume = ambience;
        this.SoundEffectVolume = soundEffect;
    }
    public MyAudioConfigData()
    { }
}
