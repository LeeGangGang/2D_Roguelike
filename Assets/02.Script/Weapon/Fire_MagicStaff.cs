using UnityEngine;

public class Fire_MagicStaff : Weapon
{
    [SerializeField] private GameObject FireBreath; // �Ϲ�
    [SerializeField] private GameObject FireField;  // ��ų

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
