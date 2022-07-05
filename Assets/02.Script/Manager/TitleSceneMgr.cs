using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneMgr : MonoBehaviour
{
    [SerializeField] private Button PlayBtn;
    [SerializeField] private Button ExitBtn;
    [SerializeField] private Button PlayerSettingBtn;
    [SerializeField] private Button ConfigBtn;

    [SerializeField] private GameObject ConfigPanelPrefab;
    [SerializeField] private GameObject PlayerSettingPrefab;
    [SerializeField] private Transform CanvasTr;

    // Data Load¿ë
    [SerializeField] private WeaponDataFact WeaponDataFact;

    // Start is called before the first frame update
    void Start()
    {
        if (!ReferenceEquals(PlayBtn, null))
            PlayBtn.onClick.AddListener(GameStart);

        if (!ReferenceEquals(ExitBtn, null))
            ExitBtn.onClick.AddListener(GameExit);

        if (!ReferenceEquals(PlayerSettingBtn, null))
            PlayerSettingBtn.onClick.AddListener(PlayerSetting);

        if (!ReferenceEquals(ConfigBtn, null))
            ConfigBtn.onClick.AddListener(OpenConfig);

        Init();

        SoundManager.Inst.PlayBGM("BGM/TitleBgm");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Init()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("WeaponReserve")))
            WeaponDataFact.IsReserve = Array.ConvertAll(PlayerPrefs.GetString("WeaponReserve").Split(','), (e) => int.Parse(e));
        else
        {
            WeaponDataFact.IsReserve = Enumerable.Repeat<int>(0, WeaponDataFact.WeaponInfoList.Count).ToArray<int>();
            WeaponDataFact.IsReserve[0] = 1;
        }

        int[] arrSelectedWeapon = new int[2] { -1, -1 };
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SelectedWeapon")))
            arrSelectedWeapon = Array.ConvertAll(PlayerPrefs.GetString("SelectedWeapon").Split(','), (e) => int.Parse(e));
        for (int i = 0; i < 2; i++)
        {
            if (arrSelectedWeapon[i] == -1)
                continue;

            WeaponInfo info = WeaponDataFact.WeaponInfoList.Where(w => w.Idx == arrSelectedWeapon[i]).First();
            PlayerSettingCtrl.SelectedWeapons[i] = info;
        }
    }

    void GameStart()
    {
        LoadPlayerSetting();

        SoundManager.Inst.PlayUISound("Button1", 2f);

        SceneManager.LoadSceneAsync("FadeScene");
        SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("Stage1_1", LoadSceneMode.Additive);
        
        SoundManager.Inst.PlayBGM("BGM/Stage1");
    }

    void GameExit()
    {
        SoundManager.Inst.PlayUISound("Button1", 2f);

        Application.Quit();
    }

    void PlayerSetting()
    {
        SoundManager.Inst.PlayUISound("Button1", 2f);
        Instantiate(PlayerSettingPrefab, CanvasTr);
    }

    void OpenConfig()
    {
        SoundManager.Inst.PlayUISound("Button1", 2f);
        GameObject configPanel = Instantiate(ConfigPanelPrefab, CanvasTr);
        configPanel.GetComponent<ConfigBoxCtrl>().EnableHomeBtn = false;
    }

    void LoadPlayerSetting()
    {
        PlayerCtrl.PlayerInfo.Name = "Player";
        PlayerCtrl.PlayerInfo.MaxHp = 100;
        PlayerCtrl.PlayerInfo.CurHp = 100;
        PlayerCtrl.PlayerInfo.MaxMp = 100;
        PlayerCtrl.PlayerInfo.CurMp = 100;
        PlayerCtrl.PlayerInfo.Attack = 1;
        PlayerCtrl.PlayerInfo.Critical_Per = 10f;
        PlayerCtrl.PlayerInfo.Defence = 1f;
    }
}
