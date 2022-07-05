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

    // G : �������κ��� �̵��ߴ� �Ÿ�
    // H : ��ǥ������ �Ÿ�
    // F : G + H
    // J : ���� ���� (�ִ� ���̺��� ���� ���� �ʿ��ϸ� ����)
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
        // NodeArray�� ũ�� �����ְ�, isWall, x, y ����
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

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[Mathf.RoundToInt(this.transform.position.x - Map_BL.x), Mathf.RoundToInt(this.transform.position.y - Map_BL.y)];
        TargetNode = NodeArray[Mathf.RoundToInt(TargetPlayer.position.x - Map_BL.x), Mathf.RoundToInt(TargetPlayer.position.y - Map_BL.y + 0.6f)];

        OpenList.Clear();
        ClosedList.Clear();
        FindPathList.Clear();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ�
            // H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
                    CurNode = OpenList[i];
            }

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // ������
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

            // �� �� �� ��
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= Map_BL.x && checkX < Map_TR.x + 1 && checkY >= Map_BL.y && checkY < Map_TR.y + 1 &&
            !NodeArray[checkX - Map_BL.x, checkY - Map_BL.y].IsGround &&
            !ClosedList.Contains(NodeArray[checkX - Map_BL.x, checkY - Map_BL.y]))
        {
            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            Node NeighborNode = NodeArray[checkX - Map_BL.x, checkY - Map_BL.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

            // �̵������ �̿���� G���� �۰ų� || ��������Ʈ�� �̿���尡 ���ٸ�
            // G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
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
