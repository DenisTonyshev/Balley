
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
	private int _enemyId;
	public float Rate=6f;
	
	private float _curGameTime;
	private float _dTimeEnemyCome;
	private float _lastTimeEnemyCome;
	public int MaxMonstersInField=3;
	private GameWorld _gameworld;
	
	public List<GameObject> Enemies;
	public List<GameObject> DiedEnemies;
	private List<GameObject> _enemyResources;
//	private Text _txt;

	private void Start ()
	{
		_enemyId = 0;
		Random.InitState(154512);
		
		_gameworld = (GameWorld) GameObject.Find("Scripts").GetComponent(typeof(GameWorld));
		
		var enemyBlue = (GameObject) Resources.Load("prefabs/enemyBlue", typeof(GameObject));
		var enemyOrange = (GameObject)Resources.Load("prefabs/enemyOrange", typeof(GameObject));
		var enemyPink = (GameObject) Resources.Load("prefabs/enemyPink", typeof(GameObject));

		_enemyResources = new List<GameObject> {enemyBlue, enemyOrange, enemyPink};

		_dTimeEnemyCome = Random.Range(Rate - Rate / 2, Rate + Rate / 2);
		_lastTimeEnemyCome = _curGameTime;
	}

	private void RemoveDeadEnemies()
	{
		for (var i=0;i<Enemies.Count;i++) 
		{
			var enScript = (Enemy) Enemies[i].GetComponent(typeof(Enemy));
			if (enScript.EnemyState != EnemyStates.Dead) continue;
			
			DiedEnemies.Add(Enemies[i]);
			Enemies.Remove(Enemies[i]);
				
			_lastTimeEnemyCome = _curGameTime;
		}
	}

	private void Update ()
	{
		RemoveDeadEnemies();	

		if (_curGameTime >= _lastTimeEnemyCome + _dTimeEnemyCome)
		{
			if (Enemies.Count >= MaxMonstersInField) return;
			
			var enemy = Instantiate(_enemyResources[(int) (Random.value * _enemyResources.Count)]);
			var i=  Mathf.RoundToInt(Random.Range(0, _gameworld.Cells.Length));
			var spawnPosition=new Vector2(_gameworld.Cells[i].Position.x, _gameworld.FieldHeightWorld);	
			enemy.transform.position = spawnPosition;
		
			var enScript = (Enemy) enemy.GetComponent(typeof(Enemy));
			enScript.Id = _enemyId;
			_enemyId++;
			
			Enemies.Add(enemy);
			_dTimeEnemyCome = Random.Range(Rate - Rate / 2, Rate + Rate / 2);
			_lastTimeEnemyCome = _curGameTime;
		}	
	_curGameTime+=Time.deltaTime;
	}


	public void Restart()
	{
		foreach (GameObject en in Enemies)
			Destroy(en);
		foreach (GameObject en in DiedEnemies)
			Destroy(en);		
		Enemies.Clear();
		DiedEnemies.Clear();
	}
	
}
