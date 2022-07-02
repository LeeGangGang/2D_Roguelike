using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneMgr : MonoBehaviour
{
    public Button PlayBtn;
    public Button ExitBtn;
    public Button PlayerSettingBtn;
    public Button ConfigBtn;

    public GameObject ConfigBox;
    public Transform CanvasTr;

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

        SoundManager.Inst.PlayBGM("BGM/TitleBgm");
    }

    // Update is called once per frame
    void Update()
    {

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

    }

    void OpenConfig()
    {
        SoundManager.Inst.PlayUISound("Button1", 2f);
        Instantiate(ConfigBox, CanvasTr);
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
