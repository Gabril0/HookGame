using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMedal : MonoBehaviour
{
    DeveloperTime developerTime;
    void Awake()
    {
        developerTime = ScriptableObject.CreateInstance<DeveloperTime>();
        developerTime.InitializeStageTime();
        float playerTime;
        playerTime = PlayerPrefs.GetFloat(gameObject.name.Replace("Level",""),100000);
        float devTime = developerTime.stageTime[int.Parse(gameObject.name.Replace("Level", ""))];

        float bronzeTime = devTime * 6;
        float silverTime = devTime * 2;
        float goldTime = devTime * 1.5f;

        transform.Find("Bronze").gameObject.SetActive(playerTime < bronzeTime);
        transform.Find("Silver").gameObject.SetActive(playerTime < silverTime);
        transform.Find("Gold").gameObject.SetActive(playerTime < goldTime);
        transform.Find("Dev").gameObject.SetActive(playerTime < devTime);
        Color myYellow = new Color(253f / 255f, 255f / 255f, 100f / 255f);
        GetComponent<Image>().color = playerTime < devTime? myYellow: Color.white;
    }

}
