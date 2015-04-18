using System;
using Sce.PlayStation.Core.Audio;

namespace WildMonsters
{
	public  class AudioManager
	{		
		//enum play stop, pause, etc..
		//method audio.play(sound name, background)
		//dictionary <key, value > key = "audioname", value= string of location 
		//		
		private  SoundPlayer blockShot1;
		private  SoundPlayer blockShot2;
		private  SoundPlayer blockShot3;
		//private  Sound blockShot;
		
		private  SoundPlayer winampBreak;		//fx only wav only
		private  Sound blockBreak;
		
		private  BgmPlayer bgmMenu;			//Background only mp3 only
		private  Bgm mainMenu;
		
		private  BgmPlayer bgmIngame1;
		private  Bgm InGameMusic1;
			
		public  AudioManager (){}
		
	
		//Disposing All Assets
		public  void Dispose()
		{
			if(blockShot1 != null)
				blockShot1.Dispose();
			
			if(blockShot2 != null)
				blockShot2.Dispose();
			
			if(blockShot3 != null)
				blockShot3.Dispose();

			if(blockBreak != null)
			{
				blockBreak.Dispose ();
			}
			
			if(winampBreak != null)
			{
				winampBreak.Dispose();
			}
			
			if(mainMenu != null)
			{
				mainMenu.Dispose();
			}
			
			if(bgmMenu != null)
			{
				bgmMenu.Dispose();
			}
						
			if(InGameMusic1 != null)
			{
				InGameMusic1.Dispose();
			}
			
			if(bgmIngame1 != null)
			{
				bgmIngame1.Dispose();
			}
		}
		
		//Creating Classes to play the sounds.
		public void PlayMenuMusic()
		{
			if (mainMenu == null)
			{
				mainMenu = new Bgm("Application/Sounds/PushWarsMenuLoop.mp3");
			}
			if (bgmMenu == null)
			{
				bgmMenu = mainMenu.CreatePlayer();
				bgmMenu.Volume = GameManager.Instance.MusicVol;
				bgmMenu.Loop = true;
			}
			bgmMenu.Play();
		}
		
		public void PlayBlockShot()
		{
			if (blockShot1 == null)
			{
				Sound blockShot = new Sound("Application/Sounds/BlockSound1.wav");
				blockShot1 = blockShot.CreatePlayer();
				blockShot1.Volume = GameManager.Instance.SoundFXVol;
				blockShot1.PlaybackRate = 1.0f;
				
				blockShot = new Sound("Application/Sounds/BlockSound2.wav");
				blockShot2 = blockShot.CreatePlayer();
				blockShot2.Volume = GameManager.Instance.SoundFXVol;
				blockShot2.PlaybackRate = 1.0f;
				
				blockShot = new Sound("Application/Sounds/BlockSound3.wav");
				blockShot3 = blockShot.CreatePlayer();
				blockShot3.Volume = GameManager.Instance.SoundFXVol;
				blockShot3.PlaybackRate = 1.0f;
			}
			
			//Messy way of randomising which sound gets played, feel free to change
			//Generally want slightly altered versions of each sound effect so it doesn't sound too repetitive
			int rng = WMRandom.GetNextInt(0,100);
			
			if(rng < 30)
			{
				blockShot1.Play ();
			}
			else if(rng < 60)
			{
				blockShot2.Play ();
			}
			else
			{
				blockShot3.Play();
			}
		}
		
		public void PlayBlockBreak()
		{
			if (winampBreak == null)
			{
				blockBreak = new Sound("Application/Sounds/PlaceHolderblockbreak.wav");
				winampBreak = blockBreak.CreatePlayer();
				winampBreak.Volume = GameManager.Instance.SoundFXVol;
				winampBreak.PlaybackRate = 1.0f;
			}
			winampBreak.Play();
		}
		
		public void PlayGameMusic()
		{
			if (InGameMusic1 == null)
			{
				InGameMusic1 = new Bgm("Application/Sounds/MainGameLoop.mp3");
			}
			
			if (bgmIngame1 == null)
			{
				bgmIngame1 = InGameMusic1.CreatePlayer();
				bgmIngame1.Volume = GameManager.Instance.MusicVol;
				bgmIngame1.Loop = true;
			}
			bgmIngame1.Play ();
		}
		
		//Creating classes to stop the sounds
		public void StopBlockshotSound()
		{
			blockShot1.Stop ();
		}
		
		public void StopMenuMusic()
		{
			if (bgmMenu != null)
			{
				bgmMenu.Stop();
			}
		}
		
		public void StopBlockbreakSound()
		{
			winampBreak.Stop();
		}
		
		public void StopGameMusic()
		{
			if(bgmIngame1 != null)
			{
				bgmIngame1.Stop();
			}
		}
	}
}

