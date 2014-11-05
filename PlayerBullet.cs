using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.up * PlayerConstant.BulletInfo.V_BULLET * Time.deltaTime);

		if ((Mathf.Abs(transform.position.x) > GameConstant.CameraSize.WIDTH_half)
		    || (Mathf.Abs(transform.position.y) > GameConstant.CameraSize.HEIGHT_half)) {
			//Destroy(this.gameObject);
			gameObject.SetActive(false); 
		}
	}

	public void SetDirect(float angle) {
		Quaternion temp = transform.rotation;
		temp.z = 0;
		transform.rotation = temp;
		transform.Rotate(new Vector3(0, 0, angle));
	}
	
	public bool OnUse() {
		return gameObject.activeInHierarchy;
	}
	
	public void SetPosition(Vector3 pPos) {
		transform.position = pPos;
	}
	
	public void Active() {
		gameObject.SetActive (true);
	}

	
	void OnTriggerEnter2D (Collider2D other) {
		Debug.Log ("Collider: " + other.name);

		if (other.tag == "enemy") {
			Enemy enemy = other.GetComponent<Enemy>();
			if (enemy.GetState() != EnemyConstant.EnemyState.ON_PANG) {
				enemy.Pang();
				//Destroy(this.gameObject);
				gameObject.transform.position = PlayerConstant.Position.POS_WAIT_REBORN;
				gameObject.SetActive(false);
			}
		}
	}
}
