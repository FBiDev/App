using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using App.Core.Properties;

namespace App.Core.Desktop
{
    public partial class DataGridBase : DataGridView
    {
        #region Defaults
        [DefaultValue(false)]
        public new bool AllowUserToAddRows
        {
            get { return base.AllowUserToAddRows; }
            set { base.AllowUserToAddRows = value; }
        }

        [DefaultValue(false)]
        public new bool AllowUserToDeleteRows
        {
            get { return base.AllowUserToDeleteRows; }
            set { base.AllowUserToDeleteRows = value; }
        }

        [DefaultValue(true)]
        public new bool AllowUserToOrderColumns
        {
            get { return base.AllowUserToOrderColumns; }
            set { base.AllowUserToOrderColumns = value; }
        }

        [DefaultValue(typeof(AnchorStyles), "Top, Bottom, Left, Right")]
        public new AnchorStyles Anchor
        {
            get { return base.Anchor; }
            set { base.Anchor = value; }
        }

        [DefaultValue(false)]
        public new bool CausesValidation
        {
            get { return base.CausesValidation; }
            set { base.CausesValidation = value; }
        }

        [DefaultValue(typeof(BorderStyle), "None")]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        [DefaultValue(typeof(DataGridViewCellBorderStyle), "SingleHorizontal")]
        public new DataGridViewCellBorderStyle CellBorderStyle
        {
            get { return base.CellBorderStyle; }
            set { base.CellBorderStyle = value; }
        }

        [DefaultValue(typeof(DataGridViewHeaderBorderStyle), "Single")]
        public new DataGridViewHeaderBorderStyle ColumnHeadersBorderStyle
        {
            get { return base.ColumnHeadersBorderStyle; }
            set { base.ColumnHeadersBorderStyle = value; }
        }

        [DefaultValue(30)]
        public new int ColumnHeadersHeight
        {
            get { return base.ColumnHeadersHeight; }
            set { base.ColumnHeadersHeight = value; }
        }

        [DefaultValue(typeof(DataGridViewColumnHeadersHeightSizeMode), "DisableResizing")]
        public new DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        {
            get { return base.ColumnHeadersHeightSizeMode; }
            set { base.ColumnHeadersHeightSizeMode = value; }
        }

        [DefaultValue(false)]
        public new bool EnableHeadersVisualStyles
        {
            get { return base.EnableHeadersVisualStyles; }
            set { base.EnableHeadersVisualStyles = value; }
        }

        [DefaultValue(false)]
        public new bool MultiSelect
        {
            get { return base.MultiSelect; }
            set { base.MultiSelect = value; }
        }

        [DefaultValue(true)]
        public new bool ReadOnly
        {
            get { return base.ReadOnly; }
            set { base.ReadOnly = value; }
        }

        [DefaultValue(typeof(DataGridViewHeaderBorderStyle), "Single")]
        public new DataGridViewHeaderBorderStyle RowHeadersBorderStyle
        {
            get { return base.RowHeadersBorderStyle; }
            set { base.RowHeadersBorderStyle = value; }
        }

        [DefaultValue(false)]
        public new bool RowHeadersVisible
        {
            get { return base.RowHeadersVisible; }
            set { base.RowHeadersVisible = value; }
        }

        [DefaultValue(typeof(DataGridViewRowHeadersWidthSizeMode), "DisableResizing")]
        public new DataGridViewRowHeadersWidthSizeMode RowHeadersWidthSizeMode
        {
            get { return base.RowHeadersWidthSizeMode; }
            set { base.RowHeadersWidthSizeMode = value; }
        }

        [DefaultValue(typeof(DataGridViewSelectionMode), "FullRowSelect")]
        public new DataGridViewSelectionMode SelectionMode
        {
            get { return base.SelectionMode; }
            set { base.SelectionMode = value; }
        }

        [DefaultValue(false)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }
        #endregion

        string DefaultColumn = string.Empty;
        ListSortDirection DefaultColumnDirection;
        List<string> BooleanColumns = new List<string>();

