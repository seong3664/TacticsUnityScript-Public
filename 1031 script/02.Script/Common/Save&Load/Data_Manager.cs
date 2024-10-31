using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class Data_Manager : MonoBehaviour
{
    private static Data_Manager instance;
    public static Data_Manager Instance 
    {  
        get { return instance; }
        private set 
        { if (instance == null)
                 instance = value;
          else if(instance != null)
                Destroy(instance.gameObject);
        }
    }
    UnitSaveData saveData = new UnitSaveData();
    public Dictionary<string, GameObject> modelPrefabs;
    GameObject playerPrefab;
    GameObject enemyPrefab;
    public string path;
    public int nowSlot;
    UnitSaveData loadedData = new UnitSaveData();
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        path = Application.persistentDataPath + "/save";
    }
    void Start()
    {
        playerPrefab = GameManager.gamemaneger.playerPrefab;
        enemyPrefab = GameManager.gamemaneger.Enemyprefab;
        modelPrefabs = new Dictionary<string, GameObject>()
    {
        { "Player", playerPrefab },
        { "Enemy", enemyPrefab },
    };
    }
    public void SaveUnitData()
    {
        
        saveData.sceneName = SceneManager.GetActiveScene().name;
        saveData.units.Clear();
        UnitStat[] Units = FindObjectsOfType<UnitStat>();
        Debug.Log(Units.Length);
        foreach (UnitStat unitStat in Units)
        {
            Debug.Log("�۵��ϴ°Ŵ�");
            Stat stat = unitStat.stat;
            Transform UnitTr= unitStat.transform;
            if (stat != null && UnitTr != null)
            {
                UnitData unistdata = new UnitData(stat, UnitTr);
                Debug.Log(unistdata.statData.dmg);
                saveData.units.Add(unistdata);
               
            }
        }
       
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path + nowSlot.ToString()+".json", json);
        Debug.Log("���� ������ ���� �Ϸ�");
    }
    public void LoadUnits(bool loadGame)
    {
        Debug.Log(nowSlot);
        if (File.Exists(path + nowSlot.ToString() + ".json"))
        {
            string json = File.ReadAllText(path + nowSlot.ToString() + ".json");
            loadedData = JsonUtility.FromJson<UnitSaveData>(json);
            if (loadGame)
                LoadGame();
        }
        else
        {
            Debug.Log("������ �������� �ʽ��ϴ�.");
        }
       
    }
    public void LoadGame()
    {
        if (File.Exists(path + nowSlot.ToString() + ".json"))
        {
            // �� �̸��� �ٸ��� �ش� ������ �̵� �� �ε�
            if (SceneManager.GetActiveScene().name != loadedData.sceneName)
            {
                // �� �ε� �� ���� �����͸� �ҷ����� ���� �̺�Ʈ ����
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(loadedData.sceneName);
            }
            else
            {
                // ���� ���� ������ ��� �ٷ� ���� ������ ����
                ApplyLoadedUnits(loadedData, modelPrefabs);
            }
        }
        else
        {
            Debug.Log("����� ���� �����Ͱ� �����ϴ�.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� �� ���� �����͸� �ҷ�����
        ApplyLoadedUnits(loadedData, modelPrefabs);

        // �̺�Ʈ ���� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void ApplyLoadedUnits(UnitSaveData loadedData, Dictionary<string, GameObject> modelPrefabs)
    {
        Debug.Log(loadedData.units.Count);
        foreach (UnitData unitData in loadedData.units)
        {
            if (modelPrefabs.TryGetValue(unitData.statData.unitCode, out GameObject prefab))
            {
                GameObject unit = Instantiate(prefab, unitData.position, unitData.rotation);
                Stat stat = ScriptableObject.CreateInstance<Stat>();
                if (stat != null)
                {
                    stat.dmg = unitData.statData.dmg;
                    stat.Hp = unitData.statData.hp;
                    stat.MovePoint = unitData.statData.movePoint;
                    stat.Action = unitData.statData.actionPoint;
                    stat.Aiming = unitData.statData.aiming;
                    stat.Evasion = unitData.statData.evasion;
                    stat.Crit = unitData.statData.crit;
                    stat.ScopeOnoff = unitData.statData.scopeOnoff;
                    stat.VestOnoff = unitData.statData.vestOnoff;
                    stat.MuzzleOnoff = unitData.statData.muzzleOnoff;
                    
                }
                if (unitData.statData.unitCode == "Player")
                {
                    stat.UnitCode = UnitCode.Player;
                    GameManager.gamemaneger.PlayerList.Add(unit);

                }
                else if (unitData.statData.unitCode == "Enemy")
                {
                    stat.UnitCode = UnitCode.Enemy;
                    GameManager.gamemaneger.EnemyList.Add(unit);
                }
                unit.GetComponent<UnitStat>().stat = stat;

               
            }
            else
            {
                Debug.LogWarning($"����Ÿ�� '{unitData.statData.unitCode}'�� �ش��ϴ� �������� ã�� �� �����ϴ�.");
            }
        }
       
    }
    
    public void DataClear()
    {
        nowSlot = -1;
        loadedData = new UnitSaveData();
    }
}
