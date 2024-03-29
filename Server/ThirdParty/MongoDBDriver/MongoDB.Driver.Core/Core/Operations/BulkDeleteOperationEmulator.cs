﻿/* Copyright 2010-present MongoDB Inc.
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
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Core.Bindings;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders;

namespace MongoDB.Driver.Core.Operations
{
    internal class BulkDeleteOperationEmulator : BulkUnmixedWriteOperationEmulatorBase<DeleteRequest>
    {
        // constructors
        public BulkDeleteOperationEmulator(
            CollectionNamespace collectionNamespace,
            IEnumerable<DeleteRequest> requests,
            MessageEncoderSettings messageEncoderSettings)
            : base(collectionNamespace, requests, messageEncoderSettings)
        {
        }

        // methods
        protected override WriteConcernResult ExecuteProtocol(IChannelHandle channel, DeleteRequest request, CancellationToken cancellationToken)
        {
            if (request.Collation != null)
            {
                throw new NotSupportedException("OP_DELETE does not support collations.");
            }
            var isMulti = request.Limit == 0;

            return channel.Delete(
               CollectionNamespace,
               request.Filter,
               isMulti,
               MessageEncoderSettings,
               WriteConcern,
               cancellationToken);
        }

        protected override Task<WriteConcernResult> ExecuteProtocolAsync(IChannelHandle channel, DeleteRequest request, CancellationToken cancellationToken)
        {
            var isMulti = request.Limit == 0;
            if (request.Collation != null)
            {
                throw new NotSupportedException("OP_DELETE does not support collations.");
            }

            return channel.DeleteAsync(
               CollectionNamespace,
               request.Filter,
               isMulti,
               MessageEncoderSettings,
               WriteConcern,
               cancellationToken);
        }
    }
}
