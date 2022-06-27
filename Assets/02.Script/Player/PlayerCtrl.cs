using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : UnitCtrl
{
    public static UnitInfo PlayerInfo = new UnitInfo();

    public GameObject[] ArrWeaponPos = new GameObject[2];
    public Weapon[] ArrWeaponCtrl = new Weapon[2];
    private int weaponIdx = 0;
    public int WeaponIdx
    {
        get { return weaponIdx; }
        set
        {
            ArrWeaponPos[value].SetActive(true);
            ArrWeaponPos[weaponIdx].SetActive(false);
            weaponIdx = value;
        }
    }
    public Weapon WeaponCtrl
    {
        get
        {
            if (ArrWeaponCtrl.Length > WeaponIdx)
                return ArrWeaponCtrl[WeaponIdx];
            else
                return ArrWeaponCtrl[0];
        }
    }

    public bool IsAttack = false;
    public bool IsSkill = false;
    private bool Jump = false;
    private int JumpMaxCnt = 2;
    private int JumpCnt = 2;
    private float h;
    private float MoveSpeed = 5.0f;
    private Vector3 MoveDir = Vector3.zero;

    private string AnimTrigger = "AnimState";
    [HideInInspector] public Animator Anim;
    [HideInInspector] public Rigidbody2D Rigid;

    public List<Interaction> InteractionObj = new List<Interaction>();

    // Start is called before the first frame update
    void Awake()
    {
        PlayerInfo.Name = "Player";

        PlayerInfo.MaxHp = 100000;
        PlayerInfo.CurHp = 100000;

        Anim = this.GetComponentInChildren<Animator>();
        Rigid = this.GetComponent<Rigidbody2D>();
        InteractionObj.Clear();

        for (int i = 0; i < 2; i++)
        {
            if (ArrWeaponPos[i].transform.childCount != 0)
            {
                ArrWeaponCtrl[i] = ArrWeaponPos[i].transform.GetChild(0).GetComponent<Weapon>();
            }
        }
        ArrWeaponPos[1].SetActive(false);

        base.Init(PlayerInfo);
    }

    void Update()
    {
        if (CurState == AnimState.Die)
            return;

        h = Input.GetAxis("Horizontal");

        if (h != 0.0f && IsSkill == false)
        {
            if (h > 0.0f)
                this.transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
            else if (h < 0.0f)
                this.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            MoveDir = new Vector3(h, 0, 0);
            if (1.0f < MoveDir.magnitude)
                MoveDir.Normalize();

            transform.position += MoveDir * MoveSpeed * Time.deltaTime;
            if (Jump == false)
                Anim.SetInteger(AnimTrigger, 1);
        }
        else
        {
            if (Jump == false)
                Anim.SetInteger(AnimTrigger, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
             if (JumpCnt > 0 && IsSkill == false)
            {
                Jump = true;
                JumpCnt--;
                Rigid.velocity = new Vector2(Rigid.velocity.x, 0f);
                Rigid.AddForce(Vector3.up * 11f, ForceMode2D.Impulse);
                Anim.SetInteger(AnimTrigger, 2);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Anim.SetTrigger("Attack");
            IsAttack = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!Jump)
            {
                Anim.SetTrigger("Skill");
                IsSkill = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
            Anim.SetBool("Stun", true);
        if (Input.GetKeyDown(KeyCode.V))
            Anim.SetBool("Stun", false);

        if (Input.GetKeyDown(KeyCode.Escape))
            Anim.SetTrigger("Die");

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (InteractionObj.Count > 0)
            {
                foreach (Interaction obj in InteractionObj)
                    obj.InteractionFunc();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && col.contacts[0].normal.y > 0.5f)
        {
            Jump = false;
            JumpCnt = JumpMaxCnt;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            Jump = true;
        }
    }
}