using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class EdgeCoordinates
    {
        public int x1;
        public int x2;
        public int y1;
        public int y2;
        public string src;
        public string dst;
        public string weight;
        public bool isShortestPath;
        public EdgeCoordinates(int x1, int x2, int y1, int y2, string src, string dst,string weight, bool isShortestPath)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            this.src = src;
            this.dst = dst;
            this.dst = dst;
            this.weight = weight;
            this.isShortestPath = isShortestPath;
        }
    }
}
