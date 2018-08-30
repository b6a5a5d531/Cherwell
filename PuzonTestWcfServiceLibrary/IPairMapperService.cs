using System;
using System.ServiceModel;

namespace PuzonTestWcfServiceLibrary
{
    /// <summary>
    /// Generic service providing two-way mapping between arbitrary pairs
    /// of objects using a specified mapper.
    /// </summary>
    [ServiceContract]
    public interface IPairMapperService
    {
        /// <summary>
        /// Returns an item's pair
        /// </summary>
        /// <param name="left">Left item in pair</param>
        /// <param name="mapperId">Mapper to use</param>
        /// <returns>Right item in pair</returns>
        [OperationContract]
        object Right(object left, Guid? mapperId);

        /// <summary>
        /// Returns an item's pair
        /// </summary>
        /// <param name="right">Right item in pair</param>
        /// <param name="mapperId">Mapper to use</param>
        /// <returns>Left item in pair</returns>
        [OperationContract]
        object Left(object right, Guid? mapperId);
    }
}
