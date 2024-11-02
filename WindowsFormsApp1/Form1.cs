using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private ComboBox groupComboBox;
        private DataGridView dataGridView;
        private TextBox textBox;
        private bool isInitialized = false;
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
        //private Dictionary<string, string[]> symbolGroups; // Предполагаем, что словарь с символами уже инициализирован

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
            // Устанавливаем размер формы
            this.Width = 800;
            this.Height = 600;

            // Создаем ComboBox для выбора группы символов
            groupComboBox = new ComboBox
            {
                Location = new Point(10, 10),
                Width = 250,
                Font = new Font("Arial", 12, FontStyle.Regular),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            groupComboBox.SelectedIndexChanged += GroupComboBox_SelectedIndexChanged;

            // Заполняем ComboBox ключами из словаря символов
            groupComboBox.Items.Add("Все элементы");
            foreach (var groupName in symbolGroups.Keys)
            {
                groupComboBox.Items.Add(groupName);
            }

            // Устанавливаем "Все элементы" как выбранный по умолчанию
            groupComboBox.SelectedIndex = 0;
            this.Controls.Add(groupComboBox);

            // Создаем TextBox для вывода символов
            textBox = new TextBox
            {
                Location = new Point(300, 10),
                Width = 350,
                Font = new Font("Arial", 12, FontStyle.Regular)
            };
            this.Controls.Add(textBox);

            // Создаем DataGridView для отображения символов
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

            // Настраиваем стиль DataGridView
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.DefaultCellStyle.Font = new Font("Arial", 20);
            dataGridView.CellClick += DataGridView_CellClick;
            dataGridView.CellPainting += DataGridView_CellPainting;

            // Настройка колонок DataGridView
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
        private void GroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitialized && groupComboBox.SelectedItem != null)
            {
                string selectedGroup = groupComboBox.SelectedItem.ToString();
                DisplaySymbols(selectedGroup);
            }
        }

        // Метод для отображения символов в DataGridView
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

            // Заполняем DataGridView символами и устанавливаем высоту строк
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
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string symbol = dataGridView[e.ColumnIndex, e.RowIndex].Value?.ToString();
                if (!string.IsNullOrEmpty(symbol))
                {
                    textBox.AppendText(symbol);
                }
            }
        }




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
    


    //    public partial class Form1 : Form
    //    {
    //        private TextBox displayTextBox;
    //        private ComboBox groupComboBox;
    //        private DataGridView buttonGrid;

    //// Словарь для хранения групп и их элементов
    //private Dictionary<string, string[]> symbolGroups = new Dictionary<string, string[]>
    //{
    //    { "Математические символы", new string[] { "±", "∙", "×", "÷", "≠", "≈", "√", "∛", "∜", "≤", "≥", "∫", "∧", "∨", "¬", "∞", "∂" } },
    //    { "Операции множеств", new string[] { "∈", "∉", "∪" } },
    //    { "Дроби", new string[] { "¼", "½", "¾", "⅐", "⅑", "⅒", "⅓", "⅔", "⅕", "⅖", "⅗", "⅘", "⅙", "⅚", "⅛", "⅜", "⅝", "⅞" } },
    //    { "Надстрочные индексы", new string[] { "⁰", "¹", "²", "³", "⁴", "⁵", "⁶", "⁷", "⁸", "⁹", "⁻", "⁺", "⁼", "⁽", "⁾", "ᵃ", "ᵇ", "ᶜ", "ᵈ", "ᵉ", "ᶠ", "ᵍ", "ʰ", "ⁱ", "ʲ", "ᵏ", "ˡ", "ᵒ", "ᵖ", "ʳ", "ˢ", "ᵗ", "ᵘ", "ᵛ", "ʷ", "ˣ", "ʸ", "ᶻ", "ᴬ", "ᴮ", "ᴰ", "ᴱ", "ᴳ", "ᴴ", "ᴵ", "ᴶ", "ᴷ", "ᴸ", "ᴹ", "ᴺ", "ᴼ", "ᴾ", "ᴿ", "ᵀ", "ᵁ", "ⱽ", "ᵂ" } },
    //    { "Подстрочные индексы", new string[] { "₀", "₁", "₂", "₃", "₄", "₅", "₆", "₇", "₈", "₉", "₋", "₊", "₌", "₍", "₎", "ₐ", "ₑ", "ₕ", "ᵢ", "ⱼ", "ₖ", "ₗ", "ₒ", "ₚ", "ᵣ", "ₛ", "ₜ", "ᵤ", "ᵥ", "ₓ" } },
    //    { "Греческий алфавит", new string[] { "Α", "Β", "Γ", "Δ", "Ε", "Ζ", "Η", "Θ", "Ι", "Κ", "Λ", "Μ", "Ν", "Ξ", "Ο", "Π", "Ρ", "Σ", "Τ", "Υ", "Φ", "Χ", "Ψ", "Ω", "α", "β", "γ", "δ", "ε", "ζ", "η", "θ", "ι", "κ", "λ", "μ", "ν", "ξ", "ο", "π", "ρ", "ς", "σ", "τ", "υ", "φ", "χ", "ψ", "ω" } },
    //    { "Знаки единиц измерения", new string[] { "‰", "‱", "°", "℃", "℉", "Å" } },
    //    { "Стрелки", new string[] { "←", "↑", "→", "↓", "↔", "↕", "↖", "↗", "↘", "↙", "⇌", "⇒", "⇔" } },
    //    { "Геометрические фигуры", new string[] { "○", "●", "□", "■", "△", "▲" } },
    //    { "Специальные символы", new string[] { "©", "®", "✓", "✕" } }
    //};


    //        public Form1()
    //        {
    //            InitializeComponent();
    //            InitializeComboBox();
    //            InitializeButtonGrid();
    //            InitializeTextBox();
    //        }

    //        private void InitializeComboBox()
    //        {
    //            // Создаем ComboBox для выбора группы
    //            groupComboBox = new ComboBox
    //            {
    //                Dock = DockStyle.Top,
    //                DropDownStyle = ComboBoxStyle.DropDownList
    //            };

    //            // Добавляем названия групп в ComboBox
    //            foreach (var group in symbolGroups.Keys)
    //            {
    //                groupComboBox.Items.Add(group);
    //            }
    //            groupComboBox.SelectedIndex = 0; // Выбираем первую группу по умолчанию
    //            groupComboBox.SelectedIndexChanged += GroupComboBox_SelectedIndexChanged;

    //            // Добавляем ComboBox на форму
    //            this.Controls.Add(groupComboBox);
    //        }

    //        private void InitializeButtonGrid()
    //        {
    //            // Создаем таблицу и устанавливаем базовые параметры
    //            buttonGrid = new DataGridView
    //            {
    //                ColumnCount = 10,
    //                Dock = DockStyle.Top,
    //                AllowUserToAddRows = false,
    //                AllowUserToDeleteRows = false,
    //                ReadOnly = true,
    //                Height = 150
    //            };

    //            // Настраиваем стиль ячеек, чтобы убрать фон и границы
    //            buttonGrid.DefaultCellStyle.BackColor = this.BackColor; // Фон, соответствующий фону формы
    //            buttonGrid.DefaultCellStyle.SelectionBackColor = this.BackColor; // Фон при выборе
    //            buttonGrid.DefaultCellStyle.SelectionForeColor = Color.Black; // Цвет текста при выборе
    //            buttonGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single; // Сетка ячеек

    //            // Добавляем обработчик события CellClick
    //            buttonGrid.CellClick += ButtonGrid_CellClick;

    //            // Добавляем таблицу на форму
    //            this.Controls.Add(buttonGrid);

    //            // Загружаем символы первой группы по умолчанию
    //            LoadSymbols(symbolGroups[groupComboBox.SelectedItem.ToString()]);
    //        }

    //        private void InitializeTextBox()
    //        {
    //            // Создаем TextBox и настраиваем его
    //            displayTextBox = new TextBox
    //            {
    //                Dock = DockStyle.Bottom,
    //                ReadOnly = true,
    //                TextAlign = HorizontalAlignment.Center
    //            };

    //            // Добавляем TextBox на форму
    //            this.Controls.Add(displayTextBox);
    //        }

    //        private void LoadSymbols(string[] symbols)
    //        {
    //            // Очищаем текущие ячейки в таблице
    //            buttonGrid.Rows.Clear();

    //            // Создаем строки и заполняем их символами
    //            int symbolIndex = 0;
    //            int rowCount = (symbols.Length + buttonGrid.ColumnCount - 1) / buttonGrid.ColumnCount; // Рассчитываем необходимое количество строк

    //            for (int row = 0; row < rowCount; row++)
    //            {
    //                buttonGrid.Rows.Add(); // Добавляем новую строку
    //                for (int col = 0; col < buttonGrid.ColumnCount; col++)
    //                {
    //                    if (symbolIndex < symbols.Length)
    //                    {
    //                        var buttonCell = new DataGridViewButtonCell
    //                        {
    //                            Value = symbols[symbolIndex] // Назначаем символ кнопке
    //                        };
    //                        buttonCell.FlatStyle = FlatStyle.Flat; // Убираем границы кнопки
    //                        buttonGrid[col, row] = buttonCell;
    //                        symbolIndex++;
    //                    }
    //                }
    //            }
    //        }

    //        private void GroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
    //        {
    //            // Загружаем символы выбранной группы
    //            string selectedGroup = groupComboBox.SelectedItem.ToString();
    //            LoadSymbols(symbolGroups[selectedGroup]);
    //        }

    //        private void ButtonGrid_CellClick(object sender, DataGridViewCellEventArgs e)
    //        {
    //            // Проверяем, что ячейка содержит кнопку
    //            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && ((DataGridView)sender)[e.ColumnIndex, e.RowIndex] is DataGridViewButtonCell)
    //            {
    //                // Получаем символ кнопки и добавляем его в TextBox
    //                string buttonSymbol = ((DataGridView)sender)[e.ColumnIndex, e.RowIndex].Value.ToString();
    //                displayTextBox.Text += buttonSymbol;
    //            }
    //        }
    //    }



    //public partial class Form1 : Form
    //{
    //    private TextBox displayTextBox;

    //    public Form1()
    //    {
    //        InitializeComponent();
    //        InitializeButtonGrid();
    //        InitializeTextBox();
    //    }

    //    private void InitializeButtonGrid()
    //    {
    //        // Специальные символы, которые будут отображаться на кнопках
    //        string[] specialChars = { "±", "∙", "×", "÷", "≠", "≈", "√", "∛", "∜", "≤", "≥", "∫", "∧", "∨", "¬", "∞" };

    //        // Создаем таблицу и устанавливаем базовые параметры
    //        DataGridView buttonGrid = new DataGridView
    //        {
    //            ColumnCount = 5,
    //            RowCount = 3,
    //            Dock = DockStyle.Top,
    //            AllowUserToAddRows = false,
    //            AllowUserToDeleteRows = false,
    //            ReadOnly = true,
    //            Height = 150
    //        };

    //        // Настраиваем стиль ячеек, чтобы убрать фон и границы
    //        buttonGrid.DefaultCellStyle.BackColor = this.BackColor; // Фон, соответствующий фону формы
    //        buttonGrid.DefaultCellStyle.SelectionBackColor = this.BackColor; // Фон при выборе
    //        buttonGrid.DefaultCellStyle.SelectionForeColor = Color.Black; // Цвет текста при выборе
    //        buttonGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single; // Сетка ячеек

    //        // Создаем кнопки и присваиваем им специальные символы
    //        int symbolIndex = 0;
    //        for (int row = 0; row < buttonGrid.RowCount; row++)
    //        {
    //            for (int col = 0; col < buttonGrid.ColumnCount; col++)
    //            {
    //                if (symbolIndex < specialChars.Length)
    //                {
    //                    var buttonCell = new DataGridViewButtonCell
    //                    {
    //                        Value = specialChars[symbolIndex] // Назначаем символ кнопке
    //                    };
    //                    buttonCell.FlatStyle = FlatStyle.Flat; // Убираем границы кнопки
    //                    buttonGrid[col, row] = buttonCell;
    //                    symbolIndex++;
    //                }
    //            }
    //        }

    //        // Добавляем обработчик события CellClick
    //        buttonGrid.CellClick += ButtonGrid_CellClick;

    //        // Добавляем таблицу на форму
    //        this.Controls.Add(buttonGrid);
    //    }

    //    private void InitializeTextBox()
    //    {
    //        // Создаем TextBox и настраиваем его
    //        displayTextBox = new TextBox
    //        {
    //            Dock = DockStyle.Bottom,
    //            ReadOnly = true,
    //            TextAlign = HorizontalAlignment.Center
    //        };

    //        // Добавляем TextBox на форму
    //        this.Controls.Add(displayTextBox);
    //    }

    //    private void ButtonGrid_CellClick(object sender, DataGridViewCellEventArgs e)
    //    {
    //        // Проверяем, что ячейка содержит кнопку
    //        if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && ((DataGridView)sender)[e.ColumnIndex, e.RowIndex] is DataGridViewButtonCell)
    //        {
    //            // Получаем символ кнопки и добавляем его в TextBox
    //            string buttonSymbol = ((DataGridView)sender)[e.ColumnIndex, e.RowIndex].Value.ToString();
    //            displayTextBox.Text += buttonSymbol;
    //        }
    //    }
    //}


    //public partial class Form1 : Form
    //{
    //    private TextBox displayTextBox;

    //    public Form1()
    //    {
    //        InitializeComponent();
    //        InitializeButtonGrid();
    //        InitializeTextBox();
    //    }

    //    private void InitializeButtonGrid()
    //    {
    //        // Создаем таблицу и устанавливаем базовые параметры
    //        DataGridView buttonGrid = new DataGridView
    //        {
    //            ColumnCount = 5,
    //            RowCount = 3,
    //            Dock = DockStyle.Top,
    //            AllowUserToAddRows = false,
    //            AllowUserToDeleteRows = false,
    //            ReadOnly = true,
    //            Height = 150
    //        };

    //        // Настраиваем стиль ячеек, чтобы убрать фон и границы
    //        buttonGrid.DefaultCellStyle.BackColor = this.BackColor; // Фон, соответствующий фону формы
    //        buttonGrid.DefaultCellStyle.SelectionBackColor = this.BackColor; // Фон при выборе
    //        buttonGrid.DefaultCellStyle.SelectionForeColor = Color.Black; // Цвет текста при выборе
    //        buttonGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single; // Сетка ячеек

    //        // Создаем кнопки и присваиваем им номера от 1 до 15
    //        int buttonNumber = 1;
    //        for (int row = 0; row < buttonGrid.RowCount; row++)
    //        {
    //            for (int col = 0; col < buttonGrid.ColumnCount; col++)
    //            {
    //                var buttonCell = new DataGridViewButtonCell
    //                {
    //                    Value = buttonNumber.ToString() // Назначаем номер кнопки
    //                };
    //                buttonCell.FlatStyle = FlatStyle.Flat; // Убираем границы кнопки
    //                buttonGrid[col, row] = buttonCell;
    //                buttonNumber++;
    //            }
    //        }

    //        // Добавляем обработчик события CellClick
    //        buttonGrid.CellClick += ButtonGrid_CellClick;

    //        // Добавляем таблицу на форму
    //        this.Controls.Add(buttonGrid);
    //    }

    //    private void InitializeTextBox()
    //    {
    //        // Создаем TextBox и настраиваем его
    //        displayTextBox = new TextBox
    //        {
    //            Dock = DockStyle.Bottom,
    //            ReadOnly = true,
    //            TextAlign = HorizontalAlignment.Center
    //        };

    //        // Добавляем TextBox на форму
    //        this.Controls.Add(displayTextBox);
    //    }

    //    private void ButtonGrid_CellClick(object sender, DataGridViewCellEventArgs e)
    //    {
    //        // Проверяем, что ячейка содержит кнопку
    //        if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && ((DataGridView)sender)[e.ColumnIndex, e.RowIndex] is DataGridViewButtonCell)
    //        {
    //            // Получаем имя кнопки и записываем его в TextBox
    //            string buttonName = ((DataGridView)sender)[e.ColumnIndex, e.RowIndex].Value.ToString();
    //            displayTextBox.Text = $"Нажата кнопка: {buttonName}";
    //        }
    //    }
    //}
}
