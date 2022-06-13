using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowArm : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector2 msPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(msPos.y - this.transform.position.y, msPos.x - this.transform.position.x) * Mathf.Rad2Deg;

        Quaternion Rot = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        this.transform.rotation = Rot;
    }
}
