using System;

namespace USC.GISResearchLab.ShortestPath.GraphStructure
{
    public class PseudoEdge
    {
        public long PrevUID;
        public Node To;

        public PseudoEdge(long prevUID, Node to)
        {
            PrevUID = prevUID;
            To = to;
        }
    }

    public sealed class Node : object, IComparable, IComparable<Node>, IEquatable<Node>
    {
        public static double Cost_NoRoute = -1.0;
        public static double Cost_NoNearNode = -2.0;
        public static double Cost_LogicError = -4.0;
        public static double Cost_MAX = -8.0;
        public static double Cost_Undeterm = -16.0;
        public static double Cost_NullInput = -32.0;
        public static double Cost_TimeOut = -64.0;

        private GraphNode myGraphNode;

        public double F { get { return H + G; } }
        public Node PreviousNode;
        public byte PreviousEdge;
        public double H;
        public double G;

        public int NeighborsCount
        {
            get
            {
                if (myGraphNode.neighbors != null) return myGraphNode.neighbors.Length;
                else return 0;
            }
        }

        public double Longitude
        {
            get { return myGraphNode.Longitude; }
        }
        public double Latitude
        {
            get { return myGraphNode.Latitude; }
        }

        public Neighbor[] Neighbors
        { get { return myGraphNode.neighbors; } }

        public Node() : this(new GraphNode(), Cost_Undeterm, Cost_Undeterm, null) { }

        public Node(GraphNode gnode) : this(gnode, Cost_Undeterm, Cost_Undeterm, null) { }

        public Node(GraphNode gnode, double h, double g, Node previousNode)
        {
            H = h;
            G = g;
            PreviousNode = previousNode;
            myGraphNode = gnode;
            PreviousEdge = 100;
        }

        public Int64 UID { get { return myGraphNode.UID; } }

        public bool Equals(Node other)
        {
            if (other == null) return false;
            else return (CompareTo(other) == 0);
        }
        public override bool Equals(object O)
        {
            if (O == null) return false;
            else
            {
                if (O is Node) return (CompareTo((Node)(O)) == 0);
                else return O.Equals(this);
            }
        }
        /*
		public static bool operator !=(Node n1, Node n2)
		{
			throw new NotImplementedException("Please use overided equals function instead.");
		}
		public static bool operator ==(Node n1, Node n2)
		{
			throw new NotImplementedException("Please use overided equals function instead.");
		}
		*/
        public override int GetHashCode()
        {
            return UID.GetHashCode();
        }
        public int CompareTo(object obj)
        {
            return (CompareTo(((Node)(obj))));
        }
        public int CompareTo(Node o)
        {
            if (myGraphNode.longitude == o.myGraphNode.longitude)
            {
                if (myGraphNode.latitude == o.myGraphNode.latitude) return 0;
                else if (myGraphNode.latitude > o.myGraphNode.latitude) return 1; else return -1;
            }
            else if (myGraphNode.longitude > o.myGraphNode.longitude) return 1; else return -1;
        }
        public override string ToString()
        {
            return myGraphNode.ToString();
        }
    }
}