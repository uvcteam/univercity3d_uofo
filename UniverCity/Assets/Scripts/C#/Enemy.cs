using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject myCreature;

    public enum EnemyType { Normal = 0, Jumping, Burrowing }
    public enum EnemyState { Normal = 0, Running, Captured }

    public EnemyState currentState = EnemyState.Normal;
    public EnemyType myType = EnemyType.Normal;

    public int currentWaypoint = 0;
    public float timeUntilCaptured = 3.0f;
    public float timeHeld = 0.0f;

    public Material[] healthMaterials = new Material[3];

    public bool hasWaypoints = true;
    public Transform[] waypoints;
    public float speed = 5.0f;
    public float lookSpeed = 10.0f;
    public float runSpeed = 15.0f;
    public float alertRange = 5.0f;
	public float jumpHeight = 15.0f;

    private Transform myTransform;

    public bool caughtInLaser = false;
    public bool atRisk = false;
    public Transform player;

    public GameLogic gameLogic;
	
	public bool isOnGround = true;
    private bool caughtAnim = false;
    private bool gotPoints = false;

    private bool beganBurrowing = false;
    private float timeStartedBurrowing = 0.0f;
    //private Quaternion startingRotation;

    void Start()
    {
        // Cache my transform.
        myTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();

        if (myType == EnemyType.Burrowing && !hasWaypoints)
            Debug.LogError("ERROR: Burrowing enemies require waypoints!");
    }

    // Update is called once per frame
    void Update()
    {
        //if (timeHeld >= timeUntilCaptured * 0.66f)
        //    renderer.material = healthMaterials[2];
        //else if (timeHeld >= timeUntilCaptured * 0.33f)
        //    renderer.material = healthMaterials[1];
        //else
        //    renderer.material = healthMaterials[0];
        Vector3 normalizedDirection = Vector3.zero;

        if (caughtInLaser)
            currentState = EnemyState.Captured;
        else if (atRisk)
            currentState = EnemyState.Running;
        else
            currentState = EnemyState.Normal;

        switch (currentState)
        {
            case EnemyState.Normal:
                caughtAnim = false;
                beganBurrowing = false;
                myCreature.animation.Play("walk");
                timeHeld = 0.0f;
                rigidbody.useGravity = true;
                normalizedDirection = waypoints[currentWaypoint].position - myTransform.position;
                normalizedDirection.Normalize();

                Quaternion newRotation = Quaternion.LookRotation(waypoints[currentWaypoint].position - myTransform.position, Vector3.up);
                newRotation.x = 0.0f;
                newRotation.z = 0.0f;
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, newRotation, Time.deltaTime * lookSpeed);

                myTransform.Translate(normalizedDirection * speed * Time.deltaTime, Space.World);
                break;

            case EnemyState.Captured:
                rigidbody.useGravity = false;
                timeHeld += Time.deltaTime;
                if (!caughtAnim)
                {
                    caughtAnim = true;
                    myCreature.animation["catch"].wrapMode = WrapMode.Once;
                    myCreature.animation.Play("catch");
                }
                if (timeHeld >= timeUntilCaptured)
                {
                    if (!gotPoints)
                    {
                        gotPoints = true;
                        gameLogic.EnemyCaptured();
                    }

                    myCreature.animation.Play("capture");
                    Destroy(gameObject, myCreature.animation["capture"].clip.length);
                }
                break;

            case EnemyState.Running:
                caughtAnim = false;
                ProcessBehavior();
                break;

            default:
                break;
        }
    }

    void LateUpdate()
    {
        //RaycastHit hit;
        if (!caughtInLaser)
        {
            if (Vector3.Distance(myTransform.position, player.position) < alertRange && !player.gameObject.GetComponent<FlyCam>().isHidden)
            {
                //if (Physics.Raycast(myTransform.position, (player.position - myTransform.position).normalized, out hit, alertRange))
                //{
                //    if (hit.transform == player)
                        atRisk = true;
                //    else
                //        atRisk = false;
                //}
            }
            else if (!beganBurrowing)
                atRisk = false;
        }
    }

    void ProcessBehavior()
    {
        Vector3 normalizedDirection = Vector3.zero;
        switch (myType)
        {
            case EnemyType.Normal:
                myCreature.animation.Play("sprint");
                timeHeld = 0.0f;
                rigidbody.useGravity = true;
                normalizedDirection = myTransform.position - player.position;
                normalizedDirection.Normalize();

                Quaternion newRotation = Quaternion.LookRotation(myTransform.position - waypoints[currentWaypoint].position, Vector3.up);
                newRotation.x = 0.0f;
                newRotation.z = 0.0f;
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, newRotation, Time.deltaTime * lookSpeed);

                myTransform.Translate(normalizedDirection * runSpeed * Time.deltaTime, Space.World);
                break;
			case EnemyType.Jumping:
                timeHeld = 0.0f;
                rigidbody.useGravity = true;
                normalizedDirection = myTransform.position - player.position;
                normalizedDirection.Normalize();

                myTransform.Translate(normalizedDirection * runSpeed * Time.deltaTime);
				if ( isOnGround )
					rigidbody.AddForce(Vector3.up * jumpHeight);
				break;
            case EnemyType.Burrowing:
                timeHeld = 0.0f;
                rigidbody.useGravity = true;

                if (!beganBurrowing)
                {
                    beganBurrowing = true;
                    myCreature.animation.Play("burrow");
                    collider.enabled = false;
                    timeStartedBurrowing = Time.time;
                }

                if (Time.time - timeStartedBurrowing >= myCreature.animation["burrow"].clip.length)
                {
                    transform.position = waypoints[Random.Range(0, waypoints.Length - 1)].position;
                    transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
                    collider.enabled = true;
                    beganBurrowing = false;
                }
                break;
            default:
                break;
        }
    }

    // Whenever I enter a waypoint...
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Waypoint" && collider.name == "Waypoint" + (currentWaypoint + 1).ToString())
            ++currentWaypoint;

        if (currentWaypoint >= waypoints.Length)
            currentWaypoint = 0;
    }
	
	void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.name == "Explorer_Environment")
			isOnGround = true;
	}
	
	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.name == "Explorer_Environment" )
			isOnGround = false;
	}
}