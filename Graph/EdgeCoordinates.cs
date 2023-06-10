using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class EdgeCoordinates
    {
        public int x1 { get; set; }
        public int x2 { get; set; }
        public int y1 { get; set; }
        public int y2 { get; set; }
        public string src { get; set; }
        public string dst { get; set; }
        public string weight { get; set; }
        public bool isShortestPath { get; set; }
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
