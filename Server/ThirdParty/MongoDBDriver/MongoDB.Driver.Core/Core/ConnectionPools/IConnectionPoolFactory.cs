/* Copyright 2013-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Core.Servers;

namespace MongoDB.Driver.Core.ConnectionPools
{
    /// <summary>
    /// Represents a connection pool factory.
    /// </summary>
    public interface IConnectionPoolFactory
    {
        // methods
        /// <summary>
        /// Creates a connection pool.
        /// </summary>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="endPoint">The end point.</param>
        /// <returns>A connection pool.</returns>
        IConnectionPool CreateConnectionPool(ServerId serverId, EndPoint endPoint);
    }
}
