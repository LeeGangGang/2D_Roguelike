using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestCtrl : Interaction
{
    public Animator Anim;
    private PlayerCtrl Player;
    bool IsOpen = false;

    [SerializeField] private WeaponDataFact WeaponDataFact;

    public override void InteractionFunc()
    {
        if (IsOpen)
            return;

        IsOpen = true;
        Anim.SetTrigger("Open");
        if (!ReferenceEquals(Player, null))
        {
            if (ReferenceEquals(Player.InteractionObj, this))
                Player.InteractionObj.Remove(this);

            InteractionOnOff(false);
        }

        int dropWeaponIdx = Random.Range(0, WeaponDataFact.WeaponInfoList.Count);
        StartCoroutine(DropItem(dropWeaponIdx));
    }

    IEnumerator DropItem(int dropWeaponIdx)
    {
        yield return new WaitForSeconds(1);
        GameObject weapon = (GameObject)Instantiate(Resources.Load("DropWeapon"));
        weapon.GetComponentInChildren<SpriteRenderer>().sprite = WeaponDataFact.WeaponInfoList[dropWeaponIdx].Img;
        weapon.GetComponentInChildren<DropWeaponCtrl>().Idx = dropWeaponIdx;
        weapon.transform.SetParent(this.transform.parent.parent.parent);
        weapon.transform.position = this.transform.position;
        weapon.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 5f, ForceMode2D.Impulse);
        Destroy(this.transform.parent.parent.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (IsOpen)
            return;

        if (col.CompareTag("Player"))
        {
            Player = col.GetComponent<PlayerCtrl>();
            if (!ReferenceEquals(Player, null))
            {
                if (Player.InteractionObj.Contains(this) == false)
                {
                    Player.InteractionObj.Add(this);
                    InteractionOnOff(true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player = col.GetComponent<PlayerCtrl>();
            if (!ReferenceEquals(Player, null))
            {
                if (Player.InteractionObj.Contains(this))
                {
                    Player.InteractionObj.Remove(this);
                    InteractionOnOff(false);
                }
            }
        }
    }
}
