using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Card condition is a class used by interactive hint
/// </summary>
public class CardCondition : MonoBehaviour
{
    public TaskProblem myProblem;
    public TMP_Text hintUI;
    private HintQuad myQuad;

    private void Start()
    {
        hintUI = GameManager.instance.hintUI.transform.Find("HintUI").gameObject.GetComponent<TMP_Text>();
        myQuad = transform.Find("HelpQuad").gameObject.GetComponent<HintQuad>();
    }

    void Update()
    {
        if (GameManager.instance.hintActive && myQuad.active)
        {
            hintUI.text = myProblem.hintText;
        }          
    }
}
