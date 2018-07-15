using Microsoft.VisualStudio.TestTools.UnitTesting;

using KeyLogger.Protocol;
using System;

namespace KeyLogger.Tests.Protocol
{
    public static class IMessageTestHelper
    {
        public static void Test_All(IMessage message)
        {
            Receive_NullStream(message);
            Send_NullStream(message);
        }

        public static void Receive_NullStream(IMessage message)
        {
            try
            {
                message.Receive(null);
                Assert.Fail("Passing a null stream to a IMessage.Receive should throw a " +
                "ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                
            }
        }

        public static void Send_NullStream(IMessage message)
        {
            try
            {
                message.Send(null);
                Assert.Fail("Passing a null stream to a IMessage.Receive should throw a " +
                "ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                
            }
        }
    }
}