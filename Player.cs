using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public GameObject bulletObject;
	private AudioSource[] soundList;

	GameManager myGameManager;

	float timeCountDownFire, timeRevival, timeOnReborn, timeFlash;
	private Animator animator;
	public int currentState;
	public int typeBullet;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		soundList = gameObject.GetComponents<AudioSource>();
		typeBullet = PlayerConstant.TypeBullet.SIMPLE;

		timeCountDownFire = 0;
		currentState = PlayerConstant.PlayerState.ON_FLASH;
		timeFlash = PlayerConstant.TimeInfo.TIME_CHANGE_FLASH;
		timeOnReborn = PlayerConstant.TimeInfo.TIME_FLASH_ON_START;
	}

	public void SetGameManager(GameManager pGameMgr) {
		this.myGameManager = pGameMgr;
	}

	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case PlayerConstant.PlayerState.ON_FLASH:
			OnFlash();
			break;

		case PlayerConstant.PlayerState.ON_CONTROL:
			OnControl();
			break;

		case PlayerConstant.PlayerState.ON_PANG:
			break;

		case PlayerConstant.PlayerState.ON_REVIVAL:
			OnRevival();
			break;
		}
	}

	void OnFlash() {
		timeFlash -= Time.deltaTime;
		if (timeFlash < 0) {
			timeFlash = PlayerConstant.TimeInfo.TIME_CHANGE_FLASH;
			transform.renderer.enabled = !transform.renderer.enabled;
		}

		timeOnReborn -= Time.deltaTime;
		if (timeOnReborn < 0) {
			currentState = PlayerConstant.PlayerState.ON_CONTROL;
			transform.renderer.enabled = true;
		}
	}

	void OnRevival() {
		if (timeRevival > 0) {
			timeRevival -= Time.deltaTime;
		}
		else {
			currentState = PlayerConstant.PlayerState.ON_CONTROL;
			transform.position = PlayerConstant.Position.POS_REBORN;
		}
	}


	void OnControl() {
		if (timeCountDownFire > 0) {
			timeCountDownFire -= Time.deltaTime;
			
			if (timeCountDownFire <= 0) {
				animator.SetBool("IsIdle", true);
				animator.SetBool("IsWait", false);
			}
		}
	}

	public bool OnFighting() {
		return (currentState == PlayerConstant.PlayerState.ON_CONTROL);
	}

	public int GetState() {
		return currentState;
	}

	public void Move(bool isLeft) {
		if (currentState != PlayerConstant.PlayerState.ON_CONTROL) 
			return;

		if (isLeft) {
			if (transform.position.x > PlayerConstant.Position.NEO_LEFT)
				transform.Translate( Vector3.left * PlayerConstant.PLAYER_SPEED * Time.deltaTime);
		}
		else {
			if (transform.position.x < PlayerConstant.Position.NEO_RIGHT)
				transform.Translate( Vector3.right * PlayerConstant.PLAYER_SPEED * Time.deltaTime);
		}
	}

	public void Fire() {
		if (timeCountDownFire > 0)
			return;

		if (currentState != PlayerConstant.PlayerState.ON_CONTROL) 
			return;

		timeCountDownFire = PlayerConstant.TimeInfo.TIME_COUNT_DOWN_FIRE;
		
		Vector3 pos = transform.position;
		pos.y += PlayerConstant.BulletInfo.DELTA_Y_BULLET;
		
		animator.SetBool("IsIdle", false);
		animator.SetBool("IsWait", true);

		//typeBullet = PlayerConstant.TypeBullet.QUADRA;

		switch(typeBullet) {
		case PlayerConstant.TypeBullet.SIMPLE:
			Fire1(pos);
			break;

		case PlayerConstant.TypeBullet.DOUBLE:
			Fire2(pos);
			break;

		case PlayerConstant.TypeBullet.TRIPLE:
			Fire3(pos);
			break;

		case PlayerConstant.TypeBullet.QUADRA:
			Fire4(pos);
			break;

		case PlayerConstant.TypeBullet.PENTA:
			Fire5(pos);
			break;
		}

		PlaySoundByIndex(PlayerConstant.SoundIndex.FIRE);
	}

	void PlaySoundByIndex(int pIndex) {
		for (int i = 0; i < pIndex; i++) {
			soundList[i].Stop();
		}
		for (int i = pIndex+1; i < soundList.Length; i++) {
			soundList[i].Stop();
		}

		soundList [pIndex].Play ();
	}

	void FireBullet(Vector3 pos, float alpha) {
		PlayerBullet playerBullet = myGameManager.GetBulletFire();
		playerBullet.SetDirect(alpha);
		playerBullet.SetPosition(pos);
		playerBullet.Active();
	}

	void Fire1(Vector3 pos) {
		FireBullet (pos, 0);
	}

	void Fire2(Vector3 pos) {
		pos.x -= 0.03f;
		FireBullet (pos, 7.5f);
		
		pos.x += 0.06f;
		FireBullet (pos, 352.5f);
	}

	void Fire3(Vector3 pos) {
		FireBullet (pos, 0);
		
		pos.x -= 0.07f;
		FireBullet (pos, 15);
		//			
		pos.x += 0.14f;
		FireBullet (pos, 345);
	}

	void Fire4(Vector3 pos) {
		pos.x -= 0.09f;
		FireBullet (pos, 18);

		pos.x += 0.06f;
		FireBullet (pos, 6);
		
		pos.x += 0.06f;
		FireBullet (pos, 354f);

		pos.x += 0.06f;
		FireBullet (pos, 342f);
	}

	void Fire5(Vector3 pos) {
		Fire2 (pos);
		pos.x -= 0.03f;
		Fire3 (pos);
	}

	public float GetX() {
		 return transform.localPosition.x;
	}

	void OnTriggerEnter2D (Collider2D other) {
		Debug.Log ("Collider: " + other.name);
		if (other.tag == "enemy") {
			Enemy enemy = other.GetComponent<Enemy>();
			if (enemy.GetState() != EnemyConstant.EnemyState.ON_PANG) {
				if (OnFighting()) {
					Pang();
					enemy.Pang();
				}
			}
		}
		else if (other.tag == "enemyBullet") {
			if (OnFighting()) {
				Pang();
				//Destroy(other.gameObject);
				other.gameObject.SetActive(false);
			}
		}
		else if (other.tag == "rewardBullet") {
			if (OnFighting()) {
				if (typeBullet != PlayerConstant.TypeBullet.PENTA) {
					typeBullet++;
				}
				Destroy(other.gameObject);
			}
		}
	}
	
	void Pang() {
		currentState = PlayerConstant.PlayerState.ON_PANG;
		typeBullet = PlayerConstant.TypeBullet.SIMPLE;

		animator.SetBool ("IsIdle", false);
		animator.SetBool ("IsWait", false);
		animator.SetBool ("IsExp", true);

		PlaySoundByIndex(PlayerConstant.SoundIndex.PANG);

		myGameManager.PangPlayer ();
	}

	/**
	 * Event on Animation (Pang_animation)
	 * // call to GameManager to check
	 */
	public void EndPang() {
		Debug.Log("Done!");
		if (myGameManager.RevivalLife ()) {
			ReCreate ();
		}
		else {
			// GAME OVER
			this.currentState = PlayerConstant.PlayerState.ON_GAME_OVER;
			this.gameObject.SetActive(false);
		}
	}

	public void ReCreate() {
		animator.SetBool ("IsExp", false);
		animator.SetBool ("IsIdle", true);
		currentState = PlayerConstant.PlayerState.ON_REVIVAL;
		timeRevival = PlayerConstant.TimeInfo.TIME_REBORN;
		transform.position = PlayerConstant.Position.POS_WAIT_REBORN;
	}
}
