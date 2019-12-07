using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScript : MonoBehaviour
{
    public float forceFactor;
    public Vector3 initPosition;

    private Vector3 startpos;
    private Vector3 endPos;
    private Vector3 offset;
    private float zCoord;

    private Vector3 forceAtBall;
    private Rigidbody2D rigidBody;

    public GameObject Trajectory;
    private GameObject[] trajectoryDots;
    public int number;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        trajectoryDots = new GameObject[number];
    }

    private void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - getMouseWorldPos();
        startpos = gameObject.transform.position;


        //Initialize trajectory dots to be on start pos of the paper ball
        for (int i = 0; i < number; i++)
        {
            trajectoryDots[i] = Instantiate(Trajectory, gameObject.transform);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = Vector2.zero;
            gameObject.transform.position = initPosition;

        }
    }

    private void OnMouseUp()
    {
        rigidBody.gravityScale = 1;
        //Destroy the path
        for (int i = 0; i < number; i++)
        {
            Destroy(trajectoryDots[i]);
            //            trajectoryDots[i].transform.position = calculatePosition(i * 0.1f);
        }


    }


    private Vector3 getMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    //Called when the sprite is dragged
    private void OnMouseDrag()
    {
        transform.position = getMouseWorldPos() + offset;
        endPos = getMouseWorldPos() + offset;

        //Set the distance of start and end position, needed for calculating trajactory
        forceAtBall = endPos - startpos;
        rigidBody.velocity = new Vector2(-forceAtBall.x * forceFactor,
            -forceAtBall.y * forceFactor);
        for (int i = 0; i < number; i++)
        {
            trajectoryDots[i].transform.position = calculatePosition(i * 0.1f);
        }


    }

    private Vector2 calculatePosition(float elapsedTime)
    {
        return new Vector2(endPos.x, endPos.y) +//X0
            new Vector2(-forceAtBall.x * forceFactor, -forceAtBall.y * forceFactor) * elapsedTime + //ut
        0.5f * Physics2D.gravity * elapsedTime * elapsedTime;

    }
}
