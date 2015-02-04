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
	public class GameScene : Scene
	{
		private LevelManager levelManager;

		public GameScene()
		{
			this.Camera.SetViewFromViewport();
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);	// Tells the director to call the update function of this "node"
			
			levelManager = new LevelManager(this);
			
		}
		
		public override void Update(float deltaTime)
		{
<<<<<<< HEAD
			List<TouchData> touches = Touch.GetData(0);
=======
			if(Input.KeyPressed (GamePadButtons.R))
				topTarget += 50.0f;
>>>>>>> 458b8efc3b413faa704e6f8b99085441ba4ccb0f
			
			foreach(TouchData data in touches)
			{
				ParticleManager.AddParticle (this, new Vector2(data.X, data.Y));
			}
			
<<<<<<< HEAD
			levelManager.Update(deltaTime);
			ParticleManager.Update();
=======
			if(Input.KeyPressed (GamePadButtons.L))
				topTarget -= 50.0f;
			
			
			if(top < topTarget)
				top += 10.0f;
			
			
			if(top > topTarget)
				top -= 10.0f;
			
			
			player1.Update (this);
			player2.Update (this);
			
			levelUI.divider.SetTop (top);
			levelUI.Update (deltaTime);
			
			grid1.SetTop (top);
			grid2.SetTop (top);
			
			grid1.Update (deltaTime);
			grid2.Update (deltaTime);
			
			//Collision Stuff trial 
			CollisionHandler.CheckBlockCollision(player1.getBalls(), grid1);
			CollisionHandler.CheckBlockCollision(player2.getBalls(), grid2);
>>>>>>> 458b8efc3b413faa704e6f8b99085441ba4ccb0f
		}
	}
}