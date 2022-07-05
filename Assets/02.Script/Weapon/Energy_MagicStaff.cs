using System.Collections;
using UnityEngine;

public class Energy_MagicStaff : Weapon
{
    [SerializeField] private GameObject EnergyBall;
    [SerializeField] private Transform ShotPos;

    public override void Attack()
    {
        Vector3 pos = ShotPos.position;
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject a_refEnergyBall = GameObject.Instantiate(EnergyBall, pos, rot);
        a_refEnergyBall.GetComponent<EnergyBallCtrl>().EnergyBallSpawn(dir, false);
        a_refEnergyBall.GetComponent<EnergyBallCtrl>().WeaponInfo = Info;

        SoundManager.Inst.PlayEffSound("EnergyBall_Shot");
    }

    public override void Skill()
    {
        Vector3 pos = ShotPos.position;
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos;

        StartCoroutine(N_Shot(pos, dir, 6));
    }

    IEnumerator N_Shot(Vector3 pos, Vector3 dir, int N)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - (15 * (int)(N / 2));
        for (int i = 0; i < N; i++)
        {
            yield return new WaitForSeconds(0.05f);
            float a_angle = angle + (15 * i);
            Quaternion rot = Quaternion.AngleAxis(a_angle, Vector3.forward);
            GameObject a_refEnergyBall = GameObject.Instantiate(EnergyBall, pos, rot);
            a_refEnergyBall.GetComponent<EnergyBallCtrl>().EnergyBallSpawn(dir, true);
            a_refEnergyBall.GetComponent<EnergyBallCtrl>().WeaponInfo = Info;

            SoundManager.Inst.PlayEffSound("EnergyBall_Shot");
        }
    }
}
