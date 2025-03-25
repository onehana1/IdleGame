using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class Player : MonoBehaviour, IDamageDealer, IDamageable
{
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    [field: SerializeField] public PlayerSO Data { get; private set; }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float stoppingDistance = 1.5f;

    public Animator Animator { get; private set; }
    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }


    [Header("Combat")]

    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private LayerMask enemyLayer;

    public NavMeshAgent Agent { get; private set; }
    public float AttackRange => Data.StatData.attackRange;
    public float DetectionRange => detectionRange;

    public float AttackCooldown => attackCooldown;
    public LayerMask EnemyLayer => enemyLayer;

    private PlayerStateMachine stateMachine;
    private float lastAttackTime;
    private Enemy targetEnemy;




    [Header("Inventory")]

    public PlayerEquipment equipment;
    public ItemData itemData;
    public Action addItem;




    public Enemy TargetEnemy
    {
        get => targetEnemy;
        set => targetEnemy = value;
    }

    private float currentHealth;
    public bool IsAlive => currentHealth > 0;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => Data.StatData.maxHealth;
    public float Damage => Data.StatData.damage;

    void Awake()
    {
        if (AnimationData != null)
        {
            AnimationData.Initialize();
        }
        else
        {
            Debug.LogError("AnimationData 없음");
        }

        if (Data != null && Data.StatData != null)
        {
            currentHealth = Data.StatData.maxHealth;
        }
        else
        {
            Debug.LogError("Data 없음");
        }
        
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Agent = GetComponent<NavMeshAgent>();
        
        // NavMeshAgent 설정
        Agent.speed = moveSpeed;
        Agent.angularSpeed = rotationSpeed;
        Agent.stoppingDistance = Data.StatData.attackRange;

        stateMachine = new PlayerStateMachine(this);
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Start()
    {
      //  Cursor.lockState = CursorLockMode.Locked;
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

    public void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(0, damage - Data.StatData.defense);
        currentHealth = Mathf.Max(0, currentHealth - actualDamage);

        if (!IsAlive)
        {
            // TODO: 사망 처리
            Debug.Log("Player died!");
        }
        else
        {
            // TODO: 피격 애니메이션 또는 효과
            Debug.Log($"Player took {actualDamage} damage! Current health: {currentHealth}");
        }
    }

    public void DealDamage(IDamageable target)
    {
        if (target != null)
        {
            target.TakeDamage(Damage);
            Debug.Log($"Dealt {Damage} damage to target");
        }
    }

}
