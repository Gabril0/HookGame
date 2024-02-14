using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    
    private PlayerController playerController;
    [SerializeField] Goal goal;
    public bool displayResults {get;private  set; }

    public bool isPaused { get; private set;  }
    void Start()
    {
        displayResults = false;
        isPaused = false;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (goal.isBeaten)
        {
            Invoke("EndStage", 0.2f);
        }

    }

    public void ReloadScene() {
        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    private void EndStage() {
        Time.timeScale = 0f;
        GameObject.Find("Player").GetComponent<PlayerController>().canControl = false;
        displayResults = true;
    }

    public void NextStage() {
        
    }


}
