using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace WildMonsters
{
	public enum ParticleState {Explosion, Smoke, Trail};
	
	public class Particle
	{
		// Private variables:
		SpriteUV sprite;
		Vector4 spriteColor;
		private ParticleState particleState;
		
		// Accessors:
		public Vector2 Velocity { get; set; }        // The speed of the particle at the current instance
		public float Angle { get; set; }            // The current angle of rotation of the particle
		public float AngularVelocity { get; set; }    // The speed that the angle is changing
		public int TTL { get; set; }                // The 'time to live' of the particle
		public SpriteUV Sprite { get{ return sprite; } }
		public bool bExplosion { get; set; }
		public Vector4 SpriteColor { get{return spriteColor;} set{spriteColor = value;} }
		public ParticleState ParticleStateVar {get{return particleState;} set{particleState = value;}}
		
		public Particle (Scene _scene, Vector2 position, int type, int quadAssign, Colour ballColour)
		{
			Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand;
			
			// Generate a random number
			rand = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator(DateTime.Now.Millisecond);
			
			particleState = (ParticleState)type;
			
			// Set velocity
			Vector2 velocity = new Vector2(2.0f, 2.0f);
			
			// If 0, it's an explosion effect
     		if(type == 0)
			{
				switch(quadAssign)
				{
				case 0:
					velocity.X *= rand.NextFloat(-1, 0);
					velocity.Y *= rand.NextFloat(0, 1);
					break;
				case 1:
					velocity.X *= rand.NextFloat(-1, 0);
					velocity.Y *= rand.NextFloat(-1, 0);
					break;
				case 2:
					velocity.X *= rand.NextFloat(0, 1);
					velocity.Y *= rand.NextFloat(-1, 0);
					break;
				case 3:
					velocity.X *= rand.NextFloat(0, 1);
					velocity.Y *= rand.NextFloat(0, 1);
					break;	
				}
			}
			
			// If 1, it's a fire effect
			else if(type == 1)
			{
				velocity = new Vector2((1f * 0),(1f * 1f));
			}
			
			// If 2, left-side trail
			else if(type == 2)
			{
				velocity = new Vector2((1f * 1f),(1f * 0f));
			}
			
			// Shield
			// if input == 2
			
			// Set angle
			float angle = 0;
			
			// Set angular velocity
    		float angularVelocity = 0.1f * (float)(rand.NextFloat(1, 2));
			
			// Set time to live
			int ttl = 200 + (int)rand.NextFloat(0,40);
			
			Velocity = velocity; 
			Angle = angle;
			AngularVelocity = angularVelocity;
			TTL = ttl;
			
			TextureInfo texInfo = new TextureInfo("/Application/textures/Particle.png");
			
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = position;
			
			//Scale is given a random value between 0.5 and 1.5
			float scale = (float)(WMRandom.GetNextInt (0,100)+50)/100.0f;
			float scaleMod = 2.0f;
			sprite.Scale = new Vector2(scale*scaleMod,scale*scaleMod);
			
			
			
			// Random Colour
//			spriteColor = new Vector4((float)rand.NextFloat(0, 1),
//            				    (float)rand.NextFloat(0, 1),
//            				    (float)rand.NextFloat(0, 1),
//			                    (float)1);
			
			
			switch(ballColour)
			{
			case Colour.Red:
				spriteColor = new Vector4(1.0f, rand.NextFloat(-0.25f, 0.25f), rand.NextFloat(-0.25f, 0.25f), 1.0f);
				break;
			case Colour.Blue:
				spriteColor = new Vector4(rand.NextFloat(-0.25f, 0.25f), 1.0f, 1.0f, 1.0f);
				break;
			case Colour.Yellow:
				spriteColor = new Vector4(1.0f, 1.0f, rand.NextFloat(-0.25f, 0.25f), 1.0f);
				break;
			case Colour.Purple:
				spriteColor = new Vector4(128.0f, rand.NextFloat(-0.25f, 0.25f), 128.0f, 1.0f);
				break;
			case Colour.Green:
				spriteColor = new Vector4(rand.NextFloat(-0.25f, 0.25f), 1.0f, rand.NextFloat(-0.25f, 0.25f), 1.0f);
				break;
			case Colour.Grey:
				spriteColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
				break;
			}
			
			_scene.AddChild (sprite);
			
		}
		
		
		public void Update()
		{	
			switch(particleState)
			{
			case ParticleState.Explosion:
				spriteColor.A -= 0.025f;
				break;
			case ParticleState.Smoke:
				spriteColor.A -= 0.008f;
				break;
			case ParticleState.Trail:
				spriteColor.A -= 0.1f;
				break;
			}

			sprite.Color = spriteColor;
			TTL--;
    		sprite.Position += Velocity;
    		Angle += AngularVelocity;
			Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator(DateTime.Now.Millisecond);
			sprite.CenterSprite(TRS.Local.Center);
			sprite.Rotate (Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Deg2Rad(Angle));
		}
	}
}

