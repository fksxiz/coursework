using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class GraphShortestPath
    {
        // Конструктор
        public GraphShortestPath(MyGraph graph)
        { 
            _graph = graph;
        }

        private MyGraph _graph;
        private List<VertexInfo> _infos;

        // Инициализация информации
        private void InitInfo()
        {
            _infos = new List<VertexInfo>();
            foreach (MyVertex vertex in _graph.Vertices)
            {
                _infos.Add(new VertexInfo(vertex));
            }
        }

        // Получение информации о вершине
        private VertexInfo GetVertexInfo(MyVertex v)
        {
            foreach(VertexInfo info in _infos)
            {
                if (info.vertex.Equals(v))
                {
                    return info;
                }
            }
            return null;
        }

        // Поиск непосещённой вершины с наименьшим весом рёбер
        public VertexInfo FindUnvisitedVertexMinSum()
        {
            int minValue = int.MaxValue;
            VertexInfo minVertexInfo = null;
            foreach (VertexInfo info in _infos)
            {
                if(info.isUnvisited && info.edgesWeightSum < minValue)
                {
                    minVertexInfo = info;
                    minValue = info.edgesWeightSum;
                }
            }
            return minVertexInfo;
        }

        // Поиск кратчайшего пути
        public List<string> FindShortestPath(MyVertex src, MyVertex dst)
        {
            InitInfo();
            if (dst == null) return new List<string>();
            if (src == null) return new List<string>();
            var first = GetVertexInfo(src);
            first.edgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexMinSum();
                if (current == null)
                {
                    break;
                }
                SetSumToNextVertex(current);
            }
            return GetPath(src, dst);
        }

        // Поиск кратчайшего пути
        public List<string> FindShortestPath(string src, string dst)
        {
            if (dst == null) return new List<string>();
            if (src == null) return new List<string>();
            return FindShortestPath(_graph.FindVertex(src), _graph.FindVertex(dst));
        }

        // Вычисление суммы весов рёбер для следующей вершины
        private void SetSumToNextVertex(VertexInfo info)
        {
            info.isUnvisited = false;
            foreach(var edge in info.vertex.edges)
            {
                var nextInfo = GetVertexInfo(edge.connectedVertex);
                var sum = info.edgesWeightSum + edge.weight;
                if(sum < nextInfo.edgesWeightSum)
                {
                    nextInfo.edgesWeightSum = sum;
                    nextInfo.previousVertex = info.vertex;
                }
            }
        }

        // Формирование пути строкой
        private List<string> GetPath(MyVertex src, MyVertex dst)
        {
            if (dst == null) return new List<string>();
            if(src == null) return new List<string>();
            var path = dst.ToString();
            while (src != dst)
            {
                try
                {
                dst = GetVertexInfo(dst).previousVertex;
                    if (dst == null) return new List<string>();
                    path = dst.ToString() + "?" + path;
                }
                catch
                {
                return new List<string>();
                }
            }
            string buf = "";
            List<string> list = new List<string>();
            for(int i=0; i < path.Length;i++)
            {
                if (path[i]!='?') 
                { 
                    buf += path[i];
                }
                else
                {
                    list.Add(buf);
                    buf = "";
                }
            }
            if (buf != "")
            {
                list.Add(buf);
            }
            return list;
        }

        /*private List<string> GetPath(MyVertex src, MyVertex dst)
        {
            var path = new List<string>();
            path.Add(dst.ToString());
            while (src != dst)
            {
                dst = GetVertexInfo(dst).previousVertex;
                path.Add(dst.ToString());
            }
            return path;
        }*/
    }
}
