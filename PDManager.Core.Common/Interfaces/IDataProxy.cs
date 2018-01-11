using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDManager.Core.Common.Interfaces
{
    /// <summary>
    /// Data Proxy Interface for Get/Insert data to PDManager Cloud Repository
    /// </summary>
    public interface IDataProxy
    {

        /// <summary>
        /// Insert into PDManager repository
        /// </summary>
        /// <typeparam name="T">Object Template</typeparam>

        /// <param name="item"> Item</param>        
        /// <example> 
        /// This sample shows how to call the <see cref="GetZero"/> method.
        /// <code>
        ///  IDataProxy proxy = new DataProxy(/*Credential Provider*/);
        ///  var observations = proxy.Get&ltPDObservation&gt("api/observations", 10, 0, "{patientid:\"5900aa2a2f2cd563c4ae3027\",deviceid:\"\",codeid:\"PDTFTS_MAX\",datefrom:0,dateto:0,aggr:\"total\"}", null).Result;
        /// </code>
        /// </example>
        /// <returns>True in case of success otherwise false</returns>
        Task<bool> Insert<T>(T item) where T : class;


        /// <summary>
        /// Get a list of items from the main repository
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>

        /// <param name="take">Take</param>
        /// <param name="skip">Skip</param>
        /// <param name="filter">Filter (Defined per type)</param>
        /// <param name="sort">Sort property</param>
        /// <param name="sortdir">Sort direction</param>
        /// <param name="lastmodified">Last modified (for syncing)</param>
        /// <returns>List of T items</returns>
        Task<IEnumerable<T>> Get<T>( int take, int skip, string filter, string sort, string sortdir = "false", long lastmodified = -1) where T : class;

        /// <summary>
        /// Get a single item
        /// </summary>
        /// <typeparam name="T">Item</typeparam>

        /// <param name="id">Item id</param>        
        /// <returns>A single T item</returns>
        Task<T> Get<T>(string id) where T : class;
    }
}