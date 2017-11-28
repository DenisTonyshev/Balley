using UnityEngine;





public class Enemy : MonoBehaviour {
	private Animator _anim;
	public int Health=100;
	public int SkinTouchDamage=1;
	public float VisibleDistance=1f;
	private Vector2 _moveToPoint;
	private Rigidbody2D _rb;
	private float _speed;
	enum States {Alive, Dead};
	private States  _state;
	private HeroScanner _heroScanner;
	
	// Scans fo Hero
	private class HeroScanner
	{
		private Ray2D _ray;
		private float _angle;
		private float _distance;
		private RaycastHit2D _hit;
		public  float AngularSpeed;

		public HeroScanner(float angle, float angularSpeed)
		{
			_angle = angle;
			AngularSpeed = angularSpeed;
		}

		public RaycastHit2D Scan(Vector2 scanFrom) 
		{
			var scanTo = new Vector2(Mathf.Cos(_angle* Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
			_angle += AngularSpeed;

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
		_state = States.Alive;
		_rb = GetComponent<Rigidbody2D>();
		_anim = GetComponent<Animator>();
		_anim.enabled = false;
		_speed = 1f;
		_heroScanner=new HeroScanner(0f,0.5f);
		_moveToPoint.x = Random.value*4+1;
	}

	public void GetHit(int amount)
	{
		Health -= amount;
		if (Health<=0)
		{
			Die();
		}
	}

	private void Die()
	{
		_anim.enabled = true;
		_anim.Play("hero_hit", -1, 0f);
		_state = States.Dead;
		_speed = 0f;
		_rb.bodyType = RigidbodyType2D.Static;

	}

	private void LookForHero()
	{	
		if (_state != States.Alive) 				return;
		var hit= _heroScanner.Scan(transform.position);
		if (hit.collider == null) 					return;
		if (!hit.collider.name.Contains("hero")) 	return;
		if (!(hit.distance <= VisibleDistance)) 	return;
		
//		Debug.Log(hit.distance);
		_moveToPoint = hit.collider.transform.position;
		
	}


	private void Update()
	{
		if (_state != States.Alive) return;
		LookForHero();
	}


	private void FixedUpdate()
	{
		if (_state != States.Alive) return;
		Move();
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
		
	_rb.velocity = new Vector2(_speed, _rb.velocity.y);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.name.Contains("wall")) return;
		
		_moveToPoint.x = Random.value*3.5f+1.5f;
		
	}
}

