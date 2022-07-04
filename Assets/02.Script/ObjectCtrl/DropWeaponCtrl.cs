using System;
using UnityEngine;

[Serializable]
public class DropWeaponCtrl : MonoBehaviour
{
    [SerializeField] private WeaponDataFact WeaponDataFact;
    public int Idx; // µµ∞® ¿Œµ¶Ω∫

    void OnTriggerStay2D(Collider2D col)
    {
        if (this.GetComponentInParent<Rigidbody2D>().velocity.y == 0)
        {
            if (col.CompareTag("Player"))
            {
                WeaponDataFact.Save(Idx, WeaponDataFact.WeaponInfoList.Count);
                Destroy(this.transform.parent.gameObject);
            }
        }
    }
}