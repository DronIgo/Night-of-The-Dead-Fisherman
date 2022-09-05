using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Transform away = null;
    public Transform close = null;
    MeshRenderer mesh = null;
    public void GoAway()
    {
        StartCoroutine(Away());
    }
    float duration = 2f;
    IEnumerator Away()
    {
        float start = Time.time;
        
        while (Time.time - start < duration)
        {
            float t = (Time.time - start)/ duration;
            mesh.material.color = new Color(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t));
            transform.position = away.position * t + (1 - t) * close.position;
            yield return new WaitForEndOfFrame();
        }
        mesh.material.color = new Color(0, 0, 0);
    }
    public void ComeIn()
    {
        StartCoroutine(Close());
    }

    public void ComeIn(float delay)
    {
        StartCoroutine(WaitClose(delay));
    }

    IEnumerator WaitClose(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        float start = Time.time;

        while (Time.time - start < duration)
        {
            float t = (Time.time - start) / duration;
            mesh.material.color = new Color(Mathf.Lerp(0, 1, t), Mathf.Lerp(0, 1, t), Mathf.Lerp(0, 1, t));
            transform.position = away.position * (1 - t) + t * close.position;
            yield return new WaitForEndOfFrame();
        }
        mesh.material.color = new Color(1, 1, 1);
    }

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
