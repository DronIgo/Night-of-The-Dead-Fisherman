using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grabber contains all the logic behind picking objects up and moving them around
/// </summary>
public class Grabber : MonoBehaviour
{
    public static Grabber instance = null;
    public float dragPlaneHeight = 15;
    public float speedModifier = 50;
    public float downwardsSpeed = -10;
    public float showDistance = 20f;
    public int holdState = 0;

    public GameObject selectedObject;
    private Vector3 objectLastPos = Vector3.zero;

    //Normalized rotations
    public Vector3 sideOneRotation = new Vector3(0, 0, 0);
    public Vector3 sideSixRotation = new Vector3(180, 0, 0);
    public Vector3 sideTwoRotation = new Vector3(90, 0, 180);
    public Vector3 sideFiveRotation = new Vector3(-90, 0, 180);
    public Vector3 sideThreeRotation = new Vector3(0, 0, -90);
    public Vector3 sideFourRotation = new Vector3(0, 0, 90);

    public CardPlace currentCardPlace { get; internal set; }
    Task currentCard = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        defForRaycast = LayerMask.GetMask("Default") + LayerMask.GetMask("Card") + LayerMask.GetMask("CubeOutOfBox");
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.instance.hintActive)
            return;
        if (currentCard != null)
            CheckCardPlace();
        else
            TurnOffCardPlace();

        //Check if LMB is being held
        if (Input.GetMouseButton(0)) {
            
            //Check if there is an object being held already
            if (holdState == 0) {

                //Locate the object
                RaycastHit hit = CastRay(defForRaycast);

                if (hit.collider != null) {
                    //If the object is not draggable, ignore
                    if (hit.collider.CompareTag("Drag"))
                    {
                        holdState = 1;
                        Grab(hit);
                    }
                    else if (hit.collider.CompareTag("Hanging")) {
                        holdState = 2;
                        PullFromSlot(hit);
                    }
                }
            }

            if (holdState == 1)
            {
                MoveToCursor();
                objectLastPos = selectedObject.transform.position;
            }
            else if (holdState == 2) {
                ShowToCamera();
            }
        }
        else {
            //Release object
            if (holdState == 1)
            {
                ReleaseDrag();
            }
            else if (holdState == 2)
            {
                ReleaseDrag();
                //Rotate correctly, change tag, reset to spawn
                //selectedObject.GetComponent<TaskPaper>().ReleaseFromCamera();

                //selectedObject = null;
                //Cursor.visible = true;
            }

            holdState = 0;
        }
    }

    public void ReleaseDrag() {
        //Release object
        if (selectedObject == null)
            return;
        if (selectedObject.TryGetComponent<TaskPaper>(out TaskPaper tp))
        {
            if (currentCardPlace != null)
            {
                currentCardPlace.PlaceCard(selectedObject);
                selectedObject.tag = "Card";
                currentCard = null;
            } else
            {
                TaskManager.instance.SetTask(selectedObject);
            }
        }
        if (selectedObject.TryGetComponent<Cube>(out Cube c))
        {
            MoveToCursor();

            //Reset object velocity
            selectedObject.GetComponent<Rigidbody>().velocity = new Vector3(0, downwardsSpeed, 0);// + speedModifier * (selectedObject.transform.position - objectLastPos);

            //Enable Gravity
            selectedObject.GetComponent<Rigidbody>().useGravity = true;
            
            //Enable Cube
            c.CustomEnable(true);      
        }
        //What we always do

        //Deselect object
        selectedObject = null;
        Cursor.visible = true;

        holdState = -1;
    }

    private void ShowToCamera()
    {
        selectedObject.transform.up = -Camera.main.transform.forward;
        selectedObject.transform.RotateAround(selectedObject.transform.position, selectedObject.transform.up, -90);
        selectedObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, showDistance));
    }
    public void switchHoldState()
    {
        if (holdState == 2)
        {
            selectedObject.GetComponent<TaskPaper>().ReleaseFromCamera();

            selectedObject.tag = "Drag";

            holdState = 1;
            /*Rigidbody rig = selectedObject.GetComponent<Rigidbody>();
            //Freeze regidbody
            rig.freezeRotation = true;
            //Turn Gravity off
            rig.useGravity = false;
            //Set speed to 0
            rig.velocity = Vector3.zero;*/
        }
    }
    private void PullFromSlot(RaycastHit hit) {
        selectedObject = hit.collider.gameObject;
        Cursor.visible = false;

        //Rigidbody rig = selectedObject.GetComponent<Rigidbody>();

        //rig.useGravity = false;
        //rig.velocity = Vector3.zero;

        selectedObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * showDistance;
        selectedObject.transform.up = -Camera.main.transform.forward;

        currentCard = selectedObject.GetComponent<Task>();
        TaskManager.instance.RemoveFromSlot(selectedObject);
    }

    void Grab(RaycastHit hit) {
        //Select object
        selectedObject = hit.collider.gameObject;
        Cursor.visible = false;

        Rigidbody rig = selectedObject.GetComponent<Rigidbody>();

        //Freeze regidbody
        rig.freezeRotation = true;
        //Turn Gravity off
        rig.useGravity = false;
        //Set speed to 0
        rig.velocity = Vector3.zero;

        //Remove cube from the bowl
        if (selectedObject.TryGetComponent<Cube>(out Cube c))
        {
            int layer = LayerMask.NameToLayer("CubeOutOfBox");
            selectedObject.layer = layer;
            normalizeCube();
            while (BowlManager.instance.cubesInBowl.Contains(c))
                BowlManager.instance.cubesInBowl.Remove(c);
            c.CustomEnable(false);
        }
    }

    private void normalizeCube() {
        if (selectedObject.TryGetComponent<Cube>(out Cube c))
        {
            int side = c.CurrentValue;
            switch (side) {
                case 1:
                    selectedObject.transform.rotation = Quaternion.Euler(sideOneRotation);
                    break;
                case 6:
                    selectedObject.transform.rotation = Quaternion.Euler(sideSixRotation);
                    break;
                case 2:
                    selectedObject.transform.forward = Vector3.up;// Quaternion.Euler(sideTwoRotation);
                    break;
                case 5:
                    selectedObject.transform.forward = -Vector3.up;//transform.rotation = Quaternion.Euler(sideFiveRotation);
                    break;
                case 3:
                    selectedObject.transform.rotation = Quaternion.Euler(sideThreeRotation);
                    break;
                case 4:
                    selectedObject.transform.rotation = Quaternion.Euler(sideFourRotation);
                    break;
                default:
                    selectedObject.transform.rotation = Quaternion.Euler(sideOneRotation);
                    break;

            }
        }
    }

    private void MoveToCursor() {
        if (selectedObject == null)
            return;

        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        selectedObject.transform.position = new Vector3(worldPosition.x, dragPlaneHeight, worldPosition.z);
    }

    void CheckCardPlace()
    {
        RaycastHit hit = CastRay(LayerMask.GetMask("CardPlace"));
        if (hit.collider != null)
        {
            TurnOffCardPlace();
            currentCardPlace = hit.collider.gameObject.GetComponent<CardPlace>();
            currentCardPlace.LightOn();
        } else
        {
            TurnOffCardPlace();
        }
    }

    void TurnOffCardPlace()
    {
        if (currentCardPlace != null)
            currentCardPlace.LightOff();
        currentCardPlace = null;
    }

    LayerMask defForRaycast;

    private RaycastHit CastRay(LayerMask mask) {
        //Vectors describe the position relative to the camera.
        Vector3 screeenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screeenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        
        //Get world position from camera-relative coordinated
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screeenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screeenMousePosNear);
        
        //Cast a ray to locate an object the cursor is hovering over
        RaycastHit hit;
        //LayerMask mask = LayerMask.GetMask("Default") + LayerMask.GetMask("Card") + LayerMask.GetMask("CubeOutOfBox");
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, Camera.main.farClipPlane, mask);

        return hit;
    }
}
