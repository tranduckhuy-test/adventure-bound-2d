using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] bool isGameStart;
    public bool IsGameStart
    {
        get { return isGameStart; }
    }

    public bool IsRespawn { get; set; }

    GameObject player;

    [SerializeField] Animator transitionAnim;
    string current_role;

    public static event Action<GameObject> OnPlayerSpawned;
    AsyncOperationHandle<GameObject> asyncOperationHandle;
    AsyncOperationHandle<SceneInstance> sceneInstance;


    string path = Application.dataPath + "/streamingAssets";

    public static GameManager instance;
    public UIManager uiMgr;

    void Awake()
    {
        // Keep GameManager from being destroyed after scene changes
        DontDestroyOnLoad(this.gameObject);


        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Subscribe to scene unloaded event
        SceneManager.sceneUnloaded += OnSceneUnloaded;

    }

    private void OnSceneUnloaded(Scene current)
    {   // Release memory
        if (asyncOperationHandle.IsValid())
        {
            StartCoroutine(DelayedRelease());
        }
    }


    // Unsubscribe
    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // Used to observe asynchronous download progress
    private IEnumerator ShowLoadingProgress(AsyncOperationHandle<GameObject> handle)
    {
        while (!handle.IsDone)
        {
            // Print current progress
            // Debug.Log($"Loading Progress: {handle.PercentComplete * 100}%");

            // Wait for the next frame to check progress
            yield return null;
        }

        // Print 100% after loading is complete
        // Debug.Log("Loading Progress: 100%");
    }

    // Load the next scene (index num will be incremented when changing scenes)
    public void LoadNextScene()
    {
        transitionAnim.SetTrigger("End");
        // Debug.Log("LoadNextLevel is being called.");
        StartCoroutine(LoadLevelAsset("Level_0" + (LevelManager.instance.current_Level + 1)));
        transitionAnim.SetTrigger("Start");

    }

    // Asynchronously load the level asset to be switched
    IEnumerator LoadLevelAsset(string level)
    {
        if (IsRespawn)
        {
            IsRespawn = false;
        }
        else
        {
            LevelManager.instance.current_Level++;
        }

        if (LevelManager.instance.current_Level > LevelManager.instance.MaxLevel)
        {
            LevelManager.instance.current_Level = 0;
            level = "Level_0" + LevelManager.instance.current_Level;
        }

        sceneInstance = Addressables.LoadSceneAsync(level, LoadSceneMode.Single, false);
        yield return sceneInstance; // Wait for sceneInstance to finish loading before continuing to the next step

        sceneInstance.Result.ActivateAsync().completed += (operation) =>
        {
            // Debug.Log("Scene loaded: " + level);

            // Load player character here
            current_role = LevelManager.instance.current_role;
            LoadRole(current_role);
            uiMgr = FindObjectOfType<UIManager>();

        };
    }

    // Delay one frame to ensure smooth scene transition before releasing resources
    private IEnumerator DelayedRelease()
    {
        // Delay one frame
        yield return null;

        if (asyncOperationHandle.IsValid())
        {
            Addressables.Release(asyncOperationHandle);
        }

        if (sceneInstance.IsValid())
        {
            Addressables.Release(sceneInstance);
        }

        // Debug.Log("Resources released");
    }

    // Asynchronously load player asset and instantiate it
    void LoadRole(string role)
    {
        // print("current role: " + current_role);
        // Check if loading is successful -> instantiate object, otherwise print error message
        asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(role);
        StartCoroutine(ShowLoadingProgress(asyncOperationHandle));
        asyncOperationHandle.Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded && isGameStart)
            {
                // print("instantiate player");
                player = Instantiate(handle.Result);
                // Check if OnPlayerSpawned is subscribed, if yes, pass player as parameter, otherwise return null
                OnPlayerSpawned?.Invoke(player);

            }
            else
            {
                Debug.Log("Failed to load character");
            }
        };

    }

    // Function for start game button
    public void ClickStartGame()
    {
        SFXManager.instance.ClickSound();
        isGameStart = true;
        LoadNextScene();

    }

    // Return to game menu
    public void BackToMenu()
    {
        SFXManager.instance.ClickSound();
        SceneManager.LoadScene("Level_00");
        LevelManager.instance.current_Level = 0;
        Time.timeScale = 1;
        isGameStart = false;

    }

    public void QuitGame()
    {
        SFXManager.instance.ClickSound();
        Application.Quit();

    }

    // Unhide the door originally hidden in the level
    public void LevelFinished()
    {
        SFXManager.instance.DoorAppear();
        uiMgr.UnhideDoor();
    }

    // Save game progress
    public void Save()
    {
        SFXManager.instance.ClickSound();
        SaveData saveData = new SaveData();
        int currentScene = LevelManager.instance.current_Level;
        string selectedRole = LevelManager.instance.current_role;

        // Write current level number
        saveData.levelNumber = currentScene;

        // Write selected role
        saveData.selectedRole = selectedRole;

        // Convert to JSON and save
        string jsonFile = JsonUtility.ToJson(saveData);
        File.WriteAllText(path + "/save.json", jsonFile);

    }

    // Load saved game
    public void Load()
    {
        SFXManager.instance.ClickSound();

        var data = ReadJson.Instance.GetSavedData();
        int levelNum = data.Item1;
        string role = data.Item2;
        isGameStart = true;

        if (IsRespawn)
        {
            levelNum = LevelManager.instance.current_Level;
            role = LevelManager.instance.current_role;
        }

        LevelManager.instance.current_role = role;
        if (levelNum != 0)
        {
            StartCoroutine(LoadLevelAsset("Level_0" + levelNum));
            LevelManager.instance.current_Level = levelNum;
        }
        else
        {
            print("You dont have any saved file");
        }
    }

    // Check isGamePause status
    public bool GetGamePauseStatus()
    {
        return uiMgr.IsGamePause;
    }

    public void PlayerDied()
    {
        uiMgr.ShowDeadMenu();
    }

    public void PlayerWin()
    {
        uiMgr.ShowWinMenu();
    }
}
