using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlotCtrl : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // ���� ���Կ� �ִ� ���� ������Ʈ
    private GameObject CurInObj;
    [SerializeField] private readonly int PosIdx;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CurInObj != null)
        {
            CurInObj.GetComponent<Image>().raycastTarget = false; // Event������ ����raycastTarget ����.
            CurInObj.transform.SetParent(GameObject.Find("Canvas").transform);
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
