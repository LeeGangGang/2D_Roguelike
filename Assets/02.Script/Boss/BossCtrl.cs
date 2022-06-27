using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCtrl : UnitCtrl
{
    public UnitData UnitData;
    private Transform TargetPlayerTr;
    private Rigidbody2D Rigid;

    private float CurAttCool;
    private float CurSkillCool = 10f;
    private float SkillCool = 10f;

    public GameObject Skill_Prefab;
    private int MaxSkillCnt = 6;

    private List<GameObject> Skill_Pool = new List<GameObject>();

    private AIPathFinder MyAIPathFinder;
    public float FindPlayerTimer = 2f;
    [HideInInspector] public Vector3 MoveNextPos;
    private float ScaleX;

    public Image CurHpImg;
    public Text CurHpTxt;

    int MoveUpDown = 0; // 0 : y Dist == 0, 1 : y Dist > 0, 2 : y Dist < 0
    bool EndLanding = true;

    // Start is called before the first frame update
    void Start()
    {
        UnitInfo MonInfo = new UnitInfo();
        MonInfo.Init(UnitData);

        TargetPlayerTr = GameObject.Find("Player").transform;
        Rigid = this.GetComponent<Rigidbody2D>();

        MyAIPathFinder = this.GetComponent<AIPathFinder>();
        ScaleX = this.transform.localScale.x;
        
        for (int i = 0; i < MaxSkillCnt; i++)
            Skill_Pool.Add(GameObject.Instantiate(Skill_Prefab));

        base.Init(MonInfo);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        BossStatusUI();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            UseSkill();

        BossAI();
    }

    void BossAI()
    {
        CurAttCool -= Time.deltaTime;
        if (unit.CurHp / unit.MaxHp < 0.75f)
            CurSkillCool -= Time.deltaTime;

        if (ReferenceEquals(TargetPlayerTr, null))
            TargetPlayerTr = GameObject.Find("Player").transform;

        FindPlayerTimer -= Time.deltaTime;

        if (CurState == AnimState.Attack || CurState == AnimState.Skill)
            return;

        float attDistX = Mathf.Abs(unit.AttSize[0].x / 2f - unit.AttCenter[0].x);
        float attDistY = Mathf.Abs(unit.AttSize[0].y / 2f - unit.AttCenter[0].y);
        float distX = Mathf.Abs(this.transform.position.x - TargetPlayerTr.position.x);
        float distY = Mathf.Abs(this.transform.position.y - TargetPlayerTr.position.y);

        bool endLanding = EndLanding && Rigid.velocity.y == 0;
        if (distX < attDistX && distY < attDistY && CurAttCool <= 0f && endLanding)
        {
            Boss_LR(TargetPlayerTr.position.x);
            CurState = AnimState.Attack;
            CurAttCool = unit.AttackCool;
        }
        else
        {
            if (CurSkillCool <= 0f && unit.CurMp > 50f)
                UseSkill();
            else
            {
                if (FindPlayerTimer <= 0)
                {
                    MyAIPathFinder.FindPlayer();
                    FindPlayerTimer = 2f;
                }
                MoveNext();
            }
        }
    }

    public float Range = 2f;
    private void UseSkill()
    {
        CurState = AnimState.Skill;

        int randomCnt = Random.Range(1, Skill_Pool.Count + 1);
        int[] randX = GetRandomInt(randomCnt, 9);
        int[] randY = GetRandomInt(randomCnt, 7);
        int curCnt = 0;
        for (int i = 0; i < randomCnt; i++)
        {
            GameObject skill = Skill_Pool[i];
            if (skill.activeSelf == true)
                continue;

            skill.transform.position =
                new Vector3(
                    TargetPlayerTr.position.x + Range * randX[i],
                    TargetPlayerTr.position.y + Range * randY[i], 0f
                    );

            skill.SetActive(true);
            skill.GetComponentInChildren<Boss_SkillCtrl>().UseSkill();

            curCnt++;
            if (randomCnt <= curCnt)
                break;
        }
        unit.CurMp -= 50;
        CurSkillCool = SkillCool;
    }

    private int[] GetRandomInt(int length, int max)
    {
        int[] randArr = new int[length];
        List<int> list = new List<int>();
        for (int i = (max - 1) / -2; i <= (max - 1) / 2; i++)
            list.Add(i);

        for (int i = 0; i < length; ++i)
        {
            int a = Random.Range(0, max - i);
            randArr[i] = list[a];
            list.RemoveAt(a);
        }
        return randArr;
    }

    void MoveNext()
    {
        if (CurState == AnimState.Attack)
            return;

        if (MoveUpDown != 0)
        {
            if (MoveUpDown == 1)
                Rigid.velocity = new Vector2(Rigid.velocity.x, 0f);

            this.transform.position = Vector3.Slerp(this.transform.position, MoveNextPos, 0.08f);
            CurState = AnimState.Idle;
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, MoveNextPos, 0.04f);
            CurState = AnimState.Walk;
        }

        if (Vector3.Distance(MoveNextPos, this.transform.position) < 0.1f)
        {
            if (MyAIPathFinder.FindPathList.Count == 0)
            {
                CurState = AnimState.Idle;
                return;
            }

            MoveNextPos = new Vector3(MyAIPathFinder.FindPathList[0].x, MyAIPathFinder.FindPathList[0].y, 0f);
            MoveUpDown = 0;
            if (Mathf.RoundToInt(transform.position.y) < MyAIPathFinder.FindPathList[0].y)
            {
                MoveNextPos.y += 0.2f;
                MoveUpDown = 1;
            }
            else if (Mathf.RoundToInt(transform.position.y) < MyAIPathFinder.FindPathList[0].y)
                MoveUpDown = 2;
            
            Boss_LR(MoveNextPos.x);

            MyAIPathFinder.FindPathList.RemoveAt(0);
        }
    }

    void Boss_LR(float posX)
    {
        float scaleX = this.transform.position.x < posX ? -ScaleX : ScaleX;
        Vector3 scale = new Vector3(scaleX, this.transform.localScale.y, this.transform.localScale.z);
        this.transform.localScale = scale;
    }

    void BossStatusUI()
    {
        CurHpImg.fillAmount = (float)unit.CurHp / unit.MaxHp;
        CurHpTxt.text = string.Format("{0} / {1}", unit.CurHp, unit.MaxHp);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && col.contacts[0].normal.y > 0.5f)
            EndLanding = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            EndLanding = false;
    }
}
