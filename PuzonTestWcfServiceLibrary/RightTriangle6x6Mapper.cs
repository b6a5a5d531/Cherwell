using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PuzonTestWcfServiceLibrary
{
    /// <summary>
    /// Implements the rules for the sample problem (namely, a 6x6 grid 
    /// of triangles with rows A-F and columns 1-12, with hypotenuse going 
    /// from upper left to lower right.
    /// Assumptions which would all need to be verified:
    /// x is 0...600 (left to right)
    /// y is 0...600 (top to bottom)
    /// row is A...F (left to right)
    /// column is 1...12 (top to bottom)
    /// </summary>
    internal class RightTriangle6x6Mapper : IPairMapper
    {
        // Map row letter to index
        private static readonly Dictionary<char, int> Rows = new Dictionary<char, int>()
        {
            { 'A', 0 }, { 'B', 1 }, { 'C', 2 }, { 'D', 3 }, { 'E', 4 }, { 'F', 5 }
        };

        private static readonly int MaxColumn = 12;
        private static readonly int LegWidthInPixels = 10;
        private static readonly Dictionary<string, string> LeftToRight = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> RightToLeft = new Dictionary<string, string>();

        static RightTriangle6x6Mapper()
        {
            BuildMaps();
        }

        private static void BuildMaps()
        {
            // each square contains two triangles - a lower left one, and an upper right one
            // +-----+-----+
            // +\ A2 +\ A4 +
            // + \   + \   +
            // +  \  +  \  +
            // +   \ +   \ +
            // + A1 \+ A3 \+
            // +-----+-----+ ...
            // +\ B2 +\ B4 +
            // + \   + \   +
            // +  \  +  \  +
            // +   \ +   \ +
            // + B1 \+ B3 \+
            // +-----+-----+
            //      ...
            // conceptually, we are mapping a label to a set of 3 vertices and back
            // however, the vertices may come out of order. we can assign them an index for sorting purposes
            foreach (var rowLabel in Rows.Keys)
            {
                for (var column = 1; column <= MaxColumn; column++)
                {
                    var key = $"{rowLabel}{column}";

                    var rowIndex = Rows[rowLabel];
                    var xLeft = (column - 1) / 2 * LegWidthInPixels;
                    var xRight = xLeft + LegWidthInPixels;
                    var yTop = rowIndex * LegWidthInPixels;
                    var yBottom = yTop + LegWidthInPixels;
                    var odd = column % 2 != 0;

                    // the hypotenuse is the same for both triangles, so the upper left and lower right vertices are always
                    // the same. The final vertex is either the lower left (for odd column numbers) or upper right (for even).
                    // so the left side looks like A1, A2, A3, A4, ...

                    var vertexIndexes = new List<int>()
                    {
                        // upper left
                        ToOrdinal(xLeft, yTop),
                        // lower right
                        ToOrdinal(xRight, yBottom),
                        // right angle vertex
                        odd ? ToOrdinal(xLeft, yBottom) : ToOrdinal(xRight, yTop)
                    };
                    vertexIndexes.Sort();

                    var valueAsText = ToCSV(vertexIndexes);
                    System.Diagnostics.Debug.WriteLine($"{rowIndex}: {key}->{valueAsText}");
                    LeftToRight.Add(key, valueAsText);
                    System.Diagnostics.Debug.WriteLine($"{rowIndex}: {key}<-{valueAsText}");
                    RightToLeft.Add(valueAsText, key);
                }
            }
        }

        /// <summary>
        /// CSV Helper
        /// </summary>
        /// <param name="value">Enumeration of values to comma-separate</param>
        /// <returns>Comma-separated value list</returns>
        private static string ToCSV<T>(IEnumerable<T> value)
        {
            // bet there is a more compact way to do this...
            var text = new System.Text.StringBuilder();
            foreach (var item in value)
            {
                text.Append($"{item},");
            }
            return text.ToString().TrimEnd(new char[] { ',' });
        }

        private static readonly int Width = LegWidthInPixels* MaxColumn / 2;

        /// <summary>
        /// Converts an X/Y pair to a ordinal index for the purpose of sorting
        /// vertices
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Ordinal</returns>
        private static int ToOrdinal(int x, int y)
        {
            return y * Width + x;
        }

        /// <summary>
        /// Converts an ordinal value to coordinates
        /// </summary>
        /// <param name="value">Ordinal</param>
        /// <returns>x/y coordinate pair</returns>
        private static Tuple<int, int> FromOrdinal(string value)
        {
            var indexInt = int.Parse(value);
            return Tuple.Create(indexInt % Width, indexInt / Width);
        }

        /// <summary>
        /// For debugging - print CSV of vertices
        /// </summary>
        /// <param name="value">Enumerable vertices</param>
        /// <returns>CSV representation</returns>
        private static string VertexSetToString(IEnumerable<Tuple<int, int>> value)
        {
            var text = new System.Text.StringBuilder();
            foreach (var item in value)
            {
                text.Append($"{item.Item1},{item.Item1} ");
            }
            return text.ToString().TrimEnd(new char[] { ' ' });
        }

        /// <summary>
        /// Converts a sequence of vertices to a sequence of ordinal (CSV)
        /// </summary>
        /// <param name="value">Sequence of vertices</param>
        /// <returns>CSV of ordinals representing vertices</returns>
        private static string VerticesToOrdinals(IEnumerable<Tuple<int, int>> value)
        {
            var indexes = value.ToList().ConvertAll(v => ToOrdinal(v.Item1, v.Item2));
            indexes.Sort();
            var text = new StringBuilder();
            foreach (var index in indexes)
                text.Append($"{index},");
            return text.ToString().TrimEnd(new char[] { ',' });
        }

        /// <summary>
        /// Converts a sequence of ordinals (CSV) to a sequence of vertices
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private IEnumerable<Tuple<int, int>> OrdinalsToVertices(string value)
        {
            return value.Split(new char[] { ',' })
                .ToList()
                .ConvertAll(indexString => FromOrdinal(indexString));
        }

        public object Right(object left)
        {
            // left must be a string in the form "A1"
            // note: this is a strict interpretation of the sample problem
            // a more generous approach could be lobbied for
            if (!(left is string key) || key.Length < 2 || key.Length > 3)
                throw new ArgumentException("Left must be a string in the form A1");

            if (!Rows.ContainsKey(key[0]))
                throw new ArgumentOutOfRangeException(nameof(left), @"First character should be one of A-F");

            var column = key.Substring(1);
            if (!int.TryParse(column, out _))
                throw new ArgumentOutOfRangeException(nameof(left), @"Column must be one of 1-12");

            if (!LeftToRight.ContainsKey(key))
                throw new ArgumentOutOfRangeException(nameof(left));

            var indexString = LeftToRight[key];
            return OrdinalsToVertices(indexString);
        }

        public object Left(object right)
        {
            // again, other types of input could be good - lots of ways to 
            // indicate a sequence of vertices...
            if (!(right is IEnumerable<Tuple<int, int>> vertices))
                throw new ArgumentException(nameof(right), "Expecting IEnumerable<Tuple<int, int>>");

            var key = VerticesToOrdinals(vertices);
            return RightToLeft.ContainsKey(key) ?
                RightToLeft[key] :
                throw new ArgumentOutOfRangeException(nameof(right), "Specified set of vertices does not seem to correspond to a triangle");
        }
    }
}
