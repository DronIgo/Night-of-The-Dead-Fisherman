using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// TaskGeneratorSource is a class with resources used in a TaskProblem and it's inherited classes
/// </summary>
public class TaskGeneratorSource : MonoBehaviour
{
    public GameObject sumPartPrefab = null;
    public GameObject productPartPrefab = null;
    public GameObject colorsPartPrefab = null;
    public GameObject colorsNoPartPrefab = null;
    public GameObject colorsMustPartPrefab = null;
    public GameObject specificPartPrefab = null;
    public GameObject allOddPrefab = null;
    public GameObject allEvenPrefab = null;

    public Material red;
    public Material white;
    public Material blue;
    public Material yellow;
    public Material cyan;

    public TMP_FontAsset taskFontNormal;
    public TMP_FontAsset taskFontRight;
    public TMP_FontAsset taskFontWrong;

    public Material taskFontNormalMaterial;
    public Material taskFontRightMaterial;
    public Material taskFontWrongMaterial;

    public Color taskFontNormalColor;
    public Color taskFontRightColor;
    public Color taskFontWrongColor;

    public static TaskGeneratorSource instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Material MaterialByColor(Color color)
    {
        if (color == Color.red)
        {
            return red;
        }
        if (color == Color.blue)
        {
            return blue;
        }
        if (color == Color.yellow)
        {
            return yellow;
        }
        if (color == Color.white)
        {
            return white;
        }
        if (color == Color.cyan)
        {
            return cyan;
        }
        return red;
    }
}
