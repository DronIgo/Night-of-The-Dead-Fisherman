using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProblemColorsMust : ProblemColorsCommon
{

    //Color[] allColors = new Color[] { Color.red, Color.blue, Color.cyan, Color.yellow, Color.white };
    //List<Color> actualColors = new List<Color>();
    public ProblemColorsMust()
    {
        int r1 = Random.Range(0, 5);
        int r2 = Random.Range(0, 5);
        actualColors.Add(allColors[r1]);
        while (r1 == r2)
            r2 = Random.Range(0, 5);
        actualColors.Add(allColors[r2]);
        hintText = "Must use at least one die of colors " + GetColorName(actualColors[0]) + " and " + GetColorName(actualColors[1]) + "."; 
    }

    public override bool CheckCondition(List<Cube> cubes)
    {
        List<Color> colorsInCubes = new List<Color>();
        foreach (Cube c in cubes)
        {
            if (!colorsInCubes.Contains(c.color))
                colorsInCubes.Add(c.color);
        }
        foreach (Color ac in actualColors)
        {
            if (!colorsInCubes.Contains(ac))
            {
                Debug.Log("Color check missing " + ac);
                return false;
            }
        }
        return true;
    }

    public override int CheckConditionInt(List<Cube> cubes)
    {
        int res = 0;
        List<Color> colorsInCubes = new List<Color>();
        foreach (Cube c in cubes)
        {
            if (!colorsInCubes.Contains(c.color))
                colorsInCubes.Add(c.color);
        }
        foreach (Color ac in actualColors)
        {
            if (colorsInCubes.Contains(ac))
            {
                //Debug.Log("Color check missing " + ac);
                res += actualColors.IndexOf(ac) + 1;
            }
        }
        return  res;
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
                return false;
            case 1:
                ChangeTextRight(0, ref part);
                if (actualColors.Count > 1)
                {
                    ChangeTextNormal(1, ref part);
                    return false;
                }
                return true;
            case 2:
                ChangeTextNormal(0, ref part);
                if (actualColors.Count > 1)
                    ChangeTextRight(1, ref part);
                return false;
            case 3:
                ChangeTextRight(0, ref part);
                if (actualColors.Count > 1)
                    ChangeTextRight(1, ref part);
                return true;
            default:
                Debug.Log("Something is wrong with CheckConditionInt");
                return false;
        }
    }

    /*public override GameObject CreateTaskPart()
    {
        GameObject prefab = TaskGeneratorSource.instance.colorsMustPartPrefab;

        TextMeshPro color1 = prefab.transform.Find("Color1").Find("Color").GetComponent<TextMeshPro>();
        TextMeshPro color2 = prefab.transform.Find("Color2").Find("Color").GetComponent<TextMeshPro>();
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
