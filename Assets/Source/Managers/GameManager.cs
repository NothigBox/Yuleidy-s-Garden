using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LevelManager))]
public class GameManager : MonoBehaviour
{
    private const int THRESHOLD = 3;

    private static GameManager instance;

    private MapGrid grid;
    private PlayBall playBall;
    private LevelManager levelManager;
    private ChangeBall changeBallButton;
    
    public GameObject walls;
    public GameObject mud;

    public static GameManager Instance = instance;

    private void Awake()
    {
        if (instance != null) 
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        levelManager = FindObjectOfType<LevelManager>();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += FindResources;
    }

    private void Start()
    {
        levelManager.SetUpLevel();

        Ball.OnBallExecuted += NextBall;
    }

    private void FindResources(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode) 
    {
        switch (scene.name)
        {
            case "Gameplay Scene":
                grid = FindObjectOfType<MapGrid>();
                playBall = FindObjectOfType<PlayBall>();
                levelManager = FindObjectOfType<LevelManager>();
                changeBallButton = FindObjectOfType<ChangeBall>();

                grid.length = levelManager.GridLength;

                changeBallButton.OnBallChanged += NextBall; 
                break;
        }
    }

    int pivot = 0;

    void NextBall(bool checkRows)
    {
        List<int> unfullRows = new List<int>();

        if (checkRows == true) 
        {
            int[] counts = grid.GetRowCounts();

            if (counts.Length >= pivot + THRESHOLD)
            {
                levelManager.GoUp(1f, grid.gameObject, walls, mud);
                pivot += THRESHOLD;
            }
        }

        changeBallButton.SetBallColor(playBall.SetBall(levelManager.GetRandomBall()));
    }
}
