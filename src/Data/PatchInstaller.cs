using System;
using System.IO;

namespace Engine.Data
{
    /// <summary>
    /// Content Update Patch Manager. Scans the `./game/patches` directory for update packs
    /// and mounts them into the VFS.
    /// </summary>
    public static class PatchInstaller
    {
        public static void MountPatches(string patchesDir, Core.VFS vfs)
        {
            if (!Directory.Exists(patchesDir)) return;

            string[] updateDirs = Directory.GetDirectories(patchesDir);
            Array.Sort(updateDirs); // Process in version order (e.g., patch_v1.0, patch_v1.1)

            foreach (var patchDir in updateDirs)
            {
                Console.WriteLine($"[VFS] Mounted update patch layer: {Path.GetFileName(patchDir)}");
                vfs.RegisterPath(patchDir);
            }
        }
    }
}
