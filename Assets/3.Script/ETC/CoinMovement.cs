using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] bool isUp = false;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        pos = transform.position + new Vector3(0, 2.5f, 0);
        Debug.Log(pos);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUp)
        {
            //transform.position = Vector3.Slerp(transform.position, Vector3.up * 3f, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);

        }

        if(Vector3.Distance(transform.position, pos) <= 1f)
        {
            isUp = true;
        }

        if (isUp)
        {
            //transform.position = Vector3.Slerp(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}
