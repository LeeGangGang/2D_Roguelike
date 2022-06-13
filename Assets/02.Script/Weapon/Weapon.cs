using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo
{
    public float Attack_Dmg = 0;    // �Ϲݰ��� ���ݷ�
    public float Skill_Dmg = 0;     // ��ų���� ���ݷ�
    public float Critical_Per = 0;  // ġ��Ÿ Ȯ��
    public float Defence = 0;       // ����

    public int Attack_NeedMp;   // �Ϲݰ��� �ʿ� ����
    public int Skill_NeedMp;    // ��ų���� �ʿ� ����

    public float Attack_Cool;   // �Ϲݰ��� ��Ÿ��
    public float Skill_Cool;    // ��ų���� ��Ÿ��

    public Sprite Img;          // ����â �̹���
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