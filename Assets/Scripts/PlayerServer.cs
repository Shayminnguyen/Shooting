using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
[RequireComponent (typeof (Controller2D))]
public class PlayerServer : MonoBehaviour {

    Collider2D coll;
	Controller2D controller;
    public InputManager inputManager;


	//Linh added
	Player player;

	public void UpdatePosition(Vector3 position){
		player.currentPosition = position;
	}

	public void UpdateAim(Vector2 cursor){
		player.cursorPos = cursor;
	}

	public void UpdateShoot(bool isShoot){
		player.isShooting = isShoot;
	}

	public void UpdateDead(bool Dead){
		player.isDead = Dead;
	}

	void Start() {

		player = GetComponent<Player> ();
	}

}
