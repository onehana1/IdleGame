using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy,

    Boss,
}

public abstract class BaseStatData : ScriptableObject
{
    [Header("Info")]
    public string characterName;
    public int LV;

    [Header("Stat")]
    public float maxHealth;
    public float speed;
    public float attackSpeed;
    public float damage;
    public float attackRange;
    public float defense;
    public float criticalChance;
    public float criticalDamage;
}
