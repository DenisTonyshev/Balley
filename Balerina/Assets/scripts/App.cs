
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour {

	// Use this for initialization
	private string _version;

	private void Start ()
	{
/*version_key*/		_version = "17-12-02.223751";
		Text text = GameObject.Find("version").GetComponent<Text>();
		text.text = "Build version: "+_version;
	
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
