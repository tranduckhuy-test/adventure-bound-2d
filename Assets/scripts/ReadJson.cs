using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ReadJson : MonoBehaviour
{
    public static ReadJson Instance;

    RootRole rootRole = new RootRole();
    int level = 0;
    string selectedRole = "";
    public int Level { get { return level; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        ReadMonsterValue();

        DontDestroyOnLoad(gameObject);
    }

    // Read monster values
    void ReadMonsterValue()
    {
        // Read the json file under the resource folder
        // Note that we are using .text here
        string path = Application.dataPath + "/streamingAssets" + "/enemyValue.json";
        string info = File.ReadAllText(path);

        // The data obtained here is of type RootRole, which lists all the monster values
        rootRole = JsonConvert.DeserializeObject<RootRole>(info);

    }

    // Get monster value by inputting the index
    public Role GetMonsterValue(int monsterIndex)
    {
        Role monster = rootRole.roles[monsterIndex - 1];
        return monster;
    }

    // Read saved data and return level and selected role
    public (int, string) GetSavedData()
    {
        // Path to the save file
        string path = Application.dataPath + "/streamingAssets/save.json";

        if (File.Exists(path))
        {
            string info = File.ReadAllText(path);
            SaveData data = JsonConvert.DeserializeObject<SaveData>(info);
            level = data.levelNumber;
            selectedRole = data.selectedRole;

            return (level, selectedRole);
        }
        else { return (0, "Role01"); }
    }
}
