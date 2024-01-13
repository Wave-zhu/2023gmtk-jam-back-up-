using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : AnimalBase
{
    [SerializeField]
    private AnimalsSO _batSO;

    [SerializeField] private Transform _destroyableGroundCheck;
    [SerializeField] private float _destroyableGroundRadius;
    [SerializeField] private LayerMask _whatIsdestroyableGround;

    protected override void Awake()
    {
        base.Awake();
        _animal = gameObject;
    }
    private void Start()
    {
        GameEventManager.MainInstance.CallEvent("UpdateParameters", _batSO,transform);
        GameEventManager.MainInstance.CallEvent("HasAttackAbility", _batSO.canAttack, transform);
    }
    private void Update()
    {
        UseAbility();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_destroyableGroundCheck.position, _destroyableGroundRadius);
    }

    protected override void UseAbility()
    {
        base.UseAbility();
        Collider2D collider = Physics2D.OverlapCircle(_destroyableGroundCheck.position, _destroyableGroundRadius, _whatIsdestroyableGround);
        if (collider != null && GameInputManager.MainInstance.Ability)
        {
            collider.gameObject.SetActive(false);
        }
    }

}
