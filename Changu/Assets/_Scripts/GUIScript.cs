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

	public GameObject hp_bar_cap_r_obj;
	private GUITexture hp_bar_cap_r;

	public Texture2D tex_near_death; //"PANIC" screen overlay
	private float red_flash_period = 0.0f;
	private float red_flash_speed = 1.0f; //speed scaling for super panic effect.

	public GameObject  sta_text_obj; //Object holds transform info
	private GUIText    sta_text;     //Actual GUI Text object

	public GameObject  sta_bar_obj;  //Object holds transform info
	private GUITexture sta_bar;      //Actual GUI Graphic

	public GameObject sta_bar_cap_r_obj;
	private GUITexture sta_bar_cap_r;
	#endregion


	// Use this for pre-initialization
	void Awake()
	{
		player_core = player.GetComponent<PlayerCore>();

		hp_text = hp_text_obj.GetComponent<GUIText>();
		hp_bar  = hp_bar_obj.GetComponent<GUITexture>();
		hp_bar_cap_r = hp_bar_cap_r_obj.GetComponent<GUITexture>();

		sta_text = sta_text_obj.GetComponent<GUIText>();
		sta_bar  = sta_bar_obj.GetComponent<GUITexture>();
		sta_bar_cap_r = sta_bar_cap_r_obj.GetComponent<GUITexture>();
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		hp_bar.pixelInset = new Rect( 0.0f, 0.0f, Mathf.Max(1, player_core.hp / player_core.max_hp * 100.0f), 16.0f );
		sta_bar.pixelInset = new Rect( 0.0f, 0.0f, Mathf.Max(1, player_core.sta / player_core.max_sta * 100.0f), 16.0f );

		hp_bar_cap_r.pixelInset = new Rect( hp_bar.pixelInset.x + hp_bar.pixelInset.width, 
		                                    hp_bar_cap_r.pixelInset.y, 
		                                    hp_bar_cap_r.pixelInset.width, 
		                                    hp_bar_cap_r.pixelInset.height 
		                                   );
		sta_bar_cap_r.pixelInset = new Rect( sta_bar.pixelInset.x + sta_bar.pixelInset.width,
		                                       sta_bar_cap_r.pixelInset.y,
		                                       sta_bar_cap_r.pixelInset.width,
		                                       sta_bar_cap_r.pixelInset.height 
		                                      );

		hp_text.pixelOffset = new Vector2( 0.0f, 17.0f );
		hp_text.text = "HP: " + (int)player_core.hp + "/" + (int)player_core.max_hp;

		sta_text.pixelOffset = new Vector2( 0.0f, 17.0f );
		sta_text.text = "STA: " + (int)player_core.sta + "/" + (int)player_core.max_sta;

		//Low HP Alarm!
		red_flash_speed = ( 1.0f  - (player_core.hp / player_core.max_hp) ) * 2.0f; //0 to 2. ( 1 @ 50%)
		red_flash_speed = red_flash_speed * 2.0f; //static scaling.

		red_flash_period +=  red_flash_speed * Time.deltaTime;
		if ( red_flash_period > Mathf.PI * 2.0f )
		{
			red_flash_period -= Mathf.PI * 2.0f;
		}
	}

	void OnGUI()
	{
		//alpha varies periodically between 50 - 100% of the value it should have.
		//display should begin to appear at 50% HP, and have alpha = 1 at 0% HP.
		float mult =  (0.25f * Mathf.Sin ( red_flash_period ) + 0.75f); // 0.5 - 1.0 x
		float alpha = Mathf.Max ( mult * (-1.0f + 2.0f * (1.0f - (player_core.hp / player_core.max_hp) ) ), 0.0f );
		GUI.color = new Color( 1.0f, 1.0f, 1.0f, alpha );

		GUI.DrawTexture( new Rect( 0, 0, Screen.width, Screen.height), tex_near_death );

		GUI.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
	}
}
