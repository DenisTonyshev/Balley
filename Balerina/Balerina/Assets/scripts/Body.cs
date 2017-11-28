using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Body : MonoBehaviour
{

	private Hero _hero;
	// Use this for initialization
	void Start () {
		_hero= (Hero) GameObject.Find("hero").GetComponent(typeof(Hero));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.name.Contains("enemy")) return;
		var enemy = (Enemy) other.gameObject.GetComponent(typeof(Enemy));
		_hero.Health -= enemy.SkinTouchDamage;
		if (_hero.Health <= 0)
		{
			Debug.Log("Game over!!!");
		}
		
		//var hearts = GameObject.Find("heart").GetComponent<SpriteRenderer>();
		
//		hearts.size.Set(hearts.drawMode.w _hero.Health/100, hearts.size.y);
	}
}
