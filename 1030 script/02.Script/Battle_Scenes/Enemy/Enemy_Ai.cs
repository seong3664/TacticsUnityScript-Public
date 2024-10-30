using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Ai : Enemy_Select,IEnemy_Ai
{

    AimCheck HitchanceCheck;
    int HitChance;
    int GethitChance;

    A_Pathfinding pathfinding;
    [SerializeField] GameObject[] PlayerUnits;

   

    private void Awake()
    {
        HitchanceCheck = GetComponent<AimCheck>();
        pathfinding = GameObject.Find("Grid_Manager").GetComponent<A_Pathfinding>();
    }
    private void Start()
    {
        
    }
    /// <summary>
    /// ���ʹ̰� �������� �̵����� �����ϴ� �޼���
    /// true�� �̵� false�� ����
    /// </summary>
    /// <param name="Unit"></param>
    /// <returns></returns>
    public bool EnemyMoveorAtk(Transform Unit)
    {
        PlayerUnits = GameObject.FindGameObjectsWithTag("Player");
        foreach (var Player in PlayerUnits)
        {
            InfiniteLoopDetector.Run();
            HitChance = HitchanceCheck.HitChanceCheckTransform(Unit,Player.transform);
            GethitChance = HitchanceCheck.HitChanceCheckTransform(Player.transform,Unit);
           if(HitChance >= 30 && GethitChance <= 50 || HitChance >= 60 )
            {
                return false;
            }
        }
        return true;
    }

    public List<A_Nodes> EnemyMove(Transform Unit,UnitStat unitStat)
    {
        List<A_Nodes> ReachableNodes = pathfinding.GetReachableNodes(Unit.position, unitStat.stat.MovePoint);
        List<int> priority = new List<int>(new int[ReachableNodes.Count]);
        for (int i = 0; i < ReachableNodes.Count; i++)
        {
            foreach (GameObject Player in PlayerUnits)
            {
                UnitStat PlayerStat = Player.GetComponent<UnitStat>();
                //���߷� ���
                HitChance = HitchanceCheck.HitChanceCheckVector(ReachableNodes[i].WorldPos, Player.transform.position, unitStat, PlayerStat);
                //�ǰݷ� ���
                GethitChance = HitchanceCheck.HitChanceCheckVector(Player.transform.position, ReachableNodes[i].WorldPos, unitStat, PlayerStat);
                //���߷��� �ǰ�Ȯ���� ��� 0�϶�,�� �Ÿ��� 30 �̻��� ������ ������ �켱���� 0���� ����
                if (HitChance == 0 && GethitChance == 0)
                {
                    priority[i] = 0;
                    continue;
                }
                else
                {
                    //���� Ȯ�� ���� �켱�� �ο�(���� ���� �켱�� ��)
                    {
                        if (HitChance >= 70)
                            priority[i] += 4;
                        else if (HitChance >= 50)
                            priority[i] += 3;
                        else if (HitChance >= 30)
                            priority[i] += 1;
                        else
                            priority[i] += 0;
                    }
                    //�ǰ� Ȯ�� ���� �켱�� �ο�(���� ���� �켱�� ��) 
                    if (ReachableNodes[i].Nodetype == NodeType.Cover || ReachableNodes[i].Nodetype == NodeType.SemiCover)
                    {
                        if (GethitChance >= 70)
                            priority[i] += 0;
                        else if (GethitChance >= 50)
                            priority[i] += 1;
                        else if (GethitChance >= 30)
                            priority[i] += 2;
                        else
                            priority[i] += 3;

                    }
                }
            }
        }
        //�켱�� ���ؼ� �켱���� ���� ���� ���� �̵�. �켱���� ���� ������ ������ �������� ����
        List<int> bestNodesIndices = BestNodeSelec(priority);

        List<int> coverNodesIndices = bestNodesIndices.Where(i => ReachableNodes[i].Nodetype == NodeType.Cover || ReachableNodes[i].Nodetype == NodeType.SemiCover).ToList();

        int selectedIndex;
        // �������� �ϳ��� ��� ����
        if (coverNodesIndices.Count > 0)
        {
            selectedIndex = coverNodesIndices[Random.Range(0, coverNodesIndices.Count)];
        }
        else
        {
            selectedIndex = bestNodesIndices[Random.Range(0, bestNodesIndices.Count)];
        }
        List<A_Nodes> TargetNodepath = new List<A_Nodes>();
        TargetNodepath = pathfinding.A_StarAlgorithm(Unit.position, ReachableNodes[selectedIndex].WorldPos, unitStat.stat.MovePoint, false);
        return TargetNodepath;
        // ������ ��� ��ȯ
    }

    private static List<int> BestNodeSelec(List<int> priority)
    {
        int maxPriority = -1;
        List<int> bestNodesIndices = new List<int>();
        bestNodesIndices.Clear();
        for (int i = 0; i < priority.Count; i++)
        {
            if (priority[i] > maxPriority)
            {
                maxPriority = priority[i];

                bestNodesIndices.Add(i); // ���ο� �ְ� �켱�� ����� �ε��� �߰�
            }
            else if (priority[i] == maxPriority)
            {
                bestNodesIndices.Add(i); // ���� �켱�� ��� �ε��� �߰�
            }
        }

        return bestNodesIndices;
    }

    public Transform EnemyAtk(Transform Unit)
    {
        int temp = 0;
        HitChance = 0;
        Transform AtkTarget = null ;
        foreach(var player in PlayerUnits)
        {
           temp = HitchanceCheck.HitChanceCheckTransform(Unit, player.transform);
            if (HitChance <= temp)
            {
                HitChance = temp;
            }
            AtkTarget = player.transform;
        }
        return AtkTarget;
    }
}

