﻿using SLZ.Interaction;
using SLZ.Props.Weapons;
using SLZ.Rig;
using SLZ.VRMK;
using UnityEngine;
using MelonLoader;

namespace Superman
{
    public static class Player
    {
        private static readonly string playerHeadPath = "[PhysicsRig]/Head"; // The path to the head relative to the rig manager

        public static RigManager rigManager { get; set; }
        public static PhysicsRig physicsRig { get; set; }
        public static ControllerRig controllerRig { get; set; }
        public static UIRig uiRig { get; set; }

        public static Hand leftHand { get; set; }
        public static Hand rightHand { get; set; }
        public static bool handsExist => leftHand != null && rightHand != null;

        public static BaseController leftController { get; set; }
        public static BaseController rightController { get; set; }
        public static bool controllersExist => leftController != null && rightController != null;

        public static Transform playerHead { get; set; }
        public static float SpeedHolder { get; set; }

        public static void SetAvatarSpeed(SLZ.VRMK.Avatar _Avatar, float Speed)
        {
            _Avatar._speed = Speed;
            SpeedHolder = Speed;
            rigManager.physicsRig.SetAvatar(_Avatar);
            //rigManager.avatar = _Avatar; -- Fuck you Bonelab.
        }
        public static float GetAvatarSpeed()
        {
            return SpeedHolder;
        }

        internal static bool FindObjectReferences(RigManager manager = null)
        {
            MelonLogger.Msg("Finding player object references");

            if (controllersExist && handsExist && controllerRig != null)
                return false;
            if (manager == null)
                manager = GameObject.FindObjectOfType<RigManager>();

            rigManager = manager;
            physicsRig = manager?.physicsRig;
            controllerRig = manager?.ControllerRig;
            uiRig = manager?.uiRig;

            leftController = manager?.ControllerRig?.leftController;
            rightController = manager?.ControllerRig?.rightController;

            leftHand = manager?.physicsRig?.leftHand;
            rightHand = manager?.physicsRig?.rightHand;

            playerHead = rigManager.transform.Find(playerHeadPath);

            MelonLogger.Msg("Found player object references");

            return controllersExist && handsExist && controllerRig != null;
        }

        /// <summary>
        /// Returns the <see cref="PhysicsRig"/>.
        /// </summary>
        public static PhysicsRig GetPhysicsRig()
        {
            if (physicsRig == null)
                physicsRig = GameObject.FindObjectOfType<PhysicsRig>();
            return physicsRig;
        }

        /// <summary>
        /// Returns the Player's current <see cref="Avatar"/>.
        /// </summary>
        public static SLZ.VRMK.Avatar GetCurrentAvatar() => rigManager.avatar;

        /// <summary>
        /// Generic method for getting any component on the object the Player is holding.
        /// </summary>
        /// <returns>null if there is no component of type <typeparamref name="T"/>, or <paramref name="hand"/> is null.</returns>
        public static T GetComponentInHand<T>(Hand hand) where T : Component
        {
            T value = null;
            if (hand != null)
            {
                GameObject heldObject = hand.m_CurrentAttachedGO;
                if (heldObject != null)
                {
                    value = heldObject.GetComponentInParent<T>();
                    if (!value)
                        value = heldObject.GetComponentInChildren<T>();
                }
            }
            return value;
        }

        // Figured I'd include this since getting a gun is probably the most common use of GetComponentInHand
        public static Gun GetGunInHand(Hand hand) => GetComponentInHand<Gun>(hand);

        /// <summary>
        /// Returns the object <paramref name="hand"/> is holding or null if <paramref name="hand"/> is null.
        /// </summary>
        public static GameObject GetObjectInHand(Hand hand) => hand?.m_CurrentAttachedGO;

        /// <summary>
        /// Positive values: Clockwise rotation 
        /// <para/>
        /// Negative values: Counterclockwise rotation
        /// </summary>
        public static void RotatePlayer(float degrees)
        {
            if (controllerRig != null)
                controllerRig.SetTwist(degrees);
        }
    }
}