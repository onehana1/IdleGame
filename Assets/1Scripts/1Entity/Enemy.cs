using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IDamageDealer
{
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }
    [field: SerializeField] public EnemySO Data { get; private set; }

    [SerializeField] private float attackDamage = 10f;
    private float currentHealth;
    public bool IsAlive => currentHealth > 0;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => Data.StatData.maxHealth;
    public float Damage => Data.StatData.damage;
    public float AttackRange => Data.StatData.attackRange;


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
        if (!IsAlive) return;

        float actualDamage = Mathf.Max(0, damage - Data.StatData.defense);
        currentHealth = Mathf.Max(0, currentHealth - actualDamage);

        if (!IsAlive)
        {
            Die();
        }
        else
        {
            Debug.Log($"{gameObject.name} took {actualDamage} damage! Current health: {currentHealth}");
            // TODO: 피격 상태로 전환
            // stateMachine.ChangeState(stateMachine.HitState);
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (Animator != null)
        {
            Animator.enabled = false;
        }

        if (Controller != null)
        {
            Controller.enabled = false;
        }

        if (TryGetComponent<Collider>(out Collider collider))
        {
            collider.enabled = false;
        }
        if(Data.dropItem != null)
        {
            Instantiate(Data.dropItem, transform.position, Quaternion.identity);
        }
        if (CharacterManager.Instance != null && CharacterManager.Instance.Player != null)
        {
            CharacterManager.Instance.Player.Coin += 5;
        }


        StageManager.instance.OnMonsterDefeated();
        StartCoroutine(DestroyAfterDelay());
    }
    private WaitForSeconds destroyDelay = new WaitForSeconds(3f);
    private IEnumerator DestroyAfterDelay()
    {
        yield return destroyDelay;
        
        Destroy(gameObject);
    }

    public void DealDamage(IDamageable target)
    {
        if (target != null)
        {
            target.TakeDamage(attackDamage);
        }
    }

}

