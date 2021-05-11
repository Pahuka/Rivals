using System;
using System.Collections.Generic;
using System.Drawing;

namespace Rivals
{
    public class RivalsTask
    {
        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var queue = new Queue<Tuple<Point, OwnedLocation>>();
            var visitPoints = new HashSet<Point>();

            for (int i = 0; i < map.Players.Length; i++)
            {
                queue.Enqueue(Tuple.Create(map.Players[i], new OwnedLocation(i, map.Players[i], 0)));
                visitPoints.Add(map.Players[i]);
            }

            while (queue.Count != 0)
            {
                var owner = queue.Dequeue();

                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx != 0 && dy != 0) continue;
                        else
                        {
                            var tPoint = new Point { X = owner.Item1.X + dx, Y = owner.Item1.Y + dy };
                            if (map.InBounds(tPoint) & !visitPoints.Contains(tPoint))
                            {
                                if (map.Maze[tPoint.X, tPoint.Y] != MapCell.Empty) continue;
                                queue.Enqueue(Tuple.Create(tPoint,
                                    new OwnedLocation(owner.Item2.Owner, tPoint, owner.Item2.Distance + 1)));
                                visitPoints.Add(tPoint);
                            }
                        }
                yield return owner.Item2;
            }
        }
    }
}