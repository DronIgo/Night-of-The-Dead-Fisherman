using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// BowlManager keeps information about all dice in the bowl, as well as contains methods for rerolling nad creating new dice
/// </summary>
public class BowlManager : MonoBehaviour
{
    public static BowlManager instance = null;
    public List<Cube> cubesInBowl = new List<Cube>();
    public Transform bowl;
    public GameObject cubePrefab = null;
    public Color[] allColors = new Color[] { 
        Color.red, 
        Color.blue,
        Color.yellow, 
        Color.white,
        Color.cyan
    };

    Dictionary<Color, int> allColorsDict = new Dictionary<Color, int>()
    {
        {Color.red , 0},
        {Color.blue , 1},
        {Color.yellow , 2},
        {Color.white , 3},
        {Color.cyan , 4}
    };
    public GameObject dices = null;
    public GameObject dicesSmall = null;
    public Color GetRandomColor()
    {
        return allColors[Random.Range(0, 5)];
    }

    public Color GetRandomColor(float[] probs)
    {
        float sum = 0;
        foreach (float p in probs)
        {
            sum += p;
        }
        float r = Random.Range(0, sum);
        for (int i = 0; i < probs.Length; i++)
        {
            r -= probs[i];
            if (r <= 0)
                return allColors[i];
        }
        return allColors[4];
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        foreach (Cube cube in FindObjectsOfType<Cube>())
        {
            cubesInBowl.Add(cube);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RollAll();
        }

    }
    /// <summary>
    /// Rolls all dice in the bowl
    /// </summary>
    public void RollAll()
    {
        if (cubesInBowl.Count > 0)
        {
            if (cubesInBowl.Count < 10)
            {
                Instantiate(dicesSmall);
            } else
            {
                Instantiate(dices);
            }
        }
        foreach (Cube cube in cubesInBowl)
            cube.Roll();
    }

    int[] numOfDicePerColor = new int[5];

    public void LowerTheNumberByColor(Color color)
    {
        numOfDicePerColor[allColorsDict[color]]--;
    }

    public void GenerateDice(int numberOfDice)
    {
        if (numberOfDice == 0)
            return;
        for (int i = 0; i < numberOfDice; i++) {
            GameObject cube = Instantiate(cubePrefab, bowl.position + new Vector3(Random.Range(-8, 8), 
                Random.Range(2, 3), Random.Range(-8, 8)), Quaternion.identity);
            Cube myCube = cube.GetComponent<Cube>();
            cubesInBowl.Add(myCube);
            myCube.Generate(numOfDicePerColor.Select(x => 1/ (1+(float)x)).ToArray()); //Basically python 
            myCube.Roll();
            numOfDicePerColor[allColorsDict[myCube.color]] += 1;
        }
        if (numberOfDice > 10)
        {
            Instantiate(dices);
        } else
        {
            Instantiate(dicesSmall);
        }
    }
}
