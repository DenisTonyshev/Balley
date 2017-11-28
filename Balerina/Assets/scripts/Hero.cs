using UnityEngine;
using UnityEngine.UI;


public class Hero : MonoBehaviour
{

    public int Health = 100;
    
    private float _startTime;
    private float _endTime;
    private Vector3 _startPos;
    private Vector2 _endPos;
    private Vector2 _swipeDistance;
    public Vector2 MinSwipeDistance;
    private float _swipeTime;
     

    // this is a koefficient that impacts a force to make right speed
    public float KSpeed = 700f;
    public float MaxWalkSpeed = 8f;
    public float MaxJumpSpeed = 5f;

    public bool _isHitting;
    
  
    
    
   // private float _walkSpeed;
    private float _jumpSpeed;
    
    private Vector2 _touchPos;
    private Text _text;
    private Animator _animator;

    private Rigidbody2D _rb;
    

    private void Start()
    {
        _isHitting = false;
        _rb = GetComponent<Rigidbody2D>();
        _text = GameObject.Find("Text").GetComponent<Text>();
        _text.text = "";

        MinSwipeDistance = new Vector3(30f, 30f);
        
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }


    private void Update()
    {
        // is plaing animation of bat hit;
        _isHitting = AnimatorIsPlaying("hero_hit");
       
    if (Input.GetMouseButtonDown(0))
        {
            _startTime = Time.time;
            _startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endTime = Time.time;
            _swipeTime = _endTime - _startTime;
            _swipeDistance = Input.mousePosition - _startPos;
            if (Mathf.Abs(_swipeDistance.x) <= MinSwipeDistance.x && 
                Mathf.Abs(_swipeDistance.y) <= MinSwipeDistance.y)
            {
                Hit();
                return;
            }

            if (Mathf.Abs(_swipeDistance.y) > Mathf.Abs(_swipeDistance.x))
            {
                
                Jump(CalcSpeed(_swipeDistance.y, _swipeTime, MaxJumpSpeed));
                return;
            }
 
            if (_swipeDistance.x < 0)
                WalkLeft(CalcSpeed(_swipeDistance.x, _swipeTime, MaxWalkSpeed));
            if (_swipeDistance.x > 0)
                WalkRight(CalcSpeed(_swipeDistance.x, _swipeTime, MaxWalkSpeed));
        }
        /*       
        
        if (Input.touchCount <= 0) return;  
        var touch = Input.GetTouch (0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _startTime = Time.time;
                _startPos = touch.position;
                break;
           
            case TouchPhase.Ended:
                _endTime = Time.time;
                _endPos = touch.position;
                _swipeDistance = _endPos.x - _startPos.x;
                _swipeTime = _endTime - _startTime;
                
                _speed = _swipeDistance / _swipeTime;
               
                _text.text +=_swipeTime.ToString() ;

                if (Mathf.Abs (_swipeDistance) > 20f) {
                    if (_swipeDistance < 0) {
                       WalkRight();
                    }
                    if (_swipeDistance > 0) {
                       WalkLeft();
                    }
                } else {
                    Hit();
                }
            
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Canceled:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        */
    }


    private float CalcSpeed(float distance, float swipeTime, float maxSpeed)
    {
        var sp = distance / swipeTime / KSpeed;
        return Mathf.Abs(sp) > maxSpeed ? maxSpeed : sp;
    }
    
    private void Hit()
    {
        _animator.enabled = true;
        _animator.Play("hero_hit", -1, 0f);
    }

    private void Jump(float force)
    {  
        _rb.velocity = new Vector2(_rb.velocity.x, force);
    }

    private void WalkRight(float speed)
    {
        Turn(-1);
        Walk(Mathf.Abs(speed));
    }

    private void WalkLeft(float speed)
    {   
        Turn(1);
        Walk(Mathf.Abs(speed) * -1f);
    }

    private void Walk(float speed)
    {
        _rb.velocity = new Vector2(speed, _rb.velocity.y);
    }

    /// <summary>
    /// sign должно быть 1 или -1
    /// </summary>
    /// <param name="sign"></param>
    private void Turn(int sign)
    {
        if (sign != -1 && sign != 1)
        {
            return;
        }

        var scale = transform.localScale;
        scale.x=Mathf.Abs(scale.x)*sign;
        transform.localScale = scale;
    }

    bool AnimatorIsPlaying(string stateName){
        return AnimatorIsPlaying() && _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    
    bool AnimatorIsPlaying(){
        return _animator.GetCurrentAnimatorStateInfo(0).length >
               _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
  
}