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
		var enemyManager = (EnemyManager) GameObject.Find("enemyManager").gameObject.GetComponent(typeof(EnemyManager));
		for (var i = 0; i < enemyManager.Enemies.Count; i++)
		{
			Destroy(enemyManager.Enemies[i]);
		}

		var hero = (Hero) GameObject.Find("hero").gameObject.GetComponent(typeof(Hero));
		hero.Health = 100;
		
		var restartPosition= Camera.main.ScreenToWorldPoint (new Vector2(enemyManager.Width/2, enemyManager.Height/2));
		restartPosition.z = 1f;
		hero.transform.position = restartPosition;
		
	}
}
