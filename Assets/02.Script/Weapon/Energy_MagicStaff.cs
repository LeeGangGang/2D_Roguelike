using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy_MagicStaff : Weapon
{
    new WeaponInfo Info = new WeaponInfo();
    public GameObject IconImg;
    public GameObject EnergyBall;
    public Transform ShotPos;

    // Start is called before the first frame update
    void Awake()
    {
        Info.Attack_Dmg = 20f;
        Info.Skill_Dmg = 20f;
        Info.Attack_NeedMp = 5;
        Info.Skill_NeedMp = 20;
        Info.Img = IconImg.GetComponent<SpriteRenderer>().sprite;
        base.Init(Info);
    }

    public override void Attack()
    {
        Vector3 pos = ShotPos.position;
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject a_refEnergyBall = GameObject.Instantiate(EnergyBall, pos, rot);
        a_refEnergyBall.GetComponent<EnergyBallCtrl>().EnergyBallSpawn(dir, 15f, 20f, false);
    }

    public override void Skill()
    {
        Vector3 pos = ShotPos.position;
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < 6; i++)
        {
            float a_angle = angle + (15 * i);
            Quaternion rot = Quaternion.AngleAxis(a_angle, Vector3.forward);
            GameObject a_refEnergyBall = GameObject.Instantiate(EnergyBall, pos, rot);
            a_refEnergyBall.GetComponent<EnergyBallCtrl>().EnergyBallSpawn(dir, 15f, 20f, true);
        }
    }
}
