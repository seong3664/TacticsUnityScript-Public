using States;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gamemaneger { get; private set; }

    [SerializeField]
    private List<Stat> stats = new List<Stat>();
    public List<Stat> Stats {
        get { return stats; }
        set { stats = value; }
                            }

    private GameObject playerPrefab;
    public List<GameObject> PlayerList = new List<GameObject>();
    private GameObject Enemyprefab;
    [SerializeField]
    public List<GameObject> EnemyList = new List<GameObject>();
    private void Awake()
    {
        if (gamemaneger == null)
        {
            gamemaneger = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
        playerPrefab = Resources.Load("Player 1") as GameObject;
        Enemyprefab = Resources.Load("Enemy") as GameObject;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle_Scenes")
        {
            CreateUnit();
        }
    }
    private void CreateUnit()
    {

        Transform[] playerSpawnTransforms = GameObject.Find("Player_Spawn").GetComponentsInChildren<Transform>();

        for (int i = 1; i < playerSpawnTransforms.Length; i++) // 1부터 시작하여 부모를 건너뜀
        {
            Transform t = playerSpawnTransforms[i];
            GameObject newUnit = Instantiate(playerPrefab, t.position, Quaternion.identity);
            PlayerList.Add(newUnit);
            
            UnitStat unitStat = newUnit.GetComponent<UnitStat>();
            if(unitStat.stat != null)
            {
                unitStat.stat = null;
            }
            unitStat.stat = stats[i - 1];
            unitStat.UpdateEquipOnoff();
        }
        for (int i = 0; i < 6; i++)
        {
            Vector3 Randompos = new Vector3(Random.Range(-29,18),0f,Random.Range(16,24));
            EnemyList.Add(Instantiate(Enemyprefab, Randompos, Enemyprefab.transform.rotation));
        }
    }
}
