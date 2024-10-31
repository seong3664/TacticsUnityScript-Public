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
            Debug.Log("작동하는거니");
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
        Debug.Log("유닛 데이터 저장 완료");
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
            Debug.Log("파일이 존재하지 않습니다.");
        }
       
    }
    public void LoadGame()
    {
        if (File.Exists(path + nowSlot.ToString() + ".json"))
        {
            // 씬 이름이 다르면 해당 씬으로 이동 후 로드
            if (SceneManager.GetActiveScene().name != loadedData.sceneName)
            {
                // 씬 로드 후 유닛 데이터를 불러오기 위해 이벤트 구독
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(loadedData.sceneName);
            }
            else
            {
                // 현재 씬이 동일할 경우 바로 유닛 데이터 적용
                ApplyLoadedUnits(loadedData, modelPrefabs);
            }
        }
        else
        {
            Debug.Log("저장된 유닛 데이터가 없습니다.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후 유닛 데이터를 불러오기
        ApplyLoadedUnits(loadedData, modelPrefabs);

        // 이벤트 구독 해제
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
                Debug.LogWarning($"유닛타입 '{unitData.statData.unitCode}'에 해당하는 프리팹을 찾을 수 없습니다.");
            }
        }
       
    }
    
    public void DataClear()
    {
        nowSlot = -1;
        loadedData = new UnitSaveData();
    }
}
