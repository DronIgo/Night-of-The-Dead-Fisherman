using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProblemSpecific : TaskProblem
{
    public ProblemSpecific() {
        specific = Random.Range(1, 7);
        hintText = "Must use at least one die with a value of " + specific;
    }

    public override bool CheckCondition(List<Cube> cubes)
    {
        foreach (Cube c in cubes)
        {
            if (c.CurrentValue == specific)
            {
                return true;
            }
        }
        Debug.Log("Specific check failed");
        return false;
    }

    public override int CheckConditionInt(List<Cube> cubes)
    {
        if (CheckCondition(cubes))
        {
            return 1;
        }
        else
            return 0;
    }

    public override bool CheckConditionAndChange(List<Cube> cubes, ref GameObject part)
    {
        int cond = CheckConditionInt(cubes);
        switch (cond)
        {
            case 0:
                ChangeTextNormal(ref part);
                return false;
            case 1:
                ChangeTextRight(ref part);
                return true;
            default:
                Debug.Log("Something is wrong with CheckConditionInt");
                return false;
        }
    }

    public override GameObject CreateTaskPart()
    {

        GameObject prefab = TaskGeneratorSource.instance.specificPartPrefab;
        TextMeshPro text1 = null;
        switch (specific)
        {
            case 1:
                text1 = prefab.transform.Find("Text").GetComponent<TextMeshPro>();
                text1.text = "^";
                break;
            case 2:
                text1 = prefab.transform.Find("Text").GetComponent<TextMeshPro>();
                text1.text = "@";
                break;
            case 3:
                text1 = prefab.transform.Find("Text").GetComponent<TextMeshPro>();
                text1.text = "[";
                break;
            case 4:
                text1 = prefab.transform.Find("Text").GetComponent<TextMeshPro>();
                text1.text = "]";
                break;
            case 5:
                text1 = prefab.transform.Find("Text").GetComponent<TextMeshPro>();
                text1.text = "(";
                break;
            case 6:
                text1 = prefab.transform.Find("Text").GetComponent<TextMeshPro>();
                text1.text = ")";
                break;
            default:
                text1 = prefab.transform.Find("Text").GetComponent<TextMeshPro>();
                text1.text = "^";
                break;
        }
        ChangeTextNormal(ref prefab);
        prefab.GetComponent<CardCondition>().myProblem = this;
        return prefab;
    }
}
