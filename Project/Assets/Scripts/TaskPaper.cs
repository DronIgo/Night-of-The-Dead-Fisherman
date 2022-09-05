using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPaper : MonoBehaviour
{
    public float spawnBoundaries = 2;
    public float rotationBoundaries = 15;

    public void PlaceOnSlot(Transform slot) {
        transform.rotation = Quaternion.Euler(-90, -90 + Random.Range(-spawnBoundaries, spawnBoundaries), Random.Range(-spawnBoundaries, spawnBoundaries));
        transform.position = slot.position - new Vector3(0, 8f, 0);
        GetComponent<Collider>().isTrigger = true;
        tag = "Hanging";
    }

    public void ReleaseFromCamera() {
        ResetPostion();
        tag = "Drag";
        GetComponent<Collider>().isTrigger = false;
    }

    public void ResetPostion()
    {
        Transform startingPoint = TaskManager.instance.taskReturningPoint;
        transform.position = startingPoint.position + new Vector3(Random.Range(-spawnBoundaries, spawnBoundaries), 0, Random.Range(-spawnBoundaries, spawnBoundaries));
        transform.rotation = Quaternion.Euler(0, -90 + Random.Range(-rotationBoundaries, rotationBoundaries), 0);
    }
}
