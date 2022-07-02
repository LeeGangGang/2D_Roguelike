using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowArm : MonoBehaviour
{
    public PlayerCtrl Player;

    // Update is called once per frame
    void Update()
    {
        if (Player.IsSkill ||
            Player.IsAttack ||
            Player.CurState == AnimState.Die)
            return;

        Vector2 msPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(msPos.y - this.transform.position.y, msPos.x - this.transform.position.x) * Mathf.Rad2Deg;
        if (this.transform.root.localScale.x > 0)
            angle += 200f;
        else
            angle -= 20f;

        Quaternion Rot = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = Rot;
    }
}
