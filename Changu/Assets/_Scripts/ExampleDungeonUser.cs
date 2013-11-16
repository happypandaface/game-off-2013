using UnityEngine;
using System.Collections;

public class ExampleDungeonUser : MonoBehaviour {
	
	public Transform playerPrefab;
	public Transform floorPrefab;
	public Transform enemyPrefab;
	public Transform wallPrefab;
	private int tileSize = 10;
	private float speed = 100;
	
	void Start () {
		float seed = Random.value;
		DungeonGenerator dg = new DungeonGenerator();
		dg.config(seed, 30, 15, 5);// parameters (random seed, width, height, # of rooms)
		ArrayList arr = dg.generateDungeon();
		for (int i = 0; i < arr.Count; ++i)
		{
			Tile ts = (Tile)arr[i];
			if (ts.type == "spawn")
			{
				Instantiate(playerPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -.3f), Quaternion.Euler(-90, 0, 0));
				transform.position = new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -10);
			}else
			if (ts.type == "enemy")
			{
				Instantiate(enemyPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -.3f), Quaternion.Euler(-90, 0, 0));
			}else
			if (ts.type == "floor")
			{
				Instantiate(floorPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -.2f), Quaternion.Euler(-90, 0, 0));
			}else
			if (ts.type == "wall")
			{
				Instantiate(wallPrefab, new Vector3(ts.xPos*tileSize, ts.yPos*tileSize, -.2f), Quaternion.Euler(-90, 0, 0));
			}
		}
	}
	
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
}
