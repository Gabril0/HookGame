using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] GameObject stageSelect;
    public void NewRun() {
        SceneManager.LoadScene("Level1");
    }
    public void DisplayLevelScreen() {
        text.SetActive(false);
        stageSelect.SetActive(true);
    }
    public void StageClick(int stage) {
        SceneManager.LoadScene("Level" + stage);
    }
    public void Exit() {
        Application.Quit();
    }
}
