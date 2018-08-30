using Microsoft.VisualStudio.TestTools.UnitTesting;
using PuzonTestWcfServiceLibrary;
using System;
using System.Collections.Generic;

namespace PuzonTestWcfServiceLibraryTest
{
    [TestClass]
    public class TriangleServiceTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetTriangleFromRowAndColumnRowOutOfRange()
        {
            var service = new TriangleService();
            var actual = service.GetTriangle('a', 1);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetTriangleFromRowAndColumnColumnOutOfRangeLow()
        {
            var service = new TriangleService();
            var actual = service.GetTriangle('A', 0);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetTriangleFromRowAndColumnColumnOutOfRangeHigh()
        {
            var service = new TriangleService();
            var actual = service.GetTriangle('A', 13);
        }

        [TestMethod]
        public void TestGetTriangleFromRowAndColumn()
        {
            var service = new TriangleService();
            var actual = service.GetTriangle('A', 1);
            Assert.AreEqual('A', actual.Row);
            Assert.AreEqual(1, actual.Column);
            Assert.AreEqual(0, actual.Vertices[0].X);
            Assert.AreEqual(0, actual.Vertices[0].Y);
            Assert.AreEqual(0, actual.Vertices[1].X);
            Assert.AreEqual(10, actual.Vertices[1].Y);
            Assert.AreEqual(10, actual.Vertices[2].X);
            Assert.AreEqual(10, actual.Vertices[2].Y);

            actual = service.GetTriangle('B', 4);
            Assert.AreEqual('B', actual.Row);
            Assert.AreEqual(4, actual.Column);
            Assert.AreEqual(10, actual.Vertices[0].X);
            Assert.AreEqual(10, actual.Vertices[0].Y);
            Assert.AreEqual(20, actual.Vertices[1].X);
            Assert.AreEqual(10, actual.Vertices[1].Y);
            Assert.AreEqual(20, actual.Vertices[2].X);
            Assert.AreEqual(20, actual.Vertices[2].Y);
        }

        [TestMethod]
        public void TestGetTriangleFromVertices()
        {
            var service = new TriangleService();
            var input = new List<Vertex>()
            {
                new Vertex() { X = 0, Y = 0 },
                new Vertex() { X = 10, Y = 10 },
                new Vertex() { X = 0, Y = 10 }
            };
            var actual = service.GetTriangle(input);
            Assert.AreEqual('A', actual.Row);
            Assert.AreEqual(1, actual.Column);

            input = new List<Vertex>()
            {
                new Vertex() { X = 10, Y = 20 },
                new Vertex() { X = 10, Y = 10 },
                new Vertex() { X = 20, Y = 20 }
            };
            actual = service.GetTriangle(input);
            Assert.AreEqual('B', actual.Row);
            Assert.AreEqual(3, actual.Column);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetTriangleFromVerticesBadInput()
        {
            var service = new TriangleService();
            var input = new List<Vertex>()
            {
                new Vertex() { X = 55, Y = 0 },
                new Vertex() { X = 10, Y = 10 },
                new Vertex() { X = 0, Y = 10 }
            };
            var actual = service.GetTriangle(input);
        }
    }
}
