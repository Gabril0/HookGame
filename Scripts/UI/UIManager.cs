using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Goal goal;
    [SerializeField] GameObject restart;
    [SerializeField] GameObject result;
    private PlayerController playerController;
    private float timer;

    private GameManager gameManager;
    void Start()
    {
        timer = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        TimerTick();
        CheckRestart();
        CheckResults();
    }
    private void TimerTick()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().isPaused && !goal.isBeaten)
        {
            timer += Time.deltaTime;
        }
        timerText.text = timer.ToString();
        
    }

    private void CheckRestart() {
        if (!playerController.isAlive)
        {
            restart.SetActive(true);
        }
        else {
            restart.SetActive(false);
        }
    }

    private void CheckResults() {
        bool isOver = gameManager.displayResults ? true: false;
        result.SetActive(isOver);
        if (result.activeSelf) {
            result.transform.Find("ResultText").GetComponent<TextMeshProUGUI>().text = "Best Developer Time: 00:19:00" + "<br>" + "Your Time: " + timer;
        }
    }
    public void Restart() {
        gameManager.ReloadScene();
    }
}
