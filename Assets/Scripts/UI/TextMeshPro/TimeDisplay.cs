using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.UI.TextMeshPro;

public class TimeDisplay : TextMeshProBase
{
    protected override void Update()
    {
        _textMeshPro.text = DateTime.Now.ToString("HH:mm:ss");
    }
}
