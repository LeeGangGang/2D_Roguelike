using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneMgr : MonoBehaviour
{
    [HideInInspector] public PlayerCtrl PlayerCtrl;
    [Header("Player Info")]
    public Image CurHpImg;
    public Image CurMpImg;
    public Text CurHpTxt;
    public Text CurMpTxt;

    [Header("Weapon Info")]
    public Image[] WeaponImg = new Image[2];
    public Transform[] WeaponTr = new Transform[2];
    private Vector3 CurWeaponPos;
    private Vector3 SubWeaponPos;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        CurWeaponPos = WeaponTr[1].position;
        SubWeaponPos = WeaponTr[0].position;
        for (int i = 0; i < 2; i++)
        {
            WeaponImg[i].sprite = PlayerCtrl.ArrWeaponCtrl[i].Info.Img;
            WeaponImg[i].type = Image.Type.Simple;
            WeaponImg[i].preserveAspect = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();

        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchWeapon();
    }

    void StateUpdate()
    {
        if (ReferenceEquals(PlayerCtrl, null))
            return;

        if (!ReferenceEquals(CurHpImg, null))
            CurHpImg.fillAmount = (float)PlayerCtrl.PlayerInfo.CurHp / PlayerCtrl.PlayerInfo.MaxHp;
        if (!ReferenceEquals(CurHpTxt, null))
            CurHpTxt.text = string.Format("{0} / {1}", PlayerCtrl.PlayerInfo.CurHp, PlayerCtrl.PlayerInfo.MaxHp);

        if (!ReferenceEquals(CurMpImg, null))
            CurMpImg.fillAmount = (float)PlayerCtrl.PlayerInfo.CurMp / PlayerCtrl.PlayerInfo.MaxMp;
        if (!ReferenceEquals(CurMpTxt, null))
            CurMpTxt.text = string.Format("{0} / {1}", PlayerCtrl.PlayerInfo.CurMp, PlayerCtrl.PlayerInfo.MaxMp);
    }

    void SwitchWeapon()
    {
        Vector3 tempPos = CurWeaponPos;
        CurWeaponPos = SubWeaponPos;
        SubWeaponPos = tempPos;
        int TrIdx_1 = WeaponTr[0].transform.GetSiblingIndex();
        int TrIdx_2 = WeaponTr[1].transform.GetSiblingIndex();
        WeaponTr[0].SetSiblingIndex(TrIdx_2);
        WeaponTr[1].SetSiblingIndex(TrIdx_1);
        PlayerCtrl.WeaponIdx = PlayerCtrl.WeaponIdx == 0 ? 1 : 0;
        StartCoroutine(SwitchWeaponAnim());
    }

    IEnumerator SwitchWeaponAnim()
    {
        while (true)
        {
            if (Vector3.Distance(WeaponTr[0].position, SubWeaponPos) < 0.01f)
                break;

            WeaponTr[0].position = Vector3.Lerp(WeaponTr[0].position, SubWeaponPos, 0.1f);
            WeaponTr[1].position = Vector3.Lerp(WeaponTr[1].position, CurWeaponPos, 0.1f);
            yield return null;
        }
    }
}
