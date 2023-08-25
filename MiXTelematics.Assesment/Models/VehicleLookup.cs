using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiXTelematics.Assesment.Models
{
    internal class VehicleLookup
    {

        private Node root;

        public VehicleLookup(List<VehiclePosition> positions)
        {
            root = ConstructVehicleLookupTree(positions, 0);
        }

        public VehiclePosition FindNearestNeighbor(Coordinate target)
        {
            return  FindNearestNeighbor(root, target, null, double.MaxValue);
        }

        private Node ConstructVehicleLookupTree(List<VehiclePosition> positions, int depth)
        {
            if (positions.Count == 0)
                return null;

            int axis = depth % 2;
            positions.Sort((a, b) => axis == 0 ? a.Coordinate.Latitude.CompareTo(b.Coordinate.Latitude) : a.Coordinate.Longitude.CompareTo(b.Coordinate.Longitude));

            int medianIndex = positions.Count / 2;
            Node medianNode = new Node(positions[medianIndex]);

            medianNode.Left = ConstructVehicleLookupTree(positions.GetRange(0, medianIndex), depth + 1);
            medianNode.Right = ConstructVehicleLookupTree(positions.GetRange(medianIndex + 1, positions.Count - medianIndex - 1), depth + 1);

            return medianNode;
        }

        private VehiclePosition FindNearestNeighbor(Node node, Coordinate target, VehiclePosition best, double bestDistance)
        {
            if (node == null)
                return best;

            double distance = node.Position.Coordinate.DistanceTo(target);

            if (distance < bestDistance)
            {
                best = node.Position;
                bestDistance = distance;
            }

            int axis = node.Depth % 2;
            Node nearerNode = node.Left;
            Node fartherNode = node.Right;

            if ((axis == 0 && target.Latitude < node.Position.Coordinate.Latitude) || (axis == 1 && target.Longitude < node.Position.Coordinate.Longitude))
            {
                nearerNode = node.Left;
                fartherNode = node.Right;
            }
            else
            {
                nearerNode = node.Right;
                fartherNode = node.Left;
            }

            return FindNearestNeighbor(nearerNode, target, best, bestDistance); ;



        }

        private class Node
        {
            public VehiclePosition Position { get; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public int Depth { get; }

            public Node(VehiclePosition position, int depth = 0)
            {
                Position = position;
                Depth = depth;
            }
        }
    }
}
