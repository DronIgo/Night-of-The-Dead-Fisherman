using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ProblemColorsNo : ProblemColorsCommon
{

    //Color[] allColors = new Color[] { Color.red, Color.blue, Color.cyan, Color.yellow, Color.white };
    //List<Color> actualColors = new List<Color>();
    public ProblemColorsNo()
    {
        int r1 = Random.Range(0, 5);
        int r2 = Random.Range(0, 5);
        actualColors.Add(allColors[r1]);
        while (r1 == r2)
            r2 = Random.Range(0, 5);
        actualColors.Add(allColors[r2]);
        hintText = "Must not use dice of colors " + GetColorName(actualColors[0]) + " and " + GetColorName(actualColors[1]) + ".";
    }

    public override bool CheckCondition(List<Cube> cubes)
    {
        foreach (Cube c in cubes)
        {
            if (actualColors.Contains(c.color))
            {
                Debug.Log("Color check not needed " + c.color);
                return false;

            }
        }
        return true;
    }

    public override int CheckConditionInt(List<Cube> cubes)
    {
        int res = 0;
        foreach (Cube c in cubes)
        {
            if (actualColors.Contains(c.color))
            {
                Debug.Log("Color check not needed " + c.color);
                res += actualColors.IndexOf(c.color) + 1;

            }
        }
        return res;
    }

    public override bool CheckConditionAndChange(List<Cube> cubes, ref GameObject part)
    {
        int res = CheckConditionInt(cubes);
        switch (res)
        {
            case 0:
                ChangeTextNormal(0, ref part);
                if (actualColors.Count > 1)
                    ChangeTextNormal(1, ref part);
                return true;
            case 1:
                ChangeTextWrong(0, ref part);
                if (actualColors.Count > 1)
                    ChangeTextNormal(1, ref part);
                return false;
            case 2:
                ChangeTextNormal(0, ref part);
                if (actualColors.Count > 1)
                    ChangeTextWrong(1, ref part);
                return false;
            case 3:
                ChangeTextWrong(0, ref part);
                if (actualColors.Count > 1)
                    ChangeTextWrong(1, ref part);
                return false;
            default:
                Debug.Log("Something is wrong with CheckConditionInt");
                return false;
        }
    }

    /*public override GameObject CreateTaskPart()
    {
        GameObject prefab = TaskGeneratorSource.instance.colorsNoPartPrefab;
        TextMeshPro color1 = prefab.transform.Find("Color1").GetComponent<TextMeshPro>();
        TextMeshPro color2 = prefab.transform.Find("Color2").GetComponent<TextMeshPro>();
        color1.color = actualColors[0];// TaskGeneratorSource.instance.FontByColor(actualColors[0], false);
        color1.enabled = true;
        if (actualColors.Count > 1)
        {
            color2.color = actualColors[1];//TaskGeneratorSource.instance.MaterialByColor(actualColors[1]);
            color2.enabled = true;
        }
        else
        {
            color2.enabled = false;
        }
        return prefab;
    }*/
}
