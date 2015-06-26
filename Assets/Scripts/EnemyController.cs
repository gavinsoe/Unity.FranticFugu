using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
    private Vector3 endPoint;
    private float speed;
    private Vector3 moveDirection;
    public int enemyType;

	// Use this for initialization
	void Start () 
    {
        int start = Random.Range(0, 13);
        int end = 0;

        // Update the sprite layers to prevent the eyes from staying on top of all bodies
        UpdateSortingLayer(start);

        if (start == 0)
        {
            end = Random.Range(6, 9);
        }
        else if (start >= 1 && start <= 3)
        {
            end = Random.Range(6, 12);
        }
        else if (start == 4)
        {
            end = Random.Range(9, 12);
        }
        else if (start >= 5 && start <= 6)
        {
            end = Random.Range(1, 8);
            if (end >= 4 && end <= 7)
            {
                end += 6;
            }
        }
        else if (start == 7)
        {
            end = Random.Range(1, 4);
            if (end >= 4 && end <= 5)
            {
                end += 8;
            }
        }
        else if (start >= 8 && start <= 10)
        {
            end = Random.Range(1, 7);
            if (end >= 7 && end <= 8)
            {
                end += 5;
            }
        }
        else if (start == 11)
        {
            end = Random.Range(2, 5);
        }
        else
        {
            end = Random.Range(2, 9);
        }
        transform.position = SpawnController.instance.spawnPoints[start];
        endPoint = SpawnController.instance.spawnPoints[end];

        /*
        moveDirection = endPoint - transform.position;
        moveDirection.z = 0;
        moveDirection.Normalize();
         * */

        if (SpawnController.instance.phase == 0)
        {
            speed = 4f;
        }
        else if (SpawnController.instance.phase == 1)
        {
            speed = 2f;
        }
        else if (SpawnController.instance.phase == 2)
        {
            speed = 4f;
        }
        else if (SpawnController.instance.phase == 3)
        {
            speed = 6f;
        }
        else if (SpawnController.instance.phase == 4)
        {
            speed = 4f;
        }
        else if (SpawnController.instance.phase == 5)
        {
            if (enemyType == 0)
                speed = 2f;
            else
                speed = 6f;
        }
        else if (SpawnController.instance.phase == 6)
        {
            if (enemyType == 0)
                speed = 2f;
            else
                speed = 6f;
        }
        else if (SpawnController.instance.phase == 7)
        {
            speed = 4f;
        }
        else if (SpawnController.instance.phase == 8)
        {
            speed = 4f;
        }
        else if (SpawnController.instance.phase == 9)
        {
            speed = 6f;
        }
        else if (SpawnController.instance.phase == 10)
        {
            speed = 6f;
        }
        else if (SpawnController.instance.phase == 11)
        {
            speed = 2f;
        }
        else if (SpawnController.instance.phase == 12)
        {
            speed = 2f;
        }
        else if (SpawnController.instance.phase == 13)
        {
            speed = 2f;
        }
        else if (SpawnController.instance.phase == 14)
        {
            if (enemyType == 1)
                speed = 2f;
            else
                speed = 6f;
        }
        else if (SpawnController.instance.phase == 15)
        {
            if (enemyType == 1)
                speed = 2f;
            else
                speed = 6f;
        }
        else if (SpawnController.instance.phase == 16)
        {
            if (enemyType == 2)
                speed = 2f;
            else
                speed = 6f;
        }
        else if (SpawnController.instance.phase == 17)
        {
            if (enemyType == 2)
                speed = 2f;
            else
                speed = 6f;
        }
        else
        {
            speed = 4f;
        }
        //speed = Random.Range(3f, 5f);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 currentPosition = transform.position;

        if (!SpawnController.instance.paused)
        {
            transform.position = Vector3.MoveTowards(currentPosition, endPoint, Time.deltaTime * speed);
            if (transform.position == endPoint)
            {
                SpawnController.instance.points += 1;
                Destroy(gameObject);
            }

            /*
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation =
              Quaternion.Slerp(transform.rotation,
                                Quaternion.Euler(0, 0, targetAngle),
                                10f * Time.deltaTime);
             */
        }
	}
    /*

    public void setStartingPoint(Vector3 start)
    {
        transform.position = start;
    }

    public void setEndPoint(Vector3 end)
    {
        endPoint = end;

        moveDirection = end - transform.position;
        moveDirection.z = 0;
        moveDirection.Normalize();
    }

    public void setSpeed(float sp)
    {
        speed = sp;
    }
     */

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("TRIGGER2");
        if (collider.gameObject.CompareTag("Player") == true)
        {
            // Dead
            SpawnController.instance.EndGame();
        }
    }

    private void UpdateSortingLayer(int startPoint)
    {
        // Retrieve all child sprite components
        var spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

        foreach(var sprite in spriteRenderers)
        {
            sprite.sortingOrder += (startPoint * 3);
        }
    }
}
