using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assimp;
using NUnit.Framework;

namespace Assimp.Test {
    [TestFixture]
    public class Color3DTestFixture {

        [Test]
        public void TestIndexer() {
            float r = .25f, g = .5f, b = .05f;
            Color3D c = new Color3D();
            c[0] = r;
            c[1] = g;
            c[2] = b;
            TestHelper.AssertEquals(r, c[0], "Test Indexer, R");
            TestHelper.AssertEquals(g, c[1], "Test Indexer, G");
            TestHelper.AssertEquals(b, c[2], "Test Indexer, B");
        }
        [Test]
        public void TestEquals() {
            float r1 = .25f, g1 = .1f, b1 = .75f;
            float r2 = .75f, g2 = 1.0f, b2 = 1.0f;

            Color3D c1 = new Color3D(r1, g1, b1);
            Color3D c2 = new Color3D(r1, g1, b1);
            Color3D c3 = new Color3D(r2, g2, b2);

            //Test IEquatable Equals
            Assert.IsTrue(c1.Equals(c2), "Test IEquatable equals");
            Assert.IsFalse(c1.Equals(c3), "Test IEquatable equals");

            //Test object equals override
            Assert.IsTrue(c1.Equals((object) c2), "Tests object equals");
            Assert.IsFalse(c1.Equals((object) c3), "Tests object equals");

            //Test op equals
            Assert.IsTrue(c1 == c2, "Testing OpEquals");
            Assert.IsFalse(c1 == c3, "Testing OpEquals");

            //Test op not equals
            Assert.IsTrue(c1 != c3, "Testing OpNotEquals");
            Assert.IsFalse(c1 != c2, "Testing OpNotEquals");
        }

        [Test]
        public void TestIsBlack() {
            Color3D c1 = new Color3D(0, 0, 0);
            Color3D c2 = new Color3D(.25f, 1.0f, .5f) * .002f;
            Color3D c3 = new Color3D(.25f, .65f, 1.0f);

            Assert.IsTrue(c1.IsBlack(), "Testing isBlack");
            Assert.IsTrue(c2.IsBlack(), "Testing isBlack");
            Assert.IsFalse(c3.IsBlack(), "Testing !isBlack");
        }

        [Test]
        public void TestOpAdd() {
            float r1 = .5f, g1 = .25f, b1 = .7f;
            float r2 = .2f, g2 = .1f, b2 = .05f;
            float r = r1 + r2;
            float g = g1 + g2;
            float b = b1 + b2;

            Color3D c1 = new Color3D(r1, g1, b1);
            Color3D c2 = new Color3D(r2, g2, b2);
            Color3D c = c1 + c2;

            TestHelper.AssertEquals(r, g, b, c, "Testing OpAdd");
        }

        [Test]
        public void TestOpAddValue() {
            float r1 = .5f, g1 = .25f, b1 = .7f;
            float value = .2f;
            float r = r1 + value;
            float g = g1 + value;
            float b = b1 + value;

            Color3D c1 = new Color3D(r1, g1, b1);

            //Test left to right
            Color3D c = c1 + value;
            TestHelper.AssertEquals(r, g, b, c, "Testing OpAddValue");

            //Test right to left
            c = value + c1;
            TestHelper.AssertEquals(r, g, b, c, "Testing OpAddValue");
        }

        [Test]
        public void TestOpSubtract() {
            float r1 = .5f, g1 = .25f, b1 = .7f;
            float r2 = .2f, g2 = .1f, b2 = .05f;
            float r = r1 - r2;
            float g = g1 - g2;
            float b = b1 - b2;

            Color3D c1 = new Color3D(r1, g1, b1);
            Color3D c2 = new Color3D(r2, g2, b2);
            Color3D c = c1 - c2;

            TestHelper.AssertEquals(r, g, b, c, "Testing OpSubtract");
        }

        [Test]
        public void TestOpSubtractByValue() {
            float r1 = .5f, g1 = .25f, b1 = .7f;
            float value = .2f;
            float r = r1 - value;
            float g = g1 - value;
            float b = b1 - value;

            Color3D c1 = new Color3D(r1, g1, b1);

            //Test left to right
            Color3D c = c1 - value;
            TestHelper.AssertEquals(r, g, b, c, "Testing OpSubtractValue");

            r = value - r1;
            g = value - g1;
            b = value - b1;

            //Test right to left
            c = value - c1;
            TestHelper.AssertEquals(r, g, b, c, "Testing OpSubtractValue");
        }

        [Test]
        public void TestOpMultiply() {
            float r1 = .5f, g1 = .25f, b1 = .7f;
            float r2 = .2f, g2 = .1f, b2 = .05f;
            float r = r1 * r2;
            float g = g1 * g2;
            float b = b1 * b2;

            Color3D c1 = new Color3D(r1, g1, b1);
            Color3D c2 = new Color3D(r2, g2, b2);
            Color3D c = c1 * c2;

            TestHelper.AssertEquals(r, g, b, c, "Testing OpMultiply");
        }

        [Test]
        public void TestOpMultiplyByScalar() {
            float r1 = .5f, g1 = .25f, b1 = .7f;
            float value = .2f;
            float r = r1 * value;
            float g = g1 * value;
            float b = b1 * value;

            Color3D c1 = new Color3D(r1, g1, b1);

            //Test left to right
            Color3D c = c1 * value;
            TestHelper.AssertEquals(r, g, b, c, "Testing OpMultiplyByValue");

            //Test right to left
            c = value * c1;
            TestHelper.AssertEquals(r, g, b, c, "Testing OpMultiplyByValue");
        }

        [Test]
        public void TestDivide() {
            float r1 = .5f, g1 = .25f, b1 = .7f;
            float r2 = .2f, g2 = .1f, b2 = .05f;
            float r = r1 / r2;
            float g = g1 / g2;
            float b = b1 / b2;

            Color3D c1 = new Color3D(r1, g1, b1);
            Color3D c2 = new Color3D(r2, g2, b2);
            Color3D c = c1 / c2;

            TestHelper.AssertEquals(r, g, b, c, "Testing OpDivide");
        }

        [Test]
        public void TestDivideByFactor() {
            float r1 = .5f, g1 = .25f, b1 = .7f;
            float value = .2f;
            float r = r1 / value;
            float g = g1 / value;
            float b = b1 / value;

            Color3D c1 = new Color3D(r1, g1, b1);

            Color3D c = c1 / value;
            TestHelper.AssertEquals(r, g, b, c, "Testing OpDivideByFactor");
        }
    }
}
