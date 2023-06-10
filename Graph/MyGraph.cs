using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class MyGraph
    {
        public List<MyVertex> Vertices { get; }

        public MyGraph() { 
            Vertices = new List<MyVertex>();
        }

        public void AddVertex(string vertex)
        {
            Vertices.Add(new MyVertex(vertex));
        }

        public void RemoveVertex(MyVertex vertex)
        {
            Vertices.Remove(vertex);
        }

        public MyVertex FindVertex(string searchName)
        {
            foreach (MyVertex vertex in Vertices)
            {
                if (vertex.name.Equals(searchName))
                {
                    return vertex;
                }
            }
            return null;
        }

        public void AddEdge(string src, string dst, int weight)
        {
            var s = FindVertex(src);
            var d = FindVertex(dst);
            if (s!=null && d!=null)
            {

                s.AddEdge(d, weight);
                d.AddEdge(s, weight);
            }
        }

        public void RemoveEdge(string src, string dst)
        {
            var s = FindVertex(src);
            var d = FindVertex(dst);

            if (dst == "")
            {
                foreach (MyVertex vertex in Vertices)
                {
                    try
                    {
                        //s.RemoveEdge(vertex);
                        vertex.RemoveEdge(s);
                    }catch (Exception ex) { }
                }
            }
            else
            {
            if(s!=null && d != null)
            {
                    try
                    {
                        s.RemoveEdge(d);
                        d.RemoveEdge(s);
                    }catch (Exception e) { }
            }
            }
        }
    }
}
