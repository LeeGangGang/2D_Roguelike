using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public WeaponInfo Info;

    private GameObject Canvas;
    private Text NameTxt;
    private Text InfoTxt;

    bool IsMvItem = false; // true : �ٸ� ������ �ش� ���� �巡�� �Ҷ�

    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Canvas");
        NameTxt = GameObject.Find("WeaponNameText").GetComponent<Text>();
        InfoTxt = GameObject.Find("WeaponInfoText").GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        NameTxt.text = Info.Name;
        InfoTxt.text = string.Format(
            "Attack Dmg : {0}\n" +
            "Attack Mp : {1}\n" +
            "Skill Dmg : {2}\n" +
            "Skill Mp : {3}\n" +
            "Skill Cool : {4}\n" +
            "Critical : {5}%\n" +
            "Defence : {6}",
            Info.Attack_Dmg, Info.Attack_NeedMp, Info.Skill_Dmg,
            Info.Skill_NeedMp, Info.Skill_Cool, Info.Critical_Per,
            Info.Defence);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NameTxt.text = string.Empty;
        InfoTxt.text = string.Empty;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsMvItem == false)
        {
            IsMvItem = true;
            PlayerSettingCtrl.StartParent = transform.parent;
            PlayerSettingCtrl.SelectWeapon = this.gameObject;
            this.GetComponent<Image>().raycastTarget = false;
            this.transform.SetParent(Canvas.transform);
        }

        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsMvItem = false;
        // ���� ������ �ƴ� ���ÿ� List�ʿ��� ������ �߸� �巡�� ������ ����ġ �ϱ�����
        if (eventData.pointerEnter == null || eventData.pointerEnter.name.Contains("Slot_") == false)
        {
            Transform contentTr = GameObject.Find("Content").transform;
            if (!ReferenceEquals(contentTr, null))
            {
                this.transform.SetParent(contentTr.GetChild(Info.Idx));
                this.transform.localPosition = Vector3.zero;
                this.GetComponent<Image>().raycastTarget = true;
            }
        }
    }
}
