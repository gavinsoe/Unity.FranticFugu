using UnityEngine;
using System.Collections;

public class PCController : MonoBehaviour 
{
    public static PCController instance;

    public float moveSpeed;
    public float sensitivity;
    private Vector2 moveDirection;
    private Vector2 distance;
    public float turnSpeed;
    public Vector2 moveToward;
    private bool isMoving;
    private Vector2 touchStart;
    private Vector2 touchCurrent;

    private float worldWidth, worldHeight;

    void Awake()
    {
        // make sure there is only 1 instance of this class.
        //if (instance == null)
        //{
            DontDestroyOnLoad(gameObject);
            instance = this;
        //}
            /*
        else if (instance != this)
        {
            Destroy(gameObject);
        }*/
    }

	// Use this for initialization
	void Start () 
    {
        transform.position = new Vector3(0, 0, 0);
        moveDirection = Vector2.right;
        moveToward = transform.position;
        isMoving = false;
        worldHeight = Camera.main.orthographicSize * 2f;
        worldWidth = worldHeight * Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 currentPosition = transform.position;
        if (!SpawnController.instance.paused)
        {
            /*
            if (Input.GetButton("Fire1"))
            {
                moveToward = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                moveDirection = moveToward - currentPosition;
                //moveDirection.z = 0;
                moveDirection.Normalize();
                //moveToward.z = 0;
            }

            transform.position = Vector2.MoveTowards(currentPosition, moveToward, Time.deltaTime * moveSpeed);
            */

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                isMoving = true;
                touchStart = Input.GetTouch(0).position;
            }
            else if (Input.touchCount > 0 && (/*Input.GetTouch(0).phase == TouchPhase.Stationary || */Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                touchCurrent = Input.GetTouch(0).position;
                //moveSpeed = Vector2.Distance(touchStart, touchCurrent) / 20f;
                //moveToward = touchCurrent - touchStart + currentPosition;
                Vector2 moved = touchCurrent;
                moved.x = touchCurrent.x - touchStart.x;
                moved.y = touchCurrent.y - touchStart.y;
                moved.x = (moved.x * worldWidth / Screen.width) * sensitivity;
                moved.y = (moved.y * worldHeight / Screen.height) * sensitivity;
                moveToward = currentPosition + moved;
                touchStart = Input.GetTouch(0).position;

                if (moveSpeed > 10)
                {
                    moveSpeed = 10;
                }
            }
            else if (Input.touchCount == 0 && isMoving == true)
            {
                isMoving = false;
                touchStart = currentPosition;
                touchCurrent = currentPosition;
            }

            if (isMoving)
            {
                transform.position = Vector2.MoveTowards(currentPosition, moveToward, Time.deltaTime * moveSpeed);
            }

            /*
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation =
              Quaternion.Slerp(transform.rotation,
                                Quaternion.Euler(0, 0, targetAngle),
                                turnSpeed * Time.deltaTime);
             */
        }
	}

    public void DestroyCharacter()
    {
        Destroy(gameObject);
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
