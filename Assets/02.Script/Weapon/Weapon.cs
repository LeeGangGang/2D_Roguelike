using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo
{
    public float Attack_Dmg = 0;    // 일반공격 공격력
    public float Skill_Dmg = 0;     // 스킬공격 공격력
    public float Critical_Per = 0;  // 치명타 확률
    public float Defence = 0;       // 방어력

    public int Attack_NeedMp;   // 일반공격 필요 마나
    public int Skill_NeedMp;    // 스킬공격 필요 마나

    public float Attack_Cool;   // 일반공격 쿨타임
    public float Skill_Cool;    // 스킬공격 쿨타임

    public Sprite Img;          // 슬롯창 이미지
}

public abstract class Weapon : MonoBehaviour
{
    public WeaponInfo Info;

    public void Init(WeaponInfo info)
    {
        Info = info;
    }

    public abstract void Attack();
    public abstract void Skill();
}