using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathCtrl : MonoBehaviour
{
    public float Damage = 10f;

    private float DelayTime = 0.2f;
    private float CurDelayTime;

    bool IsFire = false;

    AudioSource EffSound = null;

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

    void SoundOn()
    {
        SoundManager.Inst.PlayEffSound("FireBreath");
    }

    void FireOn()
    {
        IsFire = true;
    }

    void FireOff()
    {
        IsFire = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (IsFire && col.transform.CompareTag("Monster"))
        {
            if (CurDelayTime <= 0f)
            {
                float dmg = Random.Range(Damage - 1, Damage + 2);
                bool isCritical = Random.Range(0f, 100f) <= PlayerCtrl.PlayerInfo.Critical_Per;
                col.GetComponent<UnitCtrl>().TakeDamage(this.transform.position, dmg, isCritical);
                CurDelayTime = DelayTime;
            }
        }
    }
}
