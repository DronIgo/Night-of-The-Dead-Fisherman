using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogManager : MonoBehaviour
{
    /// <summary>
    /// Instance of self for singleton
    /// </summary>
    public static DialogManager instance = null;
    /// <summary>
    /// Text window for sentences
    /// </summary>
    public TextMeshProUGUI dialogText;
    /// <summary>
    /// Text window for NPC's name
    /// </summary>
    public TextMeshProUGUI nameText;
    private Queue<string> sentences;
    private UnityEvent[] dialogEvents;
    /// <summary>
    /// Wheter or not there is an active dialog at the moment
    /// </summary>
    public bool inDialog = false;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        sentences = new Queue<string>();
    }
    /// <summary>
    /// Initiates the dialog using sentences and NPC name from given instance of class Dialog
    /// </summary>
    /// <param name="dialog"></param>
    public void StartDialog(Dialog dialog)
    {
        if (!inDialog)
        {
            nameText.text = dialog.name;
            dialogEvents = dialog.events;
            //GameManager.instance.InitiateDialog();
            sentences.Clear();
            foreach (string sentence in dialog.sentences)
            {
                sentences.Enqueue(sentence);
            }
            sentenceIndex = -1;
            DisplayNextSentence();
            inDialog = true;
        }
    }

    public void DisplayNextSentence()
    {
        if (typing)
        {
            StopCoroutine("TypeSentence");
            dialogText.text = currentSentence;
            typing = false;
        }
        else
        {
            if (sentences.Count == 0)
            {
                if (sentenceIndex >= 0 && dialogEvents.Length > sentenceIndex)
                    dialogEvents[sentenceIndex].Invoke();
                sentenceIndex += 1;
                EndDialog();
                return;
            }
            else
            {
                if (sentenceIndex >= 0 && dialogEvents.Length > sentenceIndex)
                    dialogEvents[sentenceIndex].Invoke();
                sentenceIndex += 1;
                currentSentence = sentences.Dequeue();
                StartCoroutine("TypeSentence");
            }
        }
    }

    public bool typing = false;
    public float typeDelay = 0.05f;
    string currentSentence;
    int sentenceIndex = 0;

    IEnumerator TypeSentence ()
    {
        typing = true;
        dialogText.text = "";
        foreach (char letter in currentSentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typeDelay);
        }
        typing = false;
    }

    void EndDialog()
    {
        inDialog = false;
        //GameManager.instance.StopDialog();
    }
}