        string LastSortedColumn;

        [Browsable(false)]
        public Color ColorBackground { get { return BackgroundColor; } set { BackgroundColor = value; } }
        [Browsable(false)]
        public Color ColorGrid { get { return GridColor; } set { GridColor = value; } }
        [Browsable(false)]
        public Color ColorRow { get { return DefaultCellStyle.BackColor; } set { DefaultCellStyle.BackColor = value; } }
        [Browsable(false)]
        public Color ColorRowAlternate { get { return AlternatingRowsDefaultCellStyle.BackColor; } set { AlternatingRowsDefaultCellStyle.BackColor = value; } }
        [Browsable(false)]
        public Color ColorRowSelection { get { return DefaultCellStyle.SelectionBackColor; } set { DefaultCellStyle.SelectionBackColor = value; } }
        [Browsable(false)]
        public Color ColorRowMouseHover { get; set; }
        [Browsable(false)]
        public Color ColorFontRow { get { return DefaultCellStyle.ForeColor; } set { DefaultCellStyle.ForeColor = value; } }
        [Browsable(false)]
        public Color ColorFontRowSelection { get { return DefaultCellStyle.SelectionForeColor; } set { DefaultCellStyle.SelectionForeColor = value; } }

        protected Color ColorRowHeader { get { return RowHeadersDefaultCellStyle.BackColor; } set { RowHeadersDefaultCellStyle.BackColor = value; } }
        protected Color ColorRowHeaderSelection { get { return RowHeadersDefaultCellStyle.SelectionBackColor; } set { RowHeadersDefaultCellStyle.SelectionBackColor = value; } }
        protected Color ColorFontRowHeader { get { return RowHeadersDefaultCellStyle.ForeColor; } set { RowHeadersDefaultCellStyle.ForeColor = value; } }
        protected Color ColorFontRowHeaderSelection { get { return RowHeadersDefaultCellStyle.SelectionForeColor; } set { RowHeadersDefaultCellStyle.SelectionForeColor = value; } }

        public Color ColorColumnHeader { get { return ColumnHeadersDefaultCellStyle.BackColor; } set { ColumnHeadersDefaultCellStyle.BackColor = value; } }
        [Browsable(false)]
        public Color ColorColumnSelection { get { return ColumnHeadersDefaultCellStyle.SelectionBackColor; } set { ColumnHeadersDefaultCellStyle.SelectionBackColor = value; } }

        protected Color ColorFontColumnHeader { get { return ColumnHeadersDefaultCellStyle.ForeColor; } set { ColumnHeadersDefaultCellStyle.ForeColor = value; } }
        protected Color ColorFontColumnSelection { get { return ColumnHeadersDefaultCellStyle.SelectionForeColor; } set { ColumnHeadersDefaultCellStyle.SelectionForeColor = value; } }

        [Browsable(false)]
        public Color ColorColumnHeaderMouseHover { get; set; }
        [Browsable(false)]
        public Color ColorColumnHeaderReorderRec { get; set; }
        [Browsable(false)]
        public Color ColorColumnHeaderReorderDiv { get; set; }

        public DataGridBase()
        {
            InitializeComponent();

            SetStyles();

            ColumnHeaderMouseClick += Dg_ColumnHeaderMouseClick;
            ColumnHeadersHeightSizeModeChanged += Dg_ColumnHeadersHeightSizeModeChanged;
            Sorted += Dg_OnSorted;
            DataSourceChanged += Dg_DataSourceChanged;

            CellMouseDown += DataGridBase_CellMouseDown;
            CellMouseUp += DataGridBase_CellMouseUp;

            DoubleBuffered = true;
        }

        void DataGridBase_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1) return;

