using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageManager : MonoBehaviour
{
    public StageInfoData currentStage;
    public StageInfoData[] stages; 
    private int currentStageIndex = 0;
    private int remainingMonsters;

    void Start()
    {
        LoadStages(); 
        StartStage(currentStageIndex); 

    }

    void LoadStages()
    {
        stages = Resources.LoadAll<StageInfoData>("Stages");
        if (stages.Length == 0)
        {
            Debug.LogError("No stages found in Resources/Stages!");
        }
    }

    public void StartStage(int stageIndex)
    {
        if(stageIndex<0||stageIndex>=stages.Length)
        {
            Debug.LogError("Invalid stage index!");
            return;
        }

        currentStageIndex = stageIndex;
        currentStage = stages[currentStageIndex];
        SpawnMonsters();
    }


    void SpawnMonsters()
    {
        foreach (var wave in currentStage.waves)
        {
            foreach (var monster in wave.monsters)
            {
                remainingMonsters += monster.spawnCount; // 적 수 업데이트
                SpawnMonster(monster); // monster를 직접 전달
            }
        }
    }

    void SpawnMonster(MonsterSpawnData monsterData)
    {
        for (int i = 0; i < monsterData.spawnCount; i++)
        {
            GameObject spawnedMonster = Instantiate(monsterData.monsterPrefab);
            spawnedMonster.name = monsterData.monsterPrefab.name; // 적 이름 설정
            spawnedMonster.transform.position = GetSpawnPosition(); // 스폰 위치 설정
            Debug.Log($"Spawning {monsterData.spawnCount} of {spawnedMonster.name}");
        }
    }

    Vector3 GetSpawnPosition()
    {
        // 스폰 위치를 랜덤으로 설정
        return new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
    }

    public void OnMonsterDefeated()
    {
        remainingMonsters--;
        if (remainingMonsters <= 0)
        {
            NextStage();
        }
    }

    void NextStage()
    {
        Debug.Log("All monsters defeated! Moving to the next stage.");
    }
}