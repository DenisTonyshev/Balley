using UnityEngine;

/// <inheritdoc />
/// <summary>
/// The bat
/// </summary>
public class Bat : MonoBehaviour
{
	
	public int Power=25;
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.name.Contains("enemy")) return;
		
		var enemy = (Enemy) other.gameObject.GetComponent(typeof(Enemy));
		enemy.GetHit(Power);
	}
}
