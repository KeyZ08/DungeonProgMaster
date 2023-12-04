using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly LevelsHandler LevelsHandler = new();

    public void Start()
    {
        Debug.Log("Game started!");
        for (var i = 0; i < LevelsHandler.Levels.Count; i++)
        {
            //var isFinished = false;
            Debug.Log("Your map:");
            Debug.Log(LevelsHandler.Levels[0].Map.ToString());
            Debug.Log("Player start position:");
            Debug.Log(LevelsHandler.Levels[0].Player.Position.StartPosition);
        }
    }
}
