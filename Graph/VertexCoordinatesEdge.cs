using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class VertexCoordinatesEdge
    {
        public string name; public int x; public int y;
        public bool isShortestPath;

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
