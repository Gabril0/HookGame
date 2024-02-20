using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    
    private PlayerController playerController;
    [SerializeField] Goal goal;
    public bool displayResults {get;private  set; }

    public bool isPaused;
    void Start()
    {
        displayResults = false;
        isPaused = true;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.canControl = false;
    }

    void Update()
    {
        if (goal.isBeaten)
        {
            Invoke("EndStage", 0.2f);
        }

    }

    public int GetStageNumber() {
        string name = SceneManager.GetActiveScene().name;
        string fixedName = name.Replace("Level", "");
        return int.Parse(fixedName);
    }

    public void ReloadScene() {
        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    private void EndStage() {
        GameObject.Find("Player").GetComponent<PlayerController>().canControl = false;
        displayResults = true;
    }
    public void NextStage() {
        
    }


}
