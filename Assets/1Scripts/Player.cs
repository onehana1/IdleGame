using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : MonoBehaviour
{

    [field:Header("Animations")]
    [field:SerializeField] private PlayerAnimationData AnimationData;
    public Animator Animator {get; private set;}
    public PlayerController Input{get; private set;}
    public CharacterController Controller{get; private set;}    

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpHeight = 1.5f;
    

    // Start is called before the first frame update
    void Start()
    {
     AnimationData.Initialize();
     Animator = GetComponent<Animator>();
     Input = GetComponent<PlayerController>();
     Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
