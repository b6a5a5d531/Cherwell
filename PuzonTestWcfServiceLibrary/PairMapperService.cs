using System;

namespace PuzonTestWcfServiceLibrary
{
    /// <summary>
    /// Describes a mapping between pairs of objects (called left and right)
    /// such that each side can be looked up from the other
    /// </summary>
    public interface IPairMapper
    {
        /// <summary>
        /// Returns the pair of an item if it exists
        /// </summary>
        /// <param name="left">Left member of the pair</param>
        /// <returns>Right member of the pair</returns>
        object Right(object left);

        /// <summary>
        /// Returns the pair of an item if it exists
        /// </summary>
        /// <param name="right">Right member of the pair</param>
        /// <returns>Left member of the pair</returns>
        object Left(object right);
    }

    /// <summary>
    /// Implements the IPairMapperService, with only the default mapper
    /// implemented, which enforces the rules of the sample problem specification
    /// </summary>
    public class PairMapperService : IPairMapperService
    {
        public object Right(object left, Guid? mapperId)
        {
            return GetMapper(mapperId).Right(left);
        }

        public object Left(object right, Guid? mapperId)
        {
            return GetMapper(mapperId).Left(right);
        }

        private static IPairMapper GetMapper(Guid? mapperId)
        {
            if (mapperId.HasValue)
            {
                // look up mapper. Database? Other preset code beside default?
                // Some kind of loader to allow plug-in mappers might be good
                throw new NotImplementedException();
            }
            return new RightTriangle6x6Mapper();
        }
    }
}
