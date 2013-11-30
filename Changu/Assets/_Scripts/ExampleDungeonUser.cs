using UnityEngine;
using System.Collections;

public class ExampleDungeonUser : MonoBehaviour 
{
	#region vars
	public GameObject dungeon;

	//public GameObject playerPrefab;
	public GameObject playerInstance; //changed to using 1 player instance.
	public GameObject floorPrefab;
	public GameObject enemyPrefab;
	public GameObject wallPrefab;
	public GameObject wallPrefabMedieval;
	public GameObject wallPrefabMedievalCracked;
	public GameObject wallPrefabMedievalVines;
	public GameObject wallPrefabSteampunk;
	public GameObject wallPrefabFuture;

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
				//Instantiate(playerPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.identity );
				//transform.position = new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -10); //I don't think this does what you think?
				playerInstance.transform.position = new Vector3( ts.xPos * tileSize, ts.yPos * tileSize, 0.0f );
				Camera.main.transform.position = new Vector3( ts.xPos * tileSize, ts.yPos * tileSize, -10.0f );
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
				GameObject tile;

				//Gen check.
				if ( true )
				{
					//subtypes: wall, crack, vine
					tile = (GameObject)Instantiate(wallPrefabMedieval, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.identity );
				}
				else if ( true )
				{
					tile = (GameObject)Instantiate(wallPrefabSteampunk, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.identity );
				}
				else if ( true )
				{
					tile = (GameObject)Instantiate(wallPrefabFuture, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.identity );
				}
				else
				{
					tile = (GameObject)Instantiate(wallPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, 0.0f), Quaternion.identity );
				}
				//Move it.
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
