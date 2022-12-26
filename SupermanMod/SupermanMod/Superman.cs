using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using SLZ.AI;
using SuperMan;
using UnityEngine;

[assembly: MelonInfo(typeof(SupermanMod.Superman), "Superman", "1.0.0", "RSM && Joe")]

namespace SupermanMod
{
    public class Superman : MelonMod
    {

        private bool floating = false;
        private bool firstPress = false;
        private float previousPressedTime;
        private float doublePressDelay = 0.3f;

        internal HeatVisionLaserComponent laserLeft;
        internal HeatVisionLaserComponent laserRight;

        public static Transform leftEye;
        public static Transform rightEye;

        public override void OnApplicationStart()
        {
            HarmonyInstance.Patch(typeof(SLZ.Rig.RigManager).GetMethod("Awake", AccessTools.all), new HarmonyMethod(typeof(Superman).GetMethod("StartRig", BindingFlags.NonPublic | BindingFlags.Static)));
        }

        private static void StartRig()
        {
            Player.FindObjectReferences();
            leftEye = Player.rigManager.animationRig.transform.Find("Head/eyeLf");
            rightEye = Player.rigManager.animationRig.transform.Find("Head/eyeRt");
            MelonLogger.Msg(Player.rigManager.animationRig.transform.Find("Head/eyeRt").name);
            leftEye.gameObject.AddComponent<HeatVisionLaserComponent>();
            rightEye.gameObject.AddComponent<HeatVisionLaserComponent>();
        }

        public override void OnFixedUpdate()
        {
            if (!Player.handsExist)
            {
                return;
            }
            if (Player.rightController._gripForce == 1f && Player.leftController._gripForce == 1f)
            {
                Vector3 midPointHands = (Player.rightHand.transform.position + Player.leftHand.transform.position) / 2;
                Vector3 midHands2Head = (midPointHands + Player.playerHead.transform.position) / 2;
                Vector3 midHandRot = (Player.rightHand.transform.forward + Player.leftHand.transform.forward) / 2;
                float flyMagnitude = Mathf.Pow(Vector3.Distance(midHands2Head, Player.playerHead.position), 1.5f) / 2f;
                Player.physicsRig.torso.rbPelvis.AddForceAtPosition(midHandRot * flyMagnitude * 20 * Player.GetCurrentAvatar().massTotal, Player.playerHead.position, ForceMode.Impulse);
                //temp.transform.position = Player.GetCurrentAvatar().eyeCenterOverride.position;
            }

            //float
            if (Player.rightController.GetBButtonDown())
            {
                if (firstPress)
                {
                    if (Time.time - previousPressedTime <= doublePressDelay)
                    {
                        //tsuff here (in case i forget)
                        floating = !floating;
                        firstPress = false;
                    }
                }
                else
                {
                    firstPress = true;
                }

                previousPressedTime = Time.time;
            }
            if (firstPress && Time.time - previousPressedTime > doublePressDelay)
            {
                firstPress = false;
            }
            Player.physicsRig.torso.rbPelvis.isKinematic = floating;

            // Super Speed
            //if (Player.leftController.GetThumbStickDown())
            //{
            //    Player.SetAvatarSpeed(Player.GetCurrentAvatar(), 10f);
            //}
            //else if (Player.leftController.GetThumbStickUp())
            //{
            //    Player.SetAvatarSpeed(Player.GetCurrentAvatar(), Player.GetAvatarSpeed());
            //}
        }

    }
}
