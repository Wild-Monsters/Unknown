using System;


namespace WildMonsters
{
	public class CollisionHandler
	{
		public CollisionHandler (){}
		
		public void CheckCollision(Ball[,] a, Ball[] b)
		{
			for(int i =0; i< b.Length ; i++)
			{
				if(b[i].GetFired())
				{
					if(b[i].GetBounds().Overlaps(a[i,i].GetBounds().Overlaps))
					{
						
					}
				}
			}
	}
}

