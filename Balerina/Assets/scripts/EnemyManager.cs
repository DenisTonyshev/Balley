
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
	public float Rate=6f;
	public int Grid;
	public int Width, Height;
	private float _curGameTime;
	private float _dTimeEnemyCome;
	private float _lastTimeEnemyCome;
	
	
	public List<GameObject> Enemies;
	private List<GameObject> _enemyResources;
//	private Text _txt;

	private void Start ()
	{
		Random.InitState(154512);
		Grid = 6;
		
		_curGameTime = 0;
		Width = Screen.width;
		Height = Screen.height;
//		_txt = GameObject.Find("Text").GetComponent<Text>();
//		_txt.text = "width=" + _width+" height="+_height+"\n";
		
		var enemyBlue = (GameObject) Resources.Load("prefabs/enemyBlue", typeof(GameObject));
		var enemyOrange = (GameObject)Resources.Load("prefabs/enemyOrange", typeof(GameObject));
		var enemyPink = (GameObject) Resources.Load("prefabs/enemyPink", typeof(GameObject));

		_enemyResources = new List<GameObject> {enemyBlue, enemyOrange, enemyPink};

		_dTimeEnemyCome = Random.Range(Rate - Rate / 2, Rate + Rate / 2);
		_lastTimeEnemyCome = _curGameTime;

	}
	

	private void Update ()
	{
		_curGameTime+=Time.deltaTime;
		if (_curGameTime >= _lastTimeEnemyCome + _dTimeEnemyCome)
		{
			var enemy = Instantiate(_enemyResources[ (int)(Random.value*_enemyResources.Count)]);
			var targerWorldPos=Camera.main.ScreenToWorldPoint
			(new Vector3 (
				Random.value*Width*0.75f+Width*0.13f,
				Height*1.1f,
				0));
			// we need set it to zero because if we get source z=0? after tranform we get z axis equals -9 
			targerWorldPos.z = 0;
			enemy.transform.position = targerWorldPos; 
			
//			_txt.text += enemy.transform.position.x.ToString(CultureInfo.CurrentCulture);
			
			Enemies.Add(enemy);
			_dTimeEnemyCome = Random.Range(Rate - Rate / 2, Rate + Rate / 2);
			_lastTimeEnemyCome = _curGameTime;
		}
	}
	

}
