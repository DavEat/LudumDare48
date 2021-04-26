using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool startFloorFall = true;

    public delegate void FloorFall();
    public FloorFall floorFall;


    public void GameOver()
    {
        Debug.LogFormat("GameOver");
        SceneManager.LoadScene(0);
    }

    public void Update()
    {
        if (startFloorFall && floorFall != null)
        {
            startFloorFall = false;
            floorFall.Invoke();
        }
    }
}