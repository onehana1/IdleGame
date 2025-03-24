using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttackState : PlayerAttackState
{
    public bool alreadyAppliedCombo;
    public bool alreadyAppliedForce;

    private AttackInfoData attackInfoData;
    public PlayerComboAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);
        alreadyAppliedCombo = false;
        alreadyAppliedForce = false;

        int comboIndex = stateMachine.ComboIndex;
        attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfoData(comboIndex);
        stateMachine.Player.Animator.SetInteger("Combo", comboIndex);
    }



    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        if (!alreadyAppliedCombo)
        {
            stateMachine.ComboIndex = 0;
        }
    }

    public override void Update()
    {
        base.Update();

        ForceMove();
        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attackInfoData.ComboTransitionTime)
            {
                TryComboAttack();
            }
            if (normalizedTime >= attackInfoData.ForceTransitionTime)
            {
                TryApplyForce();
            }
        }
        else
        {
            if (alreadyAppliedCombo)
            {
                stateMachine.ComboIndex = attackInfoData.ComboStateIndex;
                stateMachine.ChangeState(stateMachine.ComboAttackState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }


    private void TryComboAttack()
    {
        if(alreadyAppliedCombo) return;

        if(attackInfoData.ComboStateIndex == -1) return;

        if(!stateMachine.IsAttacking) return;

        alreadyAppliedCombo = true;
    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce) return;

        alreadyAppliedForce = true;

        stateMachine.Player.ForceReceiver.Reset();

        stateMachine.Player.ForceReceiver.AddForce(Vector3.forward * attackInfoData.Force);
    }
}
