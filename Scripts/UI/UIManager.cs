using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Goal goal;
    [SerializeField] GameObject restart;
    [SerializeField] GameObject result;
    [SerializeField] GameObject turnArrow;
    private PlayerController playerController;
    private float timer;

    private bool magnitudeReached;
    private float lastMagnitude;

    private GameManager gameManager;
    void Start()
    {
        timer = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        magnitudeReached = false;
        lastMagnitude = 0;

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        TimerTick();
        CheckRestart();
        CheckResults();
        TurnArrow();
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

    private void TurnArrow() {
        
        turnArrow.SetActive(playerController.hittedHook);
        if (playerController.hittedHook){
            turnArrow.transform.localScale = playerController.tempVelocity.x >= 0 ?
            new Vector3(turnArrow.transform.localScale.x * 1, turnArrow.transform.localScale.y, turnArrow.transform.localScale.z) :
            new Vector3(turnArrow.transform.localScale.x * -1, turnArrow.transform.localScale.y, turnArrow.transform.localScale.z);
            ;
            float number = playerController.getMagnitude();
            if(!magnitudeReached)magnitudeReached = Mathf.Abs(number) <= 1f;
            turnArrow.GetComponent<Image>().enabled = magnitudeReached;
            if (Mathf.Sign(playerController.tempVelocity.x) != lastMagnitude && magnitudeReached) {
                magnitudeReached = false;
                turnArrow.GetComponent<Image>().enabled = false;
                //turnArrow.transform.localScale = new Vector3(-turnArrow.transform.localScale.x, turnArrow.transform.localScale.y, turnArrow.transform.localScale.z);
            }
            lastMagnitude = Mathf.Sign(playerController.tempVelocity.x);
        }
        
        


    }
    public void Restart() {
        gameManager.ReloadScene();
    }
}
