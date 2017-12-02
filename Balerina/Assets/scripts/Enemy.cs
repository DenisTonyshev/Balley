using System;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyStates {Alive, Dead}

public class Enemy : MonoBehaviour
{
	public int Id;
	public int Health=100;
	public int SkinTouchDamage=1;
	public float VisibleDistance=1f;

	public float MoovingForce = 6f;
	
	private Animator _anim;
	private Vector2 _moveToPoint;
	private Rigidbody2D _rb;
	
	private float _speed=0.2f;

	
	public EnemyStates  EnemyState;
	private GameWorld _gameWorld;
	
	private HeroScanner _heroScanner;
	// Scans fo Hero
	private class HeroScanner
	{
		private Ray2D _ray;
		private float _angle;
		private float _distance;
		private RaycastHit2D _hit;
		private readonly float _angularSpeed;

		public HeroScanner(float angle, float angularSpeed)
		{
			_angle = angle;
			_angularSpeed = angularSpeed;
		}

		public RaycastHit2D Scan(Vector2 scanFrom) 
		{
			var scanTo = new Vector2(Mathf.Cos(_angle* Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
			_angle += _angularSpeed;

//			scanTo.x *= 10;
//			scanTo.y *= 10;
//			
//			Debug.DrawRay(scanFrom, scanTo, Color.green);
			var layerMask = 1 << 10;// | (1 << 10);
			return Physics2D.Raycast(scanFrom, scanTo, 100,layerMask);	
		}
	}
	
	private void Start ()
	{
		Random.InitState(Time.renderedFrameCount);
		EnemyState = EnemyStates.Alive;
		_rb = GetComponent<Rigidbody2D>();
		_anim = GetComponent<Animator>();
		_anim.enabled = false;

		_gameWorld = (GameWorld) GameObject.Find("Scripts").GetComponent(typeof(GameWorld));
	
		_heroScanner=new HeroScanner(0f,0.5f);
		//_moveToPoint.x = Random.value*4+1;
	}

	public void GetHit(int amount)
	{
		Health -= amount;
		if (Health<=0)
		{
			Die();
		}
	}


	private float? SearchNearstCellPosition(float x)
	{
		float min = _gameWorld.Cells[1].Position.x - _gameWorld.Cells[0].Position.x+1f;
		DefaultNamespace.Cell nearestCell=null;
		foreach (var cell in _gameWorld.Cells)
		{
			var diff = Mathf.Abs(x - cell.Position.x);
			if (!(diff < min)) continue;
			min = diff;
			nearestCell = cell;
		}
		if (nearestCell != null) return nearestCell.Position.x;
		return null;
	}
	
	
	
	private void Die()
	{
		var nearesCellpos = SearchNearstCellPosition(transform.position.x);
		if (nearesCellpos == null) return;

		transform.position=new Vector2((float)nearesCellpos, transform.position.y );
		_anim.enabled = true;
		_anim.Play("hero_hit", -1, 0f);
		EnemyState = EnemyStates.Dead;
		_speed = 0f;
		_rb.bodyType = RigidbodyType2D.Static;
		
	}

	private void LookForHero()
	{	
		if (EnemyState != EnemyStates.Alive) 				return;
		var hit= _heroScanner.Scan(transform.position);
		if (hit.collider == null) 					return;
		if (!hit.collider.name.Contains("hero")) 	return;
		if (!(hit.distance <= VisibleDistance)) 	return;
		
//		Debug.Log(hit.distance);
		_moveToPoint = hit.collider.transform.position;
		
	}


	private void Update()
	{
		if (EnemyState != EnemyStates.Alive) return;
		LookForHero();
	}

	private void FixedUpdate()
	{
		if (EnemyState != EnemyStates.Alive) return;
		Move();
	}

	private void Jump()
	{
		_rb.AddForce(new Vector2(0f, 100f));
		
	}
	
	private void Move()
	{
		if (transform.position.x > _moveToPoint.x - _speed && transform.position.x < _moveToPoint.x + _speed)
		{
			_moveToPoint.x = Random.value*3.5f+1.5f;
			return;
		}
		if (_moveToPoint.x > transform.position.x)
		{
			_speed = Mathf.Abs(_speed);
		}
		else
		{
			_speed = -Mathf.Abs(_speed);
		}
//		Debug.Log("_speed="+_speed);
		_rb.AddForce(new Vector2(_speed*MoovingForce,0), ForceMode2D.Force);
//	_rb.velocity = new Vector2(_speed, _rb.velocity.y);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (EnemyState==EnemyStates.Dead) return;
		var n = other.gameObject.name;
		if (n.Contains("wall"))
		{
			foreach (var c in other.contacts)
			{
				//Debug.DrawRay(transform.position, -c.normal,Color.green);	
			}
			
		} else if (n.Contains("enemy"))
		{
			var enemy = (Enemy) other.gameObject.GetComponent(typeof(Enemy));
			foreach (var c in other.contacts)
			{
				// if we hitted from top or bottom - we don't jump 
				if ((int) Mathf.Abs(c.normal.x) != (int)1f) continue;
				
				// in we are moovein forwadrd

				// if we are standing - we don't jump
				//if (_rb.velocity.x <= 0.01f) continue;
				// if enemy is dead - we jump
				if (enemy.EnemyState == EnemyStates.Dead)
				{
					//Debug.DrawRay(transform.position, -c.normal, Color.red, 5);
					Jump();
					Debug.Log("Столкноверие с трупом");
				} else if (Id > enemy.Id && Math.Abs(Mathf.Sign(_speed) - (-c.normal.x)) < 0.01)
				{
					//Debug.DrawRay(transform.position, -c.normal, Color.red, 5);
					Jump();
				}
				else
				{
					_rb.velocity = Vector3.zero;	
				}
			}
		} else if (n.Contains("hero"))
		{
			foreach (var c in other.contacts)
			{
				
				//Debug.DrawRay(transform.position, -c.normal,Color.blue);
				
			}
		}
		
		_moveToPoint.x = Random.value*3.5f+1.5f;
		
	}
}

