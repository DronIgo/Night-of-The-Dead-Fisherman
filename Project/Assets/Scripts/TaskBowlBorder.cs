using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBowlBorder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent<TaskPaper>(out TaskPaper tp)) {
            tp.ResetPostion();
        }
    }
}
