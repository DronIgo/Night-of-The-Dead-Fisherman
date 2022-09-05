using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<Cube>(out Cube c)) {
            other.transform.position = BowlManager.instance.bowl.transform.position + new Vector3(0, 5, 0);
        } else if (other.TryGetComponent<TaskPaper>(out TaskPaper tp)) {
            tp.ResetPostion();
        }
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
