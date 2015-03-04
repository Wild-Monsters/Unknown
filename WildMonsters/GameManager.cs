using System;

namespace WildMonsters
{
	public class GameManager
	{
		private static GameManager instance;
		private float musicVolume;
		private float soundVolume;
		
		public static GameManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new GameManager();
				}
				return instance;
			}		
		}
		
		private GameManager()
		{
			musicVolume = 1.0f;
			soundVolume = 1.0f;
		}
		
		public float MusicVol
		{
			get {return musicVolume;}
			set
			{
				if (value < 0)
					musicVolume = 0;
				else if (value > 1)
					musicVolume = 1;
				else
					musicVolume = value;
			}
		}
		public float SoundFXVol
		{
			get {return soundVolume;}
			set
			{
				if (value < 0)
					soundVolume = 0;
				else if (value > 1)
					soundVolume = 1;
				else
					soundVolume = value;
			}
		}
	}
}

