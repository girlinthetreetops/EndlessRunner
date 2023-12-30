using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionBehaviour : MonoBehaviour
{
    //move -x every frame
    //once it reaches -50, spawn a new random one at 100 and destroy itself

    private float speed = 15f;
    private LevelManager levelManager;
    private bool canMove = true;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelManager.OnCollision.AddListener(StopMoving);
    }

    private void Update()
    {
        Move();

        if (transform.position.z < -50)
        {
            Instantiate(levelManager.GetRandomPrefabFromLevel(), new Vector3(0, 0, 100), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (canMove)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
    }

    public void StopMoving()
    {
        canMove = false;
    }
}
