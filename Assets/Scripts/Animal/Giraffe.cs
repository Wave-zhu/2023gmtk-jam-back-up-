using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giraffe : AnimalBase
{
    [SerializeField]
    private AnimalsSO _batSO;
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
    }
    protected override void UseAbility()
    {
        base.UseAbility();
    }
}
