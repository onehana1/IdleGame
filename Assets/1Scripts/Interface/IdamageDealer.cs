using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageDealer
{
    float Damage { get; }
    float AttackRange { get; }
    void DealDamage(IDamageable target);
}
