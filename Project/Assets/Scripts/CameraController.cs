using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    Quaternion directedAtWorkspace;
    Quaternion directedAtOrders;
    public float rotationSpeed = 0.5f;
    bool controllable = true;
    public AudioSource crowd = null; 
    // Start is called before the first frame update
    void Start()
    {
        directedAtWorkspace = Camera.main.transform.rotation;
        directedAtOrders = Quaternion.Euler(new Vector3(12, -90, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (controllable && !GameManager.instance.hintActive)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                controllable = false;
                print("W pressed");
                StartCoroutine("RotateForward");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                controllable = false;
                Debug.Log("S pressed");
                StartCoroutine("RotateDown");
            }
        }
    }

    public void RotateUp()
    {
        StartCoroutine(RotateForwardSpeed(1.2f));
    }

    IEnumerator RotateForwardSpeed(float speed)
    {
        Debug.Log("Routine forward started");
        Grabber.instance.ReleaseDrag();
        Quaternion targetRotation = directedAtOrders;
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        controllable = true;
        crowd.volume = 0.04f;
    }

    IEnumerator RotateForward()
    {
        Debug.Log("Routine forward started");
        Grabber.instance.ReleaseDrag();
        Quaternion targetRotation = directedAtOrders;
        while (transform.rotation != targetRotation) {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        controllable = true;
        crowd.volume = 0.04f;
    }

    IEnumerator RotateDown()
    {
        Debug.Log("Back down started");

        Grabber.instance.switchHoldState();
        while (transform.rotation != directedAtWorkspace)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, directedAtWorkspace, rotationSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        controllable = true;
        crowd.volume = 0.0f;
    }
}
