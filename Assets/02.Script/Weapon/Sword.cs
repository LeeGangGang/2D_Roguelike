using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    new WeaponInfo Info = new WeaponInfo();
    public GameObject IconImg;
    public float Damage;

    void Awake()
    {
        Info.Attack_Dmg = 20f;
        Info.Skill_Dmg = 30f;
        Info.Attack_NeedMp = 0;
        Info.Skill_NeedMp = 10;
        Info.Img = IconImg.GetComponent<SpriteRenderer>().sprite;
        base.Init(Info);
    }

    public override void Attack()
    {
        Damage = Info.Attack_Dmg;
    }

    public override void Skill()
    {
        Damage = Info.Skill_Dmg;
    }
}
