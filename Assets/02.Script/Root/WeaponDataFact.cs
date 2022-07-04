using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data List", menuName = "Scriptable Object/Weapon Data List", order = int.MaxValue)]
public class WeaponDataFact : ScriptableObject
{
    [SerializeField]
    public List<WeaponInfo> WeaponInfoList;
    public static int[] IsReserve = new int[] { };
    public static void Save(int idx, int totalCnt) // idx : 현재 먹은 무기 idx, totalCnt : 전체 갯수
    {
        string arrWeaponReserve = string.Empty;
        for (int i = 0; i < totalCnt; i++)
        {
            int reserve = 0;
            if (idx == i)
                reserve = 1;
            else
            {
                if (WeaponDataFact.IsReserve.Length > i)
                    reserve = WeaponDataFact.IsReserve[i];
            }
            arrWeaponReserve += reserve;

            if (i != totalCnt - 1)
                arrWeaponReserve += ",";
        }
        WeaponDataFact.IsReserve = Array.ConvertAll(arrWeaponReserve.Split(','), (e) => int.Parse(e));
        PlayerPrefs.SetString("WeaponReserve", arrWeaponReserve);
    }

    private void OnValidate()
    {
        for (int i = 0; i < WeaponInfoList.Count; i++)
            WeaponInfoList[i].Idx = i;
    }
}
