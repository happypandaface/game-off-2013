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
	private ArrayList tiles;
	
	public void make(int x, int y, int w, int h)
	{
		xPos = x;
		yPos = y;
		width = w;
		height = h;
		tiles = new ArrayList();
		for (int x2 = -1; x2 <= width; x2++)
		{
			for (int y2 = -1; y2 <= height; y2++)
			{
				Tile ts = new Tile();
				ts.xPos = xPos+x2;
				ts.yPos = yPos+y2;
				if (x2 == -1 || x2 == width || y2 == -1 || y2 == height)
				{
					ts.type = "wall";
				}else
				{
					ts.type = "floor";
				}
				tiles.Add(ts);
			}
		}
	}
	
	public bool check(ArrayList dungeon)
	{
		for (int i = 0; i < tiles.Count; ++i)
		{
			for (int c = 0; c < dungeon.Count; ++c)
			{
				if (((Tile)tiles[i]).xPos == ((Tile)dungeon[c]).xPos &&
					((Tile)tiles[i]).yPos == ((Tile)dungeon[c]).yPos)
					return false;
			}
		}
		return true;
	}
	
	public void addTo(ref ArrayList dungeon)
	{
		for (int i = 0; i < tiles.Count; ++i)
		{
			dungeon.Add(tiles[i]);
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
	private ArrayList tiles;
	private int width = 3;
	private int subWidth = 2;
	
	public void setWidth(int w)
	{
		width = w;
		subWidth = width-1;
	}
	
	public void make(Room r1, Room r2)
	{
		room1 = r1;
		room2 = r2;
		tiles = new ArrayList();
		int xStart = 0;
		int yStart = 0;
		int length = 0;
		// the following code assumes the rooms aren't overlapping, which they won't be
		if (room1.xPos < room2.xPos+room2.width-subWidth && room1.xPos + room1.width-subWidth > room2.xPos)
		{
			int xStartMin = room1.xPos < room2.xPos ? room2.xPos : room1.xPos;
			int xStartMax = room1.xPos+room1.width-subWidth > room2.xPos+room2.width-subWidth ? room2.xPos+room2.width-subWidth : room1.xPos+room1.width-subWidth;
			xStart = (int)Mathf.Floor(Random.value*(xStartMax-xStartMin))+xStartMin;
			// do center corridor
			if (room1.yPos < room2.yPos)
			{
				yStart = room1.yPos+room1.height;
				length = room2.yPos;
			}else
			{
				yStart = room2.yPos+room2.height;
				length = room1.yPos;
			}
			for (int y = yStart; y < length; ++y)
			{
				for (int i = -1; i < width+1; ++i)
				{
					if (i == -1 || i == width)
						doTile(ref tiles, xStart+i, y, "wall");
					else
						doTile(ref tiles, xStart+i, y, "floor");
				}
			}
		}else
		if (room1.yPos < room2.yPos+room2.height-subWidth && room1.yPos + room1.height-subWidth > room2.yPos)
		{
			int yStartMin = room1.yPos < room2.yPos ? room2.yPos : room1.yPos;
			int yStartMax = room1.yPos+room1.height-subWidth > room2.yPos+room2.height-subWidth ? room2.yPos+room2.height-subWidth : room1.yPos+room1.height-subWidth;
			yStart = (int)Mathf.Floor(Random.value*(yStartMax-yStartMin))+yStartMin;
			if (room1.xPos < room2.xPos)
			{
				xStart = room1.xPos+room1.width;
				length = room2.xPos;
			}else
			{
				xStart = room2.xPos+room2.width;
				length = room1.xPos;
			}
			for (int x = xStart; x < length; ++x)
			{
				for (int i = -1; i < width+1; ++i)
				{
					if (i == -1 || i == width)
						doTile(ref tiles, x, yStart+i, "wall");
					else
						doTile(ref tiles, x, yStart+i, "floor");
				}
			}
		}else
		{
			//do hooked corridor
		}
	}
	
	private void doTile(ref ArrayList tiles, int x, int y, string type)
	{
		Tile ts = new Tile();
		ts.type = type;
		ts.xPos = x;
		ts.yPos = y;
		tiles.Add(ts);
	}
	
	public bool check(ArrayList dungeon)
	{
		int totalWallsIntersecting = 0;
		for (int i = 0; i < tiles.Count; ++i)
		{
			for (int c = 0; c < dungeon.Count; ++c)
			{
				if (((Tile)tiles[i]).xPos == ((Tile)dungeon[c]).xPos &&
					((Tile)tiles[i]).yPos == ((Tile)dungeon[c]).yPos)
				{
					if (((Tile)dungeon[c]).type == "floor")
					{
						return false;
					}else if (((Tile)dungeon[c]).type == "wall" && ((Tile)tiles[i]).type == "floor")
					{
						++totalWallsIntersecting;
						if (totalWallsIntersecting > width*2)// don't want to intercect more than the width on each side (beginning and end)
							return false;
					}
				}
			}
		}
		return true;
	}
	
	public void addTo(ref ArrayList dungeon)
	{
		for (int i = 0; i < tiles.Count; ++i)
		{
			for (int c = 0; c < dungeon.Count; ++c)
			{
				if (((Tile)tiles[i]).xPos == ((Tile)dungeon[c]).xPos &&
					((Tile)tiles[i]).yPos == ((Tile)dungeon[c]).yPos &&
					((Tile)dungeon[c]).type == "wall")
				{
					dungeon.RemoveAt(c);
				}
			}
			dungeon.Add(tiles[i]);
		}
	}
}

public class DungeonGenerator
{
	public float seed;
	public int width;
	public int height;
	public int rooms;
	private int corridorWidth = 2;
	
	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}
	
	public void setCorridorWidth(int w)
	{
		corridorWidth = w;
	}
	
	public void config(float s, int w, int h, int rs)
	{
		seed = s;
		width = w;
		height = h;
		rooms = rs;
	}
	
	public ArrayList generateDungeon()
	{
		Random.seed = (int)(seed*100000.0f);
		ArrayList dungeon = new ArrayList();
		ArrayList roomsMade = new ArrayList();
		while (roomsMade.Count < rooms)
		{
			Room r = new Room();
			r.make ((int)Mathf.Floor(Random.value*width), (int)Mathf.Floor(Random.value*height), (int)Mathf.Floor(Random.value*height*width/rooms/rooms)+3, (int)Mathf.Floor(Random.value*height*width/rooms/rooms)+3);
			if (r.check(dungeon))
			{
				roomsMade.Add(r);
				r.addTo (ref dungeon);
			}
		}
		for (int i = 0; i < roomsMade.Count; ++i)
		{
			for (int c = 0; c < roomsMade.Count; ++c)
			{
				if (i != c)
				{
					Corridor cor = new Corridor();
					cor.setWidth(corridorWidth);
					cor.make (((Room)roomsMade[i]), ((Room)roomsMade[c]));
					if (cor.check(dungeon))
						cor.addTo (ref dungeon);
				}
			}
		}
		/*
		Room r = new Room();
		r.make (1, 1, 5, 7);
		r.addTo (ref dungeon);
		Room r2 = new Room();
		r2.make (-7, 1, 5, 6);
		r2.addTo (ref dungeon);
		Room r3 = new Room();
		r3.make (-8, 10, 7, 4);
		r3.addTo (ref dungeon);
		Corridor c = new Corridor();
		c.make (r, r2);
		c.addTo (ref dungeon);
		Corridor c2 = new Corridor();
		c2.make (r2, r3);
		c2.addTo (ref dungeon);
		Room r4 = new Room();
		r4.make (-10, -15, 7, 4);
		r4.addTo (ref dungeon);
		Corridor c3 = new Corridor();
		c3.make (r2, r4);
		c3.addTo (ref dungeon);
		*/
		Vector2 spawnPos = ((Room)roomsMade[0]).getRandomPos(seed);
		Tile ts = new Tile();
		ts.type = "spawn";
		ts.xPos = (int)spawnPos.x;
		ts.yPos = (int)spawnPos.y;
		dungeon.Add(ts);
		for (int i = 1; i < roomsMade.Count; ++i)
		{
			Vector2 enPos = ((Room)roomsMade[i]).getRandomPos(seed);
			Tile en = new Tile();
			en.type = "enemy";
			en.xPos = (int)enPos.x;
			en.yPos = (int)enPos.y;
			dungeon.Add(en);
		}
		return dungeon;
	}
}
