using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    public GameObject[] ArrBackImg = new GameObject[4];
    public float ParallaxScale = 3f;
    public float ParallaxReductionFactor = 1f;
    public float Smoothing = 1f;
    private Vector3 PreviousCamPos;

    // Start is called before the first frame update
    void Start()
    {
        PreviousCamPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBackround();
    }

    void MoveBackround()
    {
        this.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, this.transform.position.z);

        float parallax = (PreviousCamPos.x - Camera.main.transform.position.x) * ParallaxScale;
        float parallay = (PreviousCamPos.y - Camera.main.transform.position.y) * ParallaxScale;

        for (int i = 0; i < ArrBackImg.Length; i++)
        {
            float backgroundTargetPosX = ArrBackImg[i].transform.position.x + parallax * (i * ParallaxReductionFactor + 1);
            float backgroundTargetPosY = ArrBackImg[i].transform.position.y + parallay * (i * ParallaxReductionFactor + 1);

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, ArrBackImg[i].transform.position.z);
            ArrBackImg[i].transform.position = Vector3.Lerp(ArrBackImg[i].transform.position, backgroundTargetPos, Smoothing * Time.deltaTime);
        }
        PreviousCamPos = Camera.main.transform.position;
    }

    void ChangeBackImg(Texture2D[] arrImg)
    {
        for (int i = 0; i < 4; i++)
        {

        }
    }
}
