using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenAnimation : MonoBehaviour
{
    [SerializeField] RectTransform bgRect;
    [SerializeField] RectTransform slugOSRect;

    private float bgAnimationTime;
    private float slugOSAnimationTime;

    private float normalizedBgTargetY = 0.9f;
    private float normalizedSlugOSTargetY = 0.5f;

    private float bgMoveAwayOffset = -1000f;
    private float slugOSMoveAwayOffset = -1000f;

    void OnEnable()
    {
        Vector2 bgInitialPosition = new Vector2(bgRect.position.x, Screen.height * normalizedBgTargetY);
        Vector2 slugOSInitialPosition = new Vector2(slugOSRect.position.x, Screen.height * normalizedSlugOSTargetY);

        bgRect.position = new Vector3(bgRect.position.x, bgInitialPosition.y - bgMoveAwayOffset, bgRect.position.z);
        slugOSRect.position = new Vector3(slugOSRect.position.x, slugOSInitialPosition.y + slugOSMoveAwayOffset, slugOSRect.position.z);
        slugOSAnimationTime = 0;
        bgAnimationTime = 0;
    }

    void Update()
    {
        slugOSAnimationTime += Time.deltaTime;
        bgAnimationTime += Time.deltaTime;

        float bgTargetY = Screen.height * normalizedBgTargetY;
        float slugOSTargetY = Screen.height * normalizedSlugOSTargetY;

        bgRect.position = new Vector3(bgRect.position.x, Mathf.Lerp(bgRect.position.y, bgTargetY, bgAnimationTime), bgRect.position.z);
        slugOSRect.position = new Vector3(slugOSRect.position.x, Mathf.Lerp(slugOSRect.position.y, slugOSTargetY, slugOSAnimationTime), slugOSRect.position.z);
    }
}
