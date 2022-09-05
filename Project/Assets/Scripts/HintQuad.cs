using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintQuad : MonoBehaviour
{
    public bool active = false;
    public TMP_Text hintUI;

    private void Start()
    {
        hintUI = GameManager.instance.hintUI.transform.Find("HintUI").gameObject.GetComponent<TMP_Text>();
    }

    public void Activate()
    {
        active = true;
    }

    public void DeActivate()
    {
        active = false;
        hintUI.text = "";
    }
}
