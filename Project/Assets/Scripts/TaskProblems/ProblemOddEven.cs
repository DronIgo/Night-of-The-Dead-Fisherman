using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemOddEven : TaskProblem
{
    public ProblemOddEven()
    {
        if (Random.Range(0f, 1f) > 0.5f)
        {
            hintText = "Must only use dice with odd values.";
            onlyOdd = true;
        }
        else
        {
            hintText = "Must only use dice with even values.";
            onlyEven = true;
        }
    }

    public override bool CheckCondition(List<Cube> cubes)
    {
        foreach (Cube c in cubes)
        {
            if (((c.CurrentValue % 2 == 0) && onlyOdd) || ((c.CurrentValue % 2 != 0) && onlyEven))
                return false;
        }
        return true;
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
                ChangeTextWrong(ref part);
                return false;
            case 1:
                ChangeTextNormal(ref part);
                return true;
            default:
                Debug.Log("Something is wrong with CheckConditionInt");
                return false;
        }
    }

    public override GameObject CreateTaskPart()
    {

        GameObject prefab = null;
        if (onlyOdd)
            prefab = TaskGeneratorSource.instance.allOddPrefab;
        if (onlyEven)
            prefab = TaskGeneratorSource.instance.allEvenPrefab;
        ChangeTextNormal(ref prefab);
        prefab.GetComponent<CardCondition>().myProblem = this;
        return prefab;
    }
}
