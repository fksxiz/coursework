using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class VertexInfo
    {

        public VertexInfo(MyVertex vertex) { 
            this.vertex = vertex;
            isUnvisited = true;
            edgesWeightSum = int.MaxValue;
            previousVertex = null;
        }

        private MyVertex _vertex;
        private bool _isUnvisited;
        private int _edgesWeightSum;
        private MyVertex _previousVertex;

        public MyVertex vertex {
            get 
            {
                return _vertex;
            } 
            set
            {
                if (value!=_vertex)
                {
                    _vertex = value;
                }
            }
        }

        public bool isUnvisited
        {
            get
            {
                return _isUnvisited;
            }
            set { 
                if(value!=_isUnvisited)
                {
                    _isUnvisited = value;
                }  
            }
        }

        public int edgesWeightSum
        {
            get
            { 
                return _edgesWeightSum;
            }
            set
            {
                if (value!=_edgesWeightSum)
                {
                    _edgesWeightSum = value;
                }
            }
        }

        public MyVertex previousVertex
        {
            get
            {
                return _previousVertex;
            }
            set
            {
                if (value != _previousVertex)
                {
                    _previousVertex = value;
                }
            }
        }
    }
}
