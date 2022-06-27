using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallCtrl : MonoBehaviour
{
    Vector3 Dir = Vector3.right;
    float MoveSpeed = 15.0f;

    [HideInInspector] public float Damage = 20.0f;

    //유도탄 변수
    bool IsTaget = false;
    [HideInInspector] public GameObject TargetObj = null;

    public GameObject Explosion;

    // Update is called once per frame
    void Update()
    {
        if (!ReferenceEquals(TargetObj, null))
            BulletHoming();
        else
            transform.Translate(Vector2.right * Time.deltaTime * MoveSpeed);
        
        Destroy(gameObject, 3f);
    }

    public void EnergyBallSpawn(Vector3 dir, float mvSpeed = 15.0f, float damage = 20.0f, bool isHoming = false)
    {
        Dir = dir;
        Dir.z = 0f;

        MoveSpeed = mvSpeed;
        Damage = damage;

        TargetObj = null;

        //유도탄인 경우...
        if (isHoming)
        {
            if (IsTaget == false)
                FindEnemy();
        }
    }

    void BulletHoming()
    {
        Dir = TargetObj.transform.position - transform.position;
        Dir.z = 0f;
        Dir.Normalize();

        //1번 방법 : 즉시 적을 향해 회전 이동하는 방법
        // https://gnaseel.tistory.com/17
        //float angle = Mathf.Atan2(m_DesiredDir.y, m_DesiredDir.x) * Mathf.Rad2Deg;
        //Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = angleAxis;
        //m_DirTgVec = transform.right;
        //transform.Translate(Vector3.right * m_MoveSpeed * Time.deltaTime);

        // 각도를 보정해서 적을 추적하는 방법
        float rotSpeed = 5;
        float value = Vector3.Cross(Dir, transform.right).z;
        transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
        transform.Rotate(0, 0, rotSpeed * -value);
    }

    void FindEnemy()
    {
        GameObject[] a_EnemyList = GameObject.FindGameObjectsWithTag("Monster");
        if (a_EnemyList.Length <= 0)
            return;

        GameObject a_Find_Mon = null;
        float a_CacDist = 0.0f;
        Vector2 a_CacVec = Vector2.zero;
        for (int i = 0; i < a_EnemyList.Length; ++i)
        {
            if (a_EnemyList[i].GetComponent<UnitCtrl>().CurState == AnimState.Die)
                continue;

            a_CacVec = a_EnemyList[i].transform.position - transform.position;
            a_CacDist = a_CacVec.magnitude;

            if (10.0f < a_CacDist)
                continue;

            a_Find_Mon = a_EnemyList[i].gameObject;
            break;
        }

        TargetObj = a_Find_Mon;
        if (!ReferenceEquals(TargetObj, null))
            IsTaget = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody.transform.CompareTag("Monster"))
        {
            float dmg = Random.Range(Damage - 1, Damage + 2);
            bool isCritical = Random.Range(0f, 100f) <= PlayerCtrl.PlayerInfo.Critical_Per;
            col.GetComponentInParent<UnitCtrl>().TakeDamage(this.transform.position, dmg, isCritical);
            Explode();
            Destroy(this.gameObject);
        }
    }

    void Explode()
    {
        SoundManager.Inst.PlayEffSound("EnergyBall_Hit");

        GameObject explosion = Instantiate(Explosion, this.transform.position, Quaternion.identity);
        Destroy(explosion, 0.6f);
    }
}
