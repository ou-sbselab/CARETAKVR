using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementTest : MonoBehaviour
{

	private Text controllerText;

	public NPCMovement script;

	private SteamVR_TrackedObject trackedObj;
	private int leftControllerIndx;
	// 1
	public GameObject laserPrefab;
	// 2
	private GameObject laser;
	// 3
	private Transform laserTransform;
	// 4
	private Vector3 hitPoint;
	// 8
	private bool shouldSend;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input(leftControllerIndx); }
	}

	void Awake()
	{
		leftControllerIndx = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void ShowLaser(RaycastHit hit)
	{
		// 1
		laser.SetActive(true);
		// 2
		laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
		// 3
		laserTransform.LookAt(hitPoint);
		// 4
		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
			hit.distance);
	}

	// Use this for initialization
	void Start()
	{
		leftControllerIndx = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);

		// 1
		laser = Instantiate(laserPrefab);
		// 2
		laserTransform = laser.transform;

		controllerText = GetComponentInChildren<Text>();
		controllerText.text = "Working!";
	}

	// Update is called once per frame
	void Update()
	{
		leftControllerIndx = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);

		// 1
		if (Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
		{
			controllerText.text = "Should be drawing laser...";
			RaycastHit hit;

			// 2
			if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100))
			{
				hitPoint = hit.point;
				ShowLaser(hit);
				// 3
				shouldSend = true;
			}
		}
		else // 3
		{
			laser.SetActive(false);
		}

		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && shouldSend)
		{
			script.SetNPCTargetLocation(hitPoint);
			controllerText.text = "Moving to location!";
		}
	}
}