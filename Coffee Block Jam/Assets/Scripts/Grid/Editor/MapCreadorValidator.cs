using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoffeeBlockJam.Grid.Editor
{
    public class MapCreadorValidator
    {
        public bool AreMarksValid(List<CellDataJson> marks, out string message) 
        {
            message = string.Empty;
            if (marks == null || marks.Count == 0)
            {
                message = "You need at least one Mark";
                return false;
            }
            if (AreColorAndIdsTogther(marks))
            {
                message = "Some Marks of the same color and ID are separated";
                return false;
            }
            return true;
        }

        private bool AreColorAndIdsTogther(List<CellDataJson> marks)
        {
            Dictionary<(int, Color), List<CellDataJson>> groupedMarks = GetGroupedMarks(marks);
            List<List<CellDataJson>> disconnectedGroups = GetDisconnectedGroups(marks, groupedMarks);
            Debug.Log("disconnectedGroups.Count: " + disconnectedGroups.Count);
            return disconnectedGroups.Count > 0;
        }

        private Dictionary<(int, Color), List<CellDataJson>> GetGroupedMarks(List<CellDataJson> marks) 
        {
            var groupedMarks = new Dictionary<(int id, Color color), List<CellDataJson>>();

            foreach (var mark in marks)
            {
                var key = (mark.id, mark.color);
                if (!groupedMarks.ContainsKey(key))
                    groupedMarks[key] = new List<CellDataJson>();
                groupedMarks[key].Add(mark);
            }
            return groupedMarks;
        }

        private List<List<CellDataJson>> GetDisconnectedGroups(List<CellDataJson> marks, Dictionary<(int, Color), List<CellDataJson>> groupedMarks)
        {
            var disconnectedGroups = new List<List<CellDataJson>>();

            foreach (var group in groupedMarks)
            {
                var groupMarks = group.Value;
                var unvisited = new HashSet<Vector2Int>(groupMarks.Select(m => m.position));
                var alreadyVisited = new HashSet<Vector2Int>();
                int clusterCount = 0;

                while (unvisited.Count > 0)
                {
                    var queue = new Queue<Vector2Int>();
                    var connected = new List<Vector2Int>();

                    Vector2Int start = unvisited.First();
                    queue.Enqueue(start);
                    connected.Add(start);
                    unvisited.Remove(start);
                    alreadyVisited.Add(start);

                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();

                        foreach (var dir in new[]
                        {
                    Vector2Int.up, Vector2Int.down,
                    Vector2Int.left, Vector2Int.right
                })
                        {
                            var neighbor = current + dir;
                            if (unvisited.Contains(neighbor))
                            {
                                unvisited.Remove(neighbor);
                                alreadyVisited.Add(neighbor);
                                queue.Enqueue(neighbor);
                                connected.Add(neighbor);
                            }
                        }
                    }

                    clusterCount++;
                    if (clusterCount > 1)
                    {
                        var groupData = groupMarks
                            .Where(m => connected.Contains(m.position))
                            .ToList();

                        disconnectedGroups.Add(groupData);
                    }
                }
            }

            return disconnectedGroups;
        }
    }
}