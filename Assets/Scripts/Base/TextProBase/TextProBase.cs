using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.UI.TextMeshPro
{
    public abstract class TextMeshProBase : MonoBehaviour
    {
        protected TextMeshProUGUI _textMeshPro;
        protected virtual void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }
        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}

