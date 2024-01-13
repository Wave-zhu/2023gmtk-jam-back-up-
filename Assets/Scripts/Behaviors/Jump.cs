using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor;

public class Jump : Action
{
    [SerializeField]
    private Animator _animator;
    private Vector3 _begin;
    private Vector3 _end;
    public override void OnAwake()
    {
        _begin = transform.position;
        _end = _begin + new Vector3(0, 5, 0);
    }
    // Start is called before the first frame update
    public override TaskStatus OnUpdate()
    {
        _animator.Play(AnimationID.JumpID);
        transform.position = Vector3.Lerp(_begin, _end, 2*Time.deltaTime);
        transform.position = Vector3.Lerp(_end, _begin, 2*Time.deltaTime);
        return TaskStatus.Running;
    }
}
