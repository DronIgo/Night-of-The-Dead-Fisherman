using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unfreezer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Cube>(out Cube c))
        {
            if (!BowlManager.instance.cubesInBowl.Contains(c))
                BowlManager.instance.cubesInBowl.Add(c);
        }
        if (other.gameObject.TryGetComponent<TaskPaper>(out TaskPaper tp))
        {
            tp.ResetPostion();
        }
    }
}
