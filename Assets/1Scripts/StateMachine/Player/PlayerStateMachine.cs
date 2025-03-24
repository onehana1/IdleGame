using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }

    public PlayerMoveState MoveState { get; private set; }  // 얘가 적 찾아서 움직이는거

    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }

    public PlayerAttackState ComboAttackState { get; private set; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public float JumpForce { get; set; }
    public bool IsAttacking { get; set; }
    public int ComboIndex{ get; set; }



    public Enemy TargetEnemy => Player.TargetEnemy;
    public Transform Transform => Player.transform;
    public float AttackRange => Player.AttackRange;
    public float AttackDamage => Player.AttackDamage;




    public Transform MainCameraTransform { get; set; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        MainCameraTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);
        MoveState = new PlayerMoveState(this);
        ComboAttackState = new PlayerComboAttackState(this);
        MovementSpeed = player.Data.GroundedData.BaseSpeed;
        RotationDamping = player.Data.GroundedData.BaseRotationDamping;
    }

}