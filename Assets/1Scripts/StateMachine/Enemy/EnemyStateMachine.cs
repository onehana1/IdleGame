using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; private set; }

    public EnemyIdleState IdleState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;


    public bool IsAttacking { get; set; }

    public Transform MainCameraTransform { get; set; }

    public EnemyStateMachine(Enemy enemy)
    {
        Enemy = enemy;

        IdleState = new EnemyIdleState(this);
        AttackState = new EnemyAttackState(this); 
        MovementSpeed = enemy.Data.MovementData.BaseSpeed;
        RotationDamping = enemy.Data.MovementData.BaseRotationDamping;
    }


}
