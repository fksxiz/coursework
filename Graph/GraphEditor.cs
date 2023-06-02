using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{
    public class GraphEditor : Control
    {

        protected Color _VertexColor;
        protected Color _EdgeColor;
        protected Color _TextColor;
        protected Color _DarkColor;
        protected Color _LightColor;
        protected Color _ShortestPathColor;
        protected int _VertexSize;
        protected int _VertexCount;
        public enum ObjStates
        {
            osConvex,
            osConcavity
        }
        private ObjStates _ObjState;
        protected bool _IsVertexAddMode;
        protected bool _IsEdgeAddMode;
        public virtual bool IsVertexAddMode
        {
            get { return _IsVertexAddMode; }
            set { 
                if( _IsVertexAddMode != value )
                {
                    _IsVertexAddMode = value;
                }
                }
        } 
        public virtual bool IsEdgeAddMode
        {
            get { return _IsEdgeAddMode; }
            set { 
                if(_IsEdgeAddMode != value )
                {
                    _IsEdgeAddMode = value;
                }
                }
        }

        public virtual ObjStates ObjState
        {
            get
            {
                return _ObjState;
            }
            set
            {
                if (value != _ObjState)
                {
                    _ObjState = value;
                    Invalidate();
                }

            }
        }
        public GraphEditor() : base()
        { 
            BackColor = Color.AliceBlue;
            _VertexColor = Color.Black; EdgeColor = Color.Gray;
            _TextColor = Color.Red;
            _ShortestPathColor = Color.Green;
            _DarkColor = Color.DarkGray;
            _LightColor = Color.Gray;
            _graph = new MyGraph();
            _VertexCount = 0;
        }
     
        public Color VertexColor
        {
            get
            {
                return _VertexColor;
            }
            set
            {
                if (value != _VertexColor)
                {
                    _VertexColor = value;
                    Invalidate();
                }
            }
        }

        public Color ShortestPathColor
        {
            get
            {
                return _ShortestPathColor;
            }
            set
            {
                if (value != _ShortestPathColor)
                {
                    _ShortestPathColor = value;
                    Invalidate();
                }
            }
        }

        public Color EdgeColor
        {
            get
            {
                return _EdgeColor;
            }
            set
            {
                if (value != _EdgeColor)
                {
                    _EdgeColor = value;
                    Invalidate();
                }
            }
        }

        public Color TextColor
        {
            get
            {
                return _TextColor;
            }
            set
            {
                if (value != _TextColor)
                {
                    _TextColor = value;
                    Invalidate();
                }
            }
        }

        public Color DarkColor
        {
            get
            {
                return _DarkColor;
            }
            set
            {
                if (value != _DarkColor)
                {
                    _DarkColor = value;
                    Invalidate();
                }
            }
        }

        public Color LightColor
        {
            get
            {
                return _LightColor;
            }
            set
            {
                if (value != _LightColor)
                {
                    _LightColor = value;
                    Invalidate();
                }
            }
        }

        private MyGraph _graph;
        private GraphShortestPath _shortestPath;
        protected List<VertexCoordinatesEdge> vertexCoordinates = new List<VertexCoordinatesEdge>();
        protected List<EdgeCoordinates> edgeCoordinates = new List<EdgeCoordinates>();

        public void Reset()
        {
            _graph = new MyGraph();
            _shortestPath=new GraphShortestPath(_graph);
            vertexCoordinates = new List<VertexCoordinatesEdge>();
            edgeCoordinates = new List<EdgeCoordinates>();
        OnResetGraph();
            Invalidate();
        }

        private void addVertex(int x, int y)
        {
            _VertexCount++;
            vertexCoordinates.Add(new VertexCoordinatesEdge(_VertexCount.ToString(), x,y,false));
            _graph.AddVertex(_VertexCount.ToString());
            OnVertexAdd();
            Invalidate();
        }

        public void addEdge(string src,string dst,int weight)
        {
            if (_graph.FindVertex(src)!=null && _graph.FindVertex(dst) != null) {
                int x1=0;
                int x2=0;
                int y1=0;
                int y2=0;
                foreach (VertexCoordinatesEdge vertex in vertexCoordinates)
                {
                    if (vertex.name==src) { x1 = vertex.x; y1 = vertex.y; }
                    if (vertex.name==dst) { x2 = vertex.x; y2 = vertex.y; }
                }
                edgeCoordinates.Add(new EdgeCoordinates(x1, x2, y1, y2, src, dst,weight.ToString(),false));
                _graph.AddEdge(src, dst, weight);
            }
            OnEdgeAdd();
            Invalidate();
        }

        public void FindShortestPath(string src,string dst)
        {
            ResetShortestPath();
            _shortestPath = new GraphShortestPath(_graph);
            var vertices = _shortestPath.FindShortestPath(src, dst);
            SetShortestPath(vertices);
            OnFindShortestPath();
            Invalidate();
        }

        private void ResetShortestPath()
        {
            foreach (var ver in vertexCoordinates)
            {
              ver.isShortestPath = false;
            }
            foreach (var e in edgeCoordinates)
            {
              e.isShortestPath = false;
            }
        }

        private void SetShortestPath(List<string> vertices)
        {
            foreach (var vertexName in vertices)
            {
                foreach(var ver in vertexCoordinates)
                {
                    if (ver.name == vertexName)
                    {
                        ver.isShortestPath = true;
                    }
                }
            }
            for (int i = 0; i < vertices.Count-1; i++)
            {
                foreach (var e in edgeCoordinates)
                {
                    if (e.src == vertices[i] && e.dst == vertices[i + 1])
                    {
                        e.isShortestPath = true;
                    }
                }
            }
        }

        public virtual void State()
        {
            int I = (int)_ObjState + 1;
            I = I > 1 ? 0 : I;
            ObjState = (ObjStates)I;
            OnChangeState();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (width < 400)
            {
                width = 400;
            }
            if (height < 400)
            {
                height = 400;
            }
            _VertexSize = Math.Min(width, height) / 20;
            base.SetBoundsCore(x, y, width, height, specified);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Brush VertexBrush = new SolidBrush(_VertexColor);
            Brush ShortestPathBrush = new SolidBrush(_ShortestPathColor);
            Pen ShortestPathPen = new Pen(_ShortestPathColor);
            Pen EdgePen = new Pen(_EdgeColor);
            Pen LightPen = new Pen(_LightColor);
            Pen DarkPen = new Pen(_DarkColor);
            int FontSize = (int)Math.Round((_VertexSize - 16) * 120 / e.Graphics.DpiY);
            if (FontSize <= 16)
            {
                FontSize = 16;
            }
            Font font = new Font("Arial", FontSize);
            SolidBrush TextBrush = new SolidBrush(_TextColor);
            StringFormat Fmt = new StringFormat();
            Fmt.LineAlignment = StringAlignment.Center;
            for (int i = 0; i < Math.Min(Height,Width) / 15; i++)
            {
                Point[] D =
                    {
                        new Point(Width-i,i),
                        new Point(i,i),
                        new Point(i,Height-i)
                        };
                Point[] L =
            {
                        new Point(Width-i,i),
                        new Point(Width-i,Height-i),
                        new Point(i,Height-i)
                        };
                if ((int)ObjState == 0)
                {
                    e.Graphics.DrawLines(DarkPen, D);
                    e.Graphics.DrawLines(LightPen, L);
                }
                else
                {
                    e.Graphics.DrawLines(DarkPen, L);
                    e.Graphics.DrawLines(LightPen, D);
                }
            }
            DrawEdges(e,EdgePen,ShortestPathPen,font,TextBrush,Fmt);
            DrawVertices(e, VertexBrush, ShortestPathBrush, font, TextBrush, Fmt);

            base.OnPaint(e);
        }

        private void DrawEdges(PaintEventArgs e, Pen EdgePen, Pen ShortestPathPen, Font font, SolidBrush TextBrush, StringFormat Fmt)
        {
            Fmt.LineAlignment = StringAlignment.Center;
            foreach (var edge in edgeCoordinates)
            {
                if (edge != null)
                {
                    if (edge.x1 - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        edge.x1 = Math.Min(Height, Width) / 15+_VertexSize / 2;
                    }
                    else
                    {
                        if (edge.x1 + _VertexSize / 2 > Width- Math.Min(Height, Width) / 15)
                        {
                            edge.x1 = Width - Math.Min(Height, Width) / 15 - _VertexSize / 2;
                        }
                    }
                    if (edge.y1 - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        edge.y1 = Math.Min(Height, Width) / 15 + _VertexSize / 2;
                    }
                    else
                    {
                        if (edge.y1 + _VertexSize / 2 > Height- Math.Min(Height, Width) / 15)
                        {
                            edge.y1 = Height - Math.Min(Height, Width) / 15 - _VertexSize / 2;
                        }
                    }
                    if (edge.x2 - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        edge.x2 = Math.Min(Height, Width) / 15+_VertexSize / 2;
                    }
                    else
                    {
                        if (edge.x2 + _VertexSize / 2 > Width - Math.Min(Height, Width) / 15)
                        {
                            edge.x2 = Width- Math.Min(Height, Width) / 15 - _VertexSize / 2;
                        }
                    }
                    if (edge.y2 - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        edge.y2 = Math.Min(Height, Width) / 15+_VertexSize / 2;
                    }
                    else
                    {
                        if (edge.y2 + _VertexSize / 2 > Height- Math.Min(Height, Width) / 15)
                        {
                            edge.y2 = Height - _VertexSize / 2-Math.Min(Height, Width) / 15;
                        }
                    }
                    
                    if (!edge.isShortestPath)
                    {
                        e.Graphics.DrawLine(EdgePen, edge.x1, edge.y1, edge.x2, edge.y2);
                    }
                    else
                    {
                        e.Graphics.DrawLine(ShortestPathPen, edge.x1, edge.y1, edge.x2, edge.y2);
                    }
                    int x = (Math.Min(edge.x2, edge.x1) - _VertexSize) + ((Math.Max(edge.x2, edge.x1) - _VertexSize) - (Math.Min(edge.x1, edge.x2) - _VertexSize)) / 2;
                    int y = (Math.Min(edge.y2, edge.y1) - _VertexSize / 2) + ((Math.Max(edge.y2, edge.y1) - _VertexSize / 2) - (Math.Min(edge.y1, edge.y2) - _VertexSize / 2)) / 2;
                    RectangleF Rect = new RectangleF(x, y, _VertexSize * 2, _VertexSize);
                    String S = edge.weight;
                    e.Graphics.DrawString(S, font, TextBrush, Rect, Fmt);
                }
            }
        }

        private void DrawVertices(PaintEventArgs e, Brush VertexBrush, Brush ShortestPathBrush, Font font, SolidBrush TextBrush, StringFormat Fmt)
        {
            Fmt.LineAlignment = StringAlignment.Center;
            foreach (var vertex in vertexCoordinates)
            {
                if (vertex != null)
                {
                    if (vertex.x - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        vertex.x = Math.Min(Height, Width) / 15+_VertexSize / 2;
                    }
                    else
                    {
                        if (vertex.x + _VertexSize / 2 > Width- Math.Min(Height, Width) / 15)
                        {
                            vertex.x = Width- Math.Min(Height, Width) / 15 - _VertexSize / 2;
                        }
                    }
                    if (vertex.y - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        vertex.y = Math.Min(Height, Width) / 15+_VertexSize / 2;
                    }
                    else
                    {
                        if (vertex.y + _VertexSize / 2 > Height- Math.Min(Height, Width) / 15)
                        {
                            vertex.y = Height - Math.Min(Height, Width) / 15 - _VertexSize / 2;
                        }
                    }
                    if (vertex.isShortestPath)
                    {
                        e.Graphics.FillEllipse(ShortestPathBrush, vertex.x - _VertexSize / 2, vertex.y - _VertexSize / 2, _VertexSize, _VertexSize);
                    }
                    else
                    {
                        e.Graphics.FillEllipse(VertexBrush, vertex.x - _VertexSize / 2, vertex.y - _VertexSize / 2, _VertexSize, _VertexSize);
                    }
                    RectangleF Rect = new RectangleF(vertex.x - _VertexSize / 2, vertex.y - _VertexSize / 2, _VertexSize, _VertexSize);
                    String S = vertex.name;
                    e.Graphics.DrawString(S, font, TextBrush, Rect, Fmt);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            if (e.Button == MouseButtons.Left)
            {
                if (_IsVertexAddMode)
                {
                    addVertex(x,y);
                    _IsVertexAddMode = false;
                }
                base.OnMouseDown(e);
            }
            Invalidate();
        }

        protected override CreateParams CreateParams
        {
            get { 
                CreateParams P = base.CreateParams;
                P.ExStyle = P.ExStyle | 0x02000000;
                return P;
            }
        }

        protected event EventHandler _OnChangeState;
        protected event EventHandler _OnVertexAdd;
        protected event EventHandler _OnEdgeAdd;
        protected event EventHandler _OnFindShortestPath;
        protected event EventHandler _OnReset;

        public event EventHandler ChangeState
        {
            add
            {
                _OnChangeState += value;
            }
            remove
            {
                _OnChangeState -= value;
            }
        }
        public event EventHandler VertexAdd
        {
            add
            {
                _OnVertexAdd += value;
            }
            remove
            {
                _OnVertexAdd -= value;
            }
        }
        public event EventHandler EdgeAdd
        {
            add
            {
                _OnEdgeAdd += value;
            }
            remove
            {
                _OnEdgeAdd -= value;
            }
        }
        public event EventHandler FindedShortestPath
        {
            add
            {
                _OnFindShortestPath += value;
            }
            remove
            {
                _OnFindShortestPath -= value;
            }
        }
        public event EventHandler ResetGraph
        {
            add
            {
                _OnReset += value;
            }
            remove
            {
                _OnReset -= value;
            }
        }

        protected void OnChangeState()
        {
            if (_OnChangeState != null)
            {
                _OnChangeState.Invoke(this, new EventArgs());
            }
        }
        protected void OnVertexAdd()
        {
            if (_OnVertexAdd != null)
            {
                _OnVertexAdd.Invoke(this, new EventArgs());
            }
        }
        protected void OnEdgeAdd()
        {
            if (_OnEdgeAdd != null)
            {
                _OnEdgeAdd.Invoke(this, new EventArgs());
            }
        }
        protected void OnFindShortestPath()
        {
            if (_OnFindShortestPath != null)
            {
                _OnFindShortestPath.Invoke(this, new EventArgs());
            }
        }
        protected void OnResetGraph()
        {
            if (_OnReset != null)
            {
                _OnReset.Invoke(this, new EventArgs());
            }
        }
    }
}
