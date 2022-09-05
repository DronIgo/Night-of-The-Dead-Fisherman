using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreUI : MonoBehaviour
{
    TextMeshProUGUI text;
    TaskManager tm;
    GameManager gm;
    Color defaultColor;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        tm = TaskManager.instance;
        gm = GameManager.instance;
        defaultColor = text.color;
    }
    void Update()
    {
        text.text = gm.score.ToString();
    }
}
