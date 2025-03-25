using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance{get; private set;}
    public StageManager stageManager;
    public StageInfoData[] stageInfoData;
    public int currentStageIndex = 0;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    { 
        LoadStages();
        StartStage(currentStageIndex);
    }

    void LoadStages()
    {
        stageInfoData = Resources.LoadAll<StageInfoData>("Stages");
    }

    public void StartStage(int stageKey)
    {
        if(stageKey >= 0 || stageKey < stageInfoData.Length)
        {
            stageManager.StartStage(stageInfoData[stageKey]);
        }
        else
        {
            Debug.LogError("StageKey 범위 넘엇음");
        }

    }

    public void LoadNextStage()
    {
        currentStageIndex++;
        if (currentStageIndex < stageInfoData.Length)
        {
            StartStage(currentStageIndex);
        }
        else
        {
            Debug.Log("스테이지 다 클리어함");
        }
    }
}
