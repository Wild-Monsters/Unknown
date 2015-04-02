using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace WildMonsters
{
	public struct GridProperties
	{
		public int height, width, cellSize, startRows;
		public float xMargin, yMargin;
		//to know which player we are
		public bool flipped;
		public float top;
	};
	
	public class LevelGrid
	{	
		private GridProperties props;
		private Ball[,] grid;
		private LevelUI levelUI;
		private Ball[] shootables;
		private	Timer timer;
		private Timer time2;

	
		
		public LevelGrid (GridProperties _properties, LevelUI _levelUI)
		{
			timer = new Timer();
			time2 = new Timer();
			props = _properties;
			
			grid = new Ball[props.height, props.width];
			
			levelUI = _levelUI;
						
			for(int a = 0; a < props.height; a++)
			{
				for(int b = 0; b < props.width; b++)
				{
					grid[a,b] = null;
				}
			}
			
		}
		public void UpdateBallPositions(LevelGrid Lvlgrid)
		{
			GridProperties props = Lvlgrid.GetProperties();
			Ball[,] grid = Lvlgrid.getBalls();
			
			for(int x = 0; x < props.height; x++)
			{
				for(int y = 0; y < props.width; y++)
				{
					if(y > 0 && grid[x,y] !=  null && grid[x , y - 1] == null)
					{ 
						//grid[x, y] we will save 
						grid[x, y - 1] = grid[x,y] ;
						grid[x, y] = null;	
					}	
				}
			}
		}

		public void Update(float t)
		{
			levelUI.Update(t);
			props.top = levelUI.divider.Top;
					
			for(int y = 0; y < props.height; y++)
			{
				for(int x = 0; x < props.width; x++)
				{
					if(grid[y,x] != null)
					{
						float gridPositionX;
						float gridPositionY;
						
						if(props.flipped) //flipped = which way the grid is facing.
						{
							gridPositionX = ((props.top - props.xMargin) - (props.cellSize*x))-props.cellSize;
							gridPositionY = props.yMargin + (props.cellSize*y);
						}
						else
						{
							gridPositionX = (props.top + props.xMargin) + (props.cellSize*x);
							gridPositionY = props.yMargin + (props.cellSize*y);
						}
						
						if(GameOver(gridPositionX, props))
						{
							//Console.WriteLine("SUPER GAME OVERUUU CHAN");
						}
						
						grid[y,x].SetGridPosition(gridPositionX, gridPositionY);
						grid[y,x].Update ();
					}
				}
			}
		}
				public void StoreTopLevelBalls(LevelGrid Lvlgrid, Player player, Scene scene)
		{
			//colour refers to current player colour
			//this method is used for the ai part of the game 
			//it grabs and stores the balls that the ai can shoot at 
			GridProperties props = Lvlgrid.GetProperties();
			Ball[,] grid = Lvlgrid.getBalls();
			Colour playerColour = player.GetCurrentColour();
			
			shootables = new Ball[10];
			int count = 0;
			for(int x = 0; x < props.height;x++)
			{
				for(int y = 0; y < props.width; y++)
				{
					if(y >= 0 && grid[x,y] !=  null && grid[x , y + 1] == null)
					{ 			
						//grid[x, y] we will save 
						shootables[count] = grid[x,y];
						count++;
					}	
				}
			}
			//Currently we have the correct colours of the top level grid 
			Console.WriteLine("-----------------------------------");
			for (int i =0; i< 10; i++ )
			{
				if(shootables[i] != null)
				{
					Console.WriteLine(shootables[i].GetColour().ToString());
				}
			}
			Console.WriteLine("-----------------------------------");
			
			//Determine which one to fire at 
			Random rand = new Random();
			int theOne = rand.Next(0, count);
			
			if(shootables[theOne] != null && playerColour == shootables[theOne].GetColour())//this means that the player can make a match 
			{
				if(player.Sprite.Position.Y > shootables[theOne].Sprite.Position.Y)
				{
					player.MoveDown();		
					Console.WriteLine("Player move down");
				}
				if(player.Sprite.Position.Y < shootables[theOne].Sprite.Position.Y)
				{
					player.MoveUp();		
					Console.WriteLine("Player move down");
				}
				if(player.Sprite.Position.Y == shootables[theOne].Sprite.Position.Y)
				{
					if(timer.Seconds() >= 1.0)
					{
						player.Fire(scene);
						timer.Reset();
						timer = new Timer();
					}
				}
			}
			else
			{
				//move randomly and shoot 	
				if(time2.Seconds() >= 1.0)
				{
					player.Fire(scene);	
					time2.Reset();
					time2 = new Timer();
				}
			}
		}

       

		public void SearchGrid(int xPos, int yPos, int matchesNeeded)
		{
			List<Vector2i> searchList = new List<Vector2i>();
			List<Vector2i> specialList = new List<Vector2i>();
			
			searchList.Add (new Vector2i(xPos,yPos));
			
			//The colour that we're searching for
			Colour targetColour =  grid[yPos,xPos].GetColour();
			
			int searchIndex = 0;
			Vector2i target = searchList[0];
			
			//Keep searching blocks until there's no more in the list
			while(searchIndex < searchList.Count)
			{
				//Start searching from the next block in the list
				target = searchList[searchIndex];
				
				//Search the blocks horizontally adjacent to the target
				for(int x = -1; x <= 1; x+=2)
				{
					int targetY = target.Y;
					int targetX = target.X+x;
					
					//This function checks the position of the current brick
					if(CompareGridPosition (targetX, targetY))
					{
						Colour thisColour = grid[targetY, targetX].GetColour();
						
						//Check the colour of the brick
						if(thisColour == targetColour)
						{
							Vector2i newVec = new Vector2i(targetX, targetY);
					
							//If the current block isn't already in the search list then add it
							if(!searchList.Contains(newVec))
							{
								searchList.Add(newVec);
							}
						}
						else
						{
							specialList.Add(new Vector2i(targetX, targetY));
						}
					}
				}
				
				//Search the blocks vertically adjacent to the target
				for(int y = -1; y <= 1; y+= 2)
				{
					int targetY = target.Y+y;
					int targetX = target.X;
					
					//This function checks the position and colour of the current brick
					if(CompareGridPosition (targetX, targetY))
					{
						Colour thisColour = grid[targetY, targetX].GetColour();
						
						//Check the colour of the brick
						if(thisColour == targetColour)
						{
							Vector2i newVec = new Vector2i(targetX, targetY);
					
							//If the current block isn't already in the search list then add it
							if(!searchList.Contains(newVec))
							{
								searchList.Add(newVec);
							}
						}
						else
						{
							specialList.Add(new Vector2i(targetX, targetY));
						}
					}
				}
				
				//"search the next block" 
				searchIndex++;
			}
			
			//If more than one matching colour was found
			if(searchIndex+1 > matchesNeeded)
			{
				for(int a = 0; a < searchList.Count; a++)
				{
					int targetY = (int)searchList[a].Y;
					int targetX = (int)searchList[a].X;
					
					grid[targetY, targetX].RemoveObject();
					grid[targetY, targetX] = null;
					
					const float pushStrength = 10;
					
					if(props.flipped)
					{
						levelUI.divider.TopTarget = levelUI.divider.TopTarget + pushStrength;
						levelUI.divider.P1Score += 1;
					}
					else
					{
						levelUI.divider.TopTarget = levelUI.divider.TopTarget - pushStrength;
						levelUI.divider.P2Score += 1;
					}
				}
				
				CheckSpecialCases (specialList);
				
				
			}
		}
		
		
		private bool CompareGridPosition(int targetX, int targetY)
		{
			//Check the search target is in-bounds
			//and then check if the colour matches the target's colour
			if(targetX >= 0 && targetX < props.width
			&& targetY >= 0 && targetY < props.height
			&& grid[targetY, targetX] != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		private void CheckSpecialCases(List<Vector2i> specialList)
		{
			for(int a = 0; a <specialList.Count; a++)
			{
				int targetX = specialList[a].X;
				int targetY = specialList[a].Y;
				
				// if position isn't null do this..
				if(CompareGridPosition(targetX, targetY))
				{
					Colour thisColour = grid[targetY, targetX].GetColour();
				
					switch(thisColour)
					{
						case Colour.Grey:
							//grid[targetY,targetX].RandomiseColour(false);
							BombBlock(targetX, targetY);
							grid[targetY, targetX].RemoveObject();
							grid[targetY, targetX] = null;
							
						break;
					}
				}
			}
		}
		

		
		
		public void SetTop(float x)
		{
			//The Top value is used to dictate where the "top" of the grid is, the center of the divider.
			props.top = x;
		}
		
		public void Draw(Scene _scene)
		{
			//Adds objects to the scene, the Director handles the actual drawing automatically
			for(int a = 0; a < props.height; a++)
			{
				for(int b = 0; b < props.startRows; b++)
				{
					Ball ball = new Ball(_scene, props.flipped);
					ball.RandomiseColour(true);
					grid[a,b] = ball;
				}
			}
		}
		
		public Ball[,] getBalls()
		{
			return this.grid;
		}
		
		public Bounds2 GetBounds(int i, int x)//row and column
		{
			//how?
			return grid[i,x].GetBounds();	
		}
		
		//write a function that gets a position in the grid 
		//write a function that adds a block at that position in the grid now 
		
		public GridProperties GetProperties()
		{
			return props;
		}
		
		public List<Colour> GetColoursOnGrid()
		{
			List<Colour> colours = new List<Colour>();
			
			for(int x = 0; x < props.width; x++)
			{
				for(int y = 0; y < props.height; y++)
				{
					if(grid[y,x] != null)
					{
						if(grid[y,x].GetColour () != Colour.Grey && !colours.Contains (grid[y,x].GetColour()))
						{
							colours.Add (grid[y,x].GetColour ());
						}
					}
				}
			}
			
			if(colours.Count <= 0)
			{
				for(int a = 0; a < 5; a++)
				{
					colours.Add ((Colour)(a));
				}
			}
			
			return colours;
		}
		
		// Bomb Test Method
		public void BombBlock(int targetX, int targetY)
		{
			// Delete blocks left and right of bomb block
			for(int x = -1; x <= 1; x+=2)
			{
			    if(CompareGridPosition(targetX + x, targetY))
				{
					grid[targetY, targetX + x].RemoveObject();
					grid[targetY, targetX + x] = null;
				}
			}
			
			// Delete blocks north and south of bomb block
			for(int y = -1; y <= 1; y+= 2)
			{
				if(CompareGridPosition(targetX, targetY + y))
				{
					grid[targetY + y, targetX].RemoveObject();
					grid[targetY + y, targetX] = null;
				}
			}
		}
		
		public bool GameOver(float gridPosX, GridProperties props)
		{
			if(props.flipped)
			{
				if(gridPosX < 60)
				{
					Console.WriteLine("Player Two wins!");
					return true;
				}
			}
			else
			{
				if(gridPosX > 840)
				{
					Console.WriteLine("Player One wins!");
					
					return true;
				}
			}
			return false;	
		}
	}
}

