using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponInfo
{
    public string Name;             // 무기 이름

    public float Attack_Dmg = 0;    // 일반공격 공격력
    public float Skill_Dmg = 0;     // 스킬공격 공격력
    public float Critical_Per = 0;  // 추가 치명타 확률
    public float Defence = 0;       // 추가 방어력

    public int Attack_NeedMp;   // 일반공격 필요 마나
    public int Skill_NeedMp;    // 스킬공격 필요 마나

    public float Skill_Cool;    // 스킬공격 쿨타임

    public int Idx;             // 도감 인덱스
    public Sprite Img;          // 슬롯창 이미지
}

public abstract class Weapon : MonoBehaviour
{
    public WeaponInfo Info;

    public abstract void Attack();
    public abstract void Skill();
}