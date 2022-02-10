using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataManager : MonoSingleTon<DataManager>
{
    [SerializeField] private float defaultSound = 0.5f;
    [SerializeField] private PlayerData player;

    public bool isReStart { get; private set; } = false;

    public PlayerData CurrentPlayer { get { return player; } }
    string SAVE_PATH = "";
    string SAVE_FILE = "/SaveFile.Json";
    private void Awake()
    {

        DataManager[] dmanagers = FindObjectsOfType<DataManager>();
        if (dmanagers.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        SAVE_PATH = Application.dataPath + "/Save";

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        LoadFromJson();

        SoundVolumeUpdate();
    }
    private void LoadFromJson()
    {
        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string stringJson = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            player = JsonUtility.FromJson<PlayerData>(stringJson);
        }
        else
        {
            player = new PlayerData(defaultSound);
        }
        SaveToJson();
    }
    public void SaveToJson()
    {
        string stringJson = JsonUtility.ToJson(player, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, stringJson, System.Text.Encoding.UTF8);
    }
    public void DataReset()
    {
        player = new PlayerData(defaultSound);
        SaveToJson();
        Application.Quit();
    }

    private void SoundVolumeUpdate()
    {
        SoundManager.Inst.BGMVolume(player.bgmSoundVolume);
        SoundManager.Inst.EffectVolume(player.effectSoundVolume);
    }





    private void OnApplicationQuit()
    {
        SaveToJson();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveToJson();
    }
    public void StartCheck()
    {
        if(!isReStart)
        {
            isReStart = true;
        }
    }
    public void TutorialTurnOn()
    {
        player.isTutorial = true;
    }
}