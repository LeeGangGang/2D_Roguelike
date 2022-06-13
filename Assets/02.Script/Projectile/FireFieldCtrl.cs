using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFieldCtrl : MonoBehaviour
{
    public float Damage = 5f;

    private float DelayTime = 0.3f;
    private float CurDelayTime;

    bool IsFire = false;

    // Start is called before the first frame update
    void Start()
    {
        CurDelayTime = DelayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFire)
        {
            if (CurDelayTime > 0)
                CurDelayTime -= Time.deltaTime;
        }
    }

    void FireOn()
    {
        IsFire = true;
    }

    void FireOff()
    {
        IsFire = false;
        Destroy(this.transform.parent.gameObject);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (IsFire && col.transform.CompareTag("Monster"))
        {
            if (CurDelayTime <= 0f)
            {
                float dmg = Random.Range(Damage - 1, Damage + 2);
                bool isCritical = Random.Range(0f, 100f) <= PlayerCtrl.PlayerInfo.Critical_Per;
                col.GetComponent<UnitCtrl>().TakeDamage(dmg, isCritical);
                CurDelayTime = DelayTime;
            }
        }
    }
}
