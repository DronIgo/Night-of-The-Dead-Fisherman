using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ProblemProduct : TaskProblem
{
    public int[] options = new int[] { 6, 12, 36, 48, 30, 15, 9, 25};
    public int product;
    public ProblemProduct(TaskProblem comp)
    {
        Generate(comp);
    }

    public override void Generate(TaskProblem compatable)
    {
        if (compatable.onlyEven)
            options = new int[] { 16, 24, 48, 32, 72 };
        if (compatable.onlyOdd)
            options = new int[] { 15, 9, 25 };
        List<int> actualOptions = new List<int>();
        
        foreach (int i in options)
        {
            if (i % compatable.specific == 0)
            {
                actualOptions.Add(i);
            }
        }
        int r = Random.Range(0, actualOptions.Count);
        product = actualOptions[r];
        hintText = "Product of the dice must be equal to " + product;
    }

    public override bool CheckCondition(List<Cube> cubes)
    {
        int currentProduct = 1;
        foreach (Cube c in cubes)
        {
            currentProduct *= c.CurrentValue;
        }
        if (product == currentProduct)
            return true;
        else
        {
            Debug.Log("Product check " + currentProduct);
            return false;
        }
    }

    public override bool CheckConditionAndChange(List<Cube> cubes, ref GameObject part)
    {
        int currentProduct = 1;
        foreach (Cube c in cubes)
        {
            currentProduct *= c.CurrentValue;
        }
        if (product == currentProduct)
        {
            ChangeTextRight(ref part);
            return true;
        }
        else
        {
            Debug.Log("Product check " + currentProduct);
            if (product % currentProduct == 0)
                ChangeTextNormal(ref part);
            else
                ChangeTextWrong(ref part);
            return false;
        }
    }

    //# is replaced by product sign in our font (code is kinda shitty)
    public override GameObject CreateTaskPart()
    {
        GameObject prefab = TaskGeneratorSource.instance.productPartPrefab;
        TextMeshPro text = prefab.transform.Find("Text").GetComponent<TextMeshPro>();
        text.text = "#=" + product.ToString();
        ChangeTextNormal(ref prefab);
        prefab.GetComponent<CardCondition>().myProblem = this;
        return prefab;
    }
}
