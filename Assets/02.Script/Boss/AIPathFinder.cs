using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y)
    {
        IsGround = _isWall; x = _x; y = _y;
    }

    public bool IsGround;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리
    // H : 목표까지의 거리
    // F : G + H
    // J : 점프 높이 (최대 높이보다 높은 값을 필요하면 제외)
    public int x, y, G, H, J;
    public int F
    {
        get { return G + H; }
    }
}

public class AIPathFinder : MonoBehaviour
{
    private Transform TargetPlayer;

    public Vector2Int Map_BL, Map_TR;
    public List<Node> FindPathList;

    int SizeX, SizeY;
    Node[,] NodeArray;
    private Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public int MaxJump = 3;

    private BossCtrl BossAI;

    // Start is called before the first frame update
    void Start()
    {        
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        SizeX = Map_TR.x - Map_BL.x + 1;
        SizeY = Map_TR.y - Map_BL.y + 1;
        NodeArray = new Node[SizeX, SizeY];

        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                bool IsGround = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + Map_BL.x, j + Map_BL.y), 0.15f))
                {
                    if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
                        IsGround = true;
                }
                NodeArray[i, j] = new Node(IsGround, i + Map_BL.x, j + Map_BL.y);
            }
        }

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FindPathList = new List<Node>();

        BossAI = this.GetComponent<BossCtrl>();
    }

    public void FindPlayer()
    {
        PathFinding();
        if (FindPathList.Count > 0)
        {
            BossAI.MoveNextPos = new Vector3(FindPathList[0].x, FindPathList[0].y, 0f);
        }
    }

    private void PathFinding()
    {
        TargetPlayer = GameObject.Find("Player").transform;

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[Mathf.RoundToInt(this.transform.position.x - Map_BL.x), Mathf.RoundToInt(this.transform.position.y - Map_BL.y)];
        TargetNode = NodeArray[Mathf.RoundToInt(TargetPlayer.position.x - Map_BL.x), Mathf.RoundToInt(TargetPlayer.position.y - Map_BL.y + 0.6f)];

        OpenList.Clear();
        ClosedList.Clear();
        FindPathList.Clear();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면
            // H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
                    CurNode = OpenList[i];
            }

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    if (TargetCurNode.x != TargetCurNode.ParentNode.x)
                        FindPathList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FindPathList.Add(StartNode);
                FindPathList.Reverse();
                return;
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= Map_BL.x && checkX < Map_TR.x + 1 && checkY >= Map_BL.y && checkY < Map_TR.y + 1 &&
            !NodeArray[checkX - Map_BL.x, checkY - Map_BL.y].IsGround &&
            !ClosedList.Contains(NodeArray[checkX - Map_BL.x, checkY - Map_BL.y]))
        {
            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - Map_BL.x, checkY - Map_BL.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

            // 이동비용이 이웃노드 G보다 작거나 || 열린리스트에 이웃노드가 없다면
            // G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                if (CurNode.J > MaxJump)
                    return;

                if (NodeArray[NeighborNode.x, NeighborNode.y - 1].IsGround)
                {
                    NeighborNode.J = 0;
                }
                else
                {
                    if (CurNode.J > 0 && CurNode.y == NeighborNode.y)
                        return;

                    NeighborNode.J = CurNode.y <= NeighborNode.y ? CurNode.J + 1 : CurNode.J;
                }
                NeighborNode.ParentNode = CurNode;

                if (CurNode.y > NeighborNode.y && NodeArray[NeighborNode.x, NeighborNode.y - 1].IsGround == false)
                    OpenListAdd(NeighborNode.x, NeighborNode.y - 1);
                else
                    OpenList.Add(NeighborNode);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (FindPathList.Count != 0)
        {
            for (int i = 0; i < FindPathList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FindPathList[i].x, FindPathList[i].y), new Vector2(FindPathList[i + 1].x, FindPathList[i + 1].y));
        }
    }
}
