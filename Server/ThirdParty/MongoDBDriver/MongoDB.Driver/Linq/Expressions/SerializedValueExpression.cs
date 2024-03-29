﻿/* Copyright 2016-present MongoDB Inc.
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
using System.Linq.Expressions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver.Linq.Expressions
{
    internal sealed class SerializedConstantExpression : SerializationExpression
    {
        private readonly object _value;
        private readonly IBsonSerializer _serializer;

        public SerializedConstantExpression(object value, IBsonSerializer serializer)
        {
            _value = Ensure.IsNotNull(value, nameof(value));
            _serializer = Ensure.IsNotNull(serializer, nameof(serializer));
        }

        public override ExtensionExpressionType ExtensionType
        {
            get { return ExtensionExpressionType.SerializedConstant; }
        }

        public override IBsonSerializer Serializer
        {
            get { return _serializer; }
        }

        public override Type Type
        {
            get { return _serializer.ValueType; }
        }

        public object Value
        {
            get { return _value; }
        }

        public override string ToString()
        {
            return $"Constant({_value.ToString()})";
        }

        protected internal override Expression Accept(ExtensionExpressionVisitor visitor)
        {
            return visitor.VisitSerializedConstant(this);
        }
    }
}