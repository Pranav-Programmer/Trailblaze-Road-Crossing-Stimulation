using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SimpleSampleCharacterControl : Agent
{
    private enum ControlMode
    {
        /// <summary>
        /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
        /// </summary>
        Tank,
        /// <summary>
        /// Character freely moves in the chosen direction from the perspective of the camera
        /// </summary>
        Direct
    }

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;

    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private float moveSpeed = 3f;
    private float turnSpeed = 5f;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
    private bool m_jumpInput = false;

    private bool m_isGrounded;

    private float lastDistance;
    private float currentDistance;

    private List<Collider> m_collisions = new List<Collider>();

    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(-2f, 0.5f, Random.Range(45, 55));
        transform.localRotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);

        lastDistance = 22f - transform.localPosition.x;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition); // 3
        sensor.AddObservation(transform.localRotation); // 4
        sensor.AddObservation(currentDistance); // 1
    }

//     private void OnCollisionEnter(Collision collision)
//     {
//         ContactPoint[] contactPoints = collision.contacts;
//         for (int i = 0; i < contactPoints.Length; i++)
//         {
//             if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
//             {
//                 if (!m_collisions.Contains(collision.collider))
//                 {
//                     m_collisions.Add(collision.collider);
//                 }
//                 m_isGrounded = true;
//             }
//         }
//     }

//     private void OnCollisionStay(Collision collision)
//     {
//         ContactPoint[] contactPoints = collision.contacts;
//         bool validSurfaceNormal = false;
//         for (int i = 0; i < contactPoints.Length; i++)
//         {
//             if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
//             {
//                 validSurfaceNormal = true; break;
//             }
//         }

//         if (validSurfaceNormal)
//         {
//             m_isGrounded = true;
//             if (!m_collisions.Contains(collision.collider))
//             {
//                 m_collisions.Add(collision.collider);
//             }
//         }
//         else
//         {
//             if (m_collisions.Contains(collision.collider))
//             {
//                 m_collisions.Remove(collision.collider);
//             }
//             if (m_collisions.Count == 0) { m_isGrounded = false; }
//         }
//     }

//     private void OnCollisionExit(Collision collision)
//     {
//         if (m_collisions.Contains(collision.collider))
//         {
//             m_collisions.Remove(collision.collider);
//         }
//         if (m_collisions.Count == 0) { m_isGrounded = false; }
//     }

    // private void Update()
    // {
    //     if (!m_jumpInput && Input.GetKey(KeyCode.Space))
    //     {
    //         m_jumpInput = true;
    //     }
    // }

    // private void FixedUpdate()
    // {
    //     m_animator.SetBool("Grounded", m_isGrounded);

    //     switch (m_controlMode)
    //     {
    //         case ControlMode.Direct:
    //             DirectUpdate();
    //             break;

    //         case ControlMode.Tank:
    //             TankUpdate();
    //             break;

    //         default:
    //             Debug.LogError("Unsupported state");
    //             break;
    //     }

    //     m_wasGrounded = m_isGrounded;
    //     m_jumpInput = false;
    // }

    // private void TankUpdate()
    // {
    //     //print("A");
    //     float v = Input.GetAxis("Vertical");
    //     float h = Input.GetAxis("Horizontal");

    //     bool walk = Input.GetKey(KeyCode.LeftShift);

    //     if (v < 0)
    //     {
    //         if (walk) { v *= m_backwardsWalkScale; }
    //         else { v *= m_backwardRunScale; }
    //     }
    //     else if (walk)
    //     {
    //         v *= m_walkScale;
    //     }

    //     m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
    //     m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

    //     transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
    //     transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

    //     m_animator.SetFloat("MoveSpeed", m_currentV);

    //     JumpingAndLanding();
    // }

    // private void DirectUpdate()
    // {
    //     //print("B");
    //     float v = Input.GetAxis("Vertical");
    //     float h = Input.GetAxis("Horizontal");

    //     Transform camera = Camera.main.transform;

    //     if (Input.GetKey(KeyCode.LeftShift))
    //     {
    //         v *= m_walkScale;
    //         h *= m_walkScale;
    //     }

    //     m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
    //     m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

    //     Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

    //     float directionLength = direction.magnitude;
    //     direction.y = 0;
    //     direction = direction.normalized * directionLength;

    //     if (direction != Vector3.zero)
    //     {
    //         m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

    //         transform.rotation = Quaternion.LookRotation(m_currentDirection);
    //         transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

    //         m_animator.SetFloat("MoveSpeed", direction.magnitude);
    //     }

    //     JumpingAndLanding();
    // }

    // private void JumpingAndLanding()
    // {
    //     bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

    //     if (jumpCooldownOver && m_isGrounded && m_jumpInput)
    //     {
    //         m_jumpTimeStamp = Time.time;
    //         m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
    //     }

    //     if (!m_wasGrounded && m_isGrounded)
    //     {
    //         m_animator.SetTrigger("Land");
    //     }

    //     if (!m_isGrounded && m_wasGrounded)
    //     {
    //         m_animator.SetTrigger("Jump");
    //     }
    // }

    /// <summary>
    /// dActions[0] | (0) - Nothing, (1) - Running forward, (2) - Running back
    /// dActions[1] | (0) - Turn left, (1) - Turn right, (2) - Nothing
    /// </summary>
    /// <param name="actions"></param>
    public override void OnActionReceived(ActionBuffers actions)
    {   
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        var dActions = actions.DiscreteActions;
        // print(dActions[0]);
        // print(dActions[1]);

        // if (direction != Vector3.zero)
        // {
        //     m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

        //     transform.rotation = Quaternion.LookRotation(m_currentDirection);
        //     transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

        //     m_animator.SetFloat("MoveSpeed", direction.magnitude);
        // }

        if (dActions[0] == 0)
        {
            //animator.SetBool("isRunning", false);
        }
        else if (dActions[0] == 1)
        {
            //m_animator.SetFloat("MoveSpeed", direction.magnitude);
            transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);
        }
        else if (dActions[0] == 2)
        {
            //m_animator.SetFloat("MoveSpeed", direction.magnitude);
            transform.Translate(-Vector3.forward * (moveSpeed / 2f) * Time.fixedDeltaTime);
        }

        if (dActions[1] == 0) transform.Rotate(new Vector3(0f, -1f, 0f) * turnSpeed);
        else if(dActions[1] == 1) transform.Rotate(new Vector3(0f, 1f, 0f) * turnSpeed);

        currentDistance = 22f - transform.localPosition.x;

        if (currentDistance < lastDistance)
        {
            AddReward(0.05f);
            lastDistance = currentDistance;
        }
        else
        {
            AddReward(-0.005f);
        }

        if (transform.localPosition.x > 22f)
        {
            AddReward(5f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        // print(actions.Length);

        actions[0] = 0;
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = -1;
        }if (Input.GetKey(KeyCode.DownArrow))
        {
            actions[0] = 0;
        }

        actions[1] = 2;

        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car") || other.gameObject.CompareTag("EndRoad"))
        {
            AddReward(-5f);
            EndEpisode();
        }
    }

}
