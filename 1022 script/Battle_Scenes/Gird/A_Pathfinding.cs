using States;
using System.Collections.Generic;
using UnityEngine;

public class A_Pathfinding : MonoBehaviour
{
    public A_Grid grid;
    public MoveCtrl moveCtrl;
    
    HashSet<A_Nodes> closedSet = new HashSet<A_Nodes>();
    private void Start()
    {
        grid = GetComponent<A_Grid>();
    }

    /// <summary>
    /// 경로 탐색 매서드
    /// MaxMovepointretrun가 true라면 목표 위치가 이동 범위를
    /// 벗어날 때 가장 가까운 노드까지의 경로를 반환함.
    /// </summary>
    /// <param name="startpos"></param>
    /// <param name="targetpos"></param>
    /// <param name="maxActionPoints"></param>
    /// <returns></returns>
    public List<A_Nodes> A_StarAlgorithm(Vector3 startpos,Vector3 targetpos ,float maxActionPoints,bool MaxMovepointretrun)
     {
        A_Nodes startNode = grid.GetNodeFromWorldPoint(startpos);
        A_Nodes targetNode = grid.GetNodeFromWorldPoint(targetpos);
        // SortedSet을 사용하고, 우선순위를 gCost + hCost로 비교하는 Comparer를 정의
        SortedSet<A_Nodes> openSet = new SortedSet<A_Nodes>(new NodeComparer());
        closedSet.Clear();

            // 시작 노드를 openSet에 추가
            openSet.Add(startNode);
            startNode.gCost = 0;
            startNode.hCost = GetDistance(startNode, targetNode);

            while (openSet.Count > 0)
            {
                // SortedSet의 첫 번째 요소는 항상 fCost가 가장 작은 노드
                A_Nodes currentNode = openSet.Min;

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                 
                }

                foreach (A_Nodes neighbour in grid.GetNeighbours(currentNode,true))
                {
                    if (!neighbour.Iswalkable || closedSet.Contains(neighbour))
                        continue;

                    float moveCost = GetMoveCost(currentNode, neighbour);
                    float newCostToNeighbour = currentNode.gCost + moveCost;

                    if (newCostToNeighbour <= maxActionPoints && (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)))
                    {
                        // 이미 openSet에 있으면 제거 후 새로운 값으로 추가
                        if (openSet.Contains(neighbour))
                        {
                            openSet.Remove(neighbour);
                        }

                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parentNode = currentNode;

                        openSet.Add(neighbour);
                    }
                    //트루라면 목표 위치가 이동력 범위를 벗어났을 때 가장 가까운 노드를 까지의 경로를 반환
                if (newCostToNeighbour > maxActionPoints && MaxMovepointretrun == true)
                {
                    return RetracePath(startNode, currentNode);
                }
            }
            
            }
        
            return null;
            
    }
    /// <summary>
    /// maxActionPoints 내에서 도달 가능한 모든 노드를 계산해 반환하는 메서드
    /// </summary>
    /// <param name="startpos"></param>
    /// <param name="maxActionPoints"></param>
    /// <returns></returns>
    public List<A_Nodes> GetReachableNodes(Vector3 startpos, float maxActionPoints)
    {
        List<A_Nodes> reachableNodes = new List<A_Nodes>();
        A_Nodes startNode = grid.GetNodeFromWorldPoint(startpos);
        List<A_Nodes> openSet = new List<A_Nodes>();
        closedSet.Clear();

        // 시작 노드를 openSet에 추가
        openSet.Add(startNode);
        startNode.gCost = 0;

        while (openSet.Count > 0)
        {
            A_Nodes currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].gCost < currentNode.gCost ||
                    (openSet[i].gCost == currentNode.gCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            reachableNodes.Add(currentNode);  // 현재 노드를 reachableNodes에 추가

            // 현재 노드의 이웃을 탐색
            foreach (A_Nodes neighbour in grid.GetNeighbours(currentNode, true))
            {
                if (!neighbour.Iswalkable || closedSet.Contains(neighbour))
                    continue;

                float moveCost = GetMoveCost(currentNode, neighbour);
                float newCostToNeighbour = currentNode.gCost + moveCost;

                // 새로운 이동 비용이 maxActionPoints 이내이면 탐색 계속
                if (newCostToNeighbour <= maxActionPoints && (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = 0;  // 목표 노드가 없으므로 hCost는 0으로 설정
                    neighbour.parentNode = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);  // openSet에 없는 경우 추가
                    }
                }
            }
        }

        return reachableNodes;  // 이동 가능한 모든 노드를 반환
    }
    // 노드 간의 이동 비용을 계산하는 함수
    private float GetMoveCost(A_Nodes fromNode, A_Nodes toNode)
        {
            // 직선 이동이면 1, 대각선이면 1.4 같은 방식으로 설정
            return (fromNode.gridX != toNode.gridX && fromNode.gridY != toNode.gridY) ? 1.4f : 1.0f;
        }

        // 두 노드 사이의 거리(hCost)를 계산하는 함수
        private float GetDistance(A_Nodes nodeA, A_Nodes nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 1.4f * dstY + (dstX - dstY);
            return 1.4f * dstX + (dstY - dstX);
        }

        // 경로를 역추적하는 함수
        private List<A_Nodes> RetracePath(A_Nodes startNode, A_Nodes endNode)
        {
            List<A_Nodes> path = new List<A_Nodes>();
            A_Nodes currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parentNode;
            }

            path.Reverse();
            // 여기에 경로를 그리거나 처리하는 추가 로직
            grid.path = path;
        return path;
        }

        // 노드를 비교하는 커스텀 Comparer
        public class NodeComparer : IComparer<A_Nodes>
        {
            public int Compare(A_Nodes nodeA, A_Nodes nodeB)
            {
                // fCost가 낮은 순서로 정렬
                if (nodeA.fCost == nodeB.fCost)
                {
                    // fCost가 같다면 hCost로 비교
                    return nodeA.hCost.CompareTo(nodeB.hCost);
                }
                return nodeA.fCost.CompareTo(nodeB.fCost);
            }
        }
    }
