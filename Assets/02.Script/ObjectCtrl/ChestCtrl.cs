using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestCtrl : Interaction
{
    public Animator Anim;
    private PlayerCtrl Player;
    bool IsOpen = false;

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
