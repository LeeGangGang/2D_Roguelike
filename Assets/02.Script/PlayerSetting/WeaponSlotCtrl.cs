using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlotCtrl : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    // 현재 슬롯에 있는 무기 오브젝트
    public GameObject CurInObj;

    private GameObject Canvas;
    private Text NameTxt;
    private Text InfoTxt;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Canvas");
        NameTxt = GameObject.Find("WeaponNameText").GetComponent<Text>();
        InfoTxt = GameObject.Find("WeaponInfoText").GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ReferenceEquals(CurInObj, null))
            return;

        WeaponInfo info = CurInObj.GetComponent<WeaponDragHandler>().Info;
        NameTxt.text = info.Name;
        InfoTxt.text = string.Format(
            "Attack Dmg : {0}\n" +
            "Attack Mp : {1}\n" +
            "Skill Dmg : {2}\n" +
            "Skill Mp : {3}\n" +
            "Skill Cool : {4}\n" +
            "Critical : {5}%\n" +
            "Defence : {6}",
            info.Attack_Dmg, info.Attack_NeedMp, info.Skill_Dmg,
            info.Skill_NeedMp, info.Skill_Cool, info.Critical_Per,
            info.Defence);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NameTxt.text = string.Empty;
        InfoTxt.text = string.Empty;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CurInObj != null)
        {
            CurInObj.GetComponent<Image>().raycastTarget = false; // Event동작을 위해raycastTarget 끈다.
            CurInObj.transform.SetParent(Canvas.transform);
            PlayerSettingCtrl.SelectWeapon = CurInObj;    // 현재 슬롯에 있던것을 드래그 했기에
        }
        else
            PlayerSettingCtrl.SelectWeapon = null;

        PlayerSettingCtrl.StartParent = transform;   // 이동을 시작한 위치(위치변경 또는 되돌아오기 위해)
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 슬롯에 있던 무기을 드래그 했을때
        if (CurInObj != null)
            CurInObj.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PlayerSettingCtrl.SelectWeapon != null)
            CurInObj = PlayerSettingCtrl.SelectWeapon;
        
        if (eventData.pointerEnter == null || eventData.pointerEnter.name.Contains("Slot_") == false)
        {
            if (CurInObj != null)
            {
                Transform contentTr = GameObject.Find("Content").transform;
                if (!ReferenceEquals(contentTr, null))
                {
                    // Slot이 아닌 다른곳으로 던졌을 때
                    int rootIdx = CurInObj.GetComponent<WeaponDragHandler>().Info.Idx;
                    CurInObj.transform.SetParent(contentTr.GetChild(rootIdx));
                    CurInObj.transform.localPosition = Vector3.zero;
                    CurInObj.GetComponent<Image>().raycastTarget = true;
                    CurInObj = null;
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 이동할 장착 Slot에 무기가 있는 경우 서로 위치 교환을 위해
        if (CurInObj != null && this.transform.name != PlayerSettingCtrl.StartParent.name)
        {
            GameObject temp = CurInObj; // 임시 저장소
            if (!ReferenceEquals(PlayerSettingCtrl.StartParent.GetComponent<WeaponSlotCtrl>(), null))
            { // 장착한 무기 슬롯끼리 교체하는 경우
                CurInObj.transform.SetParent(PlayerSettingCtrl.StartParent);
            }
            else
            { // 장착한 무기 슬롯과 무기 리스트에서 교체하는 경우
                int posIdx = CurInObj.GetComponent<WeaponDragHandler>().Info.Idx;
                Transform contentTr = GameObject.Find("Content").transform;
                if (!ReferenceEquals(contentTr, null))
                {
                    CurInObj.transform.SetParent(contentTr.GetChild(posIdx));
                    CurInObj.GetComponent<Image>().raycastTarget = true;
                }
            }
            CurInObj.transform.localPosition = Vector3.zero;

            PlayerSettingCtrl.SelectWeapon.transform.SetParent(this.transform);
            PlayerSettingCtrl.SelectWeapon.transform.localPosition = Vector3.zero;
            
            CurInObj = PlayerSettingCtrl.SelectWeapon;
            PlayerSettingCtrl.SelectWeapon = temp;

            // 시작 위치가 무기을 모아놓은 곳이라면 다시 이벤트 적용을 위해 Raycast를 킨다.
            if (PlayerSettingCtrl.StartParent.name.Contains("Content"))
                PlayerSettingCtrl.SelectWeapon.GetComponent<Image>().raycastTarget = true;
        }
        else
        { // 빈곳으로 이동했을 때
            if (CurInObj == null)
            {
                // 이전에 있던 Slot의 무기의 흔적을 지워준다.
                if (eventData.pointerDrag.GetComponent<WeaponSlotCtrl>() != null)
                    eventData.pointerDrag.GetComponent<WeaponSlotCtrl>().CurInObj = null;
            }

            // 이동한 무기 적용
            if (PlayerSettingCtrl.SelectWeapon != null)
            {
                CurInObj = PlayerSettingCtrl.SelectWeapon;
                CurInObj.transform.SetParent(this.transform);
                CurInObj.transform.localPosition = Vector3.zero;
                PlayerSettingCtrl.SelectWeapon = null; // 해당 이벤트 완료 후 OnEndDrag 동작 제한을 위해
            }
        }
    }
}
