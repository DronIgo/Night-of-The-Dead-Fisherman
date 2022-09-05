using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Task contains the logic behind each card and it's conditions. This class generates conditions 
/// and then checks if they are fullfilled 
/// </summary>
public class Task : MonoBehaviour
{
    public Transform node1;
    public Transform node2;
    public Transform node3;

    public int forTutorial = -1;

    public GameObject animatedRoll = null;
    private void Start()
    {
        //incredably bad code
        if (forTutorial != -1)
        {
            ProblemSum tutorialSum = new ProblemSum(new TaskProblem());
            tutorialSum.sum = forTutorial;
            problems.Add(tutorialSum);
            partInstances[0] = transform.Find("node1").Find("Sum").gameObject;
        }
    }

    public List<Cube> cubesOnTask = new List<Cube>();
    public GameObject task = null;

    public CardPlace MyCardPlace { private get; set; }

    public void RemoveCube(GameObject newCube)
    {
        newCube.transform.parent = null;
        if (newCube.TryGetComponent<Cube>(out Cube c))
        {
            if (cubesOnTask.Contains(c))
            {
                cubesOnTask.Remove(c);
            }
            if (CheckCondition())
            {
                FinishTask();
            }
        }
    }

    public void AddCube(GameObject newCube)
    {
        bool finishedTask = false;
        if (newCube.TryGetComponent<Cube>(out Cube c))
        {
            if (!cubesOnTask.Contains(c))
            {
                cubesOnTask.Add(c);
            }
            if (CheckCondition())
            {
                FinishTask();
                finishedTask = true;
            }
        }
        if (!finishedTask)
            newCube.transform.SetParent(this.transform, true);
    }

    private bool CheckCondition()
    {
        bool res = true;
        for (int i = 0; i < problems.Count; i++)
        {
            TaskProblem prob = problems[i];
            if (!prob.CheckConditionAndChange(cubesOnTask, ref partInstances[i]))
                res = false;
        }
        return res;
    }

    private void FinishTask()
    {
        foreach (Cube c in cubesOnTask)
        {
            BowlManager.instance.LowerTheNumberByColor(c.color);
        }
        RollAnimation();
        TaskManager.instance.ReturnDice(cubesOnTask.Count);
        TaskManager.instance.CompleteTask(this.gameObject);
        MyCardPlace.RemoveCard();
        Destroy(task);
    }

    private void RollAnimation()
    {
        GameObject roll = Instantiate(animatedRoll, this.transform.position, this.transform.rotation);
        Transform cubesPoint = roll.transform.Find("DicePosition");
        float pos = -0.5f * (cubesOnTask.Count - 1);
        foreach (Cube c in cubesOnTask)
        {

            Destroy(c.gameObject.GetComponent<Rigidbody>());
            Destroy(c.gameObject.GetComponent<BoxCollider>());
            Destroy(c.gameObject.GetComponent<Cube>());
            Transform ct = c.gameObject.transform;

            ct.parent = null;
            ct.SetParent(cubesPoint, true);
            ct.localPosition = new Vector3(pos, 0, 0);
            ct.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            pos += 1f;
        }
    }

    enum ProblemTypes{
        Sum,
        Product,
        Specific,
        OddEven,
        ColorsNo,
        ColorsMust
    };

