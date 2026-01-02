using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Windo
{
    public partial class Form1 : Form
    {

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        // Classes for Vertex and Edge
        private class Vertex
        {
            public string Name { get; set; }
            public bool Accessible { get; set; }
            public override string ToString() => Name + (Accessible ? " (Достъпно)" : " (Недостъпно)");
        }

        private class Edge
        {
            public Vertex From { get; set; }
            public Vertex To { get; set; }
            public double Distance { get; set; }
            public double Time { get; set; }
            public double Price { get; set; }
            public bool Accessible { get; set; }
            public override string ToString() =>
                $"{From.Name} - {To.Name} | Разстояние: {Distance} км, Време: {Time} мин, Цена: {Price} €, {(Accessible ? "Достъпен" : "Недостъпен")}";
        }

        // Data collections
        private List<Vertex> vertices = new List<Vertex>();
        private List<Edge> edges = new List<Edge>();

        // Controls declaration
        private ComboBox cmbStart;
        private ComboBox cmbEnd;
        private ComboBox cmbCriteria;
        private Button btnFindPath;

        private ListBox lstVertices;
        private TextBox txtVertexName;
        private CheckBox chkVertexAccessible;
        private Button btnAddVertex;
        private Button btnEditVertex;
        private Button btnDeleteVertex;

        private ListBox lstEdges;
        private TextBox txtEdgeDistance;
        private TextBox txtEdgeTime;
        private TextBox txtEdgePrice;
        private CheckBox chkEdgeAccessible;
        private Button btnAddEdge;
        private Button btnEditEdge;
        private Button btnDeleteEdge;

        private TextBox txtPathResult;
        private Button btnSave;
        private Button btnLoad;

        private Label lblStart;
        private Label lblEnd;
        private Label lblCriteria;
        private Label lblVertices;
        private Label lblEdges;
        private Label lblVertexName;
        private Label lblEdgeDistance;
        private Label lblEdgeTime;
        private Label lblEdgePrice;

        public Form1()
        {
            InitializeComponent();
            SetUpControls();
            WireEvents();
        }

        public enum RouteCriteria
        {
            Distance,
            Time,
            Price
        }

        private void SetUpControls()
        {
            this.Text = "Справочник на пътешественика";
            this.ClientSize = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Top panel controls
            lblStart = new Label() { Text = "Начална точка:", Location = new Point(10, 15), AutoSize = true };
            cmbStart = new ComboBox() { Location = new Point(110, 12), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            lblEnd = new Label() { Text = "Крайна точка:", Location = new Point(280, 15), AutoSize = true };
            cmbEnd = new ComboBox() { Location = new Point(370, 12), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            lblCriteria = new Label() { Text = "Критерий:", Location = new Point(540, 15), AutoSize = true };
            cmbCriteria = new ComboBox() { Location = new Point(600, 12), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCriteria.Items.AddRange(new string[] { "Разстояние", "Време", "Цена" });
            cmbCriteria.SelectedIndex = 0;

            btnFindPath = new Button() { Text = "Намери маршрут", Location = new Point(770, 10), Size = new Size(110, 25) };

            // Left panel (Vertices)
            lblVertices = new Label() { Text = "Населени места", Location = new Point(10, 50), Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), AutoSize = true };
            lstVertices = new ListBox() { Location = new Point(10, 75), Size = new Size(250, 300) };

            lblVertexName = new Label() { Text = "Име:", Location = new Point(10, 390), AutoSize = true };
            txtVertexName = new TextBox() { Location = new Point(50, 387), Width = 210 };

            chkVertexAccessible = new CheckBox() { Text = "Достъпно", Location = new Point(10, 420), AutoSize = true };

            btnAddVertex = new Button() { Text = "Добави", Location = new Point(10, 450), Size = new Size(75, 30) };
            btnEditVertex = new Button() { Text = "Редактирай", Location = new Point(95, 450), Size = new Size(75, 30) };
            btnDeleteVertex = new Button() { Text = "Изтрий", Location = new Point(180, 450), Size = new Size(75, 30) };

            // Right panel (Edges)
            lblEdges = new Label() { Text = "Пътища", Location = new Point(280, 50), Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), AutoSize = true };
            lstEdges = new ListBox() { Location = new Point(280, 75), Size = new Size(600, 300) };

            lblEdgeDistance = new Label() { Text = "Разстояние (км):", Location = new Point(280, 390), AutoSize = true };
            txtEdgeDistance = new TextBox() { Location = new Point(380, 387), Width = 150 };

            lblEdgeTime = new Label() { Text = "Време (мин):", Location = new Point(280, 420), AutoSize = true };
            txtEdgeTime = new TextBox() { Location = new Point(380, 417), Width = 150 };

            lblEdgePrice = new Label() { Text = "Цена (€):", Location = new Point(280, 450), AutoSize = true };
            txtEdgePrice = new TextBox() { Location = new Point(380, 447), Width = 150 };

            chkEdgeAccessible = new CheckBox() { Text = "Достъпен", Location = new Point(280, 480), AutoSize = true };

            btnAddEdge = new Button() { Text = "Добави", Location = new Point(280, 510), Size = new Size(75, 30) };
            btnEditEdge = new Button() { Text = "Редактирай", Location = new Point(365, 510), Size = new Size(75, 30) };
            btnDeleteEdge = new Button() { Text = "Изтрий", Location = new Point(450, 510), Size = new Size(75, 30) };

            // Bottom panel (Path result and save/load)
            txtPathResult = new TextBox()
            {
                Location = new Point(10, 550),
                Size = new Size(870, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font(FontFamily.GenericMonospace, 9)
            };

            btnSave = new Button() { Text = "Запази", Location = new Point(700, 660), Size = new Size(75, 30) };
            btnLoad = new Button() { Text = "Зареди", Location = new Point(785, 660), Size = new Size(75, 30) };

            this.Controls.AddRange(new Control[]
            {
                lblStart, cmbStart, lblEnd, cmbEnd, lblCriteria, cmbCriteria, btnFindPath,
                lblVertices, lstVertices, lblVertexName, txtVertexName, chkVertexAccessible, btnAddVertex, btnEditVertex, btnDeleteVertex,
                lblEdges, lstEdges, lblEdgeDistance, txtEdgeDistance, lblEdgeTime, txtEdgeTime, lblEdgePrice, txtEdgePrice, chkEdgeAccessible, btnAddEdge, btnEditEdge, btnDeleteEdge,
                txtPathResult, btnSave, btnLoad
            });
        }

        private void WireEvents()
        {
            btnAddVertex.Click += BtnAddVertex_Click;
            btnEditVertex.Click += BtnEditVertex_Click;
            btnDeleteVertex.Click += BtnDeleteVertex_Click;

            btnAddEdge.Click += BtnAddEdge_Click;
            btnEditEdge.Click += BtnEditEdge_Click;
            btnDeleteEdge.Click += BtnDeleteEdge_Click;

            lstVertices.SelectedIndexChanged += LstVertices_SelectedIndexChanged;
            lstEdges.SelectedIndexChanged += LstEdges_SelectedIndexChanged;

            btnSave.Click += BtnSave_Click;
            btnLoad.Click += BtnLoad_Click;

            btnFindPath.Click += BtnFindPath_Click;
        }

        private void BtnAddVertex_Click(object sender, EventArgs e)
        {
            string name = txtVertexName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Въведете име на населено място.");
                return;
            }
            if (vertices.Any(ve => ve.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Населеното място вече съществува.");
                return;
            }

            Vertex v = new Vertex { Name = name, Accessible = chkVertexAccessible.Checked };
            vertices.Add(v);
            UpdateVertices();
            txtVertexName.Clear();
            chkVertexAccessible.Checked = false;
        }

        private void BtnEditVertex_Click(object sender, EventArgs e)
        {
            if (lstVertices.SelectedItem == null)
            {
                MessageBox.Show("Изберете населено място за редактиране.");
                return;
            }

            Vertex selected = (Vertex)lstVertices.SelectedItem;
            string newName = txtVertexName.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Въведете име на населено място.");
                return;
            }
            if (vertices.Any(v => v.Name.Equals(newName, StringComparison.OrdinalIgnoreCase) && v != selected))
            {
                MessageBox.Show("Населеното място вече съществува.");
                return;
            }

            // Update vertex
            selected.Name = newName;
            selected.Accessible = chkVertexAccessible.Checked;
            UpdateVertices();
            UpdateEdges();
            txtVertexName.Clear();
            chkVertexAccessible.Checked = false;
        }

        private void BtnDeleteVertex_Click(object sender, EventArgs e)
        {
            if (lstVertices.SelectedItem == null)
            {
                MessageBox.Show("Изберете населено място за изтриване.");
                return;
            }

            Vertex selected = (Vertex)lstVertices.SelectedItem;

            // Remove edges connected to this vertex
            edges.RemoveAll(edge => edge.From == selected || edge.To == selected);

            vertices.Remove(selected);
            UpdateVertices();
            UpdateEdges();
            txtVertexName.Clear();
            chkVertexAccessible.Checked = false;
        }

        private void BtnAddEdge_Click(object sender, EventArgs e)
        {
            if (cmbStart.SelectedItem == null || cmbEnd.SelectedItem == null)
            {
                MessageBox.Show("Изберете начална и крайна точка.");
                return;
            }
            Vertex start = (Vertex)cmbStart.SelectedItem;
            Vertex end = (Vertex)cmbEnd.SelectedItem;
            if (start == end)
            {
                MessageBox.Show("Началната и крайната точка не могат да са еднакви.");
                return;
            }
            if (!double.TryParse(txtEdgeDistance.Text, out double distance) || distance <= 0)
            {
                MessageBox.Show("Въведете валидно положително разстояние.");
                return;
            }
            if (!double.TryParse(txtEdgeTime.Text, out double time) || time <= 0)
            {
                MessageBox.Show("Въведете валидно положително време.");
                return;
            }
            if (!double.TryParse(txtEdgePrice.Text, out double price) || price < 0)
            {
                MessageBox.Show("Въведете валидна цена (0 или по-голяма).");
                return;
            }
            bool accessible = chkEdgeAccessible.Checked;

            // Check if edge already exists (undirected)
            if (edges.Any(el => (el.From == start && el.To == end) || (el.From == end && el.To == start)))
            {
                MessageBox.Show("Този път вече съществува.");
                return;
            }

            Edge edge = new Edge
            {
                From = start,
                To = end,
                Distance = distance,
                Time = time,
                Price = price,
                Accessible = accessible
            };
            edges.Add(edge);
            UpdateEdges();
            ClearEdgeInput();
        }

        private void BtnEditEdge_Click(object sender, EventArgs e)
        {
            if (lstEdges.SelectedItem == null)
            {
                MessageBox.Show("Изберете път за редактиране.");
                return;
            }

            Edge selected = (Edge)lstEdges.SelectedItem;

            if (!double.TryParse(txtEdgeDistance.Text, out double distance) || distance <= 0)
            {
                MessageBox.Show("Въведете валидно положително разстояние.");
                return;
            }
            if (!double.TryParse(txtEdgeTime.Text, out double time) || time <= 0)
            {
                MessageBox.Show("Въведете валидно положително време.");
                return;
            }
            if (!double.TryParse(txtEdgePrice.Text, out double price) || price < 0)
            {
                MessageBox.Show("Въведете валидна цена (0 или по-голяма).");
                return;
            }
            bool accessible = chkEdgeAccessible.Checked;

            selected.Distance = distance;
            selected.Time = time;
            selected.Price = price;
            selected.Accessible = accessible;

            UpdateEdges();
            ClearEdgeInput();
        }

        private void BtnDeleteEdge_Click(object sender, EventArgs e)
        {
            if (lstEdges.SelectedItem == null)
            {
                MessageBox.Show("Изберете път за изтриване.");
                return;
            }

            Edge selected = (Edge)lstEdges.SelectedItem;
            edges.Remove(selected);
            UpdateEdges();
            ClearEdgeInput();
        }

        private void LstVertices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVertices.SelectedItem == null) return;
            Vertex v = (Vertex)lstVertices.SelectedItem;
            txtVertexName.Text = v.Name;
            chkVertexAccessible.Checked = v.Accessible;
        }

        private void LstEdges_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEdges.SelectedItem == null) return;
            Edge el = (Edge)lstEdges.SelectedItem;
            txtEdgeDistance.Text = el.Distance.ToString();
            txtEdgeTime.Text = el.Time.ToString();
            txtEdgePrice.Text = el.Price.ToString();
            chkEdgeAccessible.Checked = el.Accessible;

            // Select the vertices in the comboboxes
            cmbStart.SelectedItem = el.From;
            cmbEnd.SelectedItem = el.To;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("data.txt"))
                {
                    // Save vertices
                    sw.WriteLine(vertices.Count);
                    foreach (Vertex v in vertices)
                    {
                        // Format: Name|Accessible (1 or 0)
                        sw.WriteLine(v.Name + "|" + (v.Accessible ? "1" : "0"));
                    }

                    // Save edges
                    sw.WriteLine(edges.Count);
                    foreach (Edge edge in edges)
                    {
                        // Format: FromName|ToName|Distance|Time|Price|Accessible
                        sw.WriteLine(
                            edge.From.Name + "|" +
                            edge.To.Name + "|" +
                            edge.Distance.ToString() + "|" +
                            edge.Time.ToString() + "|" +
                            edge.Price.ToString() + "|" +
                            (edge.Accessible ? "1" : "0")
                        );
                    }
                }

                MessageBox.Show("Данните са запазени успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при запис: " + ex.Message);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists("data.txt"))
                {
                    MessageBox.Show("Файлът с данни не съществува.");
                    return;
                }

                using (StreamReader sr = new StreamReader("data.txt"))
                {
                    vertices.Clear();
                    edges.Clear();

                    int vertexCount = int.Parse(sr.ReadLine());
                    for (int i = 0; i < vertexCount; i++)
                    {
                        string line = sr.ReadLine();
                        string[] parts = line.Split('|');
                        if (parts.Length < 2) continue;

                        Vertex v = new Vertex();
                        v.Name = parts[0];
                        v.Accessible = parts[1] == "1";
                        vertices.Add(v);
                    }

                    int edgeCount = int.Parse(sr.ReadLine());
                    for (int i = 0; i < edgeCount; i++)
                    {
                        string line = sr.ReadLine();
                        string[] parts = line.Split('|');
                        if (parts.Length < 6) continue;

                        Vertex from = null;
                        Vertex to = null;

                        foreach (Vertex v in vertices)
                        {
                            if (v.Name == parts[0]) from = v;
                            if (v.Name == parts[1]) to = v;
                        }

                        if (from == null || to == null) continue;

                        Edge edge = new Edge();
                        edge.From = from;
                        edge.To = to;
                        edge.Distance = double.Parse(parts[2]);
                        edge.Time = double.Parse(parts[3]);
                        edge.Price = double.Parse(parts[4]);
                        edge.Accessible = parts[5] == "1";

                        edges.Add(edge);
                    }
                }

                UpdateVertices();
                UpdateEdges();
                ClearVertexInput();
                ClearEdgeInput();

                MessageBox.Show("Данните са заредени успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при зареждане: " + ex.Message);
            }
        }


        private void BtnFindPath_Click(object sender, EventArgs e)
        {
            // Validate selections
            if (cmbStart.SelectedItem == null || cmbEnd.SelectedItem == null)
            {
                MessageBox.Show("Моля, изберете начална и крайна точка."); // Please select start and end points.
                return;
            }

            Vertex start = (Vertex)cmbStart.SelectedItem;
            Vertex end = (Vertex)cmbEnd.SelectedItem;

            if (start == end)
            {
                MessageBox.Show("Началната и крайната точка не могат да са еднакви."); // Start and end cannot be the same.
                return;
            }

            if (!start.Accessible || !end.Accessible)
            {
                MessageBox.Show("Началната и/или крайната точка не са достъпни."); // Start and/or end are not accessible.
                return;
            }

            // Get criteria based on selected index
            RouteCriteria criteria = RouteCriteria.Distance; // default

            switch (cmbCriteria.SelectedIndex)
            {
                case 0:
                    criteria = RouteCriteria.Distance;
                    break;
                case 1:
                    criteria = RouteCriteria.Time;
                    break;
                case 2:
                    criteria = RouteCriteria.Price;
                    break;
                default:
                    criteria = RouteCriteria.Distance;
                    break;
            }

            // Find shortest path
            PathResult result = FindShortestPath(start, end, criteria);

            if (result.Path == null || result.Path.Count == 0)
            {
                txtPathResult.Text = "Няма достъпен маршрут между избраните точки."; // No available route.
            }
            else
            {
                System.Text.StringBuilder routeBuilder = new System.Text.StringBuilder();
                for (int i = 0; i < result.Path.Count; i++)
                {
                    routeBuilder.Append(result.Path[i].Name);
                    if (i < result.Path.Count - 1)
                        routeBuilder.Append(" -> ");
                }

                // labels for criteria
                string criteriaLabel = "разстояние"; 
                switch (criteria)
                {
                    case RouteCriteria.Distance:
                        criteriaLabel = "разстояние";
                        break;
                    case RouteCriteria.Time:
                        criteriaLabel = "време";
                        break;
                    case RouteCriteria.Price:
                        criteriaLabel = "цена";
                        break;
                }

                txtPathResult.Text = string.Format("Маршрут: {0}\r\nОбщо {1}: {2:F2}",
                    routeBuilder.ToString(), criteriaLabel, result.TotalCost);
            }
        }

        private List<Edge> GetAccessibleEdges(Vertex v)
        {
            return edges
                .Where(e => (e.From == v || e.To == v) && e.Accessible && e.From.Accessible && e.To.Accessible)
                .ToList();
        }

        private double GetEdgeCost(Edge edge, RouteCriteria criteria)
        {
            switch (criteria)
            {
                case RouteCriteria.Distance:
                    return edge.Distance;
                case RouteCriteria.Time:
                    return edge.Time;
                case RouteCriteria.Price:
                    return edge.Price;
                default:
                    return edge.Distance;
            }
        }


        private class PathResult
        {
            public List<Vertex> Path { get; set; }
            public double TotalCost { get; set; }

            public PathResult()
            {
                Path = new List<Vertex>();
                TotalCost = double.PositiveInfinity;
            }
        }

        // The main Dijkstra algorithm method
        private PathResult FindShortestPath(Vertex start, Vertex end, RouteCriteria criteria)
        {
            Dictionary<Vertex, double> distances = new Dictionary<Vertex, double>();
            Dictionary<Vertex, Vertex> previous = new Dictionary<Vertex, Vertex>();
            HashSet<Vertex> unvisited = new HashSet<Vertex>();

            // Initialize unvisited with accessible vertices
            foreach (Vertex v in vertices)
            {
                if (v.Accessible)
                    unvisited.Add(v);
            }

            // Initialize distances and previous dictionaries
            foreach (Vertex v in unvisited)
            {
                distances[v] = double.PositiveInfinity;
                previous[v] = null;
            }
            distances[start] = 0;

            while (unvisited.Count > 0)
            {
                // Find the unvisited vertex with the smallest distance
                Vertex current = null;
                double smallestDist = double.PositiveInfinity;
                foreach (Vertex v in unvisited)
                {
                    if (distances[v] < smallestDist)
                    {
                        smallestDist = distances[v];
                        current = v;
                    }
                }

                if (current == null || current == end || distances[current] == double.PositiveInfinity)
                    break;

                unvisited.Remove(current);

                // Check neighbors of current vertex
                List<Edge> edgesFromCurrent = GetAccessibleEdges(current);
                foreach (Edge edge in edgesFromCurrent)
                {
                    Vertex neighbor = (edge.From == current) ? edge.To : edge.From;
                    if (!unvisited.Contains(neighbor))
                        continue;

                    double cost = GetEdgeCost(edge, criteria);
                    double alt = distances[current] + cost;
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }

            // Reconstruct path from end to start
            List<Vertex> pathList = new List<Vertex>();
            Vertex temp = end;
            if (previous.ContainsKey(temp) && (previous[temp] != null || temp == start))
            {
                while (temp != null)
                {
                    pathList.Insert(0, temp);
                    if (previous.ContainsKey(temp))
                        temp = previous[temp];
                    else
                        temp = null;
                }
            }

            PathResult result = new PathResult();
            result.Path = pathList;
            if (distances.ContainsKey(end))
                result.TotalCost = distances[end];

            // If no path found, set Path to null
            if (double.IsPositiveInfinity(result.TotalCost))
                result.Path = null;

            return result;
        }


        private void UpdateVertices()
        {
            lstVertices.DataSource = null;
            lstVertices.DataSource = vertices.OrderBy(v => v.Name).ToList();

            cmbStart.DataSource = null;
            cmbStart.DataSource = vertices.OrderBy(v => v.Name).ToList();

            cmbEnd.DataSource = null;
            cmbEnd.DataSource = vertices.OrderBy(v => v.Name).ToList();
        }

        private void UpdateEdges()
        {
            lstEdges.DataSource = null;
            lstEdges.DataSource = edges.OrderBy(e => e.From.Name).ThenBy(e => e.To.Name).ToList();
        }

        private void ClearVertexInput()
        {
            txtVertexName.Clear();
            chkVertexAccessible.Checked = false;
            lstVertices.ClearSelected();
        }

        private void ClearEdgeInput()
        {
            txtEdgeDistance.Clear();
            txtEdgeTime.Clear();
            txtEdgePrice.Clear();
            chkEdgeAccessible.Checked = false;
            lstEdges.ClearSelected();
            cmbStart.SelectedIndex = -1;
            cmbEnd.SelectedIndex = -1;
        }
    }
}
