using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : AnimalBase
{
    [SerializeField]
    private AnimalsSO _catSO;
    protected override void Awake()
    {
        base.Awake();
        _animal = gameObject;
    }
    private void Start()
    {
        GameEventManager.MainInstance.CallEvent("UpdateParameters",_catSO,transform);
        GameEventManager.MainInstance.CallEvent("HasAttackAbility",_catSO.canAttack,transform);
    }
    private void Update()
    {

    }
    protected override void UseAbility()
    {
        base.UseAbility();
    }
}
