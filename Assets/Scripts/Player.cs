using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof(Collider2D))]
[RequireComponent (typeof (Controller2D))]
public class Player : NetworkBehaviour {
    public float moveSpeed = 6;
    public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
    public float shootCooldownTime = .5f;
	public float maxShootRange = 9999;

    public Transform bodyTransform;
    public Transform gunTransform;
    public Transform gunPointTransform;
    public LayerMask platformLayerMask;
    public LayerMask playerLayerMask;


    Collider2D coll;
	Controller2D controller;
    public InputManager inputManager;

	[SyncVar]public Vector3 currentPosition;
	[SyncVar]public Vector2 cursorPos;
	[SyncVar]public bool isShooting;
	[SyncVar]public bool isDead;

	PlayerClient client;
	PlayerObserver observer;
	PlayerServer server;

	void Start() {


		if (isLocalPlayer) {
			client = gameObject.AddComponent<PlayerClient> ();
		} else {
			observer = gameObject.AddComponent<PlayerObserver> ();
		}

		if (isServer) {
			server = gameObject.AddComponent<PlayerServer> ();
		}
	}

	[Command]
	public void CmdChangePosition(Vector3 position){
		currentPosition = position;
	}

	[Command]
	public void CmdAim(Vector2 cursor){
		cursorPos = cursor;
	}

	[Command]
	public void CmdShoot(bool isShoot){
		isShooting = isShoot;
	}

	[Command]
	public void CmdDead(bool Dead){
		isDead = Dead;

	}
}
