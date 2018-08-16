using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMovement : MonoBehaviour
{

	private UnityEngine.AI.NavMeshAgent agent;

	public Text statusText;
	public SpriteRenderer emotionalIndicator;

	public GameObject[] positions;
	private GameObject currentObj = null;
	private int currentObjIndx = 0;

	private int SEED = 0xBADBEEF;

	// Use this for initialization
	void Start()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		Random.InitState(SEED);
		currentState = State.STATE_WAITING;
	}

	enum State
	{
		STATE_WAITING,
		STATE_MOVING_TO_OBJECT,
		STATE_AT_OBJECT
	}

	private State currentState;

	private State GetState()
	{
		switch (currentState)
		{
			// Waiting, let's send them to an object maybe?
			case State.STATE_WAITING:
				// If there even is anywhere to go
				if (positions.Length > 0 && currentObjIndx < positions.Length)
				{
					if (positions[currentObjIndx] != null)
					{
						currentObj = positions[currentObjIndx];
						SetNPCTargetLocation(currentObj.transform.position);
						currentObjIndx++;
						Debug.Log("Set current object index!");
						return State.STATE_MOVING_TO_OBJECT;
					}
				}
				break;
				// They're moving, did they make it there?
			case State.STATE_MOVING_TO_OBJECT:
				if (currentObj)
				{
					float dist = Vector3.Distance(transform.position, currentObj.transform.position);
					if (dist < 0.5)
					{
						return State.STATE_AT_OBJECT;
					}
					else
					{
						return State.STATE_MOVING_TO_OBJECT;
					}
				}
				break;
				// They're at the object, should they move?
			case State.STATE_AT_OBJECT:

				if (currentObj)
				{
					return State.STATE_AT_OBJECT;
				}
				break;
			default:
				return State.STATE_WAITING;
				break;
		}
		return State.STATE_WAITING;
	}

	// Update is called once per frame
	void Update()
	{
		currentState = GetState();
		switch (currentState)
		{
			case State.STATE_WAITING:
				statusText.text = "Waiting to move...";
				emotionalIndicator.color = Color.red;
				// Do nothing, logic for changing this state is in GetState
				break;
			case State.STATE_MOVING_TO_OBJECT:
				// 
				statusText.text = "Moving to an object!";
				emotionalIndicator.color = Color.green;
				break;
			case State.STATE_AT_OBJECT:
				statusText.text = "Currently at an object!";
				emotionalIndicator.color = Color.white;
				break;
			default:
				break;
		}
	}

	public void SetNPCTargetLocation(Vector3 pos)
	{
		agent.destination = pos;
		currentState = State.STATE_MOVING_TO_OBJECT;
	}
}