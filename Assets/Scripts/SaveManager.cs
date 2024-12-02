using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private string savePath;

    public string defaultSceneToLoad;

    [System.NonSerialized] public string sceneToSave;
    [System.NonSerialized] public List<string> itemsToSave;
    [System.NonSerialized] public List<string> powersToSave;
    [System.NonSerialized] public List<string> bossesToSave;
    [System.NonSerialized] public float timeToSave;

    [System.NonSerialized] public string sceneSaved;
    [System.NonSerialized] public List<string> itemsSaved;
    [System.NonSerialized] public List<string> powersSaved;
    [System.NonSerialized] public List<string> bossesSaved;
    [System.NonSerialized] public float timeSaved;
    [System.NonSerialized] public bool leftCoinSaved;
    [System.NonSerialized] public bool rightCoinSaved;


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
        bossesToSave = new List<string>();
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
            bossesSaved = new List<string>(loadedData.bosses);
            timeSaved = loadedData.time;
            leftCoinSaved = loadedData.leftCoin;
            rightCoinSaved = loadedData.rightCoin;

            itemsToSave = itemsSaved;
            powersToSave = powersSaved;
            bossesToSave = bossesSaved;

            Debug.Log($"Game loaded successfully. Scene: {sceneSaved}, Items: {string.Join(", ", itemsSaved)}, Powers: {string.Join(", ", powersSaved)}, Bosses: {string.Join(", ", bossesSaved)}");

            InventoryManager.instance.SetCollectedItems(itemsSaved);
            
            VitalEnergyManager.instance.SetMaxEnergyTime(timeSaved);

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

            SceneManager.LoadScene(sceneSaved);



        }
        else
        {
            sceneSaved = null;
            itemsSaved = new List<string>();
            powersSaved = new List<string>();
            bossesSaved = new List<string>();
            timeSaved = 300;
            leftCoinSaved = false;
            rightCoinSaved = false;

            itemsToSave = itemsSaved;
            powersToSave = powersSaved;
            bossesToSave = bossesSaved;

            

            SceneManager.LoadScene(defaultSceneToLoad);
            Debug.LogWarning("New save created.");
        }

        GameManager.instance.SetCoin(leftCoinSaved, rightCoinSaved);

        VitalEnergyManager.instance.ResetTimer();
        LifePointsManager.instance.SetHpTo(LifePointsManager.instance.GetMaxHp());
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
            bosses = this.bossesToSave,
            time = this.timeToSave,
            leftCoin = GameManager.hasLeftCoinPart,
            rightCoin = GameManager.hasRightCoinPart
        };


        string json = JsonUtility.ToJson(saveData, true); 
        File.WriteAllText(savePath, json); 
        Debug.Log("Game saved successfully.");
    }

}
