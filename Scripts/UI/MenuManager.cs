using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] GameObject stageSelect;
    [SerializeField] AudioSource source;
    public void NewRun() {
        source.Play();
        SceneManager.LoadScene("Level1");
    }
    public void DisplayLevelScreen() {
        source.Play();
        text.SetActive(false);
        stageSelect.SetActive(true);
    }
    public void StageClick(int stage) {
        source.Play();
        SceneManager.LoadScene("Level" + stage);
    }
    public void Exit() {
        source.Play();
        Application.Quit();
    }
}
