using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ProblemColorsCommon : TaskProblem
{
    protected Color[] allColors = new Color[] { Color.red, Color.blue, Color.cyan, Color.yellow, Color.white };
    protected List<Color> actualColors = new List<Color>();
    
    protected string GetColorName(Color color)
    {
        if (color == Color.red)
            return "red";
        if (color == Color.blue)
            return "blue";
        if (color == Color.cyan)
            return "cyan";
        if (color == Color.yellow)
            return "yellow";
        if (color == Color.white)
            return "white";
        return "-";
    }
    public override GameObject CreateTaskPart()
    {
        GameObject prefab = null;
        if (this is ProblemColorsMust)
            prefab = TaskGeneratorSource.instance.colorsMustPartPrefab;
        else
            prefab = TaskGeneratorSource.instance.colorsNoPartPrefab;
        TextMeshPro color1 = prefab.transform.Find("Color1").Find("Color").GetComponent<TextMeshPro>();
        TextMeshPro color2 = prefab.transform.Find("Color2").Find("Color").GetComponent<TextMeshPro>();
        color1.color = actualColors[0];// TaskGeneratorSource.instance.FontByColor(actualColors[0], false);
        color1.enabled = true;
        if (actualColors.Count > 1)
        {
            prefab.transform.Find("Color2").gameObject.SetActive(true);
            color2.color = actualColors[1];//TaskGeneratorSource.instance.MaterialByColor(actualColors[1]);
            color2.enabled = true;
        }
        else
        {
            prefab.transform.Find("Color2").gameObject.SetActive(false);
            color2.enabled = false;
        }
        ChangeTextNormal(0, ref prefab);
        if (actualColors.Count > 1)
        {
            ChangeTextNormal(1, ref prefab);
        }
        prefab.GetComponent<CardCondition>().myProblem = this;
        return prefab;
    }

    protected void ChangeTextRight(int index, ref GameObject part)
    {
        Color color1 = part.transform.Find("Color1").Find("Color").GetComponent<TextMeshPro>().color;
        Color color2 = part.transform.Find("Color2").Find("Color").GetComponent<TextMeshPro>().color;
        Color colorIndex = actualColors[index];
        if (color1 == colorIndex)
        {
            GameObject g = part.transform.Find("Color1").gameObject;
            ChangeText(Mode.Right, ref g);
        }
        if (color2 == colorIndex)
        {
            GameObject g = part.transform.Find("Color2").gameObject;
            ChangeText(Mode.Right, ref g);
        }
    }

    protected void ChangeTextNormal(int index, ref GameObject part)
    {
        Color color1 = part.transform.Find("Color1").Find("Color").GetComponent<TextMeshPro>().color;
        Color color2 = part.transform.Find("Color2").Find("Color").GetComponent<TextMeshPro>().color;
        Color colorIndex = actualColors[index];
        if (color1 == colorIndex)
        {
            GameObject g = part.transform.Find("Color1").gameObject;
            ChangeText(Mode.Normal, ref g);
        }
        if (color2 == colorIndex)
        {
            GameObject g = part.transform.Find("Color2").gameObject;
            ChangeText(Mode.Normal, ref g);
        }
    }

    protected void ChangeTextWrong(int index, ref GameObject part)
    {
        Color color1 = part.transform.Find("Color1").Find("Color").GetComponent<TextMeshPro>().color;
        Color color2 = part.transform.Find("Color2").Find("Color").GetComponent<TextMeshPro>().color;
        Color colorIndex = actualColors[index];
        if (color1 == colorIndex)
        {
            GameObject g = part.transform.Find("Color1").gameObject;
            ChangeText(Mode.Wrong, ref g);
        }
        if (color2 == colorIndex)
        {
            GameObject g = part.transform.Find("Color2").gameObject;
            ChangeText(Mode.Wrong, ref g);
        }
    }
    protected enum Mode { Wrong, Right, Normal};
    protected void ChangeText(Mode mode, ref GameObject g)
    {
        switch (mode)
        {
            case Mode.Right:
                g.GetComponent<TextMeshPro>().font = TaskGeneratorSource.instance.taskFontRight;
                g.GetComponent<TextMeshPro>().fontMaterial = TaskGeneratorSource.instance.taskFontRightMaterial;
                g.GetComponent<TextMeshPro>().color = TaskGeneratorSource.instance.taskFontRightColor;
                break;
            case Mode.Normal:
                g.GetComponent<TextMeshPro>().font = TaskGeneratorSource.instance.taskFontNormal;
                g.GetComponent<TextMeshPro>().fontMaterial = TaskGeneratorSource.instance.taskFontNormalMaterial;
                g.GetComponent<TextMeshPro>().color = TaskGeneratorSource.instance.taskFontNormalColor;
                break;
            case Mode.Wrong:
                g.GetComponent<TextMeshPro>().font = TaskGeneratorSource.instance.taskFontWrong;
                g.GetComponent<TextMeshPro>().fontMaterial = TaskGeneratorSource.instance.taskFontWrongMaterial;
                g.GetComponent<TextMeshPro>().color = TaskGeneratorSource.instance.taskFontWrongColor;
                break;
        }
    }
}
