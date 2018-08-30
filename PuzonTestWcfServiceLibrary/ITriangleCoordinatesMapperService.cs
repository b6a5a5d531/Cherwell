using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace PuzonTestWcfServiceLibrary
{
    /// <summary>
    /// A service to map between coordinate systems for a grid of triangles,
    /// the two systems being row/column and vertex/vertex/vertex
    /// </summary>
    [ServiceContract]
    public interface ITriangleCoordinatesMapperService
    {
        /// <summary>
        /// Returns a triangle for a given row and column
        /// </summary>
        /// <param name="row">Row letter (e.g. from A-F)</param>
        /// <param name="column">Column number (e.g. from 1-12)</param>
        /// <returns>An array containing each vertex of the requested triangle</returns>
        [OperationContract]
        Triangle GetTriangle(char row, int column);

        /// <summary>
        /// Returns a triangle for a given set of vertices
        /// </summary>
        /// <param name="vertex1">Vertex 1</param>
        /// <param name="vertex2">Vertex 2</param>
        /// <param name="vertex3">Vertex 3</param>
        /// <returns>Triangle for vertex, if found</returns>
        [OperationContract]
        Triangle GetTriangle(IEnumerable<Vertex> vertices);
    }

    [DataContract]
    public class Vertex
    {
        /// <summary>
        /// X (horizontal) position
        /// </summary>
        [DataMember]
        public int X { get; set; }

        /// <summary>
        /// Y (vertical) position
        /// </summary>
        [DataMember]
        public int Y { get; set; }
    }

    [DataContract]
    public class Triangle
    {
        /// <summary>
        /// Set of vertices that define this triangle
        /// </summary>
        [DataMember]
        public Vertex[] Vertices { get; set; }

        /// <summary>
        /// Row letter (e.g. A-F)
        /// </summary>
        [DataMember]
        public char Row { get; set; }

        /// <summary>
        /// Column number (e.g. 1-12)
        /// </summary>
        [DataMember]
        public int Column { get; set; }
    }
}
