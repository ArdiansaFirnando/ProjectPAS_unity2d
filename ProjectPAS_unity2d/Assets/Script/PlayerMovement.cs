using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Voice Recognizer :")]
    [SerializeField] private string[] m_Keywords;
    private KeywordRecognizer m_Recognizer;

    [Header("Config :")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer player;
    [SerializeField] Animator anim;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool isGrounded;

    private float horizontal;
    private bool jump = false;
    private bool running; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);
        switch (args.text)
        {
            case ("right"):
                horizontal = 1f;
                running = true;
                break;

            case ("left"):
                horizontal = -1f;
                running = true;
                break;

            case ("jump"):
                jump = true;
                break;

            case ("stop"):
                horizontal = 0f;
                running = false;
                break;
        }
    }

    void Update()
    {
        //horizontal = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(horizontal, 0f);
        rb.velocity = movement * speed;

        anim.SetBool("running", running);

        if (horizontal == -1)
            player.flipX = true;

        if (horizontal == 1)
            player.flipX = false;

        if (jump == true && isGrounded == true)
            rb.AddForce(Vector2.up * jumpForce);

        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = true;
            Debug.Log("grounded!");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
            isGrounded = false;
    }
}