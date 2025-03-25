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
        if (stateMachine.TargetEnemy == null)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        float distanceToEnemy = Vector3.Distance(stateMachine.Transform.position, stateMachine.TargetEnemy.transform.position);
        if (distanceToEnemy > stateMachine.Player.AttackRange)
        {
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
        if (stateMachine.TargetEnemy == null)
        {
            Debug.Log("No target enemy");
            return;
        }

        float distanceToEnemy = Vector3.Distance(stateMachine.Transform.position, stateMachine.TargetEnemy.transform.position);

        if (distanceToEnemy > stateMachine.Player.AttackRange)
        {
            Debug.Log($"Enemy too far to attack. Distance: {distanceToEnemy}, Attack Range: {stateMachine.Player.AttackRange}");
            return;
        }

        if (stateMachine.TargetEnemy.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(stateMachine.Player.Damage);
            Debug.Log($"Dealt {stateMachine.Player.Damage} damage to {stateMachine.TargetEnemy.name}");
            lastAttackTime = Time.time;
        }
        else
        {
            Debug.Log($"Enemy {stateMachine.TargetEnemy.name} is not damageable");
        }
    }

    public override void PhysicsUpdate()
    {
    }
}
