using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
[RequireComponent (typeof (Controller2D))]
public class PlayerClient : MonoBehaviour {
    Vector2 cursorPos;
    float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	
	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
    bool canShoot = true;
	bool isShoot;

    Collider2D coll;
	Controller2D controller;
    public InputManager inputManager;

	//Linh added
	Player player;

    public void Die()
    {
        print("Dead");
        coll.enabled = false;
        if (inputManager != null)
        {
            inputManager.OnInputXChanged -= OnInputXChanged;
            inputManager.OnJumpButtonPressed -= OnJumpPressed;
            inputManager.OnShootButtonPressed -= OnShootPressed;
            inputManager.OnCursorMoved -= OnCursorMoved;
        }

        StartCoroutine(AnimateDead());
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

	void Start() {
		//Linh added
		player = GetComponent<Player> ();

		controller = GetComponent<Controller2D> ();
		gravity = -(2 * player.jumpHeight) / Mathf.Pow (player.timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * player.timeToJumpApex;

        coll = GetComponent<Collider2D>();

		inputManager = InputManager.Instance;
		if(inputManager != null)
		{
			inputManager.OnInputXChanged += OnInputXChanged;
			inputManager.OnJumpButtonPressed += OnJumpPressed;
			inputManager.OnShootButtonPressed += OnShootPressed;
			inputManager.OnCursorMoved += OnCursorMoved;
		}
	}

    private void OnInputXChanged(float inputX)
    {
		float targetVelocityX = inputX * player.moveSpeed;
        velocity.x = Mathf.SmoothDamp(	velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
    }

    private void OnJumpPressed()
    {
        if (controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }
    }

    private void OnShootPressed()
    {
		Vector2 direction = cursorPos - (Vector2)player.gunTransform.position;
		RaycastHit2D hit = Physics2D.Raycast(player.gunTransform.position, direction, player.maxShootRange, player.platformLayerMask);

        float shootRange = hit ? hit.distance : player.maxShootRange;
		RaycastHit2D[] hits = Physics2D.RaycastAll(player.gunTransform.position, direction, shootRange, player.playerLayerMask);

        foreach(RaycastHit2D playerHit in hits)
        {
            if (playerHit.collider == coll) continue;

			playerHit.collider.GetComponent<Player>().CmdDead(true);

        }

		Vector2 to = (Vector2)player.gunTransform.position + direction.ScaleTo(shootRange);
		RayFactory.Instance.NewRayFromTo(player.gunPointTransform.position, to);

		//Linh add
		isShoot = true;
    }

    private void OnCursorMoved(Vector2 mousePos)
    {
        cursorPos = mousePos;
        Aim();
    }

    private void Aim()
    {
		Vector2 direction = cursorPos - (Vector2)player.gunTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        player.gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (angle > 360) angle -= 360;
        if (angle < 0) angle += 360;
        if (angle > 90 && angle < 270) player.bodyTransform.localScale = new Vector3(-1, 1, 1);
        else player.bodyTransform.localScale = Vector3.one;
    }

	void Reset(){
		isShoot = false;
	}

	void FixedUpdate() {
        if (!coll.enabled) return;

		if (controller.collisions.above || (controller.collisions.below && velocity.y < 0)) {
			velocity.y = 0;
		}

		velocity.y += gravity * Time.fixedDeltaTime;
		controller.Move (velocity * Time.fixedDeltaTime);

		//Linh added
		if (player.isDead) {
			Die ();
		} else {
			player.CmdChangePosition (transform.position);
			player.CmdAim (cursorPos);
			player.CmdShoot (isShoot);

			Reset ();
		}
	}
}
