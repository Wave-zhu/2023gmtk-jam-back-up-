using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReswapnPoint : MonoBehaviour
{
    [SerializeField]
    private Transform point;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<AnimalBase>().GetType().Name!="Fish")
        {
            collision.gameObject.transform.position=point.position;
        }
    }
}
