using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// GameManager tutorial is only used in the tutorial level and contains "road blocks" to prevent the player from progressing 
/// dialog until certain conditions are met 
/// </summary>
public class GameMangerTutorial : MonoBehaviour
{
    public NPCDialogTrigger captain = null;
    public NPCDialogTrigger man = null;

    public GameObject rerollNote = null;

    public Ghost capGhost = null;

    public bool WSBlocker = false;
    public bool DiceBlocker = false;
    public bool RerollBlocker = false;
    public bool TaskPickBlocker = false;
    public bool TaskBlocker = false;
    public bool HBlocker = false;
    public void WS()
    {
        WSBlocker = true;
    }

    public void DB()
    {
        DiceBlocker = true;
    }

    public void RB()
    {
        RerollBlocker = true;
    }
    public void TPB()
    {
        TaskPickBlocker = true;
    }
    public void TB()
    {
        TaskBlocker = true;
    }

    public void HB()
    {
        HBlocker = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ManDialog()
    {
        StartCoroutine(MD());
    }

    IEnumerator MD()
    {
        yield return new WaitForSeconds(5f);
        man.TriggerDialog();
    }

    IEnumerator CaptainDialog()
    {
        yield return new WaitForSeconds(2f);
        capGhost.ComeIn();
        yield return new WaitForSeconds(2f);
        captain.TriggerDialog();
    }


    bool w = false;
    bool s = false;

    bool start = false;
    public UnityEvent startEvents;    
    void Update()
    {
        if (!start)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                startEvents.Invoke();
                StartCoroutine(CaptainDialog());
                start = true;
            }
        }
        else
        {
            if ((!DiceBlocker && !RerollBlocker && !TaskBlocker && !TaskPickBlocker && !WSBlocker && !HBlocker) || (DialogManager.instance.typing))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DialogManager.instance.DisplayNextSentence();
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartGame();
            }

            if (WSBlocker)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    w = true;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    s = true;
                    w = false;
                }
                if (w && s)
                {
                    DialogManager.instance.DisplayNextSentence();
                    WSBlocker = false;
                }
            }
            if (DiceBlocker)
            {
                if (Grabber.instance.selectedObject != null)
                {
                    DialogManager.instance.DisplayNextSentence();
                    DiceBlocker = false;
                }
            }
            if (RerollBlocker)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    DialogManager.instance.DisplayNextSentence();
                    //rerollNote.SetActive(false);
                    RerollBlocker = false;
                }
            }
            if (HBlocker)
            {
                if (Input.GetKeyDown(KeyCode.H))
                {
                    DialogManager.instance.DisplayNextSentence();
                    //rerollNote.SetActive(false);
                    HBlocker = false;
                }
            }
            if (TaskPickBlocker)
            {
                if (TaskManager.instance.tasksInSlots[0] == null)
                {
                    DialogManager.instance.DisplayNextSentence();
                    TaskPickBlocker = false;
                }
            }
            if (TaskBlocker)
            {
                if (GameManager.instance.score > 0)
                {
                    TaskBlocker = false;
                    DialogManager.instance.DisplayNextSentence();
                }
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
}
