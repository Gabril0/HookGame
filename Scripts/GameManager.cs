using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Goal goal;
    private PlayerController playerController;
    private float timer;

    private bool isPaused = false;
    void Start()
    {
        timer = 0;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!isPaused && !goal.isBeaten) {
            timer += Time.deltaTime;
        }
        timerText.text = timer.ToString();
        if (goal.isBeaten) {
            Invoke("NextStage", 0.2f);
        }
        if (!playerController.isAlive) {
            Debug.Log("Press Space to restart");
            if (Input.GetKeyDown(KeyCode.Space)) {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentSceneIndex);
            };

        }
    }
    private void NextStage() {
        Time.timeScale = 0f;
        GameObject.Find("Player").GetComponent<PlayerController>().canControl = false;
    }


}
