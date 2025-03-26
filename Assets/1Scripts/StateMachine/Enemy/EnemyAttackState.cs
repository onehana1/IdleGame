using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
        Attack();
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "Attack");
        if (normalizedTime >= 1f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }


    private void Attack()
    {
        // 적이 플레이어에게 데미지를 줌
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable != null)
            {   
                damageable.TakeDamage(stateMachine.Enemy.Damage);
            }
        }
    }
}
