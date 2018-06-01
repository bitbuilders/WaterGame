using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 50.0f)] float m_speed = 10.0f;
    [SerializeField] [Range(1.0f, 900.0f)] float m_turnSpeed = 90.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_jumpForce = 3.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_jumpResistance = 3.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_fallSpeed = 3.0f;
    [SerializeField] Water m_water = null;
    [SerializeField] Animator m_gunAnimator = null;
    [SerializeField] Transform m_leftFoot = null;
    [SerializeField] Transform m_rightFoot = null;

    Rigidbody m_rigidBody;
    AudioSource m_audioSource;
    float rotationX = 0.0f;
    float m_actualSpeed = 0.0f;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();
        m_actualSpeed = m_speed;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_actualSpeed = m_speed + 2.0f;
        }
        else
        {
            m_actualSpeed = m_speed;
        }

        Vector3 turn = Vector3.zero;
        turn.y = Input.GetAxis("Mouse X");
        rotationX += -Input.GetAxis("Mouse Y") * Time.deltaTime * m_turnSpeed;
        turn *= Time.deltaTime * m_turnSpeed;

        transform.rotation = Quaternion.Euler(Vector3.up * turn.y) * transform.rotation;
        rotationX = Mathf.Clamp(rotationX, -35.0f, 35.0f);
        transform.localEulerAngles = new Vector3(rotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);

        Vector3 velocity = Vector3.zero;
        velocity.x = Input.GetAxis("Horizontal");
        velocity.z = Input.GetAxis("Vertical");
        velocity *= Time.deltaTime * m_actualSpeed;

        transform.position += transform.rotation * velocity;

        if (Input.GetButtonDown("Jump"))
        {
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        m_gunAnimator.SetFloat("WalkSpeed", m_actualSpeed / 5.0f);
        if (velocity.magnitude > 0.0f)
        {
            m_gunAnimator.SetFloat("Walk", 1.0f);
        }
        else
        {
            m_gunAnimator.SetFloat("Walk", 0.0f);
        }
    }

    private void FixedUpdate()
    {
        if (m_rigidBody.velocity.y > 0.0f)
        {
            m_rigidBody.velocity += (Vector3.up * Physics.gravity.y) * (m_jumpResistance - 1.0f) * Time.deltaTime;
        }
        else if (m_rigidBody.velocity.y < 0.0f)
        {
            m_rigidBody.velocity += (Vector3.up * Physics.gravity.y) * (m_fallSpeed - 1.0f) * Time.deltaTime;
        }
    }

    public void Footstep(bool left)
    {
        Vector3 origin;
        if (left)
        {
            origin = m_leftFoot.position;
        }
        else
        {
            origin = m_rightFoot.position;
        }

        Ray ray = new Ray(origin + Vector3.up * 5.0f, Vector3.down);
        m_water.Touch(ray, 2.0f);

        m_audioSource.pitch = Random.Range(0.9f, 1.0f);
        m_audioSource.Play();
    }

    public void Splash()
    {

    }
}
