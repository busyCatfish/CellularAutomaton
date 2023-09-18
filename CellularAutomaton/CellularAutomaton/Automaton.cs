using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.CellularAutomaton
{
	public class Automaton
	{
		private readonly int height;
		private readonly int width;
		private Cell[,] cells;
		private Coordinate[] neighbours = new Coordinate[] {
				new Coordinate(-1, -1), new Coordinate(-1, 0),
				new Coordinate(-1, 1), new Coordinate(0,1), new Coordinate(1,1), new Coordinate(1,0),
				new Coordinate(1,-1), new Coordinate(0,-1)
			};

		public Automaton(int height, int width, IEnumerable<Coordinate> aliveCells)
		{
			if (height < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(height), "Висота таблиці не може бути менше 1");
			}

			if (width < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(width), "Ширина таблиці не може бути менше 1");
			}

			this.height = height;
			this.width = width;
			this.GenerateDefaultCells();
			this.GenerateAliveCells(aliveCells);
		}

		//Генерує двовимірний масив із клітинами у стані за замовченням( passive )
		private void GenerateDefaultCells()
		{
			cells = new Cell[height, width];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					cells[i, j] = new Cell(i, j);
				}
			}
		}

		//Визначаємо, які клітини живі за умовами користувача
		private void GenerateAliveCells(IEnumerable<Coordinate> aliveCells)
		{
			foreach (var cell in aliveCells)
			{
				this.cells[cell.X, cell.Y] = new Cell(cell.X, cell.Y, EnumState.Active);
			}
		}

		//Проходить один цикл життя і повертає змінені клітини
		public IEnumerable<Cell> LifeCycle()
		{
			List<Cell> changedCells = new List<Cell>();

			foreach (Cell cell in this.cells)
			{
				int neighboursOfCell = this.CountNeighboursOfCell(cell);
				if (neighboursOfCell == 3 && cell.CurrentState == EnumState.Passive)
				{
					cell.Born();
					changedCells.Add(cell);
				}
				else if (cell.CurrentState == EnumState.Active)
				{
					if ((neighboursOfCell == 3 || neighboursOfCell == 2))
					{
						cell.Alive();
					}
					else if (neighboursOfCell < 2 || neighboursOfCell > 3)
					{
						cell.Kill();
						changedCells.Add(cell);
					}
				}
			}

			foreach (Cell cell in changedCells)
			{
				cell.NextLifeCycle();
			}

			return changedCells;
		}

		//Шукає кількість сусідів у клітини
		private int CountNeighboursOfCell(Cell cell)
		{
			int count = 0;

			foreach (Coordinate neighbour in this.neighbours)
			{
				int neighbourX = neighbour.X + cell.X;
				int neighbourY = neighbour.Y + cell.Y;

				if (neighbourX < 0 || neighbourX >= this.width || neighbourY < 0 || neighbourY >= this.height)
				{
					continue;
				}

				if (this.cells[neighbourX, neighbourY].CurrentState == EnumState.Active)
				{
					count++;
				}
			}

			return count;
		}
	}
}
