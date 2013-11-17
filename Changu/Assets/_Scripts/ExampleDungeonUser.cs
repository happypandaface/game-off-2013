using UnityEngine;
using System.Collections;

public class ExampleDungeonUser : MonoBehaviour 
{
	#region vars
	public GameObject dungeon;

	public GameObject playerPrefab;
	public GameObject floorPrefab;
	public GameObject enemyPrefab;
	public GameObject wallPrefab;

	private int tileSize = 1;
	//private float speed = 100;
	#endregion
	
	void Start () 
	{
		float seed = Random.value;
		DungeonGenerator dg = new DungeonGenerator();
		dg.config(seed, 30, 15, 5);// parameters (random seed, width, height, # of rooms)
		ArrayList arr = dg.generateDungeon();
		for (int i = 0; i < arr.Count; ++i)
		{
			Tile ts = (Tile)arr[i];
			if (ts.type == "spawn")
			{
				Instantiate(playerPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.identity );
				//transform.position = new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -10); //I don't think this does what you think?
			}
			else if (ts.type == "enemy")
			{
				Instantiate(enemyPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.identity );
			}
			else if (ts.type == "floor")
			{
				GameObject tile = (GameObject)Instantiate(floorPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.identity );
				tile.transform.parent = dungeon.transform;
			}
			else if (ts.type == "wall")
			{
				GameObject tile = (GameObject)Instantiate(wallPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.identity );
				tile.transform.parent = dungeon.transform;
			}
		}
	}

	//Why move the generator?
	/*
	void Update () {
		if (Input.GetKey(KeyCode.W))
		{
			transform.Translate(0,speed*Time.deltaTime,0);
		}
		if (Input.GetKey(KeyCode.S))
		{
			transform.Translate(0,-speed*Time.deltaTime,0);
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.Translate(speed*Time.deltaTime,0,0);
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.Translate(-speed*Time.deltaTime,0,0);
		}
	}
	*/
}
