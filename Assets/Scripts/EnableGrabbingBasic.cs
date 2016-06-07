/// <summary>
/// File: EnableGrabbingBasic.cs
/// Description: This is a basic grabbing script for Vive controllers. Note that this allows
/// grabbing only for objects tagged "Grabbable" and that this adds a SphereCollider component
/// to the controller if it does not find one.
/// How to use:
///     1) Attach to controller.
///     2) Use trigger to grab objects tagged "Grabbable".
/// </summary>

using UnityEngine;
using System.Collections;

namespace vive_tests {
	[RequireComponent(typeof(SteamVR_TrackedObject))]
	public class EnableGrabbingBasic : MonoBehaviour {

		private GameObject objectController;
		private GameObject objectGrabbed;
		private float radiusController = 0.05f;
		private bool hasGrabbed = false;

		// Boilerplate for Vive controller
		SteamVR_TrackedObject trackedController;
		SteamVR_Controller.Device deviceController;

		// More boilerplate for controller
		// It is unknown why this has to run before everything else
		void Awake()
		{
			trackedController = GetComponent<SteamVR_TrackedObject>();

		}

		void Start() {
			// More boilerplate for the controller
			objectController = gameObject;
			deviceController = SteamVR_Controller.Input((int)trackedController.index);

			if (objectController.GetComponent<Collider>() == null) {
				objectController.AddComponent<SphereCollider> ();
				objectController.GetComponent<SphereCollider> ().radius = radiusController;
			}
		}

		void Update() {


			if (hasGrabbed) {

				Rigidbody rigidbodyObjectGrabbed = objectGrabbed.GetComponent<Rigidbody> ();
				// Zeroes out speed of object if it has one
				if (rigidbodyObjectGrabbed != null) {
					rigidbodyObjectGrabbed.velocity = Vector3.zero;
					rigidbodyObjectGrabbed.angularVelocity = Vector3.zero;
				}

				// Transforms the grabbed object
				objectGrabbed.transform.position = objectController.transform.position;

				// Rotates the grabbed object with the controller
				objectGrabbed.transform.localRotation = objectController.transform.rotation;
			}

		}

		void OnTriggerStay (Collider colliderChecked) {

			// Grab object if it is tagged properly AND the trigger is pressed
			// Lets go otherwise
			if (colliderChecked.tag == "Grabbable" ) {
				if (deviceController.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
					hasGrabbed = true;
					objectGrabbed = colliderChecked.gameObject;
				}

				if (deviceController.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
					hasGrabbed = false;
				}
			}

		}


	}

}