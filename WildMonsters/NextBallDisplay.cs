using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
//using Sce.PlayStation.HighLevel.UI;

namespace WildMonsters
{
	
	public class NextBallDisplay
	{
		private SpriteUV sprite;
		private TextureInfo texInfo;
		private Colour colour;	
		private bool onLeftSide;
		
		//starting colour
		private Colour firstColour;
		//colour immediately after fired
		private Colour secondColour;
		//Colour help in label
		private Colour thirdColour;
		
		public NextBallDisplay (Scene scene, bool onLeftSide)
		{
			
			texInfo = new TextureInfo("/Application/textures/Blocks2.png");
			
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(50.0f,50.0f);
			sprite.VertexZ = 1.0f;
			
			this.onLeftSide = onLeftSide;
			
			if(onLeftSide)
			{
				sprite.Position = new Vector2(10.0f, 510.0f);
			}
			else
			{
				sprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width - 35.0f, 510.0f);
			}
			
			firstColour = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
			secondColour = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
			thirdColour = (Colour)WMRandom.GetNextInt(0, 5, this.GetHashCode());
			
			scene.AddChild(sprite);
		}
		
		
		public SpriteUV Sprite
		{
			get{ return sprite; }
			set{ sprite = value; }
		}
		
		
		public void Update()
		{
				
			
		}
		
		public void Cleanup()
		{
			texInfo.Dispose();
			sprite = null;
			//alive = false;
		}			
		
		public void SetColour(Colour col)
		{
			colour = col;
			
			float spriteWidth = 1.0f / 6.0f;
			sprite.UV.S = new Vector2(spriteWidth, 1.0f);
			sprite.UV.T = new Vector2(spriteWidth * (int)colour, 0.0f);	
		}
		
		public bool OnLeftSide
		{
			get{ return onLeftSide; }
			set{ onLeftSide = value; }
		}
		
		public Colour FirstColour
		{
			get{ return firstColour; }
			set{ firstColour = value; }
		}
		
		public Colour SecondColour
		{
			get{ return secondColour; }
			set{ secondColour = value; }
		}
		
		public Colour ThirdColour
		{
			get{ return thirdColour; }
			set{ thirdColour = value; }
		}
		
		
		
	}
}

