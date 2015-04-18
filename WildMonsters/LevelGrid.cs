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
		public bool powerUps;
	};
	
	public class LevelGrid
	{	
		private GridProperties props;
		private Ball[,] grid;
		private LevelUI levelUI;
		private Ball[] shootables;
		private Ball[] matchedBalls;
		private	Timer timer;
		private Timer time2;
		private Random random; 
        private bool canShoot;
        private const double AITimeDelay = 700.0;
		private GameScene gamescene;
		private Timer delayGameOver;
		private Timer delayGameOver2;
		private bool onOffTimer;
		private AudioManager audio;
		private AIGameScene AiGameScene;
		private int playerWon = 0;
		private string gameSceneName;
		public LevelGrid (GridProperties _properties, LevelUI _levelUI, GameScene _gamescene, AIGameScene SetAiGameScene)
		{
			if(_gamescene != null)
			{
				gameSceneName = _gamescene.GetName();
			}
			if(SetAiGameScene != null)
			{
				AiGameScene = SetAiGameScene;
				gameSceneName = AiGameScene.GetName();
			}
			if(audio == null)
			{
				audio = new AudioManager();
			}
			
			timer = new Timer();
			time2 = new Timer();
			random = new Random();
			props = _properties;
			
			grid = new Ball[props.height, props.width];
			
			levelUI = _levelUI;
			gamescene = _gamescene;
						
			for(int a = 0; a < props.height; a++)
			{
				for(int b = 0; b < props.width; b++)
				{
					grid[a,b] = null;
				}
			}
			onOffTimer = false;
		}
		
		
		public void UpdateBallPositions()
		{		
			for(int x = 0; x < props.height; x++)
			{
				for(int y = 0; y < props.width; y++)
				{
					if(y > 0 && grid[x,y] !=  null && grid[x , y - 1] == null)
					{ 
						grid[x, y - 1] = grid[x,y] ;
						grid[x, y] = null;	
					}	
				}
			}
		}

		public void Update(float t, ref AudioManager audioMag)
		{
			if (onOffTimer && delayGameOver == null)
			{
				delayGameOver = new Timer();
				onOffTimer = false;
			}
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
						
						grid[y,x].SetGridPosition(gridPositionX, gridPositionY);
						grid[y,x].Update ();
						
						GameOver(gridPositionX, props, ref  audio);
				
					}
				}
			}
			Console.WriteLine(gameSceneName);
