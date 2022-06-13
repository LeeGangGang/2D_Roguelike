using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoseCtrl : UnitCtrl
{
    [HideInInspector] public UnitInfo BoseInfo = new UnitInfo();

    private string AnimTrigger = "AnimState";
    private Animator Anim;

    public GameObject Skill_Prefab;
    private int MaxSkillCnt = 5;

    private List<GameObject> Skill_Pool = new List<GameObject>();

    private AIPathFinder MyAIPathFinder;
    [HideInInspector] public Vector3 MoveNextPos;
    private float ScaleX;

    public Image CurHpImg;
    public Text CurHpTxt;

    // Start is called before the first frame update
    void Awake()
    {
        BoseInfo.MaxHp = 10000;
        BoseInfo.CurHp = 10000;

        Anim = this.GetComponentInChildren<Animator>();
        MyAIPathFinder = this.GetComponent<AIPathFinder>();
        ScaleX = this.transform.localScale.x;

        base.Init(BoseInfo);

        for (int i = 0; i < MaxSkillCnt; i++)
        {
            Skill_Pool.Add(GameObject.Instantiate(Skill_Prefab));
        }
    }

    // Update is called once per frame
    void Update()
    {
        BoseStatusUI();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Anim.SetInteger(AnimTrigger, 0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Anim.SetInteger(AnimTrigger, 1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Anim.SetTrigger("Attack");
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject player = GameObject.Find("Player");
            int RandomCnt = Random.Range(1, 6);
            int CurCnt = 0;
            foreach (GameObject skill in Skill_Pool)
            {
                if (skill.activeSelf == true)
                    continue;

                skill.transform.position = new Vector3(player.transform.position.x + 0.3f * CurCnt,
                    player.transform.position.y, 0f);

                skill.SetActive(true);
                skill.GetComponentInChildren<Bose_SkillCtrl>().UseSkill();

                CurCnt++;
                if (RandomCnt <= CurCnt)
                    return;
            }
            Anim.SetTrigger("Skill");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
            Anim.SetTrigger("Hit");
        if (Input.GetKeyDown(KeyCode.Alpha6))
            Anim.SetTrigger("Die");

        MoveNext();
    }



    bool IsJump = false;
    void MoveNext()
    {
        if (IsJump)
        {
            this.transform.position = Vector3.Slerp(this.transform.position, MoveNextPos, 0.02f);
            Anim.SetInteger(AnimTrigger, 0);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, MoveNextPos, 0.02f);
            Anim.SetInteger(AnimTrigger, 1);
        }

        if (Vector3.Distance(MoveNextPos, this.transform.position) < 0.1f)
        {
            if (MyAIPathFinder.FindPathList.Count == 0)
            {
                Anim.SetInteger(AnimTrigger, 0);
                return;
            }

            MoveNextPos = new Vector3(MyAIPathFinder.FindPathList[0].x, MyAIPathFinder.FindPathList[0].y, 0f);
            IsJump = false;
            if (Mathf.RoundToInt(transform.position.y) < MyAIPathFinder.FindPathList[0].y)
            {
                MoveNextPos.y += 0.2f;
                Debug.Log("Jump");
                IsJump = true;
            }

            float scaleX = this.transform.position.x < MoveNextPos.x ? -ScaleX : ScaleX;
            Vector3 scale = new Vector3(scaleX, this.transform.localScale.y, this.transform.localScale.z);
            this.transform.localScale = scale;

            MyAIPathFinder.FindPathList.RemoveAt(0);
        }
    }

    void BoseStatusUI()
    {
        CurHpImg.fillAmount = (float)BoseInfo.CurHp / BoseInfo.MaxHp;
        CurHpTxt.text = string.Format("{0} / {1}", BoseInfo.CurHp, BoseInfo.MaxHp);
    }
}
