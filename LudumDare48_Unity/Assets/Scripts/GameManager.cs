using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public delegate void FloorFall();
    public FloorFall floorFall;

    public delegate void GameOverEvent();
    public GameOverEvent gameOver;
    public GameOverEvent nextLevel;

    [SerializeField] SO_Spawner[] m_spawnersdata;
    int m_spawnersdataIndex = 0;

    public bool frontSpawner = false;

    public void NextLevel()
    {
        Debug.LogFormat("NextLevel");

        frontSpawner = !frontSpawner;

        if (++m_spawnersdataIndex >= m_spawnersdata.Length)
            m_spawnersdataIndex = m_spawnersdata.Length - 1;

        nextLevel.Invoke();
        floorFall.Invoke();
    }
    public void GameOver()
    {
        Debug.LogFormat("GameOver");

        frontSpawner = false;
        m_spawnersdataIndex = 0;

        nextLevel.Invoke();
        gameOver.Invoke();
    }

    void Start()
    {
        PauseGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public bool IsPaused()
    {
        return Time.timeScale == 0;
    }


    public SO_Spawner GetSpawnerData()
    {
        return m_spawnersdata[m_spawnersdataIndex];
    }
}