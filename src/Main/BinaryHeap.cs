using System.Collections.Generic;

namespace USC.GISResearchLab.ShortestPath.GraphStructure
{
	public sealed class BinaryHeap
	{
        List<PseudoEdge> edges; //heap only contains the vertices of the graph
        MultiDictionary<long, long, int> edge2pos; //track the position of an edge in the heap
		public int Count { get { return edge2pos.Count; } }
		int tail;

		//O(n)
        /*
		public BinaryHeap(Graph graph)
		{
			int i = 0;
			vertices = new List<Node>(graph.NodeCount);
            edge2pos = new Dictionary<long, Dictionary<long, Node>>(graph.NodeCount);

			//O(n)
			foreach (var gnode in graph.NodeCollection) //initiate the position tracker
			{
				//O(1)
				edge2pos.Add(new Node(gnode), i);
                vertices.Add(new Node(gnode));
				i++;
			}
			tail = vertices.Count - 1;
		}
        
		public void BldHeapByInsert(Graph g)
		{
			foreach (var n in g.NodeCollection)
                Insert(new Node(n));
		}

		public void Clear()
		{
			this.edge2pos.Clear();
			this.edges.Clear();
			tail = -1;
		}
        */

        public BinaryHeap(int capacity)
		{
            edges = new List<PseudoEdge>(capacity);
            edge2pos = new MultiDictionary<long, long, int>(capacity);
			tail = -1;
		}

		/**
		 * build a heap out of the graph's nodes
		 * expected performance: O(n) : n= number of nodes of the graph
		 * complexity: O(n)
		 */
		public void BuildHeap()
		{
			int cnt;
			cnt = tail >> 1;
			for (int i = cnt; i >= 0; i--) Heapify(i);
		}

		public void Insert(long prev, Node n)
		{
			tail++;
			edges.Add(new PseudoEdge(prev, n));
			edge2pos.Set(prev, n.UID, tail);
            DecreaseKey(prev, n, n.G);
		}

        public void DecreaseKey(long prev, Node node, double gval)
		{
			int p, curr;
            PseudoEdge tmp;
			try
			{
				node.G = gval;
				curr = edge2pos.Get(prev, node.UID);
				p = (curr - 1) >> 1;
				while (p >= 0 && (edges[curr].To.F < edges[p].To.F))   //'float it up'   !!!EXP
				{
					tmp = edges[p];
					edges[p] = edges[curr];
					edges[curr] = tmp;

					//modify the position, !!!EXP
					//edge2pos[edges[p]] = p;
					//edge2pos[edges[curr]] = curr;
                    edge2pos.Set(edges[p].PrevUID, edges[p].To.UID, p);
                    edge2pos.Set(edges[curr].PrevUID, edges[curr].To.UID, curr);
					curr = p;
					p = (curr - 1) >> 1;
				}
			}
			catch (System.Threading.ThreadAbortException te)
			{
				throw te;
			}
			catch
			{
				p = 1;
			}
		}

		/**
		  * extract the node that has smallest key value---goal distance
		  * expected performance: O(lg n) : n= number of nodes of the graph
		  */
        public PseudoEdge ExtractMin()
		{
			PseudoEdge min = null;
			if (tail >= 0)
			{
				min = edges[0];
				edges[0] = edges[tail];
                edge2pos.Set(edges[tail].PrevUID, edges[tail].To.UID, 0);
				//edge2pos[edges[tail]] = 0;
                tail--;
				edge2pos.Remove(min.PrevUID, min.To.UID);
				edges.RemoveAt(tail + 1);
				Heapify(0);
			}
			return min;
		}

		public bool isEmpty()
		{
			return tail < 0;
		}

		// O(1)
        public Node FindGraphNode(long prev, GraphNode gNode)
		{
			Node n = null;
            if (edge2pos.Contains(prev, gNode.UID)) n = edges[edge2pos.Get(prev, gNode.UID)].To;
            return n;
        }

        /**
         * this function maintains the heap property for a particular node
         * int i: the serial number of a node in heap
         * complexity: O(lg n)
         */
        private void Heapify(int i)
        {
            try
            {
                int l, r, min; //(l)eft child, (r)ight child, (min)imum key among l, r and i; 
                PseudoEdge tmp;
                l = (i << 1) + 1; //use shift << instead of multiply *
                r = (i << 1) + 2;
                if (l <= tail && (edges[l].To.F < edges[i].To.F))
                    min = l;
                else
                    min = i;
                if (r <= tail && (edges[r].To.F < edges[min].To.F))
                    min = r;
                if (min != i)
                {
                    //exchange node at i with node with minimum key value, either left or right child
                    tmp = edges[i];
                    edges[i] = edges[min];
                    edges[min] = tmp;
                    //modify the position
                    edge2pos.Set(edges[min].PrevUID, edges[min].To.UID, min);
                    edge2pos.Set(edges[i].PrevUID, edges[i].To.UID, i);
                    //edge2pos[edges[min]] = min;
                    //edge2pos[edges[i]] = i;
                    Heapify(min);
                }
            }
            catch (System.Threading.ThreadAbortException te)
            {
                throw te;
            }
        }

        public bool Contains(long prev, Node node)
        {
            return edge2pos.Contains(prev, node.UID);
        }
    }
}