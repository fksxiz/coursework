using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class MyEdge
    {
        public MyEdge(MyVertex vertex, int weight)
        {
            connectedVertex = vertex;
            this.weight = weight;
        }

        private int _weight;
        private MyVertex _connectedVertex;

        public int weight
        {
            get
            {
                return _weight;
            }
            set
            {
                if(value < 0) value = 0;
                if(value > 100) value = 100;
                if(value != _weight) _weight = value; 
            }
        }

        public MyVertex connectedVertex
        {
            get { return _connectedVertex; }
            set
            {
                if(value !=  _connectedVertex) _connectedVertex = value;
            }
        }
    }
}
