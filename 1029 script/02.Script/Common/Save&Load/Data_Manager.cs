using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
