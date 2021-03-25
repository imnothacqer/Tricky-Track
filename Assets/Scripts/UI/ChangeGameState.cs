using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGameState : MonoBehaviour
{
    public GameState newState;
    
    public void Change()
    {
        GameManager.instance.currentGameState = newState;
    }
}
