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
	public enum ButtonState {Inactive, Highlighted, Released};
	
	public class Button
	{
		SpriteUV sprite;
		ButtonState state;
		
		float spriteWidth;
		float spriteHeight;
		int frames;
		
		public Button (Scene scene, int sceneLayer, TextureInfo texInfo, Vector2 position, int amountOfFrames) 
		{
			spriteWidth = texInfo.TextureSizef.X;
			spriteHeight = texInfo.TextureSizef.Y;
			this.frames = amountOfFrames;
			
			sprite = new SpriteUV(texInfo);
			sprite.Quad.S = new Vector2(spriteWidth/frames, spriteHeight);
			sprite.UV.S = new Vector2((1.0f/frames), 1.0f);
			sprite.Position = new Vector2(position.X-(spriteWidth/frames)/2, position.Y-spriteHeight/2);
			
			SetState (ButtonState.Inactive);
			
			scene.AddChild(sprite, sceneLayer);
		}
		
		public void Update()
		{
			var touches = Touch.GetData (0).ToArray();
			
			if(touches.Length > 0 && (touches[0].Status == TouchStatus.Down || touches[0].Status == TouchStatus.Move))
			{
				//Translates the weird touch values into actual coordinates
				Vector2 touchPos = GetTouchPosition(touches[0], Constants.ScreenWidth, Constants.ScreenHeight);
				
				//If the touch position is inside the bounds of the button sprite
				if(touchPos.X > sprite.Position.X && touchPos.X < sprite.Position.X + (spriteWidth/frames)
				&& touchPos.Y > sprite.Position.Y && touchPos.Y < sprite.Position.Y + spriteHeight)
				{
					SetState(ButtonState.Highlighted);
				}
				else
				{
					SetState(ButtonState.Inactive);
				}
			}
			else
			{
				if(state == ButtonState.Highlighted)
				{
					//If the button was highlighted on the last frame, set it to released for one frame
					//Buttons are usually activated on the buttons release rather than as soon as it's pressed
					SetState(ButtonState.Released);
				}
				else
				{
					SetState (ButtonState.Inactive);
				}
			}
		}
		
		public void SetState(ButtonState state)
		{
			this.state = state;
					
			if((int)state < frames)
			{
				sprite.UV.T = new Vector2((int)state * (1.0f/frames), 0.0f);
			}
		}
		
		public ButtonState GetState()
		{
			return state;
		}
		
		private Vector2 GetTouchPosition(TouchData touch, float screenWidth, float screenHeight)
		{
			float touchX =  (int)((touch.X + 0.5f) * screenWidth);
			float touchY =  (int)((touch.Y + 0.5f) * screenHeight);
			
			return new Vector2(touchX, touchY);
		}
	}
}

