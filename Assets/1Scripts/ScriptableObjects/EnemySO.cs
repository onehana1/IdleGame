using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class EnemyMovementData
{
  [field: SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
  [field: SerializeField][field: Range(0f, 25f)] public float BaseRotationDamping { get; private set; } = 1f;
}


[CreateAssetMenu(fileName = "Enemy", menuName = "Characters/Enemy")]

public class EnemySO : ScriptableObject
{
  [field: SerializeField] public EnemyMovementData MovementData { get; private set; }
  [field: SerializeField] public EnemyStatData StatData { get; private set; }
  [field: SerializeField] public Color Color { get; private set; }
}