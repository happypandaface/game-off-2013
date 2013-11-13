using UnityEngine;
using System.Collections;

public struct Tile
{
	public string type;
	public int xPos;
	public int yPos;
}

public class Room
{
	public int width;
	public int height;
	public int xPos;
	public int yPos;
	
	public void make(int x, int y, int w, int h)
	{
		xPos = x;
		yPos = y;
		width = w;
		height = h;
	}
	
	public void addTo(ref ArrayList dungeon)
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Tile ts = new Tile();
				ts.type = "floor";
				ts.xPos = xPos+x;
				ts.yPos = yPos+y;
				dungeon.Add(ts);
			}
		}
	}
	
	public Vector2 getRandomPos(float seed)
	{
		int x = (int)(Random.value*width)+xPos;
		int y = (int)(Random.value*height)+yPos;
		return new Vector2(x, y);
	}
}

public class Corridor
{
	public Room room1;
	public Room room2;
	
	public void make(Room r1, Room r2)
	{
		room1 = r1;
		room2 = r2;
	}
	
	public void addTo(ref ArrayList dungeon)
	{
		// the following code assumes the rooms aren't overlapping
		if (room1.xPos <= room2.xPos+room2.width)
		{
			if (room1.xPos + room1.width >= room2.xPos)
			{
				if (room1.xPos <= room2.xPos+room2.width && room1.xPos >= room2.xPos)
				{
					
				}else
				if (room1.xPos+room1.width <= room2.xPos+room2.width && room1.xPos+room1.width >= room2.xPos)
				{
					
				}else
				{
					//do center corridor
				}
			}
		}
		else
		if (room1.yPos <= room2.yPos+room2.height)
		{
			if (room1.yPos + room1.height >= room2.yPos)
			{
				if (room1.yPos <= room2.yPos+room2.height && room1.yPos >= room2.yPos)
				{
					if (room1.xPos < room2.xPos)
					{
						
					}else
					{
						for (int x = room2.xPos+room2.width; x < room1.xPos; x++)
						{
							Tile ts = new Tile();
							ts.type = "floor";
							ts.xPos = x;
							ts.yPos = room1.yPos;
							dungeon.Add(ts);
						}
					}
				}else
				if (room1.yPos+room1.height <= room2.yPos+room2.width && room1.yPos+room1.height >= room2.yPos)
				{
					
				}else
				{
					//do center corridor
				}
			}
		}else
		{
			//do hooked corridor
		}
	}
}

public class DungeonGenerator
{
	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}
	
	public ArrayList generateDungeon(float seed)
	{
		Random.seed = (int)(seed*100000.0f);
		ArrayList dungeon = new ArrayList();
		Room r = new Room();
		r.make (1, 1, 5, 7);
		r.addTo (ref dungeon);
		Room r2 = new Room();
		r2.make (-7, 1, 5, 6);
		r2.addTo (ref dungeon);
		Corridor c = new Corridor();
		c.make (r, r2);
		c.addTo (ref dungeon);
		Vector2 spawnPos = r.getRandomPos(seed);
		Tile ts = new Tile();
		ts.type = "spawn";
		ts.xPos = (int)spawnPos.x;
		ts.yPos = (int)spawnPos.y;
		dungeon.Add(ts);
		Vector2 enPos = r2.getRandomPos(seed);
		Tile en = new Tile();
		en.type = "enemy";
		en.xPos = (int)enPos.x;
		en.yPos = (int)enPos.y;
		dungeon.Add(en);
		return dungeon;
	}
}
