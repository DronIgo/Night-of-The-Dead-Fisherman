using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CardPlace is a class with all the logic behind card placement slots
/// </summary>
public class CardPlace : MonoBehaviour
{
    public float heightOfCard = 0.5f;
    int numOfCards = 0;
    MeshRenderer myMesh = null;
    float defY;
    GameObject cardProjection = null;

    public void Start()
    {
        cardProjection = transform.Find("cardProjection").gameObject;
        myMesh = cardProjection.GetComponent<MeshRenderer>();
        myMesh.enabled = false;
        defY = transform.position.y;
    }

    public void PlaceCard(GameObject card)
    {
        card.transform.position = transform.position;
        card.transform.rotation = Quaternion.Euler(0, -90, 0);
        card.GetComponent<Task>().MyCardPlace = this;
        numOfCards++;
        UpdateHight();
    }

    void UpdateHight()
    {
        transform.position = new Vector3(transform.position.x, numOfCards * heightOfCard + defY, transform.position.z);
    }

    public void RemoveCard()
    {
        numOfCards--;
        UpdateHight();
    }

    public void LightOn()
    {
        myMesh.enabled = true;
    }

    public void LightOff()
    {
        myMesh.enabled = false;
    }
}
