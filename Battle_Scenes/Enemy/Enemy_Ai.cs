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
    /// 에너미가 공격할지 이동할지 결정하는 메서드
    /// true면 이동 false면 공격
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
                //명중률 계산
                HitChance = HitchanceCheck.HitChanceCheckVector(ReachableNodes[i].WorldPos, Player.transform.position, unitStat, PlayerStat);
                //피격률 계산
                GethitChance = HitchanceCheck.HitChanceCheckVector(Player.transform.position, ReachableNodes[i].WorldPos, unitStat, PlayerStat);
                //명중률과 피격확률이 모두 0일때,즉 거리가 30 이상일 때에는 무조건 우선도를 0으로 설정
                if (HitChance == 0 && GethitChance == 0)
                {
                    priority[i] = 0;
                    continue;
                }
                else
                {
                    //명중 확률 따라 우선도 부여(높을 수록 우선도 업)
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
                    //피격 확률 따라 우선도 부여(낮을 수록 우선도 업) 
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
        //우선도 비교해서 우선도가 가장 높은 노드로 이동. 우선도가 같은 노드들이 있을시 랜덤으로 결정
        List<int> bestNodesIndices = BestNodeSelec(priority);

        List<int> coverNodesIndices = bestNodesIndices.Where(i => ReachableNodes[i].Nodetype == NodeType.Cover || ReachableNodes[i].Nodetype == NodeType.SemiCover).ToList();

        int selectedIndex;
        // 랜덤으로 하나의 노드 선택
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
        // 선택한 노드 반환
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

                bestNodesIndices.Add(i); // 새로운 최고 우선도 노드의 인덱스 추가
            }
            else if (priority[i] == maxPriority)
            {
                bestNodesIndices.Add(i); // 같은 우선도 노드 인덱스 추가
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

