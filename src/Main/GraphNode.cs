using System;

namespace USC.GISResearchLab.ShortestPath.GraphStructure
{
    public sealed class GraphNode : IComparable, IComparable<GraphNode>, IEquatable<GraphNode>
    {
        public static int NodeResolutionInt = 100000;
        public static double NodeResolution = 100000.0;

        internal int longitude, latitude;
        internal Neighbor[] neighbors;

        public int NeighborsCount
        {
            get
            {
                if (neighbors != null) return neighbors.Length;
                else return 0;
            }
        }

        public double Longitude
        {
            get { return longitude / NodeResolution; }
            set { longitude = Convert.ToInt32(value * NodeResolution); }
        }
        public double Latitude
        {
            get { return latitude / NodeResolution; }
            set { latitude = Convert.ToInt32(value * NodeResolution); }
        }

        public Neighbor[] Neighbors
        { get { return neighbors; } }

        public GraphNode() : this(0.0, 0.0) { }

        public GraphNode(double longitude, double latitude)
        {
            try
            {
                Longitude = longitude;
                Latitude = latitude;
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("Given Lat/Long is not within the correct range." + Environment.NewLine +
                "'" + longitude + "' should be between -180 to 180 and '" + latitude + "' should be between -90 to 90.");
            }
            neighbors = null;
        }
        public Int64 UID
        {
            get
            {
                Int64 h1 = Convert.ToInt64(latitude - (18 * NodeResolutionInt));
                Int64 h2 = Convert.ToInt64(longitude);
                if (Longitude > 0) h2 = Convert.ToInt64(-(180 * NodeResolutionInt) - ((180 * NodeResolutionInt) - longitude));
                h2 += 187 * NodeResolutionInt;
                return (h1 + h2 * (59 * NodeResolutionInt));
            }
        }
        public void AddNeighbor(Neighbor n)
        {
            if (neighbors != null)
            {
                Neighbor[] nn = new Neighbor[neighbors.Length + 1];
                Array.Copy(neighbors, 0, nn, 1, neighbors.Length);
                neighbors = null;
                nn[0] = n;
                neighbors = nn;
                nn = null;
            }
            else
            {
                neighbors = new Neighbor[1];
                neighbors[0] = n;
            }
        }
        public void AddRestriction(long parentUID, long destinationUID)
        {
            for (int i = 0; i < NeighborsCount; i++)
            {
                if (neighbors[i].DestinationNodeID == destinationUID)
                {
                    neighbors[i].AddRestriction(parentUID);
                }
            }
        }
        public bool ContainsDuplicateNeighbors()
        {
            int i = 0, j = 0;

            for (i = 0; i < NeighborsCount; i++)
                for (j = i + 1; j < NeighborsCount; j++)
                {
                    if (neighbors[i] == neighbors[j])
                    {
                        return true;
                    }
                }
            return false;
        }
        public bool Equals(GraphNode other)
        {
            if (other == null) return false;
            else return (CompareTo(other) == 0);
        }
        public override bool Equals(object O)
        {
            if (O == null) return false;
            else return (CompareTo((GraphNode)(O)) == 0);
        }
        public override int GetHashCode()
        {
            return UID.GetHashCode();
        }
        public int CompareTo(object obj)
        {
            return (CompareTo(((GraphNode)(obj))));
        }
        public int CompareTo(GraphNode o)
        {
            if (longitude == o.longitude)
            {
                if (latitude == o.latitude) return 0;
                else if (latitude > o.latitude) return 1; else return -1;
            }
            else if (longitude > o.longitude) return 1; else return -1;
        }
        public override string ToString()
        {
            return "<" + this.Longitude + "," + this.Latitude + ">";
        }
    }
}