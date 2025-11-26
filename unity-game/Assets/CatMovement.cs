using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 vector3 = new Vector3(horizontal * moveSpeed * Time.deltaTime,
                            vertical * moveSpeed * Time.deltaTime,
                            0);

        transform.Translate(vector3);
        
        animator.SetBool("isWalking", horizontal > 0);
    }
}
