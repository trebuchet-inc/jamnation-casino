using UnityEngine;
using System.Collections.Generic;

namespace NewtonVR
{
    public enum Handedness
    {
        Left,
        Right
    }
    
    public class NVRVirtualHand : NVRHand
    {
		public Handedness Hand;
		
		public float radius = 0.5f;

		void Start ()
		{
			PreInitialize(null);
		}

        public override void PreInitialize(NVRPlayer player)
        {
			IsLeft = Hand == Handedness.Left;
			IsRight = Hand == Handedness.Right;

			Player = NVRPlayer.Instance;

            CurrentInteractionStyle = InteractionStyle.Hold;

            CurrentlyHoveringOver = new Dictionary<NVRInteractable, Dictionary<Collider, float>>();

            LastPositions = new Vector3[EstimationSamples];
            LastRotations = new Quaternion[EstimationSamples];
            LastDeltas = new float[EstimationSamples];
            EstimationSampleIndex = 0;

            VisibilityLocked = false;
            
            Inputs = new Dictionary<NVRButtons, NVRButtonInputs>(new NVRButtonsComparer());
            for (int buttonIndex = 0; buttonIndex < NVRButtonsHelper.Array.Length; buttonIndex++)
            {
                if (Inputs.ContainsKey(NVRButtonsHelper.Array[buttonIndex]) == false) 
                {
                    Inputs.Add(NVRButtonsHelper.Array[buttonIndex], new NVRButtonInputs());
                }
            }

			var virtualInputDevice = this.gameObject.AddComponent<NVRVirtualInputDevice> ();
			virtualInputDevice.radius = radius;
			InputDevice = virtualInputDevice;
			InputDevice.Initialize (this);

			InitializeRenderModel();
        }

        protected override void Update()
        {
            if (CurrentHandState == HandState.Uninitialized)
            {
                if (InputDevice == null || InputDevice.ReadyToInitialize() == false)
                {
                    return;
                }
                else
                {
                    Initialize();
                    return;
                }
            }

            UpdateHovering();

            UpdateVisibilityAndColliders();
        }

        public void Hold()
        {
            print(transform.parent.name + " Hold");
            PickupClosest();

            if (IsInteracting)
            {
                CurrentHandState = HandState.GripToggleOnInteracting;
            }
        }

        public void Hold(string withID)
        {
            PickupByName(withID);

            if (IsInteracting)
            {
                CurrentHandState = HandState.GripToggleOnInteracting;
            }
        }

        public void Release()
        {
            if (CurrentlyInteracting != null)
            {
                EndInteraction (null);
            }
        }

        public void Use()
        {
            if (CurrentlyInteracting != null)
            {
                CurrentlyInteracting.UseButtonDown ();
            }
        }

        public void EndUse()
        {
            if (CurrentlyInteracting != null)
            {
                CurrentlyInteracting.UseButtonUp();
            }
        }

        public void ForceInteraction(string objectName)
        {
            print("Force interaction with " + objectName);
            GameObject o = GameObject.Find(objectName);

            BeginInteraction(o.GetComponent<NVRInteractableItem>());

            if (IsInteracting)
            {
                CurrentHandState = HandState.GripToggleOnInteracting;
            }
        }

        public override void Initialize()
        {
            Rigidbody = this.GetComponent<Rigidbody>();
            if (Rigidbody == null)
                Rigidbody = this.gameObject.AddComponent<Rigidbody>();
            Rigidbody.isKinematic = true;
            Rigidbody.maxAngularVelocity = float.MaxValue;
            Rigidbody.useGravity = false;

            Collider[] colliders = null;

            colliders = InputDevice.SetupDefaultColliders();

            if (PhysicalController != null)
            {
                PhysicalController.Kill();
            }

            PhysicalController = this.gameObject.AddComponent<NVRPhysicalController>();
            PhysicalController.Initialize(this, false);
                
            if (colliders != null)
            {
                GhostColliders = colliders;
            }

            CurrentVisibility = VisibilityLevel.Visible;

            CurrentHandState = HandState.Idle;
        }
    }
}
