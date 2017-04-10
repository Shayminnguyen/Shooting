using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
[RequireComponent (typeof (Controller2D))]
public class PlayerObserver : MonoBehaviour {
    Collider2D coll;
	Controller2D controller;
    public InputManager inputManager;

	//Linh added
	Player player;
	void Start() {
		player = GetComponent<Player> ();
	}
		
	void FixedUpdate() {
		if(player.isDead){
			//coll.enabled = false;
			StartCoroutine(AnimateDead());
		}
		else {
			transform.position = Vector3.Lerp(transform.position,player.currentPosition, 0.1f);
			Aim ();
			if (player.isShooting)
				OnShootPressed ();
		}
	}

	private void Aim()
	{
		Vector2 direction = player.cursorPos - (Vector2)player.gunTransform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		player.gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		if (angle > 360) angle -= 360;
		if (angle < 0) angle += 360;
		if (angle > 90 && angle < 270) player.bodyTransform.localScale = new Vector3(-1, 1, 1);
		else player.bodyTransform.localScale = Vector3.one;
	}

	private void OnShootPressed()
	{
		Vector2 direction = player.cursorPos - (Vector2)player.gunTransform.position;
		RaycastHit2D hit = Physics2D.Raycast(player.gunTransform.position, direction, player.maxShootRange, player.platformLayerMask);

		float shootRange = hit ? hit.distance : player.maxShootRange;
		RaycastHit2D[] hits = Physics2D.RaycastAll(player.gunTransform.position, direction, shootRange, player.playerLayerMask);

		foreach(RaycastHit2D playerHit in hits)
		{
			if (playerHit.collider == coll) continue;

			//playerHit.collider.GetComponent<PlayerClient>().Die();
		}

		Vector2 to = (Vector2)player.gunTransform.position + direction.ScaleTo(shootRange);
		RayFactory.Instance.NewRayFromTo(player.gunPointTransform.position, to);
	}

	private IEnumerator AnimateDead()
	{
		float time = 0;
		while(time < 0.5f)
		{
			transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathfx.Sinerp(0, 1, time / 0.5f));

			time += Time.deltaTime;
			yield return null;
		}

		gameObject.SetActive(false);
	}
}
