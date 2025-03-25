using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageManager : MonoBehaviour
{
    public StageInfoData currentStage; // StageInfoData 사용
    private int remainingMonsters;
    private int currentWaveIndex = 0; // 현재 웨이브 인덱스

    void Start()
    {
        //StartStage(currentStage);
    }

    public void StartStage(StageInfoData stage)
    {
        currentStage = stage;
        currentWaveIndex = 0; // 웨이브 인덱스 초기화
        SpawnNextWave(); // 첫 번째 웨이브 스폰
    }

    void SpawnNextWave()
    {
        if (currentWaveIndex < currentStage.waves.Length)
        {
            WaveData wave = currentStage.waves[currentWaveIndex];
            remainingMonsters = 0; // 남은 몬스터 수 초기화

            foreach (var monster in wave.monsters)
            {
                remainingMonsters += monster.spawnCount; // 적 수 업데이트
                SpawnMonster(monster); // 몬스터 스폰
            }

            currentWaveIndex++; // 다음 웨이브로 이동
        }
        else
        {
            Debug.Log("All waves completed!");
            StartCoroutine(NextStageAfterDealy(2f));
           
        }
    }

        IEnumerator NextStageAfterDealy(float delay)
        {
            yield return new WaitForSeconds(delay);
            GameManager.instance.LoadNextStage(); 
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

    public void OnMonsterDefeated()
    {
        remainingMonsters--;
        if (remainingMonsters <= 0)
        {
            SpawnNextWave(); // 현재 웨이브가 끝나면 다음 웨이브 스폰
        }
    }

    Vector3 GetSpawnPosition()
    {
        // 스폰 위치를 랜덤으로 설정
        return new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
    }
}