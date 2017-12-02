using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Body : MonoBehaviour
{

	private Hero _hero;
	
	void Start () {
		_hero= (Hero) GameObject.Find("hero").GetComponent(typeof(Hero));
		
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
	}
}
