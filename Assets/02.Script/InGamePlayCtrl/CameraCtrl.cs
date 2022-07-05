using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraCtrl : MonoBehaviour
{
    private Transform m_Player;

    private float Height;
    private float Width;
    private float MoveSpeed = 4f;

    private Vector2 MapCenter = Vector2.zero;
    private Vector2 MapSize = Vector2.zero;

    private bool FlowCam = true;
    private float ShakeAmount = 0.03f;
    private float ShakeCamTimer = 0.2f;

    [SerializeField] private Image BloodScreenImg;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.Find("Player").transform;

        Height = Camera.main.orthographicSize;
        Width = Height * Screen.width / Screen.height;

        GetCamLimit();
    }

    public void GetCamLimit()
    {
        GameObject a_SideTileMap = GameObject.Find("Side_Tilemap");
        if (!ReferenceEquals(a_SideTileMap, null))
        {
            if (!ReferenceEquals(a_SideTileMap.GetComponent<CompositeCollider2D>(), null))
            {
                MapCenter = a_SideTileMap.GetComponent<CompositeCollider2D>().bounds.center;
                MapSize = a_SideTileMap.GetComponent<CompositeCollider2D>().bounds.extents;
            }
        }
    }

    public void Hurt()
    {
        StartCoroutine(ShakeCam());
        StartCoroutine(BloodScreen());
    }

    IEnumerator ShakeCam()
    {
        FlowCam = false;
        float timer = 0;
        while (timer <= ShakeCamTimer)
        {
            transform.localPosition += (Vector3)Random.insideUnitCircle * ShakeAmount;

            timer += Time.deltaTime;
            yield return null;
        }
        FlowCam = true;
    }

    IEnumerator BloodScreen()
    {
        BloodScreenImg.color = new Color(0.5f, 0f, 0f, Random.Range(0.2f, 0.5f));
        yield return new WaitForSeconds(0.1f);
        BloodScreenImg.color = Color.clear;
    }

    void FixedUpdate()
    {
        if (FlowCam)
            LimitCameraArea();
    }

    void LimitCameraArea()
    {
        transform.position = Vector3.Lerp(transform.position, m_Player.position, Time.deltaTime * MoveSpeed);
        float lx = MapSize.x - Width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + MapCenter.x, lx + MapCenter.x);

        float ly = MapSize.y - Height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + MapCenter.y, ly + MapCenter.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(MapCenter, MapSize * 2);
    }
}
