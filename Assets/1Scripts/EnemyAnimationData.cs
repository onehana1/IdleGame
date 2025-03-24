using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyAnimationData
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string attackParameterName = "Attack";
    [SerializeField] private string moveParameterName = "Move";

    public int IdleParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int MoveParameterHash { get; private set; }


    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
        MoveParameterHash = Animator.StringToHash(moveParameterName);
    }
}
