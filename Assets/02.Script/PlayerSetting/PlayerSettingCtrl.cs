using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingCtrl : MonoBehaviour
{
    // ������ ������ ����
    public static WeaponInfo[] SelectedWeapons = new WeaponInfo[2];
    public Transform[] WeaponSlot = new Transform[2];

    // Drag�� �ϳ��� �����Ͽ� static���� ����
    public static Transform StartParent;    // �巡�� �� ������ �巡�� ���� ��ġ
    public static GameObject SelectWeapon;   // �巡�� �� ������ ������Ʈ

    [SerializeField] private GameObject WeaponItemPrefab;
    [SerializeField] private WeaponDataFact WeaponDataList;
    [SerializeField] private Transform Content;

    [SerializeField] private Button SaveBtn;
    [SerializeField] private Button CloseBtn;

    // Start is called before the first frame update
    void Start()
    {
        if (!ReferenceEquals(SaveBtn, null))
            SaveBtn.onClick.AddListener(Save);
        if (!ReferenceEquals(CloseBtn, null))
            CloseBtn.onClick.AddListener(Close);

        foreach (WeaponInfo weaponinfo in WeaponDataList.WeaponDataList)
        {
            GameObject weaponItem = Instantiate(WeaponItemPrefab);
            weaponItem.transform.SetParent(Content);
            weaponItem.transform.Find("Weapon").GetComponent<Image>().sprite = weaponinfo.Img;
            weaponItem.transform.Find("Weapon").GetComponent<WeaponDragHandler>().Info = weaponinfo;
            weaponItem.transform.Find("Lock").gameObject.SetActive(false);
        }
    }

    void Save()
    {
        for (int i = 0; i < 2; i++)
        {
            if (WeaponSlot[i].childCount > 0)
                SelectedWeapons[i] = WeaponSlot[i].GetComponentInChildren<WeaponDragHandler>().Info;
        }
        Destroy(this.gameObject);
    }

    void Close()
    {
        Destroy(this.gameObject);
    }
}
