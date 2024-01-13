using Game.Move;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalBase : MonoBehaviour
{
    protected GameObject _animal;
    protected PlayerController _playerController;



    private bool _ableToReverse;
    private bool _ableToUnReverse;
    public bool AbleToReverse { get => _ableToReverse; set => _ableToReverse = value; }
    public bool AbleToUnReverse { get => _ableToUnReverse; set => _ableToUnReverse = value; }

    protected virtual void UseAbility()
    {

    }
    protected virtual void Awake()
    {
        _playerController=GetComponent<PlayerController>();
    }


}
