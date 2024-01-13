using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _turnedSprite;
    [SerializeField] private List<GameObject> _controlledObjects;
    bool isInsideTrigger;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isInsideTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInsideTrigger = false;
    }
    private void Update()
    {
        if (isInsideTrigger && GameInputManager.MainInstance.TurnOn)
        {
            _spriteRenderer.sprite = _turnedSprite;
            GetComponent<BoxCollider2D>().enabled = false;
            foreach (GameObject obj in _controlledObjects)
            {
                obj.SetActive(true);
            }
        }
    }


}
