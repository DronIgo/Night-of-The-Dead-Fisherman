using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogTrigger : MonoBehaviour
{
    public Dialog dialog;

    public void TriggerDialog()
    {
        
        DialogManager.instance.StartDialog(dialog);
    }
}
