﻿using System;
using System.Runtime.Serialization;

namespace MyBank.Desktop.Model
{
    [Serializable]
    internal class NetworkException : Exception
    {
        public NetworkException()
        {
        }

        public NetworkException(string? message) : base(message)
        {
        }

        public NetworkException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NetworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}