using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{

    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundData = stateMachine.Player.Data.GroundedData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled += OnMovementCanceled;
        input.playerActions.Run.started += OnRunStarted;
        input.playerActions.Jump.started += OnJumpStarted;
        input.playerActions.Attack.performed += OnAttackPerformed;
        input.playerActions.Attack.canceled += OnAttackCanceled;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Run.started -= OnRunStarted;
        input.playerActions.Jump.started -= OnJumpStarted;
        input.playerActions.Attack.performed -= OnAttackPerformed;
        input.playerActions.Attack.canceled -= OnAttackCanceled;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        Move();
    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnAttackPerformed(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = true;
    }

    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = false;
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Move(movementDirection);

        Rotate(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();

        stateMachine.Player.Controller.Move(((direction * movementSpeed)
        + stateMachine.Player.ForceReceiver.Movement)
        * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected void ForceMove()
    {
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime);
    }


    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

}