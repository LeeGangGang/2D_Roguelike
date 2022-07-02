using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data List", menuName = "Scriptable Object/Weapon Data List", order = int.MaxValue)]
public class WeaponDataFact : ScriptableObject
{
    [SerializeField]
    public List<WeaponInfo> WeaponDataList;

    private void OnValidate()
    {
        for (int i = 0; i < WeaponDataList.Count; i++)
            WeaponDataList[i].Idx = i;
    }
}
