using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackCtrl : MonoBehaviour
{
    public Sword Weapon;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Inst.PlayerCtrl.IsAttack || GameManager.Inst.PlayerCtrl.IsSkill)
        {
            if (col.transform.CompareTag("Monster"))
            {
                float dmg = Random.Range(Weapon.Damage - 1, Weapon.Damage + 2);
                bool isCritical = Random.Range(0f, 100f) <= PlayerCtrl.PlayerInfo.Critical_Per;
                col.GetComponent<UnitCtrl>().TakeDamage(dmg, isCritical);
            }
        }
    }
}
