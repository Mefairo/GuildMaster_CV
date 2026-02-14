using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringListContainer
{
    [TextArea(10, 10)] // ћинимум 3 строки, максимум 10 (можно изменить)
    public string Text;
}
