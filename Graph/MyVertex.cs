using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class MyVertex
    {
        public List<MyEdge> edges { get; }
        public MyVertex()
        {
            name = string.Empty;
            edges = new List<MyEdge>();
        }

        public MyVertex(string name)
        {
            this.name = name;
            edges = new List<MyEdge>();
        }

        private string _name;

        public virtual string name
        {
            get
            {
                return _name;
            }
            set
            {
                if(_name != value && value is string)
                {
                    _name = value;
                } 
            }
        }

        public void AddEdge(MyEdge edge)
        {
            edges.Add(edge);
        }

        public void AddEdge(MyVertex vertex, int wieght)
        {
            edges.Add(new MyEdge(vertex,wieght));
        }

        public override string ToString()
        {
            return name;
        }
    }
}
