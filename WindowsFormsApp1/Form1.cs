using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add("Все элементы");
            comboBox1.Items.AddRange(symbolTable1.GroupNames);
            comboBox1.SelectedIndex = 0;
        }

        private void symbolTable1_CurrentCellChanged(object sender, EventArgs e)
        {
            textBox1.Text = char.TryParse(symbolTable1.CurrentCell?.Value?.ToString() ?? string.Empty, out char value)
                ? $"U+{(int)value:X4}"
                : string.Empty;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            symbolTable1.CurrentGroupIndex = comboBox1.SelectedIndex - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var value = symbolTable1.CurrentCell?.Value?.ToString();
            if (!string.IsNullOrEmpty(value))
            {
                textBox2.Text += value;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox2.Text);
        }
    }
}
