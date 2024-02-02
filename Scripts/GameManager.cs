using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Goal goal;
    private float timer;

    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused && !goal.isBeaten) {
            timer += Time.deltaTime;
        }
        timerText.text = timer.ToString();
        if (goal.isBeaten) {
            Invoke("NextStage", 0.2f);
        }
    }
    private void NextStage() {
        Time.timeScale = 0f;
        GameObject.Find("Player").GetComponent<PlayerController>().canControl = false;
    }


}
