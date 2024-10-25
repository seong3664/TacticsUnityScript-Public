using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState { PlayerTurn, WaitingTurn, EnemyTurn, GameOver }
public class turnManager : MonoBehaviour
{

    static turnManager turnmanager;
   
    public static turnManager TurnManager
    {
        private set
        {
            if (turnmanager == null)
                turnmanager = value;
            else 
                Destroy(value.gameObject);
        }
        get
        {
            return turnmanager;
        }
    }
    
    private TurnState State = TurnState.PlayerTurn;
    public TurnState state
    {
        get { return State; }
        set { State = value; }
    }
   

    public delegate void TurnEndEvent();
    public static TurnEndEvent EndTurn;
    public delegate void PlayerTurnEndEvent();
    public static PlayerTurnEndEvent PlayerEndTurn;
    public delegate void EnemyTurnEndEvent();
    public static EnemyTurnEndEvent EnemyEndTurn;


    private void Awake()
    {
        TurnManager = this;
      
    }
    private void Start()
    {
        state = TurnState.PlayerTurn;
    }
    public void EndPlayerTunr()
    {
        state = TurnState.EnemyTurn;
        EndTurn();
        PlayerEndTurn();
    }
    public void EndEnemyturn()
    {
        state = TurnState.PlayerTurn;
        EndTurn();
        EnemyEndTurn();


    }
}

    


