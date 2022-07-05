using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public PlayerCtrl PlayerCtrl;

    [SerializeField] private GameObject MiniMap;
    [SerializeField] private Transform CanvasTr;

    [SerializeField] private GameObject InGameUIPanel;
    [SerializeField] private Text TimerTxt;
    private float Timer;

    [Header("Player Info")]
    [SerializeField] private GameObject StatePanel;
    [SerializeField] private Image CurHpImg;
    [SerializeField] private Image CurMpImg;
    [SerializeField] private Text CurHpTxt;
    [SerializeField] private Text CurMpTxt;

    [Header("Weapon Info")]
    [SerializeField] private GameObject WeaponPanel;
    [SerializeField] private Image[] WeaponImg = new Image[2];
    [SerializeField] private Transform[] WeaponTr = new Transform[2];
    [SerializeField] private Vector3 CurWeaponPos;
    [SerializeField] private Vector3 SubWeaponPos;

    [Header("Config")]
    [SerializeField] private GameObject ConfigBoxPrefab;
    private GameObject ConfigBox = null;

    [Header("GameOver")]
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private Text PlayTimeTxt;
    [SerializeField] private Button LobbyBtn;

    bool IsDying = false; // CurState == Die를 한번 탔을때 true
    private string LoadScName;

    // Start is called before the first frame update
    void Start()
    {
        if (!ReferenceEquals(LobbyBtn, null))
            LobbyBtn.onClick.AddListener(GoLobby);

        ConfigBox = null;

        PlayerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        CurWeaponPos = WeaponTr[1].position;
        SubWeaponPos = WeaponTr[0].position;
        for (int i = 0; i < 2; i++)
        {
            if (ReferenceEquals(PlayerCtrl.ArrWeaponCtrl[i], null))
            {
                WeaponImg[i].gameObject.SetActive(false);
                continue;
            }
            else
                WeaponImg[i].gameObject.SetActive(true);

            WeaponImg[i].sprite = PlayerCtrl.ArrWeaponCtrl[i].Info.Img;
            WeaponImg[i].type = Image.Type.Simple;
            WeaponImg[i].preserveAspect = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCtrl.CurState != AnimState.Die)
        {
            Timer += Time.deltaTime;
            TimerTxt.text = TimeSpan.FromSeconds(Timer).ToString(@"hh\:mm\:ss");
        }
        else
        {
            if (IsDying)
                return;

            IsDying = true;
            GameOver();
        }

        StateUpdate();

        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchWeapon();
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenConfig();
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

    public void LoadAsyncMapScene(string loadScName, string unloadScName, bool isStart)
    {
        LoadScName = loadScName;
        SceneManager.UnloadSceneAsync(unloadScName);
        SceneManager.LoadSceneAsync("FadeScene", LoadSceneMode.Additive);
        StartCoroutine(LoadAsyncMapScene(isStart));
    }

    IEnumerator LoadAsyncMapScene(bool isStart)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LoadScName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
            yield return null;

        Camera.main.GetComponent<CameraCtrl>().GetCamLimit();

        PlayerSpawnPos spawnPos = GameObject.Find("PlayerPosition").GetComponent<PlayerSpawnPos>();
        if (!ReferenceEquals(spawnPos, null))
        {
            GameObject player = GameObject.Find("Player");
            if (isStart)
                player.transform.position = spawnPos.StartPos.transform.position;
            else
                player.transform.position = spawnPos.EndPos.transform.position;
        }

        GameObject cutScene = GameObject.Find("CutScene");
        if (!ReferenceEquals(cutScene, null))
        {
            Time.timeScale = 0f;
            if (!ReferenceEquals(cutScene.GetComponent<CutSceneMgr>(), null))
                cutScene.GetComponent<CutSceneMgr>().Play();
        }

        BackgroundCtrl backImgCtrl = GameObject.Find("BackGround").GetComponent<BackgroundCtrl>();
        if (!ReferenceEquals(backImgCtrl, null))
            backImgCtrl.Init(Camera.main.transform.position);
    }

    public void PlayerUIOnOff(bool isShow)
    {
        TimerTxt.gameObject.SetActive(isShow);
        MiniMap.SetActive(isShow);
        StatePanel.SetActive(isShow);
        WeaponPanel.SetActive(isShow);
    }

    void OpenConfig()
    {
        if (ConfigBox == null)
        {
            SoundManager.Inst.PlayUISound("Button1", 2f);
            ConfigBox = Instantiate(ConfigBoxPrefab, CanvasTr);
            ConfigBox.GetComponent<ConfigBoxCtrl>().EnableHomeBtn = true;
        }
        else
        {
            ConfigBox.GetComponent<ConfigBoxCtrl>().Close();
            ConfigBox = null;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        InGameUIPanel.SetActive(false);
        GameOverPanel.SetActive(true);

        PlayTimeTxt.text = "Time : " + TimeSpan.FromSeconds(Timer).ToString(@"hh\:mm\:ss");
    }

    void GoLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("FadeScene");
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);
    }
}
