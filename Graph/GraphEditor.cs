using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.VisualBasic;

namespace Graph
{
    public class GraphEditor : Control
    {
        /// <summary>
        /// Конструктор компонента
        /// </summary>
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
            _SoundsOn = true;
            vertexCoordinates = new List<VertexCoordinatesEdge>();
            edgeCoordinates = new List<EdgeCoordinates>();
            InitSounds();
        }

        //внутренние переменные
        private MyGraph _graph;
        private GraphShortestPath _shortestPath;
        private List<VertexCoordinatesEdge> vertexCoordinates;
        private List<EdgeCoordinates> edgeCoordinates;
        private System.Media.SoundPlayer addVertexOrEdge;
        private System.Media.SoundPlayer changeState;
        private System.Media.SoundPlayer findAction;
        private System.Media.SoundPlayer prohibitionOfAction;
        private System.Media.SoundPlayer resetAction;
        private int _selectedVertex = -1;
        private int startNodeIndex = -1;
        protected int _VertexSize;
        protected int _VertexCount;

        //свойства
        protected Color _VertexColor;
        protected Color _EdgeColor;
        protected Color _TextColor;
        protected Color _DarkColor;
        protected Color _LightColor;
        protected Color _ShortestPathColor;
        private ObjStates _ObjState;
        protected bool _IsVertexAddMode;
        protected bool _SoundsOn;
        protected bool _IsEdgeAddMode;
        protected bool _IsDeleteMode;

        public enum ObjStates
        {
            osConvex,
            osConcavity
        }

        public virtual bool IsVertexAddMode
        {
            get { return _IsVertexAddMode; }
            set
            {
                if (_IsVertexAddMode != value)
                {
                    _IsVertexAddMode = value;
                    _IsEdgeAddMode = false;
                    _IsDeleteMode = false;
                }
            }
        }
        public virtual bool IsEdgeAddMode
        {
            get { return _IsEdgeAddMode; }
            set
            {
                if (_IsEdgeAddMode != value)
                {
                    _IsEdgeAddMode = value;
                    _IsVertexAddMode = false;
                    _IsDeleteMode = false;
                }
            }
        }

        public virtual bool IsDeleteMode
        {
            get { return _IsDeleteMode; }
            set
            {
                if (_IsDeleteMode != value)
                {
                    _IsDeleteMode = value;
                    _IsVertexAddMode = false;
                    _IsEdgeAddMode = false;
                }
            }
        }

