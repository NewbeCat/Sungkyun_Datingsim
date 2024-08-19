using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    [SerializeField] private string selectedProfileId = "1"; // 기본값을 숫자로 설정

    private Coroutine autoSaveCoroutine;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        // Initialize the selected profile ID at the start
        InitializeSelectedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        // delete the data for this profile id
        dataHandler.Delete(profileId);
        // initialize the selected profile id
        InitializeSelectedProfileId();
        // reload the game so that our data matches the newly selected profile id
        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();

        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }

        // If no profile ID exists (e.g., first game), generate a new one
        if (string.IsNullOrEmpty(this.selectedProfileId))
        {
            this.selectedProfileId = "1"; // 첫 번째 프로필은 기본적으로 1로 설정
            Debug.Log("No profile ID found. Generated new profile ID: " + this.selectedProfileId);
        }
    }

    public void NewGame()
    {
        // Assign a new profile ID as the next available number
        int newProfileId = GetNextProfileId();
        this.selectedProfileId = newProfileId.ToString();
        this.gameData = new GameData();

        Debug.Log("Created new GameData with profile ID: " + this.selectedProfileId);
        SaveGame(); // Save the new game data with the newly created profile ID
    }

    private int GetNextProfileId()
    {
        // Load all existing profiles and find the highest profile ID
        var allProfiles = dataHandler.LoadAllProfiles();
        if (allProfiles == null || allProfiles.Count == 0)
        {
            return 1; // 첫 번째 프로필 ID는 1로 설정
        }

        // 프로필 ID는 문자열이므로 숫자로 변환하여 최대값을 찾음
        int maxProfileId = allProfiles.Keys
            .Select(id => int.Parse(id)) // 문자열 ID를 숫자로 변환
            .Max();

        // 다음 프로필 ID는 현재 최대값 + 1
        return maxProfileId + 1;
    }

    public void LoadGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence)
        {
            return;
        }

        // load any saved data from a file using the data handler
        this.gameData = dataHandler.Load(selectedProfileId);

        // if no data can be loaded, don't continue
        if (this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence)
        {
            return;
        }

        // if we don't have any data to save, log a warning here
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            Debug.Log("Saving data for: " + dataPersistenceObj.GetType().Name);
            dataPersistenceObj.SaveData(gameData);
        }

        gameData.profileID = selectedProfileId;
        // timestamp the data so we know when it was last saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        dataHandler.Save(gameData, selectedProfileId);
        Debug.Log("Game data saved for profile ID: " + selectedProfileId);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        // 데이터 지속성이 비활성화된 경우 false 반환
        if (disableDataPersistence)
        {
            return false;
        }

        // 모든 프로필의 저장 데이터를 불러옴
        Dictionary<string, GameData> allProfilesGameData = dataHandler.LoadAllProfiles();

        // 불러온 데이터 중 하나라도 유효한 데이터가 있으면 true 반환
        if (allProfilesGameData != null && allProfilesGameData.Values.Any(data => data != null))
        {
            return true;
        }

        // 유효한 데이터가 없으면 false 반환
        return false;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
