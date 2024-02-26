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
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] AudioSource source;
    private PlayerController playerController;
    private float timer;
    private DeveloperTime developerTime;

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
        developerTime = ScriptableObject.CreateInstance<DeveloperTime>();
        developerTime.InitializeStageTime();
        StartCoroutine(CountDown());

    }

    // Update is called once per frame
    void LateUpdate()
    {
        TimerTick();
        CheckRestart();
        CheckResults();
        TurnArrow();
        CheckPause();
    }
    private void TimerTick()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().isPaused && !goal.isBeaten)
        {
            timer += Time.deltaTime;
        }
        timerText.text = TimeFormat(timer);

    }

    private void CheckPause() {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.isPaused && !goal.isBeaten) {
            Time.timeScale = 0f;
            gameManager.isPaused = true;
            pauseScreen.SetActive(true);
        }

    }
    public void UnPause(){
        pauseScreen.SetActive(false);
        gameManager.isPaused = false;
        source.Play();
        StartCoroutine(CountDown());
        
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

    private IEnumerator CountDown() {
        gameManager.isPaused = true;
        playerController.canControl = false;
        countDownText.gameObject.SetActive(true);
        countDownText.text = "3";
        yield return new WaitForSecondsRealtime(1);
        countDownText.text = "2";
        yield return new WaitForSecondsRealtime(1);
        countDownText.text = "1";
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        playerController.canControl = true;
        countDownText.text = "Go!";
        gameManager.isPaused = false;
        yield return new WaitForSecondsRealtime(1);
        countDownText.gameObject.SetActive(false);
    }

    private void CheckResults() {
        bool isOver = gameManager.displayResults ? true: false;
        result.SetActive(isOver);
        if (result.activeSelf) {
            CalculateMedalsTime();
            int stageNumber = gameManager.GetStageNumber();
            string stageKey = stageNumber.ToString();

            if (PlayerPrefs.HasKey(stageKey))
            {
                float bestTime = PlayerPrefs.GetFloat(stageKey);

                if (bestTime == 0 || timer < bestTime)
                {
                    PlayerPrefs.SetFloat(stageKey, timer);
                }
            }
            else
            {
                PlayerPrefs.SetFloat(stageKey, timer);
            }
            result.transform.Find("SlugOS").transform.Find("ResultText").GetComponent<TextMeshProUGUI>().text = "Best Time: " + TimeFormat(PlayerPrefs.GetFloat("" + stageNumber)) + "<br>" + "Your Time: " + TimeFormat(timer);
        }
    }

    private void CalculateMedalsTime() {
        GameObject slugOS = result.transform.Find("SlugOS").gameObject;
        GameObject medals = slugOS.transform.Find("Medals").gameObject;
        GameObject bronzeMedal = medals.transform.Find("BronzeMedal").gameObject;
        GameObject silverMedal = medals.transform.Find("SilverMedal").gameObject;
        GameObject goldMedal = medals.transform.Find("GoldMedal").gameObject;
        GameObject developerMedal = medals.transform.Find("DeveloperMedal").gameObject;
        float devTime = developerTime.stageTime[gameManager.GetStageNumber()];

        float bronzeTime = devTime * 6;
        float silverTime = devTime * 2;
        float goldTime = devTime * 1.5f;

        bronzeMedal.SetActive(timer < bronzeTime);
        silverMedal.SetActive(timer < silverTime);
        goldMedal.SetActive(timer < goldTime);
        developerMedal.SetActive(timer < devTime);

        bronzeMedal.GetComponentInChildren<TextMeshProUGUI>().text = TimeFormat(bronzeTime);
        silverMedal.GetComponentInChildren<TextMeshProUGUI>().text = TimeFormat(silverTime);
        goldMedal.GetComponentInChildren<TextMeshProUGUI>().text = TimeFormat(goldTime);
        developerMedal.GetComponentInChildren<TextMeshProUGUI>().text = TimeFormat(devTime);
    }
    private void TurnArrow() {
        
        turnArrow.SetActive(playerController.hittedHook);
        if (playerController.hittedHook){

            turnArrow.transform.localScale = new Vector3(-Mathf.Sign(playerController.tempVelocity.x) * Mathf.Abs(turnArrow.transform.localScale.x), turnArrow.transform.localScale.y, turnArrow.transform.localScale.z);
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

    private string TimeFormat(float time) {
        string resultingTime;
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int ms = (int)(time * 100) % 100;
        string min = minutes < 10? "0" + minutes: ""+minutes;
        string sec = seconds < 10 ? "0" + seconds : "" + seconds;
        string msText = ms < 10 ? "0" + ms : "" + ms;
        resultingTime = min + ":" + sec + ":" + msText;
        return resultingTime;
    }
    public void Restart() {
        source.Play();
        gameManager.ReloadScene();
    }
    public void NextStage() {
        source.Play();
        gameManager.NextStage();
    }

    public void GoToMenu() {
        Time.timeScale = 1;
        source.Play();
        gameManager.GoToMenu();
    }
}
