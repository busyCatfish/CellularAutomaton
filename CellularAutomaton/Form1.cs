using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellularAutomaton
{
	public partial class Form1 : Form
	{
		SimulatorCellularautomaton simulator;
		public Form1()
		{
			InitializeComponent();

			dataGridView1.Size = new Size(20*30 + 5, 15*30);

			dataGridView1.SelectionChanged += (sender, e) =>
			{
				dataGridView1.ClearSelection();
			};

			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridView1.ReadOnly = true;
			dataGridView1.RowHeadersVisible = false;
			dataGridView1.ColumnHeadersVisible = false;

			simulator = new SimulatorCellularautomaton(20,20, dataGridView1);
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			try
			{
				await simulator.StartLife();
			}
			catch (OperationCanceledException)
			{
				MessageBox.Show("Автомат зупинен!");
			}
			catch (InvalidOperationException)
			{
				MessageBox.Show("Ваш клітинний автомат вже запущений!");
			}
			catch (ArgumentOutOfRangeException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			simulator.StopLife();
		}

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

				if (cell.Style.BackColor == Color.Red)
				{
					cell.Style.BackColor = Color.White;
				}
				else
				{
					cell.Style.BackColor = Color.Red;
				}
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					cell.Style.BackColor = Color.White;
				}
			}
		}
	}
}
