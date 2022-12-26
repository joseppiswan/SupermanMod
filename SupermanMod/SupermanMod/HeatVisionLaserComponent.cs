using MelonLoader;
using System;
using System.Reflection;
using SuperMan;
using UnityEngine;

namespace SuperMan
{
    [RegisterTypeInIl2Cpp]
    class HeatVisionLaserComponent : MonoBehaviour
    {
        public HeatVisionLaserComponent(IntPtr ptr) : base(ptr) { }

        public GameObject laserObject;
        public LineRenderer heatLaser;
        public GameObject hitObject;
        public Vector3 startPos;
        public Vector3 endPos;

        public bool enabled;
        void Start()
        {
            SetupLaser();
            heatLaser.positionCount = 0;
        }
        void Update()
        {
            RaycastHit h;
            if (!Physics.Raycast(startPos, transform.forward, out h, 10000f, 8))
                return;
            startPos = transform.position - new Vector3(0, 0.1f, 0);
            endPos = h.point;
            MelonLogger.Msg("got past raycast");
        }
        void LateUpdate()
        {
            if (!laserObject.GetComponent<LineRenderer>()) 
                return;
            laserObject.GetComponent<LineRenderer>().SetPositions(new Vector3[] {
                startPos,
                endPos
            });
        }
        private LineRenderer GetLaserRenderer()
        {
            return heatLaser;
        }
        public void SetupLaser()
        {
            Assembly executing = Assembly.GetExecutingAssembly();
            EmebeddedAssetBundleLakatrazz.LoadAssetBundle(executing,this);
        }
        public void EnableLaser()
        {
            heatLaser.positionCount = 2;
        }
        public void EndLaser()
        {
            heatLaser.positionCount = 0;
        }
        public void DestroyLaser()
        {
            Destroy(heatLaser);
            Destroy(this);
        }
    }
}
