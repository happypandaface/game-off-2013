using UnityEngine;
using System.Collections;

public class ExampleDungeonUser : MonoBehaviour 
{
	
	public Transform playerPrefab;
	public Transform floorPrefab;
	public Transform enemyPrefab;
	private int tileSize = 1;
	
	// Use this for initialization
	void Start () 
	{
		float seed = Random.value;
		DungeonGenerator dg = new DungeonGenerator();

		ArrayList arr = dg.generateDungeon(seed);
		for (int i = 0; i < arr.Count; ++i)
		{
			Tile ts = (Tile)arr[i];
			if (ts.type == "spawn")
			{
				Instantiate(playerPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.Euler( 0, 0, 0));
			}
			else if (ts.type == "enemy")
			{
				Instantiate(enemyPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.Euler( 0, 0, 0));
			}
			else if (ts.type == "floor")
			{
				Instantiate(floorPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.Euler( 0, 0, 0));
			}
		}
	}
}
