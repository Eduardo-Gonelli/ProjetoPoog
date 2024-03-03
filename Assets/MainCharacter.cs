using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    [HideInInspector] public bool isGrounded;
    [SerializeField] private Transform shotSpawn;





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
