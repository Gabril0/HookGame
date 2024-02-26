using UnityEngine;

public class DeveloperTime : ScriptableObject
{
    public float[] stageTime = new float[25]; 

    public void InitializeStageTime()
    {
        stageTime[1] = 4.92f;
        stageTime[2] = 6.99f;
        stageTime[3] = 18.85f;
        stageTime[4] = 26.1f;
        stageTime[5] = 21.10f;
        stageTime[6] = 15.1f;
        stageTime[7] = 21.2f;
        stageTime[8] = 16.3f;
        stageTime[9] = 14.72f;
        stageTime[10] = 16.12f;
        stageTime[11] = 31.52f;
    }
}
