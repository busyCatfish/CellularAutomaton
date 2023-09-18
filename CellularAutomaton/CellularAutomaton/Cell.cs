using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.CellularAutomaton
{
	public class Cell
	{
		private readonly int x;
		private readonly int y;
		private EnumState futureState;
		private EnumState currentState;

		public Cell(int x, int y, EnumState currentState)
		{
			this.x = x;
			this.y = y;
			this.currentState = currentState;
			this.futureState = EnumState.Passive;
		}

		public Cell(int x, int y)
		{
			this.x = x;
			this.y = y;
			this.currentState = EnumState.Passive;
			this.futureState = EnumState.Passive;
		}

		public EnumState CurrentState { get { return this.currentState; } }
		
		public int X { get { return this.x; } }

		public int Y { get { return this.y; } }

		public void Kill()
		{
			this.futureState = EnumState.Passive;
		}

		public void Born()
		{
			this.futureState = EnumState.Active;
		}

		public void Alive()
		{
			this.futureState = this.currentState;
		}

		public void NextLifeCycle()
		{
			this.currentState = this.futureState;
		}
	}
}