    List<TaskProblem> problems = new List<TaskProblem>();
    public void Generate(int difficulty)
    {
        TaskProblem prob1 = null;
        TaskProblem prob2 = null;
        TaskProblem prob3 = null;
        GameObject partPrefab1 = null;
        GameObject partPrefab2 = null;
        GameObject partPrefab3 = null;
        switch (difficulty)
        {
            case 0:
                prob1 = GenerateProblem(new Dictionary<ProblemTypes, float> { { ProblemTypes.Sum, 0.7f }, 
                    {ProblemTypes.Product, 0.3f }}, new TaskProblem());
                break;
            case 1:
                prob2 = GenerateProblem(new Dictionary<ProblemTypes, float> { { ProblemTypes.Specific, 0.5f },
                    {ProblemTypes.OddEven, 0.5f }}, new TaskProblem());
                prob1 = GenerateProblem(new Dictionary<ProblemTypes, float> { { ProblemTypes.Sum, 0.8f },
                    {ProblemTypes.Product, 0.2f }}, prob2);
                break;
            case 2:
                prob2 = GenerateProblem(new Dictionary<ProblemTypes, float> { { ProblemTypes.ColorsMust, 0.5f },
                    {ProblemTypes.ColorsNo, 0.5f }}, new TaskProblem());
                prob1 = GenerateProblem(new Dictionary<ProblemTypes, float> { { ProblemTypes.Sum, 0.7f },
                    {ProblemTypes.Product, 0.3f }}, prob2);
                break;
            case 3:
                prob3 = GenerateProblem(new Dictionary<ProblemTypes, float> { { ProblemTypes.Specific, 0.5f },
                    {ProblemTypes.OddEven, 0.5f }}, new TaskProblem());
                prob2 = GenerateProblem(new Dictionary<ProblemTypes, float> { { ProblemTypes.ColorsMust, 0.5f },
                    {ProblemTypes.ColorsNo, 0.5f }}, new TaskProblem());
                prob1 = GenerateProblem(new Dictionary<ProblemTypes, float> { { ProblemTypes.Sum, 0.6f },
                    {ProblemTypes.Product, 0.4f }}, prob3);
                break;
        }
        if (prob1 != null)
        {
            problems.Add(prob1);
            partPrefab1 = prob1.CreateTaskPart();
        }
        if (prob2 != null)
        {
            problems.Add(prob2);
            partPrefab2 = prob2.CreateTaskPart();
        }
        if (prob3 != null)
        {
            problems.Add(prob3);
            partPrefab3 = prob3.CreateTaskPart();
        }
        CreateTaskPrefab(partPrefab1, partPrefab2, partPrefab3);
    }

    private TaskProblem GenerateProblem(Dictionary<ProblemTypes, float> chances, TaskProblem comp)
    {
        float all = 0;
        foreach (float c in chances.Values)
            all += c;
        float r = Random.Range(0, all);
        ProblemTypes type = ProblemTypes.Sum;
        foreach (KeyValuePair<ProblemTypes, float> entry in chances)
        {
            r -= entry.Value;
            if (r <= 0)
            {
                type = entry.Key;
                break;
            }
        }
        switch (type)
        {
            case ProblemTypes.Sum:
                return new ProblemSum(comp);
            case ProblemTypes.Product:
                return new ProblemProduct(comp);
            case ProblemTypes.ColorsMust:
                return new ProblemColorsMust();
            case ProblemTypes.ColorsNo:
                return new ProblemColorsNo();
            case ProblemTypes.Specific:
                return new ProblemSpecific();
            case ProblemTypes.OddEven:
                return new ProblemOddEven();
            default:
                return new ProblemSum(comp);
        }
    }

    GameObject[] partInstances = new GameObject[3];

    void CreateTaskPrefab(GameObject part1, GameObject part2, GameObject part3)
    {
        for (int i = 0; i < node1.childCount; i++)
            Destroy(node1.GetChild(i).gameObject);
        for (int i = 0; i < node2.childCount; i++)
            Destroy(node2.GetChild(i).gameObject);
        for (int i = 0; i < node3.childCount; i++)
            Destroy(node3.GetChild(i).gameObject);
        if (part1 != null)
        {
            partInstances[0] = Instantiate(part1, node1.position, node1.rotation, node1);
            partInstances[0].GetComponent<CardCondition>().myProblem = problems[0];
        }
        if (part2 != null)
        {
            partInstances[1] = Instantiate(part2, node2.position, node1.rotation, node2);
            partInstances[1].GetComponent<CardCondition>().myProblem = problems[1];
        }
        if (part3 != null)
        {
            partInstances[2] = Instantiate(part3, node3.position, node1.rotation, node3);
            partInstances[2].GetComponent<CardCondition>().myProblem = problems[2];
        }
    }
}
