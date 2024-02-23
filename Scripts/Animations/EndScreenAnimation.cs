using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenAnimation : MonoBehaviour
{
    [SerializeField] RectTransform bgRect;
    [SerializeField] RectTransform slugOSRect; 

    private float bgAnimationTime;
    private float slugOSAnimationTime;
    void OnEnable()
    {
        slugOSRect.position = new Vector3(slugOSRect.position.x , - 990, slugOSRect.position.z);
        bgRect.position = new Vector3(bgRect.position.x, 1333, bgRect.position.z);
        slugOSAnimationTime = 0;
        bgAnimationTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        slugOSAnimationTime += Time.deltaTime;
        bgAnimationTime += Time.deltaTime;
        bgRect.position = new Vector3(bgRect.position.x, Mathf.Lerp(1333, 162.1f, bgAnimationTime), bgRect.position.z);
        slugOSRect.position = new Vector3(slugOSRect.position.x, Mathf.Lerp(-990, 223.5f, slugOSAnimationTime), slugOSRect.position.z);
    }
}
