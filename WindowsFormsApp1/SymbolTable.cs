using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class SymbolTable : DataGridView
    {
        private static readonly int _cellSize = 40;

        private static readonly Dictionary<string, string[]> _symbolGroups = new Dictionary<string, string[]>
        {
            { "Математические символы", new string[] { "±", "∙", "×", "÷", "≠", "≈", "√", "∛", "∜", "≤", "≥", "∫", "∧", "∨", "¬", "∞", "∂", "∀", "∅", "∇", "∃", "∄", "∑", "∝", "∐", "∏", "∣", "∥", "∠", "∾", "≂", "≃", "≄", "≉", "≮", "≯", "≰", "≱", "⊾" } },
            { "Операции множеств", new string[] { "∈", "∉", "∪" } },
            { "Дроби", new string[] { "¼", "½", "¾", "⅐", "⅑", "⅒", "⅓", "⅔", "⅕", "⅖", "⅗", "⅘", "⅙", "⅚", "⅛", "⅜", "⅝", "⅞" } },
            { "Надстрочные индексы", new string[] { "⁰", "¹", "²", "³", "⁴", "⁵", "⁶", "⁷", "⁸", "⁹", "⁻", "⁺", "⁼", "⁽", "⁾", "ᵃ", "ᵇ", "ᶜ", "ᵈ", "ᵉ", "ᶠ", "ᵍ", "ʰ", "ⁱ", "ʲ", "ᵏ", "ˡ", "ᵒ", "ᵖ", "ʳ", "ˢ", "ᵗ", "ᵘ", "ᵛ", "ʷ", "ˣ", "ʸ", "ᶻ", "ᴬ", "ᴮ", "ᴰ", "ᴱ", "ᴳ", "ᴴ", "ᴵ", "ᴶ", "ᴷ", "ᴸ", "ᴹ", "ᴺ", "ᴼ", "ᴾ", "ᴿ", "ᵀ", "ᵁ", "ⱽ", "ᵂ", "´" } },
            { "Подстрочные индексы", new string[] { "₀", "₁", "₂", "₃", "₄", "₅", "₆", "₇", "₈", "₉", "₋", "₊", "₌", "₍", "₎", "ₐ", "ₑ", "ₕ", "ᵢ", "ⱼ", "ₖ", "ₗ", "ₒ", "ₚ", "ᵣ", "ₛ", "ₜ", "ᵤ", "ᵥ", "ₓ" } },
            { "Греческий алфавит", new string[] { "Α", "Β", "Γ", "Δ", "Ε", "Ζ", "Η", "Θ", "Ι", "Κ", "Λ", "Μ", "Ν", "Ξ", "Ο", "Π", "Ρ", "Σ", "Τ", "Υ", "Φ", "Χ", "Ψ", "Ω", "α", "β", "γ", "δ", "ε", "ζ", "η", "θ", "ι", "κ", "λ", "μ", "ν", "ξ", "ο", "π", "ρ", "ς", "σ", "τ", "υ", "φ", "χ", "ψ", "ω" } },
            { "Знаки единиц измерения", new string[] { "‰", "‱", "°", "℃", "℉", "Å" } },
            { "Стрелки", new string[] { "←", "↑", "→", "↓", "↔", "↕", "↖", "↗", "↘", "↙", "⇌", "⇒", "⇔" } },
            { "Геометрические фигуры", new string[] { "○", "●", "□", "■", "△", "▲", "☆", "★" } },
            { "Специальные символы", new string[] { "©", "®", "✓", "✕", "✓", "⟲", "⟳", "§" } }
        };

        private int _currentGroupIndex = -1;

        private string[] _allSymbols;

        /// <summary>
        /// Группа символов для отображения, если указана -1, то отображаются все группы символов
        /// </summary>
        public int CurrentGroupIndex
        {
            get => _currentGroupIndex;
            set
            {
                CurrentCell = null;
                _currentGroupIndex = value;
                if (CurrentGroupIndex == -1)
                {
                    RowCount = _symbolGroups.Values.Select(p => (p.Length + ColumnCount - 1) / ColumnCount).DefaultIfEmpty(0).Sum();
                }
                else if (CurrentGroupIndex >= 0 && CurrentGroupIndex < _symbolGroups.Count)
                {
                    RowCount = (_symbolGroups.ElementAt(CurrentGroupIndex).Value.Length + ColumnCount - 1) / ColumnCount;
                }
                else
                {
                    RowCount = 0;
                }
                CurrentCell = null;
                Refresh();
            }
        }

        /// <summary>
        /// Получение наименований всех групп
        /// </summary>
        public string[] GroupNames => _symbolGroups.Keys.ToArray();

        protected override bool DoubleBuffered { get => true; }

        private string[] AllSymbols
        {
            get
            {
                if (_allSymbols == null)
                {
                    _allSymbols = _symbolGroups.Values.SelectMany(p =>
                    {
                        Array.Resize(ref p, (p.Length + ColumnCount - 1) / ColumnCount * ColumnCount);
                        return p;
                    }).ToArray();
                }
                return _allSymbols;
            }
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            RowHeadersVisible = false;
            ColumnHeadersVisible = false;
            BackgroundColor = Color.White;
            DefaultCellStyle.SelectionBackColor = Color.White;
            DefaultCellStyle.SelectionForeColor = Color.Black;
            VirtualMode = true;
            CellBorderStyle = DataGridViewCellBorderStyle.Single;
            DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DefaultCellStyle.Font = new Font("Arial", 20);
            AllowUserToAddRows = false;
            AllowUserToResizeColumns = false;
            AllowUserToResizeRows = false;
            MultiSelect = false;
            ReadOnly = true;
            RowTemplate.Height = _cellSize;

            if (ColumnCount == 0)
            {
                ColumnCount = 10;
            }

            foreach (DataGridViewColumn column in Columns)
            {
                column.Width = _cellSize;
            }

            foreach (DataGridViewRow row in Rows)
            {
                row.Height = _cellSize;
            }

            CurrentCell = null;
        }

        protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
        {
            base.OnCellValueNeeded(e);

            var index = ColumnCount * e.RowIndex + e.ColumnIndex;
            if (CurrentGroupIndex == -1)
            {
                var symbols = AllSymbols;
                e.Value = index >= 0 && index < symbols.Length ? symbols[index] : null;
            }
            else if (CurrentGroupIndex >= 0 && CurrentGroupIndex < _symbolGroups.Count)
            {
                var symbols = _symbolGroups.ElementAt(CurrentGroupIndex).Value;
                e.Value = index >= 0 && index < symbols.Length ? symbols[index] : null;
            }
            else
            {
                e.Value = null;
            }
        }

        protected override void OnCurrentCellChanged(EventArgs e)
        {
            base.OnCurrentCellChanged(e);
            Refresh();
        }

        protected override void OnCellMouseMove(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                if (this[e.ColumnIndex, e.RowIndex] != CurrentCell)
                {
                    CurrentCell = this[e.ColumnIndex, e.RowIndex];
                }
            }
        }

        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            CurrentCell = null;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CurrentCell = null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (CurrentCell != null)
            {
                Rectangle cellRect = GetCellDisplayRectangle(CurrentCell.ColumnIndex, CurrentCell.RowIndex, true);
                if (cellRect.Width != 0 && cellRect.Height != 0)
                {
                    if (CurrentCell.Value != null)
                    {
                        int x = cellRect.X + cellRect.Width / 2 - _cellSize;
                        int y = cellRect.Y + cellRect.Height / 2 - _cellSize;

                        Rectangle rect = new Rectangle(x, y, _cellSize * 2, _cellSize * 2);

                        int vShift = VerticalScrollBar.Visible ? VerticalScrollBar.Width : 0;
                        int hShift = HorizontalScrollBar.Visible ? HorizontalScrollBar.Height : 0;

                        if (rect.X < 0)
                        {
                            rect.X = 1;
                        }
                        else if ((rect.X + rect.Width) > (Width - vShift))
                        {
                            rect.X = Width - vShift - rect.Width - 2;
                        }

                        if (rect.Y < 0)
                        {
                            rect.Y = 1;
                        }
                        else if ((rect.Y + rect.Height) > (Height - hShift))
                        {
                            rect.Y = Height - hShift - rect.Height - 2;
                        }

                        using (Pen pen = new Pen(Color.Gray, 1))
                        using (SolidBrush brush = new SolidBrush(Color.White))
                        using (SolidBrush blackBrush = new SolidBrush(Color.Black))
                        using (Font font = new Font(DefaultCellStyle.Font.Name, DefaultCellStyle.Font.Size * 2))
                        {
                            e.Graphics.FillRectangle(brush, rect);
                            e.Graphics.DrawRectangle(pen, rect);

                            var value = CurrentCell.Value?.ToString();
                            var size = e.Graphics.MeasureString(value, font);
                            e.Graphics.DrawString(value, font, blackBrush, rect.X + (rect.Width - size.Width) / 2, rect.Y + (rect.Height - size.Height) / 2);

                            rect.Width += 1;
                            rect.Height += 1;
                            e.Graphics.DrawRectangle(pen, rect);
                        }
                    }
                    else
                    {
                        cellRect.Width -= 2;
                        cellRect.Height -= 2;

                        using (Pen pen = new Pen(Color.Gray, 1))
                        {
                            pen.DashStyle = DashStyle.Dash;
                            e.Graphics.DrawRectangle(pen, cellRect);
                        }
                    }
                }
            }
        }
    }
}
