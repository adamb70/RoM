using System;
using System.Linq;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using VRage.Components.Session;
using VRageMath;

namespace Equinox76561198048419394.Core.Util
{
    public static class MiscMath
    {
        public static double PlanetaryWavePhaseFactor(Vector3D worldPos, double periodMeters)
        {
            var planetCenter = MyGamePruningStructureSandbox.GetClosestPlanet(worldPos)?.GetPosition() ?? Vector3D.Zero;
            var planetDir = worldPos - planetCenter;
            var currRadius = planetDir.Normalize();

            var angle = Math.Acos(planetDir.X);
            var surfaceDistance = angle * currRadius;
            var elevationDistance = currRadius;
            return (surfaceDistance + elevationDistance) / periodMeters;
        }

        // r, theta (inc), phi (az)
        public static Vector3D ToSpherical(Vector3D world)
        {
            var r = world.Length();
            if (r <= 1e-3f)
                return new Vector3D(0, 0, r);
            var theta = Math.Acos(world.Z / r);
            var phi = Math.Atan2(world.Y, world.X);
            return new Vector3D(r, theta, phi);
        }

        public static Vector3D FromSpherical(Vector3D spherical)
        {
            var sinTheta = spherical.X * Math.Sin(spherical.Y);
            return new Vector3D(sinTheta * Math.Cos(spherical.Z), sinTheta * Math.Sin(spherical.Z),
                spherical.X * Math.Cos(spherical.Y));
        }
    }
}