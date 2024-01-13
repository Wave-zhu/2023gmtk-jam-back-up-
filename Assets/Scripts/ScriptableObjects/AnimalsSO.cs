using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Assets;

[CreateAssetMenu(fileName = "Animal", menuName = "Create/Assets/Animal", order = 0)]
public class AnimalsSO : ScriptableObject  
{
    public bool canSwim;
    public bool canDiveWater;

    public bool canFly;
    public bool canJumpFar;

    public bool canAttack;

    public bool canBeTiny;

    public bool canClimb;
    public bool canJumpFromHighPoint;

    public bool canDig;
    public bool canNightVision;

    public float jumpForce;
    public float fallMutiplier;

    public float groundCheckRadius;

    public float dashSpeed = 30f;
    public float dashDuration = 0.1f;


    public Vector2 grabRightOffset = new Vector2(0.16f, 0f);
    public Vector2 grabLeftOffset = new Vector2(-0.16f, 0f);
    public float grabCheckRadius = 0.24f;
    public float slideSpeed = 2.5f;
    public Vector2 wallJumpForce = new Vector2(10.5f, 18f);
    public Vector2 wallClimbForce = new Vector2(4f, 14f);
}




