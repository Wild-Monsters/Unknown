using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.Core.Imaging;

namespace WildMonsters
{
	public class Divider
	{
		private SpriteUV sprite;
		private float top, topTarget;
		private int speed = 2;
		
		private Label p1ScoreLabel;
		private Label p2ScoreLabel;
		
		private int p1Score;
		private int p2Score;

		
		public Divider (Scene _scene)
		{
//			int width = Director.Instance.GL.Context.GetViewport().Width;
//			int height = Director.Instance.GL.Context.GetViewport().Height;
//			
//			Image img = new Image(ImageMode.Rgba, new ImageSize(width / 2, height / 2), new ImageColor(255, 0, 0, 0));
//			img.DrawText("Score: ", new ImageColor(255, 0, 0, 255), new Font(FontAlias.System, 170, FontStyle.Regular), new ImagePosition(0, 150));
//			
//			Texture2D texture = new Texture2D(width / 2, height / 2, false, PixelFormat.Rgba);
//			texture.SetPixels(0, img.ToBuffer());
//			img.Dispose();
			
			TextureInfo texInfo = new TextureInfo("/Application/textures/Divider2.png");
			//texInfo.Texture = texture;
			
			
			sprite = new SpriteUV();
			sprite.TextureInfo = texInfo;
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			p1ScoreLabel = new Label();
			p1ScoreLabel.Position = new Vector2(0.0f, 0.0f);
			p1ScoreLabel.Scale = new Vector2(1.5f, 1.5f);
			p1ScoreLabel.Text = "Player 1 Score: ";
			
			p2ScoreLabel = new Label();
			p2ScoreLabel.Position = new Vector2(750.0f, 0.0f);
			p2ScoreLabel.Scale = new Vector2(1.5f, 1.5f);
			p2ScoreLabel.Text = "Player 2 Score: ";
			
			_scene.AddChild (sprite);
			_scene.AddChild(p1ScoreLabel);
			_scene.AddChild(p2ScoreLabel);

		}
		
		public void Update(float t)
		{
			if(top<topTarget)
			{
				top+=speed;
			}
			
			if(top>topTarget)
			{
				top -= speed;
			}
			
			sprite.Position = new Vector2(top-(sprite.Quad.S.X/2), 0.0f);
			
			p1ScoreLabel.Text = "Player 1 Score: " + p1Score;
			p2ScoreLabel.Text = "Player 2 Score: " + p2Score;

		}
		
		
		public float TopTarget
		{
			set{ topTarget = value; }
			get{ return topTarget; }
		}
		
		public float Top
		{
			set{ top= value; }
			get{ return top; }
		}
		
		public SpriteUV Sprite
		{
			get{ return sprite; }
			set{ sprite = value; }
		}
		
		public int P1Score
		{
			get{ return p1Score; }
			set{ p1Score = value; }
		}
		
		public int P2Score
		{
			get{ return p2Score; }
			set{ p2Score = value; }
		}
	}
}