//			if(!(props.flipped))
//			{
//				this.DestroyAll();
//			}
			
			ZeroBlocksGameOver();
			
			if(delayGameOver != null)
			{
			//	Console.WriteLine(onOffTimer +"    " + delayGameOver.Milliseconds());		
				if(delayGameOver.Milliseconds() > 1200.0)
				{
					audioMag.StopGameMusic();	
					audioMag.Dispose();
					Director.Instance.ReplaceScene (new EndGameScene(playerWon, true, gameSceneName));
				}
			}
		}
		public void StoreTopLevelBalls(LevelGrid Lvlgrid, Player player, Scene scene)
		{
			//colour refers to current player colour
			//this method is used for the ai part of the game 
			//it grabs and stores the balls that the ai can shoot at 
			Colour playerColour = player.GetCurrentColour();	
			
			matchedBalls = new Ball[10];
			
			canShoot = false;
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
			//create a different array of the correct colours 
			int counted = 0;
			for(int i =0; i < count; i++)
			{
				if(shootables[i].GetColour() == playerColour)
				{
					//save the shootable
					matchedBalls[counted] = shootables[i];
					counted++;
				}
			}
			
			//Determine which one to fire at 
			Random rand = new Random();
			int theOne = rand.Next(0, counted);
			
			if(matchedBalls[theOne] != null && playerColour == matchedBalls[theOne].GetColour())//this means that the player can make a match 
			{
				canShoot = true;
				
				if(player.Sprite.Position.Y > matchedBalls[theOne].Sprite.Position.Y && time2.Milliseconds() >= AITimeDelay)
				{
					player.MoveDown();
					time2.Reset();
				}
				if(player.Sprite.Position.Y < matchedBalls[theOne].Sprite.Position.Y && time2.Milliseconds() >= AITimeDelay)
				{
					player.MoveUp();		
					time2.Reset();
				}
				if(player.Sprite.Position.Y == matchedBalls[theOne].Sprite.Position.Y)
				{
					player.AIFiresDifferently(scene);
					canShoot = false;

				}
			}
			else if(!canShoot)
			{
			
				MoveRandomnly(ref player);
				//move randomly and shoot 	
				player.AIFiresDifferently(scene);
				time2.Reset();
				time2 = new Timer();
			}
			
		}
		private void MoveRandomnly(ref Player player)
		{
			int dir = random.Next(0, 10);	 //0 - 5 = up 6-10 = down
			int maxLength = 0;
			//determine how much the ai can move by its position on the grid
			//using its direction so that it doesn't try to go over the grid
			string pos = player.Sprite.Position.Y.ToString();
			switch(pos)
			{
			case "500":
				{
					if(dir <= 5){
						maxLength = 9;
					}
					else{
						maxLength = 0;
					}
					break;
				}
			case "450":
				{
					if(dir <= 5){
						maxLength = 8;
					}
					else{
						maxLength = 1;
					}
				break;
				}
			case "400":
				{
					if(dir <= 5){
						maxLength = 7;
					}
					else{
						maxLength = 2;
					}
				break;
				}
			case "350":
				{
					if(dir <= 5){
						maxLength = 6;
					}
					else{
						maxLength = 3;
					}
				break;
				}
			case "300":
				{
					if(dir <= 5){
						maxLength = 5;
					}
					else{
						maxLength = 4;
					}
				break;
				}
			case "250":
				{
					if(dir <= 5){
						maxLength = 4;
					}
					else{
						maxLength = 5;
					}
				break;
				}
			case "200":
				{
					if(dir <= 5){
						maxLength = 3;
					}
					else{
						maxLength = 6;
					}
				break;
				}
			case "150":
				{
					if(dir <= 5){
						maxLength = 2;
					}
					else{
						maxLength = 7;
					}
				break;
				}
			case "100":
				{
					if(dir <= 5){
						maxLength = 1;
					}
					else{
						maxLength = 8;
					}
				break;
				}
			case "50":
				{
					if(dir <= 5){
						maxLength = 0;
					}
					else{
						maxLength = 9;
					}
				break;
				}
			}
			
			int length = random.Next(0, maxLength); //how much 
			for (int i = 0;i< length; i++)
			{
				if (dir <= 5)
				{
					player.MoveDown();
					//Console.WriteLine("direction 1 down and 0 up" + dir);
				}
				else if(dir >= 6)
				{
					player.MoveUp();
					//Console.WriteLine("direction 1 down and 0 up" + dir);
				}
			}
			
		}
		private bool MissOrNot(string difficutlty)
		{
			switch(difficutlty)
			{
				
			case "Easy":
				{
					if(true)
					{
						return true;
					}
					else 
					{
						return false;
					}
				}
			case "Medium":
				{
					if(true)
					{
						return true;
					}
					else 
					{
						return false;
					}
				}
				
			case "Hard":
				{
					if(true)
					{
						return true;
					}
					else 
					{
						return false;
					}
				}
			default:
				return true;
					
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
		
		
		public bool CompareGridPosition(int targetX, int targetY)
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
							grid[targetY, targetX].RandomiseColour(false, false);
							grid[targetY, targetX].AddExplosion();
						break;
						
						case Colour.Bomb:
							PowerUp.BombBlock(targetX, targetY, grid, this);
							RemoveAndNull(grid, targetX, targetY);
						break;
						
						case Colour.Rand:
							if(gamescene != null)
							{
								PowerUp.RandColour(props, gamescene);
								RemoveAndNull(grid, targetX, targetY);
							}
						break;
						
						case Colour.Stone:
							if(gamescene != null)
							{
								PowerUp.RandGrey(props, gamescene);
								RemoveAndNull(grid, targetX, targetY);
							}
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
					ball.RandomiseColour(true, false);
					grid[a,b] = ball;
				}
			}
			
			DrawPowerUps();
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
						if(grid[y,x].GetColour () != Colour.Grey
						&& grid[y,x].GetColour () != Colour.Bomb
						&& grid[y,x].GetColour () != Colour.Rand
						&& grid[y,x].GetColour () != Colour.Stone
						&& !colours.Contains (grid[y,x].GetColour()))
						{
							colours.Add (grid[y,x].GetColour ());
						}
					}
				}
			}

			
			return colours;
		}
		
		private void RemoveAndNull(Ball[,] grid, int targetX, int targetY)
		{
			grid[targetY, targetX].RemoveObject();
			grid[targetY, targetX] = null;
		}
		
		private void DrawPowerUps()
		{
			for(int i = 0; i < 3; i++)
			{
				Vector2i pos = GetRandomBlockPos();
				
				if(props.powerUps)
				{
					grid[pos.Y, pos.X].RandomiseColour(false, true);
				}
				else
				{
					grid[pos.Y, pos.X].RandomiseColour(true,false);
				}
			}
		}
		
		private Vector2i GetRandomBlockPos()
		{
			int x = WMRandom.GetNextInt(1, 3);
			int y = WMRandom.GetNextInt(1, 10);
			
			return new Vector2i(x, y);
		}
		
		public void GameOver(float gridPosX, GridProperties props, ref AudioManager audioMag)
		{
			if(props.flipped)
			{
				if(gridPosX < 60)
				{
					onOffTimer = true;
					this.DestroyAll();
					playerWon = 2;
					if(gamescene != null)
					{
						gamescene.SetGameOver();
					} 
					else if(AiGameScene != null)
					{
						AiGameScene.SetGameOver();
					}
				}
			}
			else
			{
				if(gridPosX > 840)
				{
					onOffTimer = true;
					this.DestroyAll();
					playerWon = 1;
					if(gamescene != null)
					{
						gamescene.SetGameOver();
					} 
					else if(AiGameScene != null)
					{
						AiGameScene.SetGameOver();
					}
				}
			}
		}
		
		public void ZeroBlocksGameOver()
		{
			if(props.flipped)
			{
				if(ZeroBlocks())
				{
					onOffTimer = true;
					playerWon = 1;
					if(gamescene != null)
					{
						gamescene.SetGameOver();
					} 
					else if(AiGameScene != null)
					{
						AiGameScene.SetGameOver();
					}
				}
			}
			else
			{
				if(ZeroBlocks())
				{
					onOffTimer = true;
					playerWon = 2;
					if(gamescene != null)
					{
						gamescene.SetGameOver();
					} 
					else if(AiGameScene != null)
					{
						AiGameScene.SetGameOver();
					}
				}
			}
		}
        
        public void DestroyAll()
        {
            for(int x = 0; x < props.width; x++)
            {
                for(int y = 0; y < props.height; y++)
                {
                    if(grid[y,x] != null)
                    {
                        grid[y,x].RemoveObject();
                        grid[y,x] = null;
                    }
                }
            }
        }
		
		public bool ZeroBlocks() // Checks to see if a player has successfully destroyed all of their own blocks. If so, end the game.
		{
			bool noBlocks = true;
			for(int x = 0; x < props.width; x++)
            {
                for(int y = 0; y < props.height; y++)
                {
					if(grid[y,x] != null)
					{
						noBlocks = false;
					}
				}
			}
			
			return noBlocks;
		}
	}
}

