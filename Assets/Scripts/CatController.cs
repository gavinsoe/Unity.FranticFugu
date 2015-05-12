using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour {

    public float moveSpeed;
    private Vector3 moveDirection;
    private Vector3 distance;
    public float turnSpeed;
    public Vector3 moveToward;

	// Use this for initialization
	void Start () {
        moveDirection = Vector3.right;
        moveToward = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 currentPosition = transform.position;
        if (!SpawnController.instance.paused)
        {
            /*
            if (Input.GetButton("Fire1"))
            {
                moveToward = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                moveDirection = moveToward - currentPosition;
                moveDirection.z = 0;
                moveDirection.Normalize();
                moveToward.z = 0;
            }

             */

            if (Input.touchCount > 0)
            {
                moveToward = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                moveDirection = moveToward - currentPosition;
                moveDirection.z = 0;
                moveDirection.Normalize();
                moveToward.z = 0;
            }
            transform.position = Vector3.MoveTowards(currentPosition, moveToward, Time.deltaTime * moveSpeed);

            /*
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation =
              Quaternion.Slerp(transform.rotation,
                                Quaternion.Euler(0, 0, targetAngle),
                                turnSpeed * Time.deltaTime);
             */
        }
	}

    /*
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("TRIGGER1");
        if (collider.gameObject.CompareTag("Enemy") == true)
        {
            // Dead
            SpawnController.instance.EndGame();
        }
    }
     * */
}
