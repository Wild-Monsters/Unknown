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
		private  SoundPlayer winampShot;
		private  Sound blockShot;
		
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
			if(blockShot != null)
			{
				blockShot.Dispose();
			}
			
			if(winampShot != null)
			{
				winampShot.Dispose();
			}

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
				mainMenu = new Bgm("Application/Sounds/PlaceHoldermainmenu.mp3");
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
			if (winampShot == null)
			{
				blockShot = new Sound("Application/Sounds/PlaceHolderblockshot.wav");
				winampShot = blockShot.CreatePlayer();
				winampShot.Volume = GameManager.Instance.SoundFXVol;
				winampShot.PlaybackRate = 1.0f;
			}
			winampShot.Play ();
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
				InGameMusic1 = new Bgm("Application/Sounds/PlaceholderGameMusic.mp3");
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
			winampShot.Stop ();
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

