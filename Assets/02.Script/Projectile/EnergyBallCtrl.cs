using UnityEngine;

public class EnergyBallCtrl : MonoBehaviour
{
    Vector3 Dir = Vector3.right;
    float MoveSpeed = 15f;

    [HideInInspector] public WeaponInfo WeaponInfo;

    //유도탄 변수
    bool IsTaget = false;
    private GameObject TargetObj = null;

    [SerializeField] private GameObject Explosion;

    // Update is called once per frame
    void Update()
    {
        if (TargetObj != null)
            BulletHoming();
        else
            transform.Translate(Vector2.right * Time.deltaTime * MoveSpeed);
        
        Destroy(gameObject, 3f);
    }

    public void EnergyBallSpawn(Vector3 dir, bool isHoming = false)
    {
        Dir = dir;
        Dir.z = 0f;

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

        float rotSpeed = 5;
        float value = Vector3.Cross(Dir, transform.right).z;
        transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
        transform.Rotate(0, 0, rotSpeed * -value);
    }

    void FindEnemy()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Monster");
        if (enemyList.Length <= 0)
            return;

        GameObject find_Mon = null;
        float dist = 0f;
        Vector2 vec = Vector2.zero;
        for (int i = 0; i < enemyList.Length; ++i)
        {
            if (enemyList[i].GetComponent<UnitCtrl>().CurState == AnimState.Die)
                continue;

            vec = enemyList[i].transform.position - transform.position;
            if (10f < vec.magnitude)
                continue;

            if (dist > vec.magnitude || ReferenceEquals(find_Mon, null))
            {
                dist = vec.magnitude;
                find_Mon = enemyList[i].gameObject;
            }
        }

        TargetObj = find_Mon;
        if (!ReferenceEquals(TargetObj, null))
            IsTaget = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody.transform.CompareTag("Monster"))
        {
            float dmg = WeaponInfo.Attack_Dmg + PlayerCtrl.PlayerInfo.Attack;
            float randDmg = Random.Range(dmg - 1, dmg + 2);
            bool isCritical = Random.Range(0, 101) >= PlayerCtrl.PlayerInfo.Critical_Per + WeaponInfo.Critical_Per;
            col.GetComponentInParent<UnitCtrl>().TakeDamage(this.transform.position, randDmg, isCritical);
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
