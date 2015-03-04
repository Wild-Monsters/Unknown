using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
//using Sce.PlayStation.HighLevel.UI;

namespace WildMonsters
{
	public enum Colour {Red, Blue, Yellow, Purple, Green, Grey};
	public enum BallState {Rising, Locked, Falling, Nostate};
	
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
		private Vector2 velocity;
		
		private bool onLeftSide;
		
		
		public Ball (Scene scene, bool onLeftSide)
		{
			//Initialise a bunch of stuff
			parent = scene;
			state = BallState.Locked;
			this.onLeftSide = onLeftSide;
			
			//Create/initialise the sprite and texture objects
			texInfo = new TextureInfo("/Application/textures/Blocks4.png");
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			
			SetColour (Colour.Grey);
			
			//Initialise position values
			SetInitialPosition(onLeftSide);
		
			//Rotate sprite to face the side of the screen it's on
			SetAngleOfSprite(onLeftSide);
			
			//Add to scene
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
					ParticleManager.AddLeftTrail(parent, sprite.Position, colour);
				}
				else
				{
					sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);
					ParticleManager.AddRightTrail(parent, sprite.Position, colour);
				}
				break;
				
			case BallState.Falling:
				if(onLeftSide)
				{
					sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);
				}
				else
				{
					sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);
				}
				break;
				
			case BallState.Locked:
					
				Vector2 DistanceVector = gridPosition-sprite.Position;
				
				if(DistanceVector.Length () > 1.0f)
				{
					Vector2 DesiredVelocity = DistanceVector.Normalize () * (DistanceVector.Length ()/speed);

					sprite.Position += DesiredVelocity - velocity;
				}
				else
				{
					sprite.Position = gridPosition;
				}

				break;
			case BallState.Nostate:
				//Temporary
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
			ParticleManager.AddExplosion(parent, sprite.Position, colour); 
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
		
		public bool OnLeftSide
		{
			get{ return onLeftSide; }
			set{ onLeftSide = value; }
		}
		
		private void SetAngleOfSprite(bool onLeftSide)
		{
			//Set the point of rotation to the center of the sprite
			sprite.Pivot = new Vector2(25,25);
			
			//Rotate sprite left or right depending on the side of the screen
			if(onLeftSide)
			{
				sprite.Angle = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Deg2Rad(-90.0f);
			}
			else
			{
				sprite.Angle = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Deg2Rad(90.0f);
			}
		}
		
		private void SetInitialPosition(bool onLeftSide)
		{
			gridPosition = new Vector2(0.0f,0.0f);
			
			if(onLeftSide)
			{
				sprite.Position = new Vector2(0.0f, 544.0f/2);
			}
			else
			{
				sprite.Position = new Vector2(960.0f, 544.0f/2);
			}
		}
		
		
	}
}

