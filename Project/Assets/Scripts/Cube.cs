using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for cubes, contains methods for getting dice value and interact with cards 
/// </summary>
public class Cube : MonoBehaviour
{
    public float upwardsMomentum = 0.5f;
    public float sideMomentum = 2f;
    public float rollDuration = 2f;
    public int currentValue = 1;
    public Color color;

    public void Generate()
    {
        color = BowlManager.instance.GetRandomColor();
        transform.Find("ActualCube").GetComponent<MeshRenderer>().material.color = color;
    }

    public void Generate(float[] probs)
    {
        color = BowlManager.instance.GetRandomColor(probs);
        transform.Find("ActualCube").GetComponent<MeshRenderer>().material.color = color;
    }

    public int CurrentValue
    {
        get {
            LayerMask mask = LayerMask.GetMask("sides");
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.up, out hit, 10f, mask);
            Debug.DrawLine(transform.position, transform.position + Vector3.up, Color.blue, 0.1f);
            switch (hit.collider.name)
            {
                case "Side1":
                    return 1;
                    break;
                case "Side2":
                    return 2;
                    break;
                case "Side3":
                    return 3;
                    break;
                case "Side4":
                    return 4;
                    break;
                case "Side5":
                    return 5;
                    break;
                case "Side6":
                    return 6;
                    break;
                default:
                    return 1;
                    break;
            }
        }
    }

    public void CustomEnable(bool e)
    {
        enable = e;
        if (!e)
        {
            if (myCard != null)
                myCard.RemoveCube(this.gameObject);
            myCard = null;
        }
    }

    bool enable = true;

    public void Update()
    {
        if (enable)
        {
            currentValue = CurrentValue;
            CheckDown();
        }
    }

    Task myCard = null;

    private void CheckDown()
    {
        LayerMask mask = LayerMask.GetMask("Card");
        RaycastHit hit;
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 3f, Color.red, 0.1f);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3f, mask))
        {
            if (hit.transform.gameObject.TryGetComponent<Task>(out Task task))
            {
                if (myCard != task)
                {
                    if (myCard != null)
                    {
                        myCard.RemoveCube(this.gameObject);
                    }
                    task.AddCube(this.gameObject);
                    myCard = task;
                    
                }
            } else
            {
                if (myCard != null)
                {
                    myCard.RemoveCube(this.gameObject);
                    myCard = null;
                }
            }
        } else
        {
            if (myCard != null)
            {
                myCard.RemoveCube(this.gameObject);
                myCard = null;
            }
        }

    }
    bool inRoll = false;
    public void Roll()
    {
        StartCoroutine(DoRoll(rollDuration));
    }

    IEnumerator DoRoll(float duration)
    {
        this.tag = "Untagged";
        inRoll = true;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = false;

        float startTime = Time.time;
        while (Time.time - startTime < duration) {
            Vector3 tangent = Vector3.Cross((transform.position - BowlManager.instance.bowl.position).normalized, Vector3.up).normalized;
            Vector3 toCenter = (BowlManager.instance.bowl.position - transform.position).normalized;
            Vector3 random = new Vector3(Mathf.Sin(Random.Range(0, 2 * Mathf.PI)), 0, Mathf.Cos(Random.Range(0, 2 * Mathf.PI)))*1.5f;
        
            float r = 0.5f;
            
            Vector3 result = tangent * r + toCenter * (1 - r) + random;
            Vector3 momentum = new Vector3(result.x * sideMomentum, upwardsMomentum, result.z * sideMomentum);//Mathf.Cos(Random.Range(0, 2 * Mathf.PI)) * sideMomentum);
            
            rigidbody.AddForce(momentum, ForceMode.Impulse);
            rigidbody.angularVelocity += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            
            yield return new WaitForSeconds(0.05f);
        }
        
        while (rigidbody.velocity.sqrMagnitude > 0.01f)
            yield return new WaitForEndOfFrame();

        rigidbody.freezeRotation = true;
        inRoll = false;
        this.tag = "Drag";
    }

}
