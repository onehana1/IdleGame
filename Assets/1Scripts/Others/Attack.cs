using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
  Player player;
  PlayerStateMachine stateMachine;

  private void Start()
  {
    player = GetComponentInParent<Player>();
    stateMachine = player.stateMachine;
  } 
  
    public void Attacking()
    {
        if (player.TargetEnemy == null || !player.TargetEnemy.IsAlive)
        {
            return;
        }
        Debug.Log("AttackTry");
        if (stateMachine.TargetEnemy.TryGetComponent<IDamageable>(out IDamageable damageable))
        {

            damageable.TakeDamage(stateMachine.Player.Damage);
        }
    }
}
