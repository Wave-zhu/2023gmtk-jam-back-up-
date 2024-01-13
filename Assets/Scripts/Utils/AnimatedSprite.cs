using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] _sprites;
    public float _frameRate = 1f / 6f;

    private SpriteRenderer _spriteRenderer;
    private int _frame = 0;

    public bool _isLoop = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (_isLoop)
        {
            InvokeRepeating(nameof(LoopAnimate), _frameRate, _frameRate);
        }
        else
        {
            StartCoroutine(Animate());
        }
        
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void LoopAnimate()
    {
        _frame++;

        if (_frame >= _sprites.Length)
        {
            _frame = 0;
        }

        if (_frame >= 0 && _frame < _sprites.Length)
        {
            _spriteRenderer.sprite = _sprites[_frame];
        }

    }

    private IEnumerator Animate()
    {
        while (_frame >= 0 && _frame < _sprites.Length)
        {
            _spriteRenderer.sprite = _sprites[_frame];
            yield return new WaitForSeconds(_frameRate);
            _frame++;
        }

        Destroy(gameObject);
    }

}