        public virtual bool SoundsOn
        {
            get { return _SoundsOn; }
            set
            {
                if (_SoundsOn != value)
                {
                    _SoundsOn = value;
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

        public virtual Color VertexColor
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

        public virtual Color ShortestPathColor
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

        public virtual Color EdgeColor
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

        public virtual Color TextColor
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

        public virtual Color DarkColor
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

        public virtual Color LightColor
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

        /// <summary>
        /// Метод сброса графа
        /// </summary>
        public virtual void Reset()
        {
            _graph = new MyGraph();
            _shortestPath = new GraphShortestPath(_graph);
            vertexCoordinates = new List<VertexCoordinatesEdge>();
            edgeCoordinates = new List<EdgeCoordinates>();
            _VertexCount = 0;
            OnResetGraph();
            if (SoundsOn)
                resetAction.Play();
            Invalidate();
        }

        /// <summary>
        /// Метод добавления грани с графа
        /// </summary>
        public virtual void addEdge(string src, string dst, int weight)
        {
            if (src == dst)
            {
                if (SoundsOn) prohibitionOfAction.Play();
                return;
            }
            if (_graph.FindVertex(src) != null && _graph.FindVertex(dst) != null)
            {
                lock (edgeCoordinates)
                {
                    {
                        foreach (EdgeCoordinates edge in edgeCoordinates)
                        {

                            if ((edge.src == src && edge.dst == dst) || (edge.src == dst && edge.dst == src))
                            {
                                if (SoundsOn)
                                    prohibitionOfAction.Play();
                                return;
                            }
                        }
                    }
                    int x1 = 0;
                    int x2 = 0;
                    int y1 = 0;
                    int y2 = 0;
                    foreach (VertexCoordinatesEdge vertex in vertexCoordinates)
                    {
                        if (vertex.name == src) { x1 = vertex.x; y1 = vertex.y; }
                        if (vertex.name == dst) { x2 = vertex.x; y2 = vertex.y; }
                    }
                    edgeCoordinates.Add(new EdgeCoordinates(x1, x2, y1, y2, src, dst, weight.ToString(), false));
                    _graph.AddEdge(src, dst, weight);
                    if (SoundsOn)
                        addVertexOrEdge.Play();
                    OnEdgeAdd();
                }
                Invalidate();
            }
        }

        /// <summary>
        /// Метод удаления грани на граф
        /// <param name="src">имя первой вершины</param>
        /// <param name="dst">имя второй вершины</param>
        /// </summary>
        public virtual void removeEdge(string src, string dst)
        {
            if (src == dst)
            {
                if (SoundsOn) prohibitionOfAction.Play();
                return;
            }
            if (src != null && dst != null)
            {
                EdgeCoordinates buf = null;
                for (int i = 0; i <= edgeCoordinates.Count; i++)
                {
                    EdgeCoordinates edge = edgeCoordinates[i];
                    if ((edge.src == src && edge.dst == dst) || (edge.src == dst && edge.dst == src))
                    {
                        edgeCoordinates.RemoveAt(i);
                        if (SoundsOn)
                            resetAction.Play();
                        break;
                    }
                }
                //edgeCoordinates.Remove(buf);
                ResetShortestPath();
                _graph.RemoveEdge(src, dst);
                Invalidate();
            }
        }


        /// <summary>
        /// Метод удаления вершины с графа
        /// <param name="vertexName">имя вершины</param>
        /// </summary>
        public virtual void removeVertex(string vertexName)
        {
            removeEdgesForVertex(vertexName);
            var vertex = _graph.FindVertex(vertexName);
            if (vertex == null)
            {
                if (SoundsOn)
                    prohibitionOfAction.Play();
                return;
            }
            VertexCoordinatesEdge ver = null;
            foreach (VertexCoordinatesEdge v in vertexCoordinates)
            {
                if (v.name == vertexName)
                {
                    ver = v; break;
                }
            }
            vertexCoordinates.Remove(ver);
            _graph.RemoveVertex(vertex);
            if (SoundsOn)
                resetAction.Play();
            ResetShortestPath();
            Invalidate();
        }

        /// <summary>
        /// Метод поиска кратчайшего пути в графе
        /// </summary>
        public virtual string FindShortestPath(string src, string dst)
        {
            ResetShortestPath();
            _shortestPath = new GraphShortestPath(_graph);
            var vertices = _shortestPath.FindShortestPath(src, dst);
            SetShortestPath(vertices);
            if (vertices.Count > 0) OnFindShortestPath();
            Invalidate();
            string buf = "";
            foreach (string vertex in vertices)
            {
                if (buf != "") buf += ", ";
                buf += vertex;
            }
            if (SoundsOn)
                if (buf != "") findAction.Play(); else prohibitionOfAction.Play();
            return buf;
        }
        
        /// <summary>
        /// Метод смены состояний полотна
        /// </summary>
        public virtual void State()
        {
            int I = (int)_ObjState + 1;
            I = I > 1 ? 0 : I;
            ObjState = (ObjStates)I;
            if (SoundsOn)
                changeState.Play();
            OnChangeState();
        }

        /// <summary>
        /// Метод инициализации звуков
        /// </summary>
        protected virtual void InitSounds()
        {
            var exePath = Environment.CurrentDirectory;
            addVertexOrEdge = new System.Media.SoundPlayer(Path.Combine(exePath, @"sounds\", "addVertexOrEdge.wav"));
            changeState = new System.Media.SoundPlayer(Path.Combine(exePath, @"sounds\", "changeState.wav"));
            findAction = new System.Media.SoundPlayer(Path.Combine(exePath, @"sounds\", "findAction.wav"));
            prohibitionOfAction = new System.Media.SoundPlayer(Path.Combine(exePath, @"sounds\", "prohibitionOfAction.wav"));
            resetAction = new System.Media.SoundPlayer(Path.Combine(exePath, @"sounds\", "reset.wav"));
        }

        /// <summary>
        /// Метод добавления вершины на граф
        /// </summary>
        /// <param name="x">x координата вершины</param>
        /// <param name="y">y координата вершины</param>
        protected virtual void addVertex(int x, int y)
        {
            if ((x <= Math.Min(Height, Width) / 15) || (x >= Width - Math.Min(Height, Width) / 15) || (y <= Math.Min(Height, Width) / 15) || (y >= Height - Math.Min(Height, Width) / 15))
            {
                if (SoundsOn)
                    prohibitionOfAction.Play();
                return;
            }
            foreach (VertexCoordinatesEdge v in vertexCoordinates)
            {
                if (((x <= v.x + _VertexSize && x >= v.x - _VertexSize) && (y <= v.y + _VertexSize && y >= v.y - _VertexSize)))
                {
                    if (SoundsOn)
                        prohibitionOfAction.Play();
                    return;
                }
            }
            _VertexCount++;
            vertexCoordinates.Add(new VertexCoordinatesEdge(_VertexCount.ToString(), x, y, false));
            _graph.AddVertex(_VertexCount.ToString());
            OnVertexAdd();
            //_IsVertexAddMode = false;
            if (SoundsOn)
                addVertexOrEdge.Play();
            Invalidate();
        }

        /// <summary>
        /// Метод удаления всех граней для вершины
        /// <param name="src">имя вершины</param>
        /// </summary>
        protected virtual void removeEdgesForVertex(string src)
        {
            if (_graph.FindVertex(src) != null)
            {
                for (int i = 0; i <= _graph.FindVertex(src).edges.Count; i++)
                {
                    try
                    {
                        var buf = edgeCoordinates;
                        foreach (EdgeCoordinates edge in edgeCoordinates)
                        {
                            if (edge.src == src || edge.dst == src)
                            {
                                buf.Remove(edge);
                            }
                        }
                        edgeCoordinates = buf;
                        Invalidate();
                        _graph.RemoveEdge(src, "");
                    }
                    catch (Exception e) { }
                }
            }
        }

        /// <summary>
        /// Метод сброса флагов кратчайшего пути для вершин и граней
        /// </summary>
        protected virtual void ResetShortestPath()
        {
            lock (vertexCoordinates)
            {
                foreach (var ver in vertexCoordinates)
                {
                    ver.isShortestPath = false;
                }
            }
            lock (edgeCoordinates)
            {
                foreach (var e in edgeCoordinates)
                {
                    e.isShortestPath = false;
                }
            }
        }

        /// <summary>
        /// Метод установки флагов кратчайшего пути для вершин и граней
        /// </summary>
        protected virtual void SetShortestPath(List<string> vertices)
        {
            foreach (var vertexName in vertices)
            {
                foreach (var ver in vertexCoordinates)
                {
                    if (ver.name == vertexName)
                    {
                        ver.isShortestPath = true;
                    }
                }
            }
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                foreach (var e in edgeCoordinates)
                {
                    if ((e.src == vertices[i] && e.dst == vertices[i + 1]) || (e.dst == vertices[i] && e.src == vertices[i + 1]))
                    {
                        e.isShortestPath = true;
                    }
                }
            }
            Invalidate();
        }

        /// <summary>
        /// Метод масштабирования полотна
        /// </summary>
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
            _VertexSize = 20;
            base.SetBoundsCore(x, y, width, height, specified);
            Invalidate();
        }

        /// <summary>
        /// Метод отрисовки полотна
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Brush VertexBrush = new SolidBrush(_VertexColor);
            Brush ShortestPathBrush = new SolidBrush(_ShortestPathColor);
            Pen ShortestPathPen = new Pen(_ShortestPathColor, 5);
            Pen EdgePen = new Pen(_EdgeColor, 5);
            Pen LightPen = new Pen(_LightColor);
            Pen DarkPen = new Pen(_DarkColor);
            int FontSize = (int)Math.Round((_VertexSize - 16) * 92 / e.Graphics.DpiY);
            if (FontSize <= 12)
            {
                FontSize = 12;
            }
            Font font = new Font("Arial", FontSize, FontStyle.Bold);
            SolidBrush TextBrush = new SolidBrush(_TextColor);
            StringFormat Fmt = new StringFormat();
            Fmt.LineAlignment = StringAlignment.Center;
            Fmt.Alignment = StringAlignment.Center;
            DrawWindowState(e, DarkPen, LightPen);
            DrawEdges(e, EdgePen, ShortestPathPen, font, TextBrush, Fmt);
            DrawVertices(e, VertexBrush, ShortestPathBrush, font, TextBrush, Fmt);

            base.OnPaint(e);
        }

        /// <summary>
        /// Метод отрисовки выпуклостей/вогнутостей
        /// </summary>
        /// <param name="e">Графика</param>
        /// <param name="DarkPen">Тёмный карандаш</param>
        /// <param name="LightPen">Светлый карандаш</param>
        protected virtual void DrawWindowState(PaintEventArgs e, Pen DarkPen, Pen LightPen)
        {
            for (int i = 0; i < Math.Min(Height, Width) / 15; i++)
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
        }

        /// <summary>
        /// Метод отрисовки граней
        /// </summary>
        /// <param name="e">Графика</param>
        /// <param name="EdgePen">Карандаш для отрисовки граней</param>
        /// <param name="ShortestPathPen">Карандаш для отрисовки граней кратчайшего пути</param>
        /// <param name="font">Шрифт</param>
        /// <param name="TextBrush">Кисть для отрисовки текста</param>
        /// <param name="Fmt">Формат строки</param>
        protected virtual void DrawEdges(PaintEventArgs e, Pen EdgePen, Pen ShortestPathPen, Font font, SolidBrush TextBrush, StringFormat Fmt)
        {
            Fmt.LineAlignment = StringAlignment.Center;
            foreach (var edge in edgeCoordinates)
            {
                if (edge != null)
                {
                    if (edge.x1 - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        edge.x1 = Math.Min(Height, Width) / 15 + _VertexSize / 2;
                    }
                    else
                    {
                        if (edge.x1 + _VertexSize / 2 > Width - Math.Min(Height, Width) / 15)
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
                        if (edge.y1 + _VertexSize / 2 > Height - Math.Min(Height, Width) / 15)
                        {
                            edge.y1 = Height - Math.Min(Height, Width) / 15 - _VertexSize / 2;
                        }
                    }
                    if (edge.x2 - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        edge.x2 = Math.Min(Height, Width) / 15 + _VertexSize / 2;
                    }
                    else
                    {
                        if (edge.x2 + _VertexSize / 2 > Width - Math.Min(Height, Width) / 15)
                        {
                            edge.x2 = Width - Math.Min(Height, Width) / 15 - _VertexSize / 2;
                        }
                    }
                    if (edge.y2 - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        edge.y2 = Math.Min(Height, Width) / 15 + _VertexSize / 2;
                    }
                    else
                    {
                        if (edge.y2 + _VertexSize / 2 > Height - Math.Min(Height, Width) / 15)
                        {
                            edge.y2 = Height - _VertexSize / 2 - Math.Min(Height, Width) / 15;
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

        /// <summary>
        /// Метод отрисовки граней
        /// </summary>
        /// <param name="e">Графика</param>
        /// <param name="VertexBrush">Кисть для отрисовки вершин</param>
        /// <param name="ShortestPathBrush">Кисть для отрисовки вершин кратчайшего пути</param>
        /// <param name="font">Шрифт</param>
        /// <param name="TextBrush">Кисть для отрисовки текста</param>
        /// <param name="Fmt">Формат строки</param>
        protected virtual void DrawVertices(PaintEventArgs e, Brush VertexBrush, Brush ShortestPathBrush, Font font, SolidBrush TextBrush, StringFormat Fmt)
        {
            Fmt.LineAlignment = StringAlignment.Center;
            foreach (var vertex in vertexCoordinates)
            {
                if (vertex != null)
                {
                    if (vertex.x - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        vertex.x = Math.Min(Height, Width) / 15 + _VertexSize / 2;
                    }
                    else
                    {
                        if (vertex.x + _VertexSize / 2 > Width - Math.Min(Height, Width) / 15)
                        {
                            vertex.x = Width - Math.Min(Height, Width) / 15 - _VertexSize / 2;
                        }
                    }
                    if (vertex.y - _VertexSize / 2 < Math.Min(Height, Width) / 15)
                    {
                        vertex.y = Math.Min(Height, Width) / 15 + _VertexSize / 2;
                    }
                    else
                    {
                        if (vertex.y + _VertexSize / 2 > Height - Math.Min(Height, Width) / 15)
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
                    RectangleF Rect = new RectangleF(vertex.x - _VertexSize, vertex.y - _VertexSize / 2, _VertexSize * 2, _VertexSize);
                    String S = vertex.name;
                    e.Graphics.DrawString(S, font, TextBrush, Rect, Fmt);
                }
            }
        }

        /// <summary>
        /// Слушатель нажатия кнопок мыши
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            if (e.Button == MouseButtons.Left)
            {
                if (_IsVertexAddMode)
                {
                    addVertex(x, y);
                }
                else
                {
                    if (_IsEdgeAddMode)
                    {
                        edgeAddByMouse(e);
                    }
                    else
                    {
                        if (_IsDeleteMode)
                        {
                            deleteMode(e);
                        }
                    }
                }
            }
            
            base.OnMouseDown(e);
            Invalidate();
        }

        //Метод удаления ребра или вершины мышкой
        private void deleteMode(MouseEventArgs e)
        {
            // Проверяем, попали ли мы в какой-либо элемент (вершину или ребро)
            for (int i = 0; i < vertexCoordinates.Count; i++)
            {
                Rectangle nodeBounds = new Rectangle(vertexCoordinates[i].x - 10, vertexCoordinates[i].y - 10, 20, 20);
                if (nodeBounds.Contains(e.Location))
                {
                    // Подсвечиваем выбранный элемент
                    _selectedVertex = i;
                    Invalidate(); // Перерисовываем компонент

                    // Отображаем предупреждение об удалении
                    DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту вершину и связанные с ней ребра?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // Удаляем вершину и связанные ребра
                        removeVertex(vertexCoordinates[i].name);
                    }
                    return;
                }
            }

            // Проверяем, попали ли мы в какое-либо ребро
            for (int i = 0; i < edgeCoordinates.Count; i++)
            {
                Tuple<Point, Point> edge;
                if (IsPointOnEdge(e.Location, edgeCoordinates[i].x1, edgeCoordinates[i].y1, edgeCoordinates[i].x2, edgeCoordinates[i].y2))
                {
                    // Отображаем предупреждение об удалении
                    DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить это ребро?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // Удаляем ребро
                        removeEdge(edgeCoordinates[i].src, edgeCoordinates[i].dst);
                    }
                    return;
                }
            }
        }

        // Метод для проверки, попадает ли точка на ребро
        private bool IsPointOnEdge(Point point, int x1, int y1, int x2, int y2)
        {
            const int tolerance = 3; // Допустимое отклонение от ребра

            float distance = DistancePointToLine(point, x1, y1, x2, y2);
            return distance <= tolerance;
        }

        // Метод для вычисления расстояния от точки до линии
        private float DistancePointToLine(Point point, int x1, int y1, int x2, int y2)
        {
            float a = point.X - x1;
            float b = point.Y - y1;
            float c = x2 - x1;
            float d = y2 - y1;

            float dot = a * c + b * d;
            float lenSq = c * c + d * d;
            float param = dot / lenSq;

            float xx, yy;

            if (param < 0)
            {
                xx = x1;
                yy = y1;
            }
            else if (param > 1)
            {
                xx = x2;
                yy = y2;
            }
            else
            {
                xx = x1 + param * c;
                yy = y1 + param * d;
            }

            float dx = point.X - xx;
            float dy = point.Y - yy;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        //метод добавления грани мышкой
        private void edgeAddByMouse(MouseEventArgs e)
        {
            // Проверяем, попали ли мы в какую-либо вершину
            for (int i = 0; i < vertexCoordinates.Count; i++)
            {
                Rectangle nodeBounds = new Rectangle(vertexCoordinates[i].x - 10, vertexCoordinates[i].y - 10, 20, 20);
                if (nodeBounds.Contains(e.Location))
                {
                    // Проверяем, является ли это начальной вершиной
                    if (_selectedVertex == -1)
                    {
                        // Выбираем начальную вершину и меняем ее цвет
                        startNodeIndex = i;
                        _selectedVertex = i;
                        Invalidate(); // Перерисовываем компонент
                    }
                    else
                    {
                        // Создаем ребро между начальной и конечной вершинами
                        int s = 0;
                        try
                        {
                            s = int.Parse(Interaction.InputBox("Введите вес: "));
                        }
                        catch (Exception ex)
                        {

                        }

                        addEdge(vertexCoordinates[startNodeIndex].name, vertexCoordinates[i].name, s);

                        // Сбрасываем начальную вершину
                        startNodeIndex = -1;
                        _selectedVertex = -1;
                        Invalidate(); // Перерисовываем компонент
                    }
                    return;
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams P = base.CreateParams;
                P.ExStyle = P.ExStyle | 0x02000000;
                return P;
            }
        }


        //События
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
