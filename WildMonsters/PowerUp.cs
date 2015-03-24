using System;

namespace WildMonsters
{
	public static class PowerUp
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand;
		
		// Randomised the enemy's grey block locations
		public static void RandEnemyGrey(Ball[,] grid, GridProperties props)
		{
			int iDone = 2;
			int x = 0;
			int y = 0;
			// Have a while loop that says whilst iGrey > 0 do:
			while (iDone > 0)
			{
				// Generate a random number
				rand = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator(DateTime.Now.Millisecond);
				
				// Pick a random x below props.width
				x = (int)rand.NextFloat(0, (props.width - 1));
				// Pick a random y below props.height
				y = (int)rand.NextFloat(0, (props.height - 1));
				
				// If the block is not a grey, turn it into a grey block, then decrement iGrey.
				if(grid[y,x] != null)
				{
					if(grid[y,x].GetColour() != Colour.Grey)
					{
						grid[y,x].SetColour(Colour.Grey);
						iDone--;
					}
				}
				
				// If it IS a grey block, do nothing.
			}
		}
		
		// Decides which side will be randomised in grey, call this to call the RandEnemyGrey function.
		public static void RandGrey(GridProperties props, GameScene gamescene)
		{
			if(props.flipped)
			{
				RandEnemyGrey(gamescene.GetGrid2().getBalls(), props);
			}
			else
			{
				RandEnemyGrey(gamescene.GetGrid1().getBalls(), props);
			}
		}
		
		// Randomise Grid Colour Method
		public static void RandGridColour(Ball[,] grid, GridProperties props)
		{
			for(int x = 0; x < props.width; x++)
			{
				for(int y = 0; y < props.height; y++)
				{
					if(grid[y,x] != null)
					{
						if(grid[y,x].GetColour () != Colour.Grey
						&& grid[y,x].GetColour () != Colour.Bomb
						&& grid[y,x].GetColour () != Colour.Rand
						&& grid[y,x].GetColour () != Colour.Stone
						   )
						{
							grid[y, x].RandomiseColour(false);
						}
					}
				}
			}
		}
		
		// Picks which grid's colour will be randomised when the power-up has been activated, depending on who is calling the function.
		public static void RandColour(GridProperties props, GameScene gamescene)
		{
			if(props.flipped)
			{
				RandGridColour(gamescene.GetGrid2().getBalls(), props);
			}
			else
			{
				RandGridColour(gamescene.GetGrid1().getBalls(), props);
			}
		}
		
		// Bomb Powerup Method
		public static void BombBlock(int targetX, int targetY, Ball[,] grid, LevelGrid thisGrid)
		{
			// Delete blocks left and right of bomb block
			for(int x = -1; x <= 1; x+=2)
			{
			    if(thisGrid.CompareGridPosition(targetX + x, targetY))
				{
					grid[targetY, targetX + x].RemoveObject();
					grid[targetY, targetX + x] = null;
				}
			}
			
			// Delete blocks north and south of bomb block
			for(int y = -1; y <= 1; y+= 2)
			{
				if(thisGrid.CompareGridPosition(targetX, targetY + y))
				{
					grid[targetY + y, targetX].RemoveObject();
					grid[targetY + y, targetX] = null;
				}
			}
		}
	}
}

