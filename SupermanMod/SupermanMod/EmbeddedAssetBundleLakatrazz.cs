using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;

using MelonLoader;
using System;
using UnhollowerRuntimeLib;

namespace SuperMan
{
    public static class EmebeddedAssetBundleLakatrazz
    {

        public static AssetBundle LoadFromAssembly(Assembly assembly, string name)
        {
            string[] manifestResources = assembly.GetManifestResourceNames();

            if (manifestResources.Contains(name))
            {
                MelonLogger.Msg($"Loading embedded bundle data {name}...", ConsoleColor.DarkCyan);

                byte[] bytes;
                using (Stream str = assembly.GetManifestResourceStream(name))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    str.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                MelonLogger.Msg($"Loading bundle from data {name}, please be patient...", ConsoleColor.DarkCyan);
                var temp = AssetBundle.LoadFromMemory(bytes);
                MelonLogger.Msg($"Done!", ConsoleColor.DarkCyan);
                return temp;
            }

            return null;
        }
        internal static void LoadAssetBundle(Assembly executingAssembly,HeatVisionLaserComponent yes)
        {
            AssetBundle bundle = LoadFromAssembly(executingAssembly, "SuperMan.Resources.supermanmod.superman_assets");
            if (bundle == null)
            {
                throw new NullReferenceException("ASSET BUNDLE NULL; YOU SUCK AT PROGRAMMING JOE");
            }
            LineRenderer render = UnityEngine.Object.Instantiate(bundle.LoadAsset("HEAT_LASER", Il2CppType.Of<GameObject>()).Cast<GameObject>()).GetComponent<LineRenderer>();
            yes.heatLaser = render;
            yes.laserObject = render.gameObject;
            render.hideFlags = HideFlags.DontUnloadUnusedAsset;
            MelonLogger.Msg(render.name);
            bundle.Unload(false);
        }
    }
}
