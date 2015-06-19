using System;

namespace USC.GISResearchLab.ShortestPath.GraphStructure
{
	public sealed class Neighbor : IComparable, IComparable<Neighbor>
	{
		Int64 _ID;
        // bool _PrivateRoad;
        int len;
        byte speed;
		long[] restrictedParentIDs;

        public bool PrivateRoad { get { return len < 0; } }

		internal Neighbor Duplicate()
		{
			var n = new Neighbor();
			n.len = this.len;
			n.speed = this.speed;
			n._ID = this._ID;
			n.restrictedParentIDs = null;

			if ((restrictedParentIDs != null) && (restrictedParentIDs.Length > 0))
			{
				n.restrictedParentIDs = new long[restrictedParentIDs.Length];
				for (int i = 0; i < restrictedParentIDs.Length; i++)
					n.restrictedParentIDs[i] = restrictedParentIDs[i];
			}
			return n;
		}
		public Neighbor() { }

		public Int64 DestinationNodeID { get { return _ID; } }

		public double GetCost(Graph.MetricType metricType)
		{
            double l = Convert.ToDouble(Math.Abs(len)) / 1609.344;
            if (metricType == Graph.MetricType.Distance) return l;
            else return l / Convert.ToDouble(speed);
		}
		public Neighbor(Int64 nodeID, double length, byte speedLimit, bool privateRoad)
		{
			_ID = nodeID;
            len = Math.Abs(Convert.ToInt32(length * 1609.344));
            speed = speedLimit;
			restrictedParentIDs = null;
            if ((len < 1) || (speed < 1)) 
                throw new OverflowException("Cost of zero on edge (" + this.ToString() + ")");
            if (privateRoad) len = -len;
		}
		public bool ContainsRestriction(Int64 parentID)
		{
			if (restrictedParentIDs == null) return false;
			else
			{
				for (int i = 0; i < restrictedParentIDs.Length; i++)
				{
					if (restrictedParentIDs[i] == parentID) return true;
				}
				return false;
			}
		}
		internal void AddRestriction(Int64 parentID)
		{
			if (restrictedParentIDs == null)
			{
				restrictedParentIDs = new long[1];
				restrictedParentIDs[0] = parentID;
			}
			else
			{
				long[] nn = new long[restrictedParentIDs.Length + 1];
				Array.Copy(restrictedParentIDs, 0, nn, 1, restrictedParentIDs.Length);
				restrictedParentIDs = null;
				nn[0] = parentID;
				restrictedParentIDs = nn;
				nn = null;
			}
		}
		public override bool Equals(object O)
		{
			if (O is Neighbor) return Equals((Neighbor)(O));
			else return false;
		}
		public bool Equals(Neighbor O)
		{
			return (_ID == O._ID);
		}
		public override int GetHashCode()
		{
			return _ID.GetHashCode();
		}
		public int CompareTo(object obj)
		{
			if (obj is Neighbor)
				return CompareTo((Neighbor)(obj));
			else return 0;
		}
		public int CompareTo(Neighbor o)
		{
			return (this._ID.CompareTo(o._ID));
		}
		public override string ToString()
		{
			return _ID.ToString() + "," + len + "," + speed;
		}
	}
}