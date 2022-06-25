using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public PlayerCtrl PlayerCtrl;

    public GameObject MiniMap;
    [Header("Player Info")]
    public GameObject StatePanel;
    public Image CurHpImg;
    public Image CurMpImg;
    public Text CurHpTxt;
    public Text CurMpTxt;

    [Header("Weapon Info")]
    public GameObject WeaponPanel;
    public Image[] WeaponImg = new Image[2];
    public Transform[] WeaponTr = new Transform[2];
    private Vector3 CurWeaponPos;
    private Vector3 SubWeaponPos;

    private string LoadScName;

    private static GameManager Instance;
    public static GameManager Inst
    {
        get
        {
            if (!Instance)
                Instance = FindObjectOfType(typeof(GameManager)) as GameManager;

            return Instance;
        }
    }
    private void Awake()
    {
        if (ReferenceEquals(Instance, null))
            Instance = this;
        else if (!ReferenceEquals(Instance, this))
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

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
        MiniMap.SetActive(isShow);
        StatePanel.SetActive(isShow);
        WeaponPanel.SetActive(isShow);
    }
}
