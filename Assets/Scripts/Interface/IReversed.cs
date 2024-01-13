using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReversed
{
    bool AbleToReverse { get; set; }
    bool AbleToUnReverse { get; set; }
    void Reverse(Transform where);
    void UnReverse(Transform where);
}
