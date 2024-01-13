using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Monkey : AnimalBase
{
    [SerializeField]
    private AnimalsSO _monkeySO;
    private Animator _animator;
    private Rigidbody2D _rb2D;
    private bool _enableAbility = true;
    protected override void Awake()
    {
        base.Awake();
        _animal = gameObject;
        _animator = GetComponentInChildren<Animator>();
        _rb2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        GameEventManager.MainInstance.CallEvent("UpdateParameters", _monkeySO, transform);
        GameEventManager.MainInstance.CallEvent("HasAttackAbility", _monkeySO.canAttack, transform);
    }
    private void Update()
    {
        UseAbility();
    }
    private void ResetAbility()
    {
        _enableAbility = true;
    }
    protected override void UseAbility()
    {
        base.UseAbility();

        if (Physics2D.OverlapPoint(transform.position, 1 << 11)&&GameInputManager.MainInstance.Ability
            &&_enableAbility)
        {
            _enableAbility = false;
            _animator.Play(AnimationID.AbilityID);
            _rb2D.AddForce(new Vector2(10f, 10f));
            TimerManager.MainInstance.TryGetOneTimer(0.2f, ResetAbility);
        }
    }
}
