using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponInfo
{
    public string Name;             // ���� �̸�

    public float Attack_Dmg = 0;    // �Ϲݰ��� ���ݷ�
    public float Skill_Dmg = 0;     // ��ų���� ���ݷ�
    public float Critical_Per = 0;  // �߰� ġ��Ÿ Ȯ��
    public float Defence = 0;       // �߰� ����

    public int Attack_NeedMp;   // �Ϲݰ��� �ʿ� ����
    public int Skill_NeedMp;    // ��ų���� �ʿ� ����

    public float Skill_Cool;    // ��ų���� ��Ÿ��

    public int Idx;             // ���� �ε���
    public Sprite Img;          // ����â �̹���
}

public abstract class Weapon : MonoBehaviour
{
    public WeaponInfo Info;

    public abstract void Attack();
    public abstract void Skill();
}