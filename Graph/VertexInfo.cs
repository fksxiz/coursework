using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    /// <summary>
    /// Информация о вершине
    /// </summary>
    public class VertexInfo
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="vertex">Вершина</param>
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

        /// <summary>
        /// Вершина
        /// </summary>
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

        /// <summary>
        /// Не посещенная вершина
        /// </summary>
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

        /// <summary>
        /// Сумма весов ребер
        /// </summary>
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

        /// <summary>
        /// Предыдущая вершина
        /// </summary>
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
