using UnityEngine;

public class PortalCtrl : Interaction
{
    public string SceneName = "Stage1_Boss";

    public override void InteractionFunc()
    {
        GameManager mgr = GameObject.Find("GameManager").GetComponent<GameManager>();
        mgr.LoadAsyncMapScene("Stage1_Boss", "Stage1_1", true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerCtrl Player = col.GetComponent<PlayerCtrl>();
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
            PlayerCtrl Player = col.GetComponent<PlayerCtrl>();
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
