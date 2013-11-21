using UnityEngine;
using System.Collections;

public class titleSplashScreen : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		this.gameObject.GetComponent<GUITexture>().pixelInset = new Rect( 0, 0, Screen.width, Screen.height );
	}
}
