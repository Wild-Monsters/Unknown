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
			count = (int)(hash +randomGen.Next (2,4))/(DateTime.Now.Millisecond+1);
			return randomGen.Next (min, max);
		}
	}
}

