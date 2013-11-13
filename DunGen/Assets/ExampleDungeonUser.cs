using UnityEngine;
using System.Collections;

public class ExampleDungeonUser : MonoBehaviour {
	
	public Transform playerPrefab;
	public Transform floorPrefab;
	public Transform enemyPrefab;
	private int tileSize = 10;
	
	// Use this for initialization
	void Start () {
		float seed = Random.value;
		DungeonGenerator dg = new DungeonGenerator();
		ArrayList arr = dg.generateDungeon(seed);
		for (int i = 0; i < arr.Count; ++i)
		{
			Tile ts = (Tile)arr[i];
			if (ts.type == "spawn")
			{
				Instantiate(playerPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -.3f), Quaternion.Euler(-90, 0, 0));
			}else
			if (ts.type == "enemy")
			{
				Instantiate(enemyPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -.3f), Quaternion.Euler(-90, 0, 0));
			}else
			if (ts.type == "floor")
			{
				Instantiate(floorPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -.2f), Quaternion.Euler(-90, 0, 0));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
