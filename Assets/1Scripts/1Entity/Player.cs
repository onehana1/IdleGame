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
    public float rotationSpeed = 10f;
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

    public PlayerStateMachine stateMachine;
    private float lastAttackTime;
    private Enemy targetEnemy;




    [Header("Inventory")]

    public InventoryManager inventory;
    public PlayerEquipment equipment;
    public ItemData itemData;
    public Action addItem;

    public int Coin;



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

    public int Level => Data.StatData.LV;

    public string PlayerName => Data.StatData.characterName;
    void Awake()
    {
        if (AnimationData != null)
        {
            AnimationData.Initialize();
        }
        else
        {
        }

        if (Data != null && Data.StatData != null)
        {
            currentHealth = Data.StatData.maxHealth;
        }
        else
        {
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

        equipment = GetComponent<PlayerEquipment>();

        CharacterManager.Instance.Player = this;
    }


    void Start()
    {
      //  inventory = gameObject.AddComponent<InventoryManager>();
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
        }
        else
        {
            // TODO: 피격 애니메이션 또는 효과
        }
    }

    public void RotationDamping(float rotationDamping)
    {
        rotationDamping = rotationSpeed;
    }
    public void DealDamage(IDamageable target)
    {
        if (target != null)
        {
            target.TakeDamage(Damage);
        }
    }

    public void Attack()
    {
        if (stateMachine.TargetEnemy == null || !stateMachine.TargetEnemy.IsAlive)
        {
            return;
        }

        if (stateMachine.TargetEnemy.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(stateMachine.Player.Damage);
            lastAttackTime = Time.time;
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {

    //     if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
    //     {
    //         Debug.LogAssertion("Item 충돌");
    //         ItemObject item = other.GetComponent<ItemObject>();
    //         if (item != null)
    //         {
    //             inventory.AddItem(item.data);
    //             Debug.Log($"{item.data.name} added to inventory.");
    //             Destroy(other.gameObject);
    //         }
    //     }
    // }
}
