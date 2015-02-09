using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;


namespace WildMonsters
{
	public class Particle
	{
		// Private variables:
		//Vector2 _myPos;
		Vector2 _speed;
		SpriteUV sprite;
		Vector4 spriteColor;
		   
		public Vector2 Velocity { get; set; }        // The speed of the particle at the current instance
		public float Angle { get; set; }            // The current angle of rotation of the particle
		public float AngularVelocity { get; set; }    // The speed that the angle is changing
		public int TTL { get; set; }                // The 'time to live' of the particle
		public SpriteUV Sprite { get{ return sprite; } }
		public bool bExplosion { get; set; }
		public Vector4 SpriteColor { get{return spriteColor;} set{spriteColor = value;} }
		
		
		// Accessors:
		
		//public Particle (Scene _scene, Vector2 position)
		public Particle (Scene _scene, Vector2 position)
		{
			Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand;
			
			// Generate a random number
			rand = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator(DateTime.Now.Millisecond);
			
			// Set velocity
			Vector2 velocity = new Vector2(1f * (float)(rand.NextFloat(1, 2)),
            							   1f * (float)(rand.NextFloat(1, 2)));
			
			// Upward flame
			// if input == 0
			
			// Explosion
			// if input == 1
			var gamePadData = GamePad.GetData(0);
			if((gamePadData.Buttons & GamePadButtons.Square) != 0)
        	{
                bExplosion = true;
        	}
			
			if(bExplosion)
			{
				Random r = new Random();
				if(r.NextDouble() > 0.75f && r.NextDouble() < 1.0f)
				{
					velocity.X *= rand.NextFloat(-1, 0);
					velocity.Y *= rand.NextFloat(0, 1);
				}
				else if(r.NextDouble() > 0.5f && r.NextDouble() < 0.75f)
				{
					velocity.X *= rand.NextFloat(-1, 0);
					velocity.Y *= rand.NextFloat(-1, 0);
				}
				else if(r.NextDouble() > 0.25f && r.NextDouble() < 0.5f)
				{
					velocity.X *= rand.NextFloat(0, 1);
					velocity.Y *= rand.NextFloat(-1, 0);
				}
				else if(r.NextDouble() > 0.0f && r.NextDouble() < 0.25f)
				{
					velocity.X *= rand.NextFloat(0, 1);
					velocity.Y *= rand.NextFloat(0, 1);
				}
			}
			
			// Shield
			// if input == 2
			
//			Random r = new Random();
//			if(r.NextDouble() > 0.75f && r.NextDouble() < 1.0f)
//			{
//				velocity.X *= rand.NextFloat(-1, 0);
//				velocity.Y *= rand.NextFloat(0, 1);
//			}
//			else if(r.NextDouble() > 0.5f && r.NextDouble() < 0.75f)
//			{
//				velocity.X *= rand.NextFloat(-1, 0);
//				velocity.Y *= rand.NextFloat(-1, 0);
//			}
//			else if(r.NextDouble() > 0.25f && r.NextDouble() < 0.5f)
//			{
//				velocity.X *= rand.NextFloat(0, 1);
//				velocity.Y *= rand.NextFloat(-1, 0);
//			}
//			else if(r.NextDouble() > 0.0f && r.NextDouble() < 0.25f)
//			{
//				velocity.X *= rand.NextFloat(0, 1);
//				velocity.Y *= rand.NextFloat(0, 1);
//			}
			
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
			
			
			//TextureInfo texInfo = new TextureInfo("/Application/textures/star.png");
			TextureInfo texInfo = new TextureInfo("/Application/textures/particle.png");
			
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = position;
			
			spriteColor = new Vector4((float)rand.NextFloat(0, 1),
            				    (float)rand.NextFloat(0, 1),
            				    (float)rand.NextFloat(0, 1),
			                    (float)1);
			
			
			sprite.Color = spriteColor;
//			sprite.Color = new Vector4((float)rand.NextFloat(0, 1),
//            				           (float)rand.NextFloat(0, 1),
//            				  		   (float)rand.NextFloat(0, 1),
//			                  		   (float)rand.NextFloat(0, 1));
			
			_scene.AddChild (sprite);
			
			
			// Initialise variables:
			//_speed = new Vector2(0.0f, -2.5f);
		}
		
		public Vector2 getMyPos()
		{
			return sprite.Position;
		}
		
		//public void Update(Scene _scene)
		public void Update()
		{
			//_myPos.Add (_speed);
			//sprite.Position.Add ();
			//sprite.Position = Vector2.Add (sprite.Position, _speed);
			 	spriteColor.A -= 0.01f;
				sprite.Color = spriteColor;
			    TTL--;
    			sprite.Position += Velocity;
    			Angle += AngularVelocity;
		}
	}
}

