using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    public static TaskManager instance = null;
    public int maxAmountOfTasks = 15;
    public List<Transform> slots = new List<Transform>();
    public GameObject taskPrefab;
    public int numberOfTasks = 0;
    public Transform taskReturningPoint;

    public List<GameObject> tasksInSlots;
    private int numberOfSlots = 6;

    public AudioSource taskCreated; 
    public AudioSource taskCompleted;
    public AudioSource taskriped;


    private void Awake() {
        if (instance == null)
            instance = this;

        for (int i = 0; i < slots.Count; i++)
            tasksInSlots.Add(null);
    }

    public void SetNewTask(GameObject task) //only used in a tutorial - this code is super bad
    {
        numberOfTasks += 1;
        SetTask(task);
    }

    public bool SetTask(GameObject task)
    {
        if (numberOfTasks >= maxAmountOfTasks)
        {
            return false;
        }

        bool areSlotsFull = true;
        int slotsIndex = 0;
        for (int index = 0; index < slots.Count; index++)
        {
            if (tasksInSlots[index] == null)
            {
                slotsIndex = index;
                areSlotsFull = false;
                break;
            }
        }

        if (areSlotsFull)
        {
            return false;
        }
        float pitch = 0;
        for (int index = 0; index < slots.Count; index++)
        {
            if (tasksInSlots[index] != null)
            {
                pitch--;
            }
        }
        taskCreated.pitch = 2f + 0.4f * pitch;
        taskCreated.Play();
        tasksInSlots[slotsIndex] = task;
        task.GetComponent<TaskPaper>().PlaceOnSlot(slots[slotsIndex]);
        return true;
    }

    public void CreateNewTask(int difficulty) {
        GameObject task = Instantiate(taskPrefab, Vector3.zero, Quaternion.identity);
        task.GetComponent<Task>().Generate(difficulty);
        numberOfTasks += 1;
        //Debug.Log(slots.Count);

        if (!SetTask(task))
        {
            GameManager.instance.GameOver();
        }
    }

    public void RemoveFromSlot(GameObject task) {
        taskriped.Play();
        int idx = tasksInSlots.IndexOf(task);
        tasksInSlots[idx] = null;
    }

    public void CompleteTask(GameObject task) {
        taskCompleted.Play();
        numberOfTasks -= 1;
        GameManager.instance.IncrementScore(1); ;
    }

    public void ReturnDice(int numOfDice)
    {
        StartCoroutine(ReturnDiceCor(numOfDice));
    }

    public IEnumerator ReturnDiceCor(int numOfDice)
    {
        yield return new WaitForSeconds(1f);
        BowlManager.instance.GenerateDice(numOfDice);
    }
}
