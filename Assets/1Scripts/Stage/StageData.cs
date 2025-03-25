using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewStageInfo", menuName = "Stage Info")]
public class StageInfoData : ScriptableObject
{
    public int stageKey;
    public WaveData[] waves;
}


[System.Serializable]
public class StageInfo
{
    public int stageKey;
    public WaveData[] waves;

    public StageInfo(int stageKey, WaveData[] waves)
    {
        this.stageKey = stageKey;
        this.waves = waves;
    }
}

[System.Serializable]
public class WaveData
{
    public MonsterSpawnData[] monsters; // 적 데이터
    public bool hasBoss;
    public string bossType;

    public WaveData(MonsterSpawnData[] monsters, bool hasBoss, string bossType)
    {
        this.monsters = monsters;
        this.hasBoss = hasBoss;
        this.bossType = bossType;
    }
}

[System.Serializable]
public class MonsterSpawnData
{
    public GameObject monsterPrefab; // 적 프리팹
    public int spawnCount; // 적 수

    public MonsterSpawnData(GameObject monsterPrefab, int spawnCount)
    {
        this.monsterPrefab = monsterPrefab;
        this.spawnCount = spawnCount;
    }
}