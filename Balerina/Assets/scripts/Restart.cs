using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Restart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnMouseDown()
	{
		Debug.Log("restart");
		var enemyManager = (EnemyManager) GameObject.Find("Scripts").gameObject.GetComponent(typeof(EnemyManager));
		
		
		enemyManager.Restart();
		

		var hero = (Hero) GameObject.Find("hero").gameObject.GetComponent(typeof(Hero));
		hero.Health = 100;

		var gameWorld = (GameWorld) GameObject.Find("Scripts").gameObject.GetComponent(typeof(GameWorld));
		
		var restartPosition= Camera.main.ScreenToWorldPoint (gameWorld.Center);
		restartPosition.z = 1f;
		hero.StopAllForces();
		hero.transform.position = restartPosition;
		
		
	}
}
