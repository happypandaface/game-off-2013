using UnityEngine;
using System.Collections;

// Dungeon Generator returns an array of these:
public struct Tile
{
	public string type;
	public ArrayList subTypes;// this should only be accessed by classes inside this file (no keyword for this in C#)
	// if you wanna check a sub type use DungeonGenerator.checkSubType(tile, "subType") TODO
	public int xPos;
	public int yPos;
}

public class DungeonGenerator
{
	public float seed;
	public int width;
	public int height;
	public int rooms;
	private int corridorWidth = 2;
	private int minWidth = 4;
	private int maxWidth = 10;
	private int minHeight = 4;
	private int maxHeight = 10;
	
	// setup the max and min width and min and max height for rooms
	public void configRoomSize(int minW, int maxW, int minH, int maxH)
	{
		minWidth = minW;
		maxWidth = maxW;
		minHeight = minH;
		maxHeight = maxH;
	}
	
	// set up the seed, width, height and # of rooms
	public void config(float s, int w, int h, int rs)
	{
		seed = s;
		width = w;
		height = h;
		rooms = rs;
	}
	
	// set the width of corridors
	public void setCorridorWidth(int w)
	{
		corridorWidth = w;
	}
	
	// use this to check tiles for certain sub types
	public bool checkSubType(Tile tile, string sType)
	{
		for (int i = 0; i < tile.subTypes.Count; ++i)
		{
			if (((string)tile.subTypes[i]) == sType)
			{
				return true;
			}
		}
		return false;
	}
	
	// returns a dungeon with the previously given config
	public ArrayList generateDungeon()
	{
		Random.seed = (int)(seed*100000.0f);
		while (true)
		{
			ArrayList dungeon = new ArrayList();
			ArrayList roomsMade = new ArrayList();
			while (roomsMade.Count < rooms)
			{
				Room r = new Room();
				r.make ((int)Mathf.Floor(Random.value*width), (int)Mathf.Floor(Random.value*height), (int)Mathf.Floor(Random.value*(maxWidth-minWidth))+minWidth, (int)Mathf.Floor(Random.value*(maxHeight-minHeight))+minHeight);
				if (r.check(dungeon))
				{
					roomsMade.Add(r);
					r.addTo (ref dungeon);
				}
			}
			ArrayList corridorsMade = new ArrayList();
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
						{
							corridorsMade.Add(cor);
							cor.addTo (ref dungeon);
						}
					}
				}
			}
			for (int i = 0; i < roomsMade.Count; ++i)
			{
				((Room)roomsMade[i]).touched = false;
			}
			if (!checkIfDungeonWorks(0, roomsMade, corridorsMade))
				continue;
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
	
	private bool checkIfDungeonWorks(int roomNum, ArrayList rooms, ArrayList corridors)
	{
		((Room)rooms[roomNum]).touched = true;
		// check if all rooms have been touched
		int num = 0;
		for (int i = 0; i < rooms.Count; ++i)
		{
			if (((Room)rooms[i]).touched == false)
				break;
			if (((Room)rooms[i]).touched == true && i == rooms.Count-1)
				return true;
		}
		// if not, do recursion on rooms connected to this one that aren't touched
		for (int i = 0; i < corridors.Count; ++i)
		{
			Room nextRoom = null;
			if (((Corridor)corridors[i]).room1 == rooms[roomNum])
				nextRoom = ((Room)((Corridor)corridors[i]).room2);
			if (((Corridor)corridors[i]).room2 == rooms[roomNum])
				nextRoom = ((Room)((Corridor)corridors[i]).room1);
			if (nextRoom != null && !nextRoom.touched)
			{
				for (int c = 0; c < rooms.Count; ++c)
				{
					if (rooms[c] == nextRoom)
					{
						if (checkIfDungeonWorks(c, rooms, corridors))
							return true;
						else
							break;
					}
				}
			}
		}
		return true;
	}
}

// internal class
class Room
{
	public int width;
	public int height;
	public int xPos;
	public int yPos;
	public bool touched;// used to check that all rooms are connected by corridors
	private ArrayList tiles;
	
	// sets config options and (internally) creates the array of tiles needed to make the room
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
	
	// check the internal array of tiles with the given array of tiles to see if the room can be added.
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
	
	// add the room to the array of tiles
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

class Corridor
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
			// TODO: do hooked corridor
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
						if (totalWallsIntersecting > width*2)// don't want to intersect more than the width on each side (beginning and end)
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
