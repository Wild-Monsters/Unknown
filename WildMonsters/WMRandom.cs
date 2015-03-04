using System;

namespace WildMonsters
{
	public static class WMRandom
	{
		private static Random randomGen;
		private static int count;
		
		public static int GetNextInt(int min, int max, int hash)
		{
			randomGen = new Random(count + DateTime.Now.Millisecond + hash);
			count += randomGen.Next (100);
			
			if(count > 99999)
			{
				count = randomGen.Next (100);
			}
				
			return randomGen.Next (min, max);
		}
		
		public static int GetNextInt(int min, int max)
		{	
			return GetNextInt (min,max,0);
		}
	}
}

