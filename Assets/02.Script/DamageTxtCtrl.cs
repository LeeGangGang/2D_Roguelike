using TMPro;
using UnityEngine;

public class DamageTxtCtrl : MonoBehaviour
{
    private TextMeshPro CurTxt = null;
    Color Alpha;

    [HideInInspector] public int DamageVal = 0;
    [HideInInspector] public bool IsCritical = false;
    private float MoveSpeed;
    private float AlphaSpeed;
    private float DestroyTime;

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = 2.0f;
        AlphaSpeed = 2.0f;
        DestroyTime = 2.0f;

        CurTxt = GetComponent<TextMeshPro>();
        if (IsCritical)
        {
            CurTxt.fontSize += 20f;
            CurTxt.color = Color.yellow;
        }
        Alpha = CurTxt.color;
        CurTxt.text = DamageVal.ToString();
        Destroy(this.gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, MoveSpeed * Time.deltaTime, 0)); // 텍스트 위치

        Alpha.a = Mathf.Lerp(Alpha.a, 0, Time.deltaTime * AlphaSpeed); // 텍스트 알파값
        CurTxt.color = Alpha;
    }
}
