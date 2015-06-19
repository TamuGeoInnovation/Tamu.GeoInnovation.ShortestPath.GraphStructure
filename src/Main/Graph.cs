using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace USC.GISResearchLab.ShortestPath.GraphStructure
{
	[Serializable]
	public sealed class Graph
	{
		Dictionary<long, GraphNode> nodes;
		public bool IsSpatialEnabled;
		public enum MetricType { Time = 0, Distance = 1 }
		private string _roadNetworkDBDescription = string.Empty;
		private string _roadNetworkDBName = string.Empty;
		public object Tag;

		public string RoadNetworkDBName { get { return _roadNetworkDBName; } }
		public string RoadNetworkDBDescription { get { return _roadNetworkDBDescription; } }
		public Dictionary<long, GraphNode>.ValueCollection NodeCollection { get { return nodes.Values; } }

		public GraphNode GetNode(long UID)
		{
			GraphNode a = null;
			if (nodes.TryGetValue(UID, out a)) return a; else return null;
		}
		public Int32 NodeCount
		{
			get { return nodes.Count; }
		}
		public Graph() : this(string.Empty, string.Empty, 0) { }

		public Graph(string roadNetworkDBName, string roadNetworkDBDescription, Int32 capacity)
		{
			nodes = new Dictionary<long, GraphNode>(capacity);
			_roadNetworkDBName = roadNetworkDBName;
			_roadNetworkDBDescription = roadNetworkDBDescription;
			IsSpatialEnabled = false;
		}
		public void InsertNode(long UID, GraphNode node)
		{
			if (!nodes.ContainsKey(UID)) nodes.Add(UID, node);
			else throw new Exception("Duplicate node/UID cannot be added");
		}
		public bool Contains(long UID)
		{
			return (nodes.ContainsKey(UID));
		}

		public static void SaveGraph2Disk(Graph g, string filename)
		{
			var binaryFmt = new BinaryFormatter();
			var fs = new FileStream(filename, FileMode.OpenOrCreate);
			binaryFmt.Serialize(fs, g);
			fs.Close();
		}
		public static Graph ReadFromDisk(string filename)
		{
			var binaryFmt = new BinaryFormatter();
			var fs = new FileStream(filename, FileMode.OpenOrCreate);
			Graph g = (Graph)(binaryFmt.Deserialize(fs));
			fs.Close();
			return g;
		}
	}
}