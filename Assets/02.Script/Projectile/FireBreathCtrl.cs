using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathCtrl : MonoBehaviour
{
    [HideInInspector] public WeaponInfo WeaponInfo;

    private float DelayTime = 0.2f;
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
        if (IsFire && col.attachedRigidbody.transform.CompareTag("Monster"))
        {
            if (CurDelayTime <= 0f)
            {
                float dmg = WeaponInfo.Attack_Dmg + PlayerCtrl.PlayerInfo.Attack;
                float randDmg = Random.Range(dmg - 1, dmg + 2);
                bool isCritical = Random.Range(0, 101) >= PlayerCtrl.PlayerInfo.Critical_Per + WeaponInfo.Critical_Per;
                col.GetComponentInParent<UnitCtrl>().TakeDamage(this.transform.position, randDmg, isCritical);
                CurDelayTime = DelayTime;
            }
        }
    }
}
