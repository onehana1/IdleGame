using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StageManager stageManager;
    public StageInfoData[] stageInfoData;

    void Start()
    {
        StartStage(0); // 0부터 시작
    }

    public void StartStage(int stageKey)
    {
        if(stageKey >= 0 || stageKey < stageInfoData.Length)
        {
            stageManager.StartStage(stageKey);
        }
        else
        {
            Debug.LogError("StageKey 범위 넘엇음");
        }

    }
}
