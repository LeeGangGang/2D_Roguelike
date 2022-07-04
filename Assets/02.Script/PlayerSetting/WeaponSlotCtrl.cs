using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlotCtrl : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    // ���� ���Կ� �ִ� ���� ������Ʈ
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
            CurInObj.GetComponent<Image>().raycastTarget = false; // Event������ ����raycastTarget ����.
            CurInObj.transform.SetParent(Canvas.transform);
            PlayerSettingCtrl.SelectWeapon = CurInObj;    // ���� ���Կ� �ִ����� �巡�� �߱⿡
        }
        else
            PlayerSettingCtrl.SelectWeapon = null;

        PlayerSettingCtrl.StartParent = transform;   // �̵��� ������ ��ġ(��ġ���� �Ǵ� �ǵ��ƿ��� ����)
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ���Կ� �ִ� ������ �巡�� ������
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
                    // Slot�� �ƴ� �ٸ������� ������ ��
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
        // �̵��� ���� Slot�� ���Ⱑ �ִ� ��� ���� ��ġ ��ȯ�� ����
        if (CurInObj != null && this.transform.name != PlayerSettingCtrl.StartParent.name)
        {
            GameObject temp = CurInObj; // �ӽ� �����
            if (!ReferenceEquals(PlayerSettingCtrl.StartParent.GetComponent<WeaponSlotCtrl>(), null))
            { // ������ ���� ���Գ��� ��ü�ϴ� ���
                CurInObj.transform.SetParent(PlayerSettingCtrl.StartParent);
            }
            else
            { // ������ ���� ���԰� ���� ����Ʈ���� ��ü�ϴ� ���
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

            // ���� ��ġ�� ������ ��Ƴ��� ���̶�� �ٽ� �̺�Ʈ ������ ���� Raycast�� Ų��.
            if (PlayerSettingCtrl.StartParent.name.Contains("Content"))
                PlayerSettingCtrl.SelectWeapon.GetComponent<Image>().raycastTarget = true;
        }
        else
        { // ������� �̵����� ��
            if (CurInObj == null)
            {
                // ������ �ִ� Slot�� ������ ������ �����ش�.
                if (eventData.pointerDrag.GetComponent<WeaponSlotCtrl>() != null)
                    eventData.pointerDrag.GetComponent<WeaponSlotCtrl>().CurInObj = null;
            }

            // �̵��� ���� ����
            if (PlayerSettingCtrl.SelectWeapon != null)
            {
                CurInObj = PlayerSettingCtrl.SelectWeapon;
                CurInObj.transform.SetParent(this.transform);
                CurInObj.transform.localPosition = Vector3.zero;
                PlayerSettingCtrl.SelectWeapon = null; // �ش� �̺�Ʈ �Ϸ� �� OnEndDrag ���� ������ ����
            }
        }
    }
}
