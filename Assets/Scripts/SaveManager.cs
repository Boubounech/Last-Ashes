using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private string savePath;


    [System.NonSerialized] public string sceneToSave;
    [System.NonSerialized] public List<string> itemsToSave;
    [System.NonSerialized] public List<string> powersToSave;
    [System.NonSerialized] public float timeToSave;

    [System.NonSerialized] public string sceneSaved;
    [System.NonSerialized] public List<string> itemsSaved;
    [System.NonSerialized] public List<string> powersSaved;
    [System.NonSerialized] public float timeSaved;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        itemsToSave = new List<string>();
        powersToSave = new List<string>();
    }

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "LastAshesSave.json");
        Debug.Log($"Save file path: {savePath}");
    }

    public void LoadFile()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath); 
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json); 

            sceneSaved = loadedData.scene;
            itemsSaved = new List<string>(loadedData.items);
            powersSaved = new List<string>(loadedData.powers);
            timeSaved = loadedData.time;

            itemsToSave = itemsSaved;
            powersToSave = powersSaved;

            Debug.Log($"Game loaded successfully. Scene: {sceneSaved}, Items: {string.Join(", ", itemsSaved)}, Powers: {string.Join(", ", powersSaved)}");

            InventoryManager.instance.SetCollectedItems(itemsSaved);
            
            VitalEnergyManager.instance.SetMaxEnergyTime(timeSaved);
            VitalEnergyManager.instance.ResetTimer();

            if (powersSaved.Contains("Dash"))
            {
                PlayerEvents.OnDashObtained.Invoke();
            }
            if (powersSaved.Contains("WallJump"))
            {
                PlayerEvents.OnWallJumpObtained.Invoke();
            }
            if (powersSaved.Contains("Dive"))
            {
                PlayerEvents.OnDiveObtained.Invoke();
            }
            if (powersSaved.Contains("Fireball"))
            {
                PlayerEvents.OnFireballObtained.Invoke();
            }
            if (powersSaved.Contains("DoubleJump"))
            {
                PlayerEvents.OnDoubleJumpObtained.Invoke();
            }

        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }

    public void SaveFile()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        this.sceneToSave = currentSceneName;

        SaveData saveData = new SaveData
        {
            scene = this.sceneToSave,
            items = this.itemsToSave,
            powers = this.powersToSave,
            time = this.timeToSave
        };


        string json = JsonUtility.ToJson(saveData, true); 
        File.WriteAllText(savePath, json); 
        Debug.Log("Game saved successfully.");
    }

}
