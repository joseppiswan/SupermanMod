using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using SLZ.AI;
using Superman;
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

        public override void OnApplicationStart()
        {
            HarmonyInstance.Patch(typeof(SLZ.Rig.RigManager).GetMethod("Awake", AccessTools.all), new HarmonyMethod(typeof(Superman).GetMethod("StartRig", BindingFlags.NonPublic | BindingFlags.Static)));
        }

        private static void StartRig()
        {
            Player.FindObjectReferences();
        }
        private static void AItest(AIBrain blah)
        {
            blah.behaviour.health.maxHitPoints *= 1000f;
        }

        public override void OnFixedUpdate()
        {
            if (!Player.handsExist)
            {
                MelonLogger.Msg("Couldn't find...");
                return;
            }
            if (Player.rightController._gripForce == 1f && Player.leftController._gripForce == 1f)
            {
                MelonLogger.Msg("Gripping...");
                Vector3 midPointHands = (Player.rightHand.transform.position + Player.leftHand.transform.position) / 2;
                MelonLogger.Msg("Got past midPointHands...");
                Vector3 midHands2Head = (midPointHands + Player.playerHead.transform.position) / 2;
                MelonLogger.Msg("Got past midHands2Head...");
                Vector3 midHandRot = (Player.rightHand.transform.forward + Player.leftHand.transform.forward) / 2;
                MelonLogger.Msg("Got past midHandRot...");
                float flyMagnitude = Mathf.Pow(Vector3.Distance(midHands2Head, Player.playerHead.position), 2f) / 3;
                MelonLogger.Msg(flyMagnitude);
                Player.physicsRig.torso.rbPelvis.AddForceAtPosition(midHandRot * flyMagnitude * 20 * Player.GetCurrentAvatar().massTotal, Player.playerHead.position, ForceMode.Impulse);
                MelonLogger.Msg(Player.GetCurrentAvatar().name + " || " + Player.playerHead.name);
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
