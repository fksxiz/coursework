using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class VertexCoordinatesEdge
    {
        public string name { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool isShortestPath { get; set; }

        public VertexCoordinatesEdge()
        {

        }
        public VertexCoordinatesEdge(string name,int x, int y,bool isShortestPath)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.isShortestPath = isShortestPath;
        }
    }
}
