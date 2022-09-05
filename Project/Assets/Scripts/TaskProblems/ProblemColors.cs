using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemColors : TaskProblem
{

    Color[] allColors = new Color[] { Color.red, Color.blue, Color.cyan, Color.yellow, Color.white };
    List<Color> actualColors = new List<Color>();
    public ProblemColors()
    {
        int r1 = Random.Range(0, 5);
        int r2 = Random.Range(0, 5);
        int r3 = Random.Range(0, 5);
        actualColors.Add(allColors[r1]);
        if (r1 != r2)
            actualColors.Add(allColors[r2]);
        if (r3 != r1 && r3 != r2)
            actualColors.Add(allColors[r3]);
    }

    public override bool CheckCondition(List<Cube> cubes)
    {
        List<Color> colorsInCubes = new List<Color>();
        foreach (Cube c in cubes)
        {
            if (!actualColors.Contains(c.color))
            {
                Debug.Log("Color check not needed " + c.color);
                return false;
                
            }
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

    public override GameObject CreateTaskPart()
    {
        GameObject prefab = TaskGeneratorSource.instance.colorsPartPrefab;
        MeshRenderer color1 = prefab.transform.Find("Color1").GetComponent<MeshRenderer>();
        MeshRenderer color2 = prefab.transform.Find("Color2").GetComponent<MeshRenderer>();
        MeshRenderer color3 = prefab.transform.Find("Color3").GetComponent<MeshRenderer>();
        color1.material = TaskGeneratorSource.instance.MaterialByColor(actualColors[0]);
        color1.enabled = true;
        if (actualColors.Count > 1)
        {
            color2.material = TaskGeneratorSource.instance.MaterialByColor(actualColors[1]);
            color2.enabled = true;
        } else
        {
            color2.enabled = false;
        }
        if (actualColors.Count > 2)
        {
            color3.material = TaskGeneratorSource.instance.MaterialByColor(actualColors[2]);
            color3.enabled = true;
        }
        else
        {
            color3.enabled = false;
        }
        return prefab;
    }
}
