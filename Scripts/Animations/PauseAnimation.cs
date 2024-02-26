using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimation : MonoBehaviour
{
    [SerializeField] RectTransform buttons;
    [SerializeField] RectTransform scanlines;
    [SerializeField] RectTransform osAndSlug;
    RectTransform slugRoll;

    private float buttonsTime;
    private float scanlinesTime;
    private float osAndSlugTime;


    private float normalizedButtonsTargetX = 0.5f; 
    private float normalizedScanlinesTargetX = 0.5f;
    private float normalizedOsAndSlugTargetX = 0.5f;

    void OnEnable()
    {
        buttons.position = new Vector3(-1000, buttons.position.y, buttons.position.z);
        scanlines.position = new Vector3(-1500, scanlines.position.y, scanlines.position.z);
        osAndSlug.position = new Vector3(-2000, osAndSlug.position.y, osAndSlug.position.z);

        slugRoll = osAndSlug.transform.Find("SlugRoll").GetComponent<RectTransform>();

        buttonsTime = 0;
        scanlinesTime = 0;
        osAndSlugTime = 0;
    }

    void Update()
    {
        buttonsTime += Time.fixedUnscaledDeltaTime;
        scanlinesTime += Time.fixedUnscaledDeltaTime / 2;
        osAndSlugTime += Time.fixedUnscaledDeltaTime / 4;

        float buttonsTargetX = Screen.width * normalizedButtonsTargetX;
        float scanlinesTargetX = Screen.width * normalizedScanlinesTargetX;
        float osAndSlugTargetX = Screen.width * normalizedOsAndSlugTargetX;

        buttons.position = new Vector3(Mathf.Lerp(buttons.position.x, buttonsTargetX, buttonsTime), buttons.position.y, buttons.position.z);
        scanlines.position = new Vector3(Mathf.Lerp(scanlines.position.x, scanlinesTargetX, scanlinesTime), scanlines.position.y, scanlines.position.z);
        osAndSlug.position = new Vector3(Mathf.Lerp(osAndSlug.position.x, osAndSlugTargetX, osAndSlugTime), osAndSlug.position.y, osAndSlug.position.z);

        slugRoll.eulerAngles = new Vector3(0, 0, buttonsTime * 10);
    }
}
