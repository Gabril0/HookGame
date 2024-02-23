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
    void OnEnable()
    {
        buttons.position = new Vector3(-1000,buttons.position.y,buttons.position.z);
        scanlines.position = new Vector3(-1500, scanlines.position.y, scanlines.position.z);
        osAndSlug.position = new Vector3(-2000, osAndSlug.position.y, osAndSlug.position.z);
        slugRoll = osAndSlug.transform.Find("SlugRoll").GetComponent<RectTransform>();
        buttonsTime = 0;
        scanlinesTime = 0;
        osAndSlugTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        buttonsTime += Time.fixedUnscaledDeltaTime;
        scanlinesTime += Time.fixedUnscaledDeltaTime / 2;
        osAndSlugTime += Time.fixedUnscaledDeltaTime / 4;
        buttons.position = new Vector3(Mathf.Lerp(buttons.position.x,405,buttonsTime), buttons.position.y, buttons.position.z);
        scanlines.position = new Vector3(Mathf.Lerp(scanlines.position.x, 405, scanlinesTime), scanlines.position.y, scanlines.position.z);
        osAndSlug.position = new Vector3(Mathf.Lerp(osAndSlug.position.x, 405, osAndSlugTime), osAndSlug.position.y, osAndSlug.position.z);
        slugRoll.eulerAngles = new Vector3(0, 0, buttonsTime * 10);

    }
}
