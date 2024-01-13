using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class MoveRandom : Action
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Transform _target;
    private float _time=2f;
    public override void OnAwake()
    {
    }
    // Start is called before the first frame update
    public override TaskStatus OnUpdate()
    {
        _time -= Time.deltaTime;
        if (_time < 0f)
        {
            int temp = Random.Range(0, 6);
            if (temp <= 3)
            {
                _animator.Play(AnimationID.RunID);
                if (temp <= 1)
                {
                    _target.eulerAngles = new Vector3(0, 0f, 0);
                }
                else
                {
                    _target.eulerAngles = new Vector3(0, 180f, 0);
                }
               
            }
            else
            {
                _animator.Play(AnimationID.IdleID);
                _target.eulerAngles = new Vector3(0, 0f, 0);
            }
            _time = 2f;
        }
        return TaskStatus.Running;
    }
}
