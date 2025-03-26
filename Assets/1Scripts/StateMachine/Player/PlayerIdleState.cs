using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    private float idleTimer = 0f;
    private const float IDLE_DURATION = 1f;
    private const float ENEMY_CHECK_INTERVAL = 0.5f;
    private float enemyCheckTimer = 0f;

    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
        stateMachine.Player.TargetEnemy = null;
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
        idleTimer = 0f;
    }

    public override void HandleInput(){}
    public override void Update()
    {
        idleTimer += Time.deltaTime;
        enemyCheckTimer += Time.deltaTime;

        // 주기적으로 적 존재 여부 체크
        if (enemyCheckTimer >= ENEMY_CHECK_INTERVAL)
        {
            enemyCheckTimer = 0f;
            bool enemiesExist = CheckForEnemies();

            // 적이 존재하고 일정 시간이 지났다면 Move 상태로 전환
            if (enemiesExist && idleTimer >= IDLE_DURATION)
            {
                stateMachine.ChangeState(stateMachine.MoveState);
            }
        }

    }

    public override void PhysicsUpdate(){}

    private bool CheckForEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(
            stateMachine.Transform.position,
            stateMachine.Player.DetectionRange,
            stateMachine.Player.EnemyLayer);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy) && enemy.IsAlive)
            {
                return true;
            }
        }

        return false;
    }
}