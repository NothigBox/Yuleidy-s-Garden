using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [SerializeField] GameObject homePanel;
    public void Endless() 
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Levels()
    {
        homePanel.SetActive(false);
    }

    public void Credits()
    {

    }
}
