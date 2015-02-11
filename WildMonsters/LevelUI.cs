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
	public class LevelUI
	{
		public Divider divider;
		public Sidebar sidebar;
		
		public LevelUI (Scene _scene)
		{
			divider = new Divider(_scene);
			sidebar = new Sidebar(_scene);
		}
		
		public void Update(float t)
		{
			divider.Update (t);
		}
		
		public Divider Divider
		{
			get{ return divider; }
			
		}
	}
}

