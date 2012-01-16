using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assimp;
using NUnit.Framework;

namespace Assimp.Test {
    /// <summary>
    /// Helper for Assimp.NET testing.
    /// </summary>
    public static class TestHelper {
        public const float DEFAULT_TOLERANCE = 0.00000001f;
        public static float Tolerance = DEFAULT_TOLERANCE;

        public static void AssertEquals(float expected, float actual) {
            Assert.IsTrue(Math.Abs(expected - actual) <= Tolerance);
        }

        public static void AssertEquals(float expected, float actual, String msg) {
            Assert.IsTrue(Math.Abs(expected - actual) <= Tolerance, msg);
        }

        public static void AssertEquals(float x, float y, Vector2D v) {
            AssertEquals(x, v.X);
            AssertEquals(y, v.Y);
        }

        public static void AssertEquals(float x, float y, Vector2D v, String msg) {
            AssertEquals(x, v.X, msg + " : checking X component");
            AssertEquals(y, v.Y, msg + " : checking Y component");
        }

        public static void AssertEquals(float x, float y, float z, Vector3D v) {
            AssertEquals(x, v.X);
            AssertEquals(y, v.Y);
            AssertEquals(z, v.Z);
        }

        public static void AssertEquals(float x, float y, float z, Vector3D v, String msg) {
            AssertEquals(x, v.X, msg + " : checking X component");
            AssertEquals(y, v.Y, msg + " : checking Y component");
            AssertEquals(z, v.Z, msg + " : checking Z component");
        }

        public static void AssertEquals(float r, float g, float b, float a, Color4D c) {
            AssertEquals(r, c.R);
            AssertEquals(g, c.G);
            AssertEquals(b, c.B);
            AssertEquals(a, c.A);
        }

        public static void AssertEquals(float r, float g, float b, float a, Color4D c, String msg) {
            AssertEquals(r, c.R, msg + " : checking R component");
            AssertEquals(g, c.G, msg + " : checking G component");
            AssertEquals(b, c.B, msg + " : checking B component");
            AssertEquals(a, c.A, msg + " : checking A component");
        }

        public static void AssertEquals(float r, float g, float b, Color3D c) {
            AssertEquals(r, c.R);
            AssertEquals(g, c.G);
            AssertEquals(b, c.B);
        }

        public static void AssertEquals(float r, float g, float b, Color3D c, String msg) {
            AssertEquals(r, c.R, msg + " : checking R component");
            AssertEquals(g, c.G, msg + " : checking G component");
            AssertEquals(b, c.B, msg + " : checking B component");
        }
    }
}
