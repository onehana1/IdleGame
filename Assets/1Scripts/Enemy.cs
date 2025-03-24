using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }
    [field: SerializeField] public EnemySO Data { get; private set; }

    private float currentHealth;
    public bool IsAlive => currentHealth > 0;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => Data.StatData.maxHealth;

    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }

    private EnemyStateMachine stateMachine;

    private SkinnedMeshRenderer meshRenderer;

    private void Awake()
    {
        AnimationData.Initialize();
        currentHealth = Data.StatData.maxHealth;
        
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        stateMachine = new EnemyStateMachine(this);
        stateMachine.ChangeState(stateMachine.IdleState);

        if (meshRenderer != null && Data != null)
        {
            meshRenderer.material.SetColor("_MainColor", Data.Color);
        }
    }

    private void Start()
    {

    }
    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public bool IsPlayerInRange(float detectionRange)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        // 거리 계산
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= detectionRange;
    }

    public void RotateTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // 수평 회전만

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Data.MovementData.BaseRotationDamping * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(0, damage - Data.StatData.defense);
        currentHealth = Mathf.Max(0, currentHealth - actualDamage);

        if (!IsAlive)
        {
            // TODO: 사망 처리
            Debug.Log($"{gameObject.name} died!");
        }
        else
        {
            // TODO: 피격 애니메이션 또는 효과
            Debug.Log($"{gameObject.name} took {actualDamage} damage! Current health: {currentHealth}");
        }
    }
}
