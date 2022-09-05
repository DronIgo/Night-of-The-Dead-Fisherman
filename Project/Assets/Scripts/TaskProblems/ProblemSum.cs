using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ProblemSum : TaskProblem
{
    public int sum;
    public GameObject part = null;
    public ProblemSum(TaskProblem comp) { //Needs to be refactored this is ass
        sum = ThreeRolls();
        if (comp.onlyEven)
        {
            while (sum % 2 != 0 || sum < specific + 2)
                sum = ThreeRolls();
        }
        else
        {
            while (sum < specific + 2)
                sum = ThreeRolls();
        }
        hintText = "The sum of all dice on card must be equal to " + sum;
    }

    int ThreeRolls()
    {
        return Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
    }

    public override bool CheckCondition(List<Cube> cubes)
    {
        int currentSum = 0;
        foreach (Cube c in cubes)
        {
            currentSum += c.CurrentValue;
        }
        if (currentSum == sum)
            return true;
        else
        {
            Debug.Log("Sum check " + currentSum);
            return false;
        }
    }

    public override bool CheckConditionAndChange(List<Cube> cubes, ref GameObject part)
    {
        int currentSum = 0;
        foreach (Cube c in cubes)
        {
            currentSum += c.CurrentValue;
        }
        if (currentSum == sum)
        {
            ChangeTextRight(ref part);
            return true;
        }
        else
        {
            Debug.Log("Sum check " + currentSum);
            if (currentSum > sum)
                ChangeTextWrong(ref part);
            else
                ChangeTextNormal(ref part);
            return false;
        }
    }

    //$ is replaced by sum sign in our font (code is kinda shitty)
    public override GameObject CreateTaskPart()
    {
        GameObject prefab = TaskGeneratorSource.instance.sumPartPrefab;
        TextMeshPro text = prefab.transform.Find("Text").GetComponent<TextMeshPro>();
        text.text = "$=" + sum.ToString();
        ChangeTextNormal(ref prefab);
        prefab.GetComponent<CardCondition>().myProblem = this;
        return prefab;
    }
}
