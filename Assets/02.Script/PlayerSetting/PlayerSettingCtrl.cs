using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingCtrl : MonoBehaviour
{
    // 장착한 무기의 정보
    public static WeaponInfo[] SelectedWeapons = new WeaponInfo[2];
    public Transform[] WeaponSlot = new Transform[2];

    // Drag는 하나만 가능하여 static으로 관리
    public static Transform StartParent;    // 드래그 한 무기의 드래그 시점 위치
    public static GameObject SelectWeapon;   // 드래그 한 무기의 오브젝트

    [SerializeField] private GameObject WeaponItemPrefab;
    [SerializeField] private WeaponDataFact WeaponDataFact;
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

        foreach (WeaponInfo weaponinfo in WeaponDataFact.WeaponInfoList)
        {
            GameObject weaponItem = Instantiate(WeaponItemPrefab);
            weaponItem.transform.SetParent(Content);

            Transform weaponImg = weaponItem.transform.Find("Weapon");
            weaponImg.GetComponent<Image>().sprite = weaponinfo.Img;
            weaponImg.GetComponent<WeaponDragHandler>().Info = weaponinfo;

            if (SelectedWeapons.Contains(weaponinfo))
            {
                int idx = Array.IndexOf(SelectedWeapons, weaponinfo);
                weaponImg.SetParent(WeaponSlot[idx]);
                WeaponSlot[idx].GetComponent<WeaponSlotCtrl>().CurInObj = weaponImg.gameObject;
                weaponImg.GetComponent<Image>().raycastTarget = false;
                weaponImg.localPosition = Vector3.zero;
            }

            if (WeaponDataFact.IsReserve[weaponinfo.Idx] == 1)
                weaponItem.transform.Find("Lock").gameObject.SetActive(false);
            else
                weaponItem.transform.Find("Lock").gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // Test
        if (Input.GetKeyDown(KeyCode.T))
        {
            WeaponDataFact.Save(1, WeaponDataFact.WeaponInfoList.Count);
            WeaponDataFact.Save(3, WeaponDataFact.WeaponInfoList.Count);
        }
    }

    void Save()
    {
        string arrSelectedWeapon = string.Empty;
        for (int i = 0; i < 2; i++)
        {
            if (WeaponSlot[i].childCount > 0)
            {
                SelectedWeapons[i] = WeaponSlot[i].GetComponentInChildren<WeaponDragHandler>().Info;
                arrSelectedWeapon += SelectedWeapons[i].Idx;
            }
            else
            {
                SelectedWeapons[i] = null;
                arrSelectedWeapon += "-1";
            }

            if (i == 0)
                arrSelectedWeapon += ",";
        }

        PlayerPrefs.SetString("SelectedWeapon", arrSelectedWeapon);

        Destroy(this.gameObject);
    }

    void Close()
    {
        Destroy(this.gameObject);
    }
}
