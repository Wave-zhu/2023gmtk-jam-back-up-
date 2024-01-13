using Game.Move;
using UnityEngine;

public class Dog : AnimalBase
{
    [SerializeField]
    private AnimalsSO _DogSO;
    protected override void Awake()
    {
        base.Awake();
        _animal = gameObject;
    }
    private void Start()
    {
        GameEventManager.MainInstance.CallEvent("UpdateParameters", _DogSO, transform);
        GameEventManager.MainInstance.CallEvent("HasAttackAbility", _DogSO.canAttack, transform);
    }
    private void Update()
    {
    }
    protected override void UseAbility()
    {
        base.UseAbility();
    }

}
