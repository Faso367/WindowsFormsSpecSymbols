using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private ComboBox groupComboBox;
        private DataGridView dataGridView;
        private TextBox textBox;
        private bool isInitialized = false;
        private Button chooseButton;   // Кнопка "Выбрать"
        private string selectedSymbol; // Хранит выбранную кнопку
        private Label enlargedSymbolLabel; // Увеличенная кнопка для предпросмотра

        private Dictionary<string, string[]> symbolGroups = new Dictionary<string, string[]>
        {
            { "Математические символы", new string[] { "±", "∙", "×", "÷", "≠", "≈", "√", "∛", "∜", "≤", "≥", "∫", "∧", "∨", "¬", "∞", "∂" } },
            { "Операции множеств", new string[] { "∈", "∉", "∪" } },
            { "Дроби", new string[] { "¼", "½", "¾", "⅐", "⅑", "⅒", "⅓", "⅔", "⅕", "⅖", "⅗", "⅘", "⅙", "⅚", "⅛", "⅜", "⅝", "⅞" } },
            { "Надстрочные индексы", new string[] { "⁰", "¹", "²", "³", "⁴", "⁵", "⁶", "⁷", "⁸", "⁹", "⁻", "⁺", "⁼", "⁽", "⁾", "ᵃ", "ᵇ", "ᶜ", "ᵈ", "ᵉ", "ᶠ", "ᵍ", "ʰ", "ⁱ", "ʲ", "ᵏ", "ˡ", "ᵒ", "ᵖ", "ʳ", "ˢ", "ᵗ", "ᵘ", "ᵛ", "ʷ", "ˣ", "ʸ", "ᶻ", "ᴬ", "ᴮ", "ᴰ", "ᴱ", "ᴳ", "ᴴ", "ᴵ", "ᴶ", "ᴷ", "ᴸ", "ᴹ", "ᴺ", "ᴼ", "ᴾ", "ᴿ", "ᵀ", "ᵁ", "ⱽ", "ᵂ" } },
            { "Подстрочные индексы", new string[] { "₀", "₁", "₂", "₃", "₄", "₅", "₆", "₇", "₈", "₉", "₋", "₊", "₌", "₍", "₎", "ₐ", "ₑ", "ₕ", "ᵢ", "ⱼ", "ₖ", "ₗ", "ₒ", "ₚ", "ᵣ", "ₛ", "ₜ", "ᵤ", "ᵥ", "ₓ" } },
            { "Греческий алфавит", new string[] { "Α", "Β", "Γ", "Δ", "Ε", "Ζ", "Η", "Θ", "Ι", "Κ", "Λ", "Μ", "Ν", "Ξ", "Ο", "Π", "Ρ", "Σ", "Τ", "Υ", "Φ", "Χ", "Ψ", "Ω", "α", "β", "γ", "δ", "ε", "ζ", "η", "θ", "ι", "κ", "λ", "μ", "ν", "ξ", "ο", "π", "ρ", "ς", "σ", "τ", "υ", "φ", "χ", "ψ", "ω" } },
            { "Знаки единиц измерения", new string[] { "‰", "‱", "°", "℃", "℉", "Å" } },
            { "Стрелки", new string[] { "←", "↑", "→", "↓", "↔", "↕", "↖", "↗", "↘", "↙", "⇌", "⇒", "⇔" } },
            { "Геометрические фигуры", new string[] { "○", "●", "□", "■", "△", "▲" } },
            { "Специальные символы", new string[] { "©", "®", "✓", "✕" } }
        };

        public Form1()
        {
            try
            {
                InitializeComponent();
                InitializeUI();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при инициализации формы: " + ex.Message);
            }
        }

        private void InitializeUI()
        {
            this.Width = 800;
            this.Height = 600;

            groupComboBox = new ComboBox
            {
                Location = new Point(10, 10),
                Width = 250,
                Font = new Font("Arial", 12, FontStyle.Regular),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            groupComboBox.SelectedIndexChanged += GroupComboBox_SelectedIndexChanged;

            groupComboBox.Items.Add("Все элементы");
            foreach (var groupName in symbolGroups.Keys)
            {
                groupComboBox.Items.Add(groupName);
            }

            // Устанавливаем "Все элементы" как выбранный по умолчанию
            groupComboBox.SelectedIndex = 0;
            this.Controls.Add(groupComboBox);

            textBox = new TextBox
            {
                Location = new Point(300, 10),
                Width = 300,
                Height = 18,
                Font = new Font("Arial", 14, FontStyle.Regular)
            };
            this.Controls.Add(textBox);

            dataGridView = new DataGridView
            {
                Location = new Point(10, 50),
                Width = 1000,
                Height = 600,
                ColumnCount = 10,
                RowTemplate = { Height = 40 },
                AllowUserToAddRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                ReadOnly = true
            };

            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.DefaultCellStyle.Font = new Font("Arial", 20);
            dataGridView.CellClick += DataGridView_CellClick;
            dataGridView.CellPainting += DataGridView_CellPainting;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.Width = 60; // Ширина каждой колонки
            }

            this.Controls.Add(dataGridView);

            // Устанавливаем флаг после полной инициализации формы
            isInitialized = true;

            // Отображаем символы для начальной группы
            DisplaySymbols("Все элементы");
        }

        /// <summary>
        /// Удаляет предыдущий Label (увеличенную кнопку)
        /// </summary>
        private void DeletePreviousLabel()
        {
            if (enlargedSymbolLabel != null)
            {
                this.Controls.Remove(enlargedSymbolLabel);
                enlargedSymbolLabel.Dispose();
                enlargedSymbolLabel = null;
            }
        }

        /// <summary>
        /// Удаляет Label и отображает таблицу
        /// </summary>
        private void GroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitialized && groupComboBox.SelectedItem != null)
            {
                string selectedGroup = groupComboBox.SelectedItem.ToString();
                DeletePreviousLabel();
                DisplaySymbols(selectedGroup);
            }
        }

        /// <summary>
        /// Отрисовывает таблицу
        /// </summary>
        private void DisplaySymbols(string groupName)
        {
            // Очищаем DataGridView перед загрузкой новых символов
            dataGridView.Rows.Clear();

            // Получаем символы для выбранной группы
            List<string> symbolsToDisplay = new List<string>();
            if (groupName == "Все элементы")
            {
                // Собираем все символы из всех групп
                foreach (var symbolList in symbolGroups.Values)
                {
                    symbolsToDisplay.AddRange(symbolList);
                }
            }
            else if (symbolGroups.TryGetValue(groupName, out string[] symbols))
            {
                symbolsToDisplay.AddRange(symbols);
            }

            int symbolIndex = 0;
            int rowCount = (int)Math.Ceiling(symbolsToDisplay.Count / 10.0);
            dataGridView.RowCount = rowCount;

            // Задаем ширину колонок
            for (int col = 0; col < dataGridView.ColumnCount; col++)
            {
                dataGridView.Columns[col].Width = 50;
            }

            for (int row = 0; row < rowCount; row++)
            {
                dataGridView.Rows[row].Height = 40; // Устанавливаем высоту строки

                for (int col = 0; col < dataGridView.ColumnCount; col++)
                {
                    if (symbolIndex < symbolsToDisplay.Count)
                    {
                        dataGridView[col, row].Value = symbolsToDisplay[symbolIndex++];
                    }
                    else
                    {
                        dataGridView[col, row].Value = string.Empty;
                    }
                }
            }

            if (chooseButton == null)
            {
                chooseButton = new Button
                {
                    Text = "Выбрать",
                    Location = new Point(650, 10),
                    Width = 100,
                    Height = 32,
                    Font = new Font("Arial", 12)
                };
                chooseButton.Click += ChooseButton_Click;
                
                this.Controls.Add(chooseButton);
            }
        }

        /// <summary>
        /// Удаляет предыдущий Label и создает новый, сохраняет символ
        /// </summary>
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string symbol = dataGridView[e.ColumnIndex, e.RowIndex].Value?.ToString();
                if (!string.IsNullOrEmpty(symbol))
                {
                    selectedSymbol = symbol; // Сохраняем выбранный символ
                }

                DeletePreviousLabel();

                enlargedSymbolLabel = new Label
                {
                    Text = symbol,
                    Font = new Font("Arial", 30, FontStyle.Bold),
                    //Size = new Size(40, 40),
                    AutoSize = true,  // Чтобы размер Label подстраивался под текст
                    BackColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                // Получаем прямоугольник текущей ячейки
                Rectangle cellRect = dataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                // Определяем координаты для размещения Label в центре ячейки
                int labelX = cellRect.X + dataGridView.Location.X + (cellRect.Width - enlargedSymbolLabel.PreferredWidth) / 2;
                int labelY = cellRect.Y + dataGridView.Location.Y + (cellRect.Height - enlargedSymbolLabel.PreferredHeight) / 2;

                enlargedSymbolLabel.Location = new Point(labelX, labelY);

                this.Controls.Add(enlargedSymbolLabel);
                enlargedSymbolLabel.BringToFront();
            }
        }

        /// <summary>
        /// Удаляет Label и добавляет текст в TextBox
        /// </summary>
        private void ChooseButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedSymbol))
            {
                textBox.AppendText(selectedSymbol);
                DeletePreviousLabel();
            }
        }

        /// <summary>
        /// Кастомный отрисовщик символов в ячейках
        /// </summary>
        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);

                string symbol = e.Value?.ToString();
                if (!string.IsNullOrEmpty(symbol))
                {
                    using (var brush = new SolidBrush(e.CellStyle.ForeColor))
                    {
                        var font = new Font("Arial", 20, FontStyle.Regular);
                        var format = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };

                        e.Graphics.DrawString(symbol, font, brush, e.CellBounds, format);
                    }
                }

                e.Handled = true; // Отменяем стандартную отрисовку текста
            }
        }
    }
}