            SelectedColumnIndex = -1;
            SelectedColumnPos = default(Rectangle);
            SelectedColumnClickPos = default(Point);
            Divider = false;
        }

        int SelectedColumnIndex = -1;
        Rectangle SelectedColumnPos;
        Point SelectedColumnClickPos;
        bool Divider;

        void DataGridBase_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1) return;

            SelectedColumnIndex = e.ColumnIndex;
            SelectedColumnPos = GetCellDisplayRectangle(SelectedColumnIndex, -1, false);
            SelectedColumnClickPos = PointToClient(Cursor.Position);

            Divider = e.X <= 5 || e.X >= (SelectedColumnPos.Width - 5);
        }

        protected void SetStyles()
        {
            //MAIN
            Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right);

            CausesValidation = false;

            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToOrderColumns = true;

            BackgroundColor = ColorBackground;
            BorderStyle = BorderStyle.None;
            GridColor = ColorGrid;
            //Margin = new System.Windows.Forms.Padding(0);
            MultiSelect = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ReadOnly = true;
            TabStop = true;
            StandardTab = true;
            EnableHeadersVisualStyles = false;

            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            //ROWS
            RowTemplate.Height = 30;
            RowTemplate.Resizable = DataGridViewTriState.False;

            //Alternate Rows
            AlternatingRowsDefaultCellStyle.BackColor = ColorRowAlternate;

            //ROWS_HEADERS
            var RowHeadersStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = ColorRowHeader,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = ColorFontRowHeader,
                SelectionBackColor = ColorRowHeaderSelection,
                SelectionForeColor = ColorFontRowHeaderSelection,
                WrapMode = DataGridViewTriState.True
            };
            RowHeadersDefaultCellStyle = RowHeadersStyle;

            RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            RowHeadersVisible = false;

            //CELLS
            var CellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = ColorRow,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = ColorFontRow,
                SelectionBackColor = ColorRowSelection,
                SelectionForeColor = ColorFontRowSelection,
                WrapMode = DataGridViewTriState.False
            };

            DefaultCellStyle = CellStyle;
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            //COLUMNS
            var ColumnHeadersStyle = new DataGridViewCellStyle
            {
                BackColor = ColorColumnHeader,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = ColorFontColumnHeader,
                SelectionBackColor = ColorColumnSelection,
                SelectionForeColor = ColorFontColumnSelection,
                WrapMode = DataGridViewTriState.True
            };

            ColumnHeadersDefaultCellStyle = ColumnHeadersStyle;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ColumnHeadersHeight = 30;
        }

        //public void Reload()
        //{
        //    Refresh();
        //    SortDefaultColumn();
        //}

        public void AddColumnInvisible<T>(string name, string headerText = "", string propertyName = "")
        {
            AddColumn<T>(name, headerText, propertyName, "", ColumnAlign.NotSet, null, false);
        }

        public void AddColumn<T>(string name, string headerText = "", string propertyName = "", string format = "", ColumnAlign align = 0, int? index = null, bool visible = true, int width = 100)
        {
            var c = new DataGridViewColumn();

            Type _Type = typeof(T);

            switch (_Type.Name)
            {
                case "String":
                    c = new DataGridViewTextBoxColumn { CellTemplate = new DataGridViewTextBoxCell() };
                    //c.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    if (width != 0) { c.Width = width; }
                    //c.AutoSizeMode = ColumnAutoSizeMode;
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    c.DefaultCellStyle.Alignment = align == 0 ? DataGridViewContentAlignment.MiddleLeft : (DataGridViewContentAlignment)align;
                    c.DefaultCellStyle.Format = string.IsNullOrEmpty(format) ? "" : format;
                    break;
                case "Int32":
                    c = new DataGridViewTextBoxColumn { CellTemplate = new DataGridViewTextBoxCell() };
                    //c.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    if (width != 0) { c.Width = width; }
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Alignment = align == 0 ? DataGridViewContentAlignment.MiddleRight : (DataGridViewContentAlignment)align;
                    c.DefaultCellStyle.Format = string.IsNullOrEmpty(format) ? "" : format;
                    c.DefaultCellStyle.NullValue = null;
                    break;
                case "Single":
                    c = new DataGridViewTextBoxColumn { CellTemplate = new DataGridViewTextBoxCell() };
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    c.DefaultCellStyle.Format = string.IsNullOrEmpty(format) ? "N0" : format;
                    c.DefaultCellStyle.NullValue = null;
                    break;
                case "TimeSpan":
                    c = new DataGridViewTextBoxColumn { CellTemplate = new DataGridViewTextBoxCell() };
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Format = "hh\\:mm";
                    c.DefaultCellStyle.NullValue = null;
                    break;
                case "Boolean":
                    AddBooleanColumn(name);
                    c = new DataGridViewCheckBoxColumn
                    {
                        CellTemplate = new DataGridViewCheckBoxCell(),
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                    };
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.NullValue = false;
                    break;
                case "DateTime":
                    c = new DataGridViewTextBoxColumn { CellTemplate = new DataGridViewTextBoxCell() };
                    //c.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    if (width != 100) { c.Width = width; } else { c.Width = 110; }
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Format = "d";
                    c.DefaultCellStyle.NullValue = null;
                    break;
                case "Bitmap":
                    c = new DataGridViewImageColumn
                    {
                        CellTemplate = new DataGridViewImageCell(),
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                        Resizable = DataGridViewTriState.False
                    };
                    if (width != 100) { c.Width = width; }
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    break;
            }

            //c.ValueType = _Type;
            c.Name = name;
            c.HeaderText = string.IsNullOrEmpty(headerText) ? name : headerText;
            c.DataPropertyName = string.IsNullOrEmpty(propertyName) ? name : propertyName;

            //AutoSizeMode All Cells in Bitmap have poor performance
            //c.AutoSizeMode = ColumnAutoSizeMode;

            c.Visible = visible;
            c.SortMode = DataGridViewColumnSortMode.Automatic;

            if (Columns.Count == 0 || index == null)
            {
                Columns.Add(c);
            }
            else
            {
                if (index > Columns.Count)
                {
                    Columns.Insert(Columns.Count, c);
                }
                else
                {
                    Columns.Insert(Cast.ToInt(index), c);
                }
            }
        }

        //Fix Default Values
        bool firstChange = true;
        protected void Dg_ColumnHeadersHeightSizeModeChanged(object sender, EventArgs e)
        {
            if (firstChange)
            {
                firstChange = false;
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                ColumnHeadersHeight = 30;
            }
        }

        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ResetColumnHeaderColor();
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Invalidate();
        }

        void ResetColumnHeaderColor()
        {
            foreach (DataGridViewColumn column in Columns)
            {
                if (column.HeaderCell.Style.BackColor != ColorColumnHeader)
                {
                    column.HeaderCell.Style.BackColor = Color.Empty;
                    column.HeaderCell.Style.SelectionBackColor = Color.Empty;
                }
            }
        }

        #region Paint
        readonly Pen resizePen = new Pen(Colors.RGB(86, 86, 86), 1) { DashStyle = DashStyle.Dot };
        readonly Pen reorderPenRec = new Pen(Colors.RGB(172, 172, 172), 1) { DashStyle = DashStyle.Dot };
        readonly Pen reorderPenDiv = new Pen(SystemColors.ControlDark, 3) { DashStyle = DashStyle.Solid };

        bool InColumnResize { get { return (MouseButtons == MouseButtons.Left) && (Cursor == Cursors.SizeWE); } }
        bool InColumnReorder { get { return (MouseButtons == MouseButtons.Left) && (Cursor == Cursors.Default); } }

        //AdjustImageQuality
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);

            reorderPenRec.Color = ColorColumnHeaderReorderRec;
            reorderPenDiv.Color = ColorColumnHeaderReorderDiv;

            e.Graphics.InterpolationMode = InterpolationMode.Bilinear;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Default;

            if (e.RowHeader())
            {
                return;
            }

            if (e.Value is Bitmap)
            {
                var image = e.Value as Bitmap;
                var cellSize = e.CellBounds;
                var padding = e.CellStyle.Padding.All;

                if (image.Width > cellSize.Width - padding || image.Height > cellSize.Height - padding)
                {
                    e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                }
                else
                {
                    e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    //e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Vertical Line Resize Column
            if (InColumnResize && Divider)
            {
                var xPoint = PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)).X;
                var yStart = ColumnHeadersHeight;
                var yEnd = Size.Height;

                e.Graphics.DrawLine(resizePen, xPoint - 1, yStart, xPoint - 1, yEnd);
                e.Graphics.DrawLine(resizePen, xPoint, yStart + 1, xPoint, yEnd - 1);
                e.Graphics.DrawLine(resizePen, xPoint + 1, yStart, xPoint + 1, yEnd);
            }

            else if (AllowUserToOrderColumns && InColumnReorder && SelectedColumnIndex > -1)
            {
                var mousePos = PointToClient(Cursor.Position);
                var mousePos2 = PointToClient(MousePosition);
                if (mousePos != SelectedColumnClickPos)
                {
                    int columnHoverIndex = HitTest(mousePos.X, mousePos.Y).ColumnIndex;
                    int rowHoverIndex = HitTest(mousePos.X, mousePos.Y).RowIndex;

                    if (columnHoverIndex < 0 || rowHoverIndex > -1)
                        return;

                    //Rectangle Reorder Column
                    var mousePosDiference = mousePos.X - SelectedColumnClickPos.X;
                    e.Graphics.DrawRectangle(reorderPenRec, SelectedColumnPos.X + mousePosDiference, SelectedColumnPos.Y, SelectedColumnPos.Width - 1, SelectedColumnPos.Height - 1);
                    e.Graphics.DrawRectangle(reorderPenRec, SelectedColumnPos.X + mousePosDiference + 1, SelectedColumnPos.Y + 1, SelectedColumnPos.Width - 3, SelectedColumnPos.Height - 3);
                    e.Graphics.DrawRectangle(reorderPenRec, SelectedColumnPos.X + mousePosDiference + 2, SelectedColumnPos.Y + 2, SelectedColumnPos.Width - 5, SelectedColumnPos.Height - 5);

                    //Column Divider Draw
                    var hoverColumn = Columns[columnHoverIndex];
                    var hoverColumnPos = GetCellDisplayRectangle(columnHoverIndex, -1, false);
                    var hoverColumnPosFinal = new Point(hoverColumnPos.X + hoverColumnPos.Width - 1, hoverColumnPos.Y + hoverColumnPos.Height - 1);

                    if (SelectedColumnIndex == columnHoverIndex) return;

                    if (mousePos.X - hoverColumnPos.X > (hoverColumn.Width / 2))
                    {
                        if (hoverColumnPosFinal.X == SelectedColumnPos.X - 1) return;

                        if (hoverColumnPosFinal.X + 1 == Width)
                            e.Graphics.DrawLine(reorderPenDiv, new Point(hoverColumnPosFinal.X - 1, hoverColumnPos.Y), new Point(hoverColumnPosFinal.X - 1, hoverColumnPos.Height));
                        else
                            e.Graphics.DrawLine(reorderPenDiv, new Point(hoverColumnPosFinal.X, hoverColumnPos.Y), new Point(hoverColumnPosFinal.X, hoverColumnPos.Height));
                    }
                    else if (mousePos.X - hoverColumnPos.X < (hoverColumn.Width / 2))
                    {
                        if (hoverColumnPos.X == SelectedColumnPos.X + SelectedColumnPos.Width) return;

                        if (hoverColumnPos.X == 0)
                            e.Graphics.DrawLine(reorderPenDiv, new Point(hoverColumnPos.X + 1, hoverColumnPos.Y), new Point(hoverColumnPos.X + 1, hoverColumnPos.Height));
                        else
                            e.Graphics.DrawLine(reorderPenDiv, new Point(hoverColumnPos.X - 1, hoverColumnPos.Y), new Point(hoverColumnPos.X - 1, hoverColumnPos.Height));
                    }
                }
            }
        }

        //MouseMove ChangeColor
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (InColumnResize) { Invalidate(); }

            base.OnMouseMove(e);

            int rIndex = HitTest(e.X, e.Y).RowIndex;

            if (rIndex > -1)
            {
                var row = Rows[rIndex];
                row.DefaultCellStyle.BackColor = ColorRowMouseHover;
                row.DefaultCellStyle.SelectionBackColor = ColorRowMouseHover;
            }

            ResetColumnHeaderColor();

            int cIndex = HitTest(e.X, e.Y).ColumnIndex;

            if (rIndex == -1 && cIndex > -1)
            {
                var column = Columns[cIndex];
                column.HeaderCell.Style.BackColor = ColorColumnHeaderMouseHover;
                column.HeaderCell.Style.SelectionBackColor = ColorColumnHeaderMouseHover;
            }
        }

        //ChangeRowColorBackToNormal
        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            base.OnRowPostPaint(e);

            if (RectangleToScreen(e.RowBounds).Contains(MousePosition))
            {
                //row.DefaultCellStyle.BackColor = ColorRowMouseHover;
                //row.DefaultCellStyle.SelectionBackColor = ColorRowMouseHover;

                //Color c = Color.FromArgb(50, Color.Blue);
                //using (var b = new SolidBrush(RowMouseHoverColor))
                //using (var p = new Pen(RowMouseHoverColor))
                //{
                //    var r = e.RowBounds;
                //    r.Width -= 1;
                //    r.Height -= 1;
                //    e.Graphics.FillRectangle(b, r);
                //    e.Graphics.DrawRectangle(p, r);
                //}
            }
            else
            {
                var row = Rows[e.RowIndex];
                row.DefaultCellStyle.SelectionBackColor = ColorRowSelection;

                if (e.RowIndex % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = ColorRow;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = ColorRowAlternate;
                }
            }
        }

        public void RefreshCellsForeColor(string columnId, string valueId, string columnName, bool change, Color? c = null)
        {
            foreach (DataGridViewRow Row in Rows)
            {
                if (Row.Cells[columnId].Value.ToString() == valueId)
                {
                    ChangeCellForeColor(Row.Cells[columnName], change, c);
                }
            }
        }

        public void ChangeCellForeColor(DataGridViewCell cell, bool change, Color? c = null)
        {
            Color color = Color.Red;

            if (c != null) { color = (Color)c; }

            if (!change)
            {
                cell.Style.SelectionForeColor = color;
                cell.Style.ForeColor = color;
            }
            else
            {
                cell.Style.SelectionForeColor = DefaultCellStyle.SelectionForeColor;
                cell.Style.ForeColor = DefaultCellStyle.ForeColor;
            }
        }
        #endregion

        #region SortColumns
        //public void ReSort()
        //{
        //    this.Sort(this.Columns[LastSortedColumn], LastSortedColumnDirection);
        //    this.Refresh();
        //}

        protected void Dg_OnSorted(object sender, EventArgs e)
        {
            LoadBooleanImages();
        }

        protected void Dg_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            LastSortedColumn = Columns[e.ColumnIndex].Name;

            SortImageColumns(sender, e);
        }

        public void OrderBy(string columnName, bool ascending = true)
        {
            DefaultColumn = columnName;
            DefaultColumnDirection = ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            LastSortedColumn = DefaultColumn;
        }

        public void SortDefaultColumn(bool lastSortedDirection = false)
        {
            if (Columns.Contains(DefaultColumn))
            {
                if (!lastSortedDirection)
                {
                    Sort(Columns[DefaultColumn], DefaultColumnDirection);
                }
                else
                {
                    //Maintain sort direction after refresh
                    if (Columns[LastSortedColumn] is DataGridViewImageColumn)
                    {
                        var a = new DataGridViewCellMouseEventArgs(Columns[LastSortedColumn].Index, 0, 0, 0, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
                        SortImageColumns(null, a);
                        SortImageColumns(null, a);
                    }
                    else
                    {
                        if (SortOrder == SortOrder.Descending)
                        {
                            Sort(Columns[LastSortedColumn], ListSortDirection.Descending);
                        }
                        else
                        {
                            Sort(Columns[LastSortedColumn], ListSortDirection.Ascending);
                        }
                    }
                }
            }
        }

        public void SortImageColumns(object sender, DataGridViewCellMouseEventArgs e)
        {
            string clickedColumn = Columns[e.ColumnIndex].Name;
            string sortColumn = string.Empty;

            if (clickedColumn.Length > 2)
            {
                sortColumn = clickedColumn.Substring(0, clickedColumn.Length - 3);
            }

            var booleanColumns = GetBooleanColumns();

            if (booleanColumns != null && sortColumn != string.Empty && booleanColumns.Exists(s => s.EndsWith(sortColumn, StringComparison.OrdinalIgnoreCase)))
            {
                SortColumn(Columns[sortColumn], Columns[clickedColumn], e);
            }
        }

        void SortColumn(DataGridViewColumn sortColumn, DataGridViewColumn clickedColumn, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = clickedColumn.DataGridView;

            if (dgv.Columns[e.ColumnIndex].Name == clickedColumn.Name)
            {
                if (dgv.SortOrder == SortOrder.Descending)
                {
                    dgv.Sort(sortColumn, ListSortDirection.Ascending);
                    clickedColumn.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(sortColumn, ListSortDirection.Descending);
                    clickedColumn.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
        }
        #endregion

        #region ImageColumns
        protected void Dg_DataSourceChanged(object sender, EventArgs e)
        {
            if (AutoGenerateColumns == false)
                SetBooleanColumns(GetBooleanColumns());
            SortDefaultColumn();
        }

        readonly string boolColumnSufix = "Bol";
        public void SetBooleanColumns(List<string> boolColumns)
        {
            BooleanColumns = boolColumns;

            foreach (string columnName in BooleanColumns)
            {
                //Disable old Column
                if (Columns.Contains(columnName))
                {
                    string newColumnName = columnName + boolColumnSufix;

                    //Add Image Columns
                    if (Columns.Contains(newColumnName))
                    {
                        int index = Columns[newColumnName].Index;
                        Columns.RemoveAt(index);
                    }

                    AddColumn<Bitmap>(newColumnName, columnName, "", "", ColumnAlign.NotSet, Columns[columnName].Index, true, 65);

                    Columns[columnName].Visible = false;
                }
            }
        }

        public void AddBooleanColumn(string columnName)
        {
            BooleanColumns.Add(columnName);
        }

        public List<string> GetBooleanColumns()
        {
            return BooleanColumns;
        }

        readonly Bitmap imgtrue = Resources.img_true_ico;
        readonly Bitmap imgfalse = Resources.img_false_ico;

        public void LoadBooleanImages()
        {
            var booleanColumns = GetBooleanColumns();

            if (booleanColumns == null) { return; }

            var imgtruePerformatic = imgtrue.Clone32bpp();
            var imgfalsePerformatic = imgfalse.Clone32bpp();

            foreach (DataGridViewRow row in Rows)
            {
                foreach (string boolColumn in booleanColumns)
                {
                    if (Columns.Contains(boolColumn) && Columns.Contains(boolColumn + boolColumnSufix))
                    {
                        string cellValue = row.Cells[boolColumn].Value != null ? row.Cells[boolColumn].Value.ToString() : string.Empty;
                        row.Cells[boolColumn + boolColumnSufix].Value = (cellValue == "True") ? imgtruePerformatic : imgfalsePerformatic;
                    }
                }
            }
        }

        public void RefreshRows()
        {
            //SortDefaultColumn(true);
            LoadBooleanImages();
            Refresh();
        }

        //public void RefreshBooleanImage(string column, string cellValue, string booleanColumn, bool value)
        //{
        //    foreach (DataGridViewRow Row in this.Rows)
        //    {
        //        if (Row.Cells[column].Value.ToString() == cellValue)
        //        {
        //            Row.Cells[booleanColumn + boolColumnSufix].Value = (value == true) ? imgtrue : imgfalse;
        //        }
        //    }
        //}
        #endregion
    }
}