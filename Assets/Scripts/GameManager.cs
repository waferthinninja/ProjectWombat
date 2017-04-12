using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {

    //MAKE INSTANCE
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    //END MAKE INSTANCE

    public GameState GameState;


    void Start()
    {
        UpdateGameState(GameState.Planning);
    }

    public 

    void UpdateGameState(GameState gameState)
    {
        GameState = gameState;     
    }

}


