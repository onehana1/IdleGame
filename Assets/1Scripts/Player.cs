using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class Player : MonoBehaviour
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
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private LayerMask enemyLayer;

    public NavMeshAgent Agent { get; private set; }
    public float AttackRange => attackRange;
    public float DetectionRange => detectionRange;
    public float AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;
    public LayerMask EnemyLayer => enemyLayer;

    private PlayerStateMachine stateMachine;
    private float lastAttackTime;
    private Enemy targetEnemy;

    public Enemy TargetEnemy
    {
        get => targetEnemy;
        set => targetEnemy = value;
    }

    void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();

        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Agent = GetComponent<NavMeshAgent>();
        
        // NavMeshAgent 설정
        Agent.speed = moveSpeed;
        Agent.angularSpeed = rotationSpeed;
        Agent.stoppingDistance = stoppingDistance;

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

        // 가장 가까운 적 찾기
        if (targetEnemy == null)
        {
            FindNearestEnemy();
        }

        if (targetEnemy != null)
        {
            // 적을 향해 이동
            Agent.SetDestination(targetEnemy.transform.position);
            Debug.Log($"Moving towards enemy: {targetEnemy.name}");

            // 공격 범위 안에 들어오면 공격
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                // 적이 너무 멀어지면 끝
                if (Vector3.Distance(transform.position, targetEnemy.transform.position) > detectionRange)
                {
                    targetEnemy = null;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void FindNearestEnemy()
    {
        // 감지 범위 내의 모든 적 찾기
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        Debug.Log($"Found {colliders.Length} enemies in range");
        
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                Debug.Log($"Enemy found at distance: {distance}");
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    targetEnemy = enemy;
                }
            }
        }

        if (targetEnemy != null)
        {
            Debug.Log($"Target enemy set: {targetEnemy.name}");
        }
    }

    public void Attack()
    {
        if (targetEnemy != null && Time.time >= lastAttackTime + attackCooldown)
        {
            targetEnemy.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }
}
