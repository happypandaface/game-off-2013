using UnityEngine;
using System.Collections;

public class GUITextureButton : MonoBehaviour 
{
	#region vars
	public GameObject my_gui; //the GUI object that handles this
	GUITexture my_texture;
	#endregion

	//We want to use x, y
	//inset x, y, w, h 
	//and mouse to determine if the button has been pushed.
	//from there, we send a message to the GUI
	//and it handles deleting objects, state changes, all that.

	// Use this for initialization
	void Start () 
	{
		my_texture = this.gameObject.GetComponent<GUITexture>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( Input.GetMouseButtonDown( 0 ) ) //left clicked
		{
			int min_x = (int)(Screen.width * this.gameObject.transform.position.x + my_texture.pixelInset.x);
			int max_x = (int)(Screen.width * this.gameObject.transform.position.x + my_texture.pixelInset.x + my_texture.pixelInset.width);
			int min_y = (int)(Screen.width * this.gameObject.transform.position.y + my_texture.pixelInset.y);
			int max_y = (int)(Screen.width * this.gameObject.transform.position.y + my_texture.pixelInset.y + my_texture.pixelInset.height);

			//check if mouse position is in bounds
			if ( Input.mousePosition.x >= min_x && Input.mousePosition.x <= max_x )
			{
				if ( Input.mousePosition.y >= min_y && Input.mousePosition.y <= max_y )
				{
					//clicked
					//my_gui.GetComponent<>().();
				}
			}
		}
	}
}
