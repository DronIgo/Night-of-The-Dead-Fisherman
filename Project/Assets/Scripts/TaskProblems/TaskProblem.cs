using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// TaskProblem is a virtual class for a condition on the card (each card has up to 3 conditions), TaskProblem generates the logic behind the condition 
/// and contains method for creating GameObject corresponding to the condition 
/// </summary>
public class TaskProblem
{
    /// <summary>
    /// specific value required by condition, it's 1 by default because of compatability check with ProblemProduct (bad code)
    /// </summary>
    public int specific = 1;
    public bool onlyEven = false;
    public bool onlyOdd = false;
    /// <summary>
    /// Text for an interactable hint
    /// </summary>
    public string hintText = "";
    /// <summary>
    /// Virtual method checks wheter or not the condition of the task is fullfiled
    /// </summary>
    /// <param name="cubes"></param>
    /// <returns></returns>
    public virtual bool CheckCondition(List<Cube> cubes)
    {
        return true;
    }

    /// <summary>
    /// Auxiliary method for CheckConditionAndChange
    /// </summary>
    /// <param name="cubes"></param>
    /// <returns></returns>
    public virtual int CheckConditionInt(List<Cube> cubes)
    {
        return 0;
    }

    /// <summary>
    /// Checks if the condition is met and changes the text font if the condition is fullfiled/violated
    /// </summary>
    /// <param name="cubes"></param>
    /// <param name="part"></param>
    /// <returns></returns>
    public virtual bool CheckConditionAndChange(List<Cube> cubes, ref GameObject part)
    {
        int res = CheckConditionInt(cubes);
        switch (res)
        {
            case -1:
                ChangeTextWrong(ref part);
                return false;
            case 0:
                ChangeTextNormal(ref part);
                return false;
            case 1:
                ChangeTextRight(ref part);
                return false;
            default:
                Debug.Log("Something is wrong with CheckConditionInt");
                return false;
        }
    }

    /// <summary>
    /// Generates a new problem
    /// </summary>
    /// <param name="compatable"></param>
    public virtual void Generate(TaskProblem compatable)
    {
        return;
    }

    /// <summary>
    /// Creates a GameObject representing the problem 
    /// </summary>
    /// <returns></returns>
    public virtual GameObject CreateTaskPart()
    {
        return null;
    }

    public virtual void ChangeTextRight(ref GameObject part)
    {
        GameObject textObj = part.transform.Find("Text").gameObject;
        textObj.GetComponent<TextMeshPro>().font = TaskGeneratorSource.instance.taskFontRight;
        textObj.GetComponent<TextMeshPro>().fontMaterial = TaskGeneratorSource.instance.taskFontRightMaterial;
        textObj.GetComponent<TextMeshPro>().color = TaskGeneratorSource.instance.taskFontRightColor;
    }

    public virtual void ChangeTextWrong(ref GameObject part)
    {
        GameObject textObj = part.transform.Find("Text").gameObject;
        textObj.GetComponent<TextMeshPro>().font = TaskGeneratorSource.instance.taskFontWrong;
        textObj.GetComponent<TextMeshPro>().fontMaterial = TaskGeneratorSource.instance.taskFontWrongMaterial;
        textObj.GetComponent<TextMeshPro>().color = TaskGeneratorSource.instance.taskFontWrongColor;
    }

    public virtual void ChangeTextNormal(ref GameObject part)
    {
        GameObject textObj = part.transform.Find("Text").gameObject;
        textObj.GetComponent<TextMeshPro>().font = TaskGeneratorSource.instance.taskFontNormal;
        textObj.GetComponent<TextMeshPro>().fontMaterial = TaskGeneratorSource.instance.taskFontNormalMaterial;
        textObj.GetComponent<TextMeshPro>().color = TaskGeneratorSource.instance.taskFontNormalColor;
        Color c = Color.red;
    }
}
