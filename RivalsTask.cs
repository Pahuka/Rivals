using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Rivals
{
	public class RivalsTask
	{
		public static IEnumerable<OwnedLocation> AssignOwners(Map map)
		{
            var visitPoint = new HashSet<Point>();
            var queue = new Queue<Point>();
            var result = new Dictionary<Point, OwnedLocation>();

            for (int i = 0; i < map.Players.Length; i++)
            {
                result.Add(map.Players[i], new OwnedLocation(i, map.Players[i], 0));
                queue.Enqueue(map.Players[i]);
            }

            visitPoint.Add(map.Players.First());

            while (queue.Count != 0)
            {
                var point = queue.Dequeue();

                if (map.Maze[point.X, point.Y] != MapCell.Empty) continue;

                PointGeneration(visitPoint, map, queue, result, point);
            }

            foreach (var item in result)
            {
                yield return item.Value;
            }
            yield break;
        }

        public static void PointGeneration(HashSet<Point> visitPoint, Map map,
            Queue<Point> queue, Dictionary<Point, OwnedLocation> result, Point point)
        {
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                    if (dx != 0 && dy != 0) continue;
                    else
                    {
                        var tPoint = new Point { X = point.X + dx, Y = point.Y + dy };
                        if (!visitPoint.Contains(tPoint) & map.InBounds(tPoint) & !queue.Contains(tPoint))
                        {
                            if (map.Maze[tPoint.X, tPoint.Y] != MapCell.Empty) continue;
                            queue.Enqueue(tPoint);
                            visitPoint.Add(tPoint);
                            var owned = result.ContainsKey(tPoint) ? 
                                result[tPoint] :
                                new OwnedLocation(result[point].Owner, tPoint, result[point].Distance + 1);
                            result[tPoint] = owned;
                        }
                    }
        }
    }
}
