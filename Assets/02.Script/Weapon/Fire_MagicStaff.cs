using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_MagicStaff : Weapon
{
    public GameObject FireBreath; // 일반
    public GameObject FireField;  // 스킬

    public override void Attack()
    {
        FireBreath.GetComponent<Animator>().SetTrigger("Start");
        FireBreath.GetComponent<FireBreathCtrl>().WeaponInfo = Info;
    }

    public override void Skill()
    {
        Vector3 pos = GameObject.Find("Player").transform.position;
        GameObject a_refFireField = GameObject.Instantiate(FireField, pos, Quaternion.identity);
        a_refFireField.GetComponentInChildren<FireFieldCtrl>().WeaponInfo = Info;
    }
}
