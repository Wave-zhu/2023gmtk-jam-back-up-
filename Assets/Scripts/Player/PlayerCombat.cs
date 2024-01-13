using Game.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float _attackColdDuration;
    [SerializeField] private float _abilityColdDuration;

    private bool _hasAttackAbility;
    private bool _canCombatInput => !_animator.AnimationAtTag("Dash");
    private bool _canAttackInput = true;
    private bool _canAbilityInput = true;
    private Animator _animator;

    private void HasAttackAbility(bool what,Transform where)
    {
        if (transform != where)
        {
            return;
        }
        _hasAttackAbility = what;
    }
    private void CanAttack()
    {
        _canAttackInput = true;
    }
    private void CanUseAbility()
    {
        _canAbilityInput = true;
    }

    private void UpdateAttackInput()
    {
        if(!_hasAttackAbility)
        {
            return;
        }
        if (GameInputManager.MainInstance.Attack && _canCombatInput && _canAttackInput)
        {
            _canAttackInput = false;
            _animator.Play(AnimationID.AttackID);
            TimerManager.MainInstance.TryGetOneTimer(_attackColdDuration, CanAttack);
        }
        else if (GameInputManager.MainInstance.Ability && _canCombatInput && _canAbilityInput)
        {
            _canAbilityInput = false;
            _animator.Play(AnimationID.AbilityID);
            TimerManager.MainInstance.TryGetOneTimer(_abilityColdDuration, CanUseAbility);
        }
    }
    private void Awake()
    {
        _animator= GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        GameEventManager.MainInstance.AddEventListener<bool,Transform>("HasAttackAbility", HasAttackAbility);
    }
    private void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEvent<bool,Transform>("HasAttackAbility", HasAttackAbility);
    }
    private void Update()
    {
        UpdateAttackInput();
    }


}
