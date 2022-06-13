using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraCtrl : MonoBehaviour
{
    Transform m_Player;

    float Height;
    float Width;
    float MoveSpeed = 4f;

    Vector2 MapCenter = Vector2.zero;
    Vector2 MapSize = Vector2.zero;

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
        if (a_SideTileMap != null)
        {
            if (a_SideTileMap.GetComponent<CompositeCollider2D>() != null)
            {
                MapCenter = a_SideTileMap.GetComponent<CompositeCollider2D>().bounds.center;
                MapSize = a_SideTileMap.GetComponent<CompositeCollider2D>().bounds.extents;
            }
        }
    }

    void FixedUpdate()
    {
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
