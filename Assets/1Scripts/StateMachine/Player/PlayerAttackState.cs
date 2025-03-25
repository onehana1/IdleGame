using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float lastAttackTime;
    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }


    public override void Update()
    {
        // 타겟이 없거나 죽은 경우 체크
        if (stateMachine.TargetEnemy == null || !stateMachine.TargetEnemy.IsAlive)
        {
            Debug.Log("Target is dead or null, changing to idle state");
            stateMachine.Player.TargetEnemy = null;  // 타겟 초기화
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        float distanceToEnemy = Vector3.Distance(stateMachine.Transform.position, stateMachine.TargetEnemy.transform.position);
        if (distanceToEnemy > stateMachine.Player.AttackRange)
        {
            Debug.Log("Target out of range, changing to move state");
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }

        if (Time.time >= lastAttackTime + stateMachine.Player.AttackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (stateMachine.TargetEnemy == null || !stateMachine.TargetEnemy.IsAlive)
        {
            return;
        }

        if (stateMachine.TargetEnemy.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(stateMachine.Player.Damage);
            Debug.Log($"Attack hit! Dealt {stateMachine.Player.Damage} damage to {stateMachine.TargetEnemy.name}");
            lastAttackTime = Time.time;
        }
    }

    public override void PhysicsUpdate()
    {
    }
}
