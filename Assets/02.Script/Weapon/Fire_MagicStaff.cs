using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_MagicStaff : Weapon
{
    new WeaponInfo Info = new WeaponInfo();
    public GameObject IconImg;
    public GameObject FireBreath; // 일반
    public GameObject FireField;  // 스킬

    // Start is called before the first frame update
    void Awake()
    {
        Info.Attack_Dmg = 10f;
        Info.Skill_Dmg = 5f;
        Info.Attack_NeedMp = 5;
        Info.Skill_NeedMp = 10;
        Info.Img = IconImg.GetComponent<SpriteRenderer>().sprite;
        base.Init(Info);
    }

    public override void Attack()
    {
        FireBreath.GetComponent<Animator>().SetTrigger("Start");
        FireBreath.GetComponent<FireBreathCtrl>().Damage = Info.Attack_Dmg;
    }

    public override void Skill()
    {
        Vector3 pos = GameObject.Find("Player").transform.position;
        GameObject a_refFireField = GameObject.Instantiate(FireField, pos, Quaternion.identity);
        a_refFireField.GetComponentInChildren<FireFieldCtrl>().Damage = Info.Skill_Dmg;
    }
}
