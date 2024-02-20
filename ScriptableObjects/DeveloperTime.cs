using UnityEngine;

public class DeveloperTime : ScriptableObject
{
    public float[] stageTime = new float[25]; 

    public void InitializeStageTime()
    {
        stageTime[1] = 20.63f;
    }
}
