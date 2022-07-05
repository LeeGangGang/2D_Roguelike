using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    public GameObject[] ArrBackImg = new GameObject[4];
    private float ParallaxScale = 3f;
    private float ParallaxReductionFactor = 1f;
    private float Smoothing = 1f;
    private Vector3 PreviousCamPos;

    // Update is called once per frame
    void Update()
    {
        MoveBackround();
    }

    public void Init(Vector3 camPos)
    {
        PreviousCamPos = camPos;
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
}
