using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzonTestWcfServiceLibrary
{
    public class TriangleService : ITriangleCoordinatesMapperService
    {
        public Triangle GetTriangle(char row, int column)
        {
            // this is effectively a service consumer of the IPairMapperService but for 
            // convenience uses it directly...
            var key = $"{row}{column}";
            var value = new PairMapperService().Right(left: key, mapperId: null)
                as IEnumerable<Tuple<int, int>>;

            // because we peek, we know value is a HashSet<Tuple<int, int>> representing 
            // the vertices.
            // unpack that into our triangle object
            return new Triangle()
            {
                Row = row,
                Column = column,
                Vertices = value
                                .ToList()
                                .Select(v => new Vertex() { X = v.Item1, Y = v.Item2 })
                                .ToArray()
            };
        }

        public Triangle GetTriangle(IEnumerable<Vertex> vertices)
        {
            // this is effectively a service consumer of the IPairMapperService but for 
            // convenience uses it directly...
            var verticesList = vertices.ToList();
            if (verticesList.Count() != 3)
                throw new ArgumentException(@"A triangle should have 3 vertices", @"vertices");

            var key = new List<Tuple<int, int>>
                (verticesList.Select(v => Tuple.Create(v.X, v.Y)));

            if (!(new PairMapperService().Left(right: key, mapperId: null) is string value))
                throw new ArgumentOutOfRangeException(nameof(vertices));

            // returned value is a string representing the row and column i.e. "A1"

            var row = value[0];
            var column = int.Parse(value.Substring(1));

            return new Triangle()
            {
                Row = row,
                Column = column,
                Vertices = new Vertex[] { verticesList[0], verticesList[1], verticesList[2]}
            };
        }
    }
}
