using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;
using Game.UI.TextMeshPro;

public class ScaleChange : TextMeshProBase
{
    [SerializeField] private float _countTime;
    [SerializeField] private float _beginSize;
    [SerializeField] private float _endSize;
    private float _targetSize;

    protected override void Start()
    {
        base.Start();
        _countTime = 5f;
        _beginSize =_textMeshPro.fontSize;
        _endSize = 100f;
        _targetSize = _endSize;
    }

    protected override void Update()
    {
        _countTime -= Time.deltaTime;
        _textMeshPro.fontSize = Mathf.Lerp(_textMeshPro.fontSize, _targetSize, 0.01f);
        if (_countTime <= 0)
        {
            _countTime = 5f;
            _targetSize = _beginSize + _endSize - _targetSize;           
        }
    }

}
