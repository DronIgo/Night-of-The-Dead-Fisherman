using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialog
{
    /// <summary>
    /// Dialog text by sentences
    /// </summary>
    [TextArea(3, 10)]
    public string[] sentences;
    /// <summary>
    /// Name of the NPC
    /// </summary>
    public string name;
    /// <summary>
    /// Unity events that should trigger after each sentences
    /// </summary>
    public UnityEvent[] events;
}
