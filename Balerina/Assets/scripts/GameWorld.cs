using DefaultNamespace;
using UnityEngine;



public class GameWorld : MonoBehaviour {

	public int CellCount=7;
	private Vector2 _cellSize;
	private float _wallWidthPrecentage=0.05f;
	private float _fieldWidth;
	private float _fieldLeft;
	private float _fieldHeight;

	public Vector2 Center
	{
		get { return new Vector2(_fieldLeft + _fieldWidth / 2, _fieldHeight / 2); }
	}

	public float FieldHeightWorld
	{
		get {return Camera.main.ScreenToWorldPoint(new Vector2(0,_fieldHeight)).y;}
	}

	public Cell []Cells;

	private void Awake()
	{
		_fieldHeight = Screen.height;
		_fieldWidth = Screen.width - Screen.width * _wallWidthPrecentage * 2;
		_cellSize=new Vector2(_fieldWidth/CellCount, _fieldWidth/CellCount);
		_fieldLeft = Screen.width * _wallWidthPrecentage;
		Cells=new Cell[CellCount];
		for (var i = 0; i < CellCount; i++)
		{
			var cell=new Cell(
				Camera.main.ScreenToWorldPoint(new Vector2(_fieldLeft + i * _cellSize.x + _cellSize.x / 2, 0f)),
				Camera.main.ScreenToWorldPoint(new Vector2(_cellSize.x, _cellSize.y)));
			Cells[i] = cell;
		}
	}
	
	
	

	
}
