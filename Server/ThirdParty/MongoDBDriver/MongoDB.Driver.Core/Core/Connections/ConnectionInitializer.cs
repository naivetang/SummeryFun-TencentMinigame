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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver.Core.Authentication;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Core.WireProtocol;

namespace MongoDB.Driver.Core.Connections
{
    /// <summary>
    /// Represents a connection initializer (opens and authenticates connections).
    /// </summary>
    internal class ConnectionInitializer : IConnectionInitializer
    {
        private readonly BsonDocument _clientDocument;

        public ConnectionInitializer(string applicationName)
        {
            _clientDocument = ClientDocumentHelper.CreateClientDocument(applicationName);
        }

        public ConnectionDescription InitializeConnection(IConnection connection, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(connection, nameof(connection));
            var isMasterCommand = CreateInitialIsMasterCommand(connection.Settings.Authenticators);
            var isMasterProtocol = IsMasterHelper.CreateProtocol(isMasterCommand);
            var isMasterResult = IsMasterHelper.GetResult(connection, isMasterProtocol, cancellationToken);

            var buildInfoProtocol = CreateBuildInfoProtocol();
            var buildInfoResult = new BuildInfoResult(buildInfoProtocol.Execute(connection, cancellationToken));

            var description = new ConnectionDescription(connection.ConnectionId, isMasterResult, buildInfoResult);

            AuthenticationHelper.Authenticate(connection, description, cancellationToken);

            try
            {
                var getLastErrorProtocol = CreateGetLastErrorProtocol();
                var getLastErrorResult = getLastErrorProtocol.Execute(connection, cancellationToken);

                description = UpdateConnectionIdWithServerValue(description, getLastErrorResult);
            }
            catch
            {
                // if we couldn't get the server's connection id, so be it.
            }

            return description;
        }

        public async Task<ConnectionDescription> InitializeConnectionAsync(IConnection connection, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(connection, nameof(connection));
            var isMasterCommand = CreateInitialIsMasterCommand(connection.Settings.Authenticators);
            var isMasterProtocol = IsMasterHelper.CreateProtocol(isMasterCommand);
            var isMasterResult = await IsMasterHelper.GetResultAsync(connection, isMasterProtocol, cancellationToken).ConfigureAwait(false);

            var buildInfoProtocol = CreateBuildInfoProtocol();
            var buildInfoResult = new BuildInfoResult(await buildInfoProtocol.ExecuteAsync(connection, cancellationToken).ConfigureAwait(false));

            var description = new ConnectionDescription(connection.ConnectionId, isMasterResult, buildInfoResult);

            await AuthenticationHelper.AuthenticateAsync(connection, description, cancellationToken).ConfigureAwait(false);

            try
            {
                var getLastErrorProtocol = CreateGetLastErrorProtocol();
                var getLastErrorResult = await getLastErrorProtocol.ExecuteAsync(connection, cancellationToken).ConfigureAwait(false);

                description = UpdateConnectionIdWithServerValue(description, getLastErrorResult);
            }
            catch
            {
                // if we couldn't get the server's connection id, so be it.
            }

            return description;
        }

        // private methods
        private CommandWireProtocol<BsonDocument> CreateBuildInfoProtocol()
        {
            var buildInfoCommand = new BsonDocument("buildInfo", 1);
            var buildInfoProtocol = new CommandWireProtocol<BsonDocument>(
                databaseNamespace: DatabaseNamespace.Admin,
                command: buildInfoCommand,
                slaveOk: true,
                resultSerializer: BsonDocumentSerializer.Instance,
                messageEncoderSettings: null);
            return buildInfoProtocol;
        }

        private CommandWireProtocol<BsonDocument> CreateGetLastErrorProtocol()
        {
            var getLastErrorCommand = new BsonDocument("getLastError", 1);
            var getLastErrorProtocol = new CommandWireProtocol<BsonDocument>(
                databaseNamespace: DatabaseNamespace.Admin,
                command: getLastErrorCommand,
                slaveOk: true,
                resultSerializer: BsonDocumentSerializer.Instance,
                messageEncoderSettings: null);
            return getLastErrorProtocol;
        }
        
        private BsonDocument CreateInitialIsMasterCommand(IReadOnlyList<IAuthenticator> authenticators)
        {
            var command = IsMasterHelper.CreateCommand();
            IsMasterHelper.AddClientDocumentToCommand(command, _clientDocument);
            return IsMasterHelper.CustomizeCommand(command, authenticators);
        }

        private ConnectionDescription UpdateConnectionIdWithServerValue(ConnectionDescription description, BsonDocument getLastErrorResult)
        {
            BsonValue connectionIdBsonValue;
            if (getLastErrorResult.TryGetValue("connectionId", out connectionIdBsonValue))
            {
                var connectionId = description.ConnectionId.WithServerValue(connectionIdBsonValue.ToInt32());
                description = description.WithConnectionId(connectionId);
            }

            return description;
        }
    }
}