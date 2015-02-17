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
		//private bool alive;
		private Colour colour;
		BallState state;
		private int speed = 10;
		private Bounds2 bounds;

		private int colourAmount = 5;
		
		
		public Ball (Scene scene)
		{
			parent = scene;
			
			state = BallState.Locked;
			
			texInfo = new TextureInfo("/Application/textures/Blocks.png");
			
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			SetColour (Colour.Grey);
			
			//TODO: Initialise ball position to be at the position of the cannon
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
		
		
		public void Update(bool playerLeftOfScreen)
		{
			//TODO: Logic to travel up to and/or lock on to grid herel
			if(state == BallState.Rising)
			{
				if(playerLeftOfScreen)
				{
					sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);
					Vector2 randPos = new Vector2(ParticleManager.CreateRandomPosition().X, ParticleManager.CreateRandomPosition().Y);
					ParticleManager.AddParticle(parent, new Vector2(sprite.Position.X, sprite.Position.Y + (float)randPos.Y), 1, 2, 0, colour);
				}
				else
				{
					sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);
					Vector2 randPos = new Vector2(ParticleManager.CreateRandomPosition().X, ParticleManager.CreateRandomPosition().Y);
					ParticleManager.AddParticle(parent, new Vector2(sprite.Position.X + 40.0f, sprite.Position.Y + (float)randPos.Y), 1, 2, 0, colour);
				}
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
			int quadAssign = 0;
			// Produce 10 particles per call
			for(int i = 0; i < 16; i++)
			{
				quadAssign++;
				if(quadAssign >= 4)
				{
					quadAssign = 0;
				}
				Vector2 randPos = new Vector2(ParticleManager.CreateRandomPosition().X, ParticleManager.CreateRandomPosition().Y);
				ParticleManager.AddParticle (parent, new Vector2(sprite.Position.X + (float)randPos.X,
					sprite.Position.Y + (float)randPos.Y), 1, 0, quadAssign, colour);
			}
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

			//Generate grey blocks or not? 
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
		
		
	}
}

