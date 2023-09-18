using CellularAutomaton.CellularAutomaton;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellularAutomaton
{
	public class SimulatorCellularautomaton
	{
		private readonly int height;
		private readonly int width;
		private readonly DataGridView dataGridView;
		private bool isStart = false;
		private CancellationToken cancellationToken;
		private CancellationTokenSource cancellationTokenSource;

		public SimulatorCellularautomaton(int height, int width, DataGridView dataGridView)
		{
			if(height < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(height), "Висота таблиці не може бути менше 1");
			}

			if (width < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(width), "Ширина таблиці не може бути менше 1");
			}

			this.height = height;
			this.width = width;
			this.dataGridView = dataGridView;
			this.CreateTable();
		}

		private void CreateTable()
		{
			for (int i = 0; i < this.width; i++)
			{
				DataGridViewColumn column = new DataGridViewColumn()
				{
					HeaderText = "",
					Name = "",
					CellTemplate = new DataGridViewTextBoxCell()
				};

				column.Width = 30;

				this.dataGridView.Columns.Add(column);
			}

			this.dataGridView.RowTemplate.Height = 30;
			for (int i = 0; i < this.height; i++)
			{
				DataGridViewRow row = new DataGridViewRow();
				this.dataGridView.Rows.Add(row);
			}
		}

		private IEnumerable<Coordinate> DetermineAliveCells()
		{
			var aliveCells = new List<Coordinate>();

			for (int row = 0; row < this.height; row++)
			{
				for (int col = 0; col < this.width; col++)
				{
					DataGridViewCell cell = this.dataGridView[col, row];

					if (cell.Style.BackColor == Color.Red)
					{
						aliveCells.Add(new Coordinate(row, col));
					}
				}
			}

			return aliveCells;
		}

		private void DrawCellsByState(IEnumerable<Cell> cells)
		{
			foreach (var cell in cells)
			{
				DataGridViewCell tableCell = this.dataGridView[cell.Y, cell.X];
				if(cell.CurrentState == EnumState.Active)
				{
					tableCell.Style.BackColor = Color.Red;
				}
				else
				{
					tableCell.Style.BackColor = Color.White;
				}
			}
		}

		public async Task StartLife()
		{
			if (!this.isStart)
			{
				this.cancellationTokenSource = new CancellationTokenSource();
				this.cancellationToken = cancellationTokenSource.Token;
				this.isStart = true;
				var aliveCells = this.DetermineAliveCells();
				Automaton automaton = new Automaton(height, width, aliveCells);

				//try
				//{
				//	while (!cancellationToken.IsCancellationRequested)
				//	{
				//		var changedCells = automaton.LifeCycle();

				//		this.DrawCellsByState(changedCells);
				//		//DataGridViewCell tableCell = this.dataGridView[1, 1];
				//		//tableCell.Style.BackColor = Color.Black;
				//		await Task.Delay(1000);
				//	}
				//}
				//catch (OperationCanceledException)
				//{
				//	Console.WriteLine("Цикл скасовано.");
				//}
				while (!cancellationToken.IsCancellationRequested)
				{
					var changedCells = automaton.LifeCycle();

					this.DrawCellsByState(changedCells);
					await Task.Delay(1000);
				}
			}
			else throw new InvalidOperationException("Automaton is always going");
		}

		public void StopLife() 
		{
			if (this.isStart)
			{
				this.cancellationTokenSource.Cancel();
				this.isStart = false;
			}
		}
	}
}
