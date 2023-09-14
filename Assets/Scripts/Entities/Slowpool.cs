using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowpool : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Dissapear());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy obj = collision.GetComponent<Enemy>();

        if (obj == null) return;

        obj.MoveSpeed /= 2f;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy obj = collision.GetComponent<Enemy>();

        if (obj == null) return;

        obj.MoveSpeed *= 2f;
    }

    private IEnumerator Dissapear()
    {
        while(this.transform.localScale.x >= .2f)
        {
            this.transform.localScale -= Vector3.one * .5f * Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
