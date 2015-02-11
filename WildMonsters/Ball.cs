using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
//using Sce.PlayStation.HighLevel.UI;

namespace WildMonsters
{
	public enum Colour {Red, Blue, Yellow, Purple, Green, Grey};
	public enum BallState {Rising, Locked, Falling};
	
	public class Ball
	{
		private Scene parent;
		private SpriteUV sprite;
		private TextureInfo texInfo;
		private Colour colour;
		
		//private int colourAmount = 5;
		private int speed = 10;
		
		private BallState state;
		
		private Bounds2 bounds;
		private Vector2 gridPosition;
		
		private Vector2 offsetPosition;
		private Vector2 velocity;
		private float weight;
		private float elasticEnergy = 100.0f;
		
		private bool onLeftSide;
		
		
		public Ball (Scene scene, bool onLeftSide)
		{
			parent = scene;
			
			state = BallState.Locked;
			
			//weight = WMRandom.GetNextInt(8,12,this.GetHashCode ());
			weight = 10;
			
			texInfo = new TextureInfo("/Application/textures/Blocks2.png");
			
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			
			this.onLeftSide = onLeftSide;
			if(onLeftSide)
			{
				sprite.Position = new Vector2(0.0f, 544.0f/2);
			}
			else
			{
				sprite.Position = new Vector2(960.0f, 544.0f/2);
			}
			
			gridPosition = new Vector2(0.0f,0.0f);
			
			SetColour (Colour.Grey);
			
			scene.AddChild(sprite);
		}
		
		public Bounds2 GetBounds()
		{
			sprite.GetContentWorldBounds(ref bounds);
			
			return this.bounds;
		}
		
		public SpriteUV Sprite
		{
			get{ return sprite; }
			set{ sprite = value; }
		}
		
		
		public void Update()
		{
			//TODO: Logic to travel up to and/or lock on to grid herel
			switch(state)
			{
			case BallState.Rising:
				if(onLeftSide)
				{
					sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);
				}
				else
				{
					sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);
				}
				break;
				
			case BallState.Locked:
				
				Vector2 DistanceVector = gridPosition-sprite.Position;
				
				if(DistanceVector.Length () > 1.0f)
				{
					
					Vector2 DesiredVelocity = DistanceVector.Normalize () * (DistanceVector.Length ()/weight);

					sprite.Position += DesiredVelocity - velocity;
				}
				else
				{
					sprite.Position = gridPosition;
				}

				
				break;
			}
				
			
		}
		
		public void Cleanup()
		{
			texInfo.Dispose();
			sprite = null;
			//alive = false;
		}
		
		public void RemoveObject()
		{
			parent.RemoveChild (this.Sprite, true);
		}
		
		
		//Getters and Setters
		public void SetState(BallState peanut)
		{
			this.state = peanut;
		}
		
		public BallState GetState()
		{
			return this.state;
		}
	
		public Colour GetColour()
		{
			return colour;
		}
		
		public void SetColour(Colour col)
		{
			colour = col;
			
			float spriteWidth = 1.0f / 6.0f;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			sprite.UV.T = new Vector2(spriteWidth * (int)colour, 0.0f);	
		}
		
		public void RandomiseColour(bool specials)
		{
			Colour nextColour;

			//Generate special blocks or not? (grey blocks etc)
			if(specials)
			{
				nextColour = (Colour)(int)FMath.Floor(WMRandom.GetNextInt(0,6,this.GetHashCode ()));
			}
			else
			{
				nextColour = (Colour)(int)FMath.Floor(WMRandom.GetNextInt(0,5,this.GetHashCode ()));
			}
			
			SetColour (nextColour);
		}
		
		public void SetGridPosition(float x, float y)
		{
			gridPosition = new Vector2(x,y);
		}
		
		
	}
}
