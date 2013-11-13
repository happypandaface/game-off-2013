using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour 
{
	#region vars
	public GameObject player;
	//Access player's attributes via script.
	private PlayerCore player_core;

	public GameObject  hp_text_obj;  //Object holds transform info
	private GUIText    hp_text;      //Actual GUI Text object

	public GameObject  hp_bar_obj;   //Object holds transform info
	private GUITexture hp_bar;       //Actual GUI Graphic


	public GameObject  sta_text_obj; //Object holds transform info
	private GUIText    sta_text;     //Actual GUI Text object

	public GameObject  sta_bar_obj;  //Object holds transform info
	private GUITexture sta_bar;      //Actual GUI Graphic
	#endregion


	// Use this for pre-initialization
	void Awake()
	{
		player_core = player.GetComponent<PlayerCore>();

		hp_text = hp_text_obj.GetComponent<GUIText>();
		hp_bar  = hp_bar_obj.GetComponent<GUITexture>();

		sta_text = sta_text_obj.GetComponent<GUIText>();
		sta_bar  = sta_bar_obj.GetComponent<GUITexture>();
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		hp_bar.pixelInset = new Rect( 0.0f, 0.0f, Mathf.Max(1, player_core.hp / player_core.max_hp * 100.0f), 10.0f );
		sta_bar.pixelInset = new Rect( 0.0f, 10.0f, Mathf.Max(1, player_core.sta / player_core.max_sta * 100.0f), 10.0f );

		hp_text.pixelOffset = new Vector2( 0.0f, 0.0f + 10.0f );
		hp_text.text = "HP: " + (int)player_core.hp + "/" + (int)player_core.max_hp;

		sta_text.pixelOffset = new Vector2( 0.0f, 10.0f + 10.0f);
		sta_text.text = "STA: " + (int)player_core.sta + "/" + (int)player_core.max_sta;
	}
}
