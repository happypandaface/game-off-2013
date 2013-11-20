using UnityEngine;
using System.Collections;


//Class that handles displaying cutscene text.

//TODO: Disable all other GUI Elements when this is up.
//TODO: Freeze game while this is up.

//TODO: Add fade in/out, transitions: text change, bg change, timing.


public class Cutscene : MonoBehaviour 
{
	#region vars
	public Texture2D bg_blank; //collection of BGs?

	public GameObject background_obj; //assign in editor
	GUITexture background;

	public GameObject text_obj; //assign in editor
	GUIText text;

	//state
	string state = "Fade In";
	float t = 0.0f;
	float t_goal = 1.0f;
	#endregion

	// Use this for initialization
	void Start () 
	{
		background = background_obj.GetComponent<GUITexture>();
		background.pixelInset = new Rect( 0, 0, Screen.width, Screen.height );
		background.texture = bg_blank;

		text = text_obj.GetComponent<GUIText>();
		text.pixelOffset = new Vector2( Screen.width / 2, 32 );
		text.text = "Once upon a time ...";

		state = "Fade In";
	}
	
	// Update is called once per frame
	void Update () 
	{
		t += Time.deltaTime;
		if ( t >= t_goal ) //state transition
		{
			if ( state == "Fade In" )
			{
				state = "Temp";
				t = 0.0f;
				t_goal = 5.0f;
			}
			else if ( state == "Temp" ) //TODO: change this to some meaningfull transition. Also, allow for text + BG fade in/out.
			{
				state = "Fade Out";
				t = 0.0f;
				t_goal = 1.0f;
			}
			else if ( state == "Fade Out" )
			{
				state = "Off";
				t = 0.0f;
				t_goal = 1.0f;
			}
			else
			{
				//Not on!
				t = 0.0f;
			}
		}

		//Blending.
		if ( state == "Fade In" )
		{
			background.color = new Color( 1.0f, 1.0f, 1.0f, t / t_goal );
		}
		else if ( state == "Fade Out" )
		{
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f - ( t / t_goal ) );
		}
		else if ( state == "Temp" )
		{
			//full alpha
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		}
		else
		{
			//off
			background.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
			text.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
		}
	}
}
