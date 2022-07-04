using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackCtrl : MonoBehaviour
{
    public Sword WeaponInfo;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerCtrl player = this.transform.root.GetComponent<PlayerCtrl>();
        if (player.IsAttack || player.IsSkill)
        {
            if (col.attachedRigidbody.transform.CompareTag("Monster"))
            {
                float weaponDmg = player.IsAttack ? WeaponInfo.Info.Attack_Dmg : WeaponInfo.Info.Skill_Dmg;
                float dmg = weaponDmg + PlayerCtrl.PlayerInfo.Attack;
                float randDmg = Random.Range(dmg - 1, dmg + 2);
                bool isCritical = Random.Range(0, 101) >= PlayerCtrl.PlayerInfo.Critical_Per + WeaponInfo.Info.Critical_Per;
                col.GetComponentInParent<UnitCtrl>().TakeDamage(this.transform.position, randDmg, isCritical);
            }
        }
    }
}
