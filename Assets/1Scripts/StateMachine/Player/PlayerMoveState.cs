using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Move State");
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);

        FindNearestEnemy();
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    public override void HandleInput() { }

    public override void Update()
    {
        // 적이 없으면 적 찾기
        if (stateMachine.Player.TargetEnemy == null)
        {
            FindNearestEnemy();
            // 적이 없으면 idle 상태
            if (stateMachine.Player.TargetEnemy == null)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
        }

        // 적을 향해 이동
        stateMachine.Player.Agent.SetDestination(stateMachine.Player.TargetEnemy.transform.position);

        // 공격 범위 안에 들어오면 공격 상태로 전환
        if (Vector3.Distance(stateMachine.Player.transform.position, stateMachine.Player.TargetEnemy.transform.position) <= stateMachine.Player.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.ComboAttackState);
        }
        // 적이 너무 멀어지면 타겟 해제하고 대기 상태로 전환
        else if (Vector3.Distance(stateMachine.Player.transform.position, stateMachine.Player.TargetEnemy.transform.position) > stateMachine.Player.DetectionRange)
        {
            stateMachine.Player.TargetEnemy = null;
            stateMachine.ChangeState(stateMachine.IdleState);
        }


        
    }

    public override void PhysicsUpdate()
    {
    }

    private void FindNearestEnemy()
    {
        // 감지 범위 내의 모든 적 찾기
        Collider[] colliders = Physics.OverlapSphere(stateMachine.Player.transform.position, stateMachine.Player.DetectionRange, stateMachine.Player.EnemyLayer);
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {

            if (collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && enemy.IsAlive)
            {
                float distance = Vector3.Distance(stateMachine.Player.transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    stateMachine.Player.TargetEnemy = enemy;
                }

            }
        }
    }


}
