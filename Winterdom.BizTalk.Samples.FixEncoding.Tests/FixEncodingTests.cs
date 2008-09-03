
//
// FixEncodingTests.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.DefaultPipelines;
using XMLNORM;

using NUnit.Framework;

using Winterdom.BizTalk.PipelineTesting;
using Winterdom.BizTalk.PipelineTesting.Simple;

namespace Winterdom.BizTalk.Samples.FixEncoding.Tests
{
   [TestFixture]
   public class FixEncodingTests
   {
      /// <summary>
      /// Tests the encoding is set correctly
      /// </summary>
      [Test]
      public void Test_SetEncoding()
      {
         FixEncodingComponent component = new FixEncodingComponent();
         component.Encoding = Encoding.UTF32;
         ReceivePipelineWrapper pipeline = Pipelines.Receive()
            .WithDecoder(component);

         IBaseMessage inputMessage = MessageHelper.CreateFromString("hello!");

         MessageCollection outputMessages = pipeline.Execute(inputMessage);
         Assert.IsNotNull(outputMessages);
         Assert.IsTrue(outputMessages.Count > 0);
         Assert.AreEqual("utf-32", outputMessages[0].BodyPart.Charset);
      }

      [Test]
      public void Test_SetTargetCharset()
      {
         SetEncodingComponent component = new SetEncodingComponent();
         component.Encoding = Encoding.UTF32;
         SendPipelineWrapper pipeline = Pipelines.Send()
            .WithPreAssembler(component);

         IBaseMessage inputMessage = 
            MessageHelper.CreateFromString("<message>hello!</message>");
         IBaseMessage outputMessage = pipeline.Execute(inputMessage);
         Assert.IsNotNull(outputMessage);

         TargetCharset targetCharset = new TargetCharset();
         string charset = (string)outputMessage.Context.Read(
            targetCharset.Name.Name, targetCharset.Name.Namespace
         );
         Assert.AreEqual("utf-32", charset);
      }
   } // class FixEncodingTests

} // namespace Winterdom.BizTalk.Samples.FixEncoding.Tests
