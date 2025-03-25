using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageManager : MonoBehaviour
{
    public StageInfoData currentStage; // StageInfo 대신 StageInfoData 사용
    private int remainingMonsters;

    void Start()
    {
        StartStage(currentStage);
    }

    public void StartStage(StageInfoData stage)
    {
        currentStage = stage;
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