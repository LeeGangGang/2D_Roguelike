using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectorGroundCtrl : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //if (col.contacts[0].normal.y > 0.5f)
              //this. GetComponent<CompositeCollider2D>().isTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //this.GetComponent<CompositeCollider2D>().isTrigger = false;
        }
    }
}
