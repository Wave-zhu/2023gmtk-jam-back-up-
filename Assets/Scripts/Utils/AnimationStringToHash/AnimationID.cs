using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationID
{
    public static readonly int MovementID = Animator.StringToHash("Movement");
    public static readonly int HasInputID = Animator.StringToHash("HasInput");
    public static readonly int RunID = Animator.StringToHash("Run");
    public static readonly int IdleID = Animator.StringToHash("Idle");
    public static readonly int JumpID = Animator.StringToHash("Jump");
    public static readonly int FallID = Animator.StringToHash("Fall");
    public static readonly int DashID = Animator.StringToHash("Dash");
    public static readonly int SlideID = Animator.StringToHash("Slide");
    public static readonly int AttackID = Animator.StringToHash("Attack");
    public static readonly int AbilityID = Animator.StringToHash("Ability");
}
