
//
// SetEncodingComponent.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//


using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.Messaging;
using XMLNORM;


namespace Winterdom.BizTalk.Samples.FixEncoding
{
   using Design;
   
   /// <summary>
   /// Sets the encoding to use when sending out messages
   /// in Send Pipelines for the Assembler components to use.
   /// Allows you more flexibility than the built-in facilities
   /// for this.
   /// </summary>
   [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
   [System.Runtime.InteropServices.Guid("19416f39-d2f4-405a-9b64-7ca889e8a0cb")]
   [ComponentCategory(CategoryTypes.CATID_Any)]
   public class SetEncodingComponent : 
      Microsoft.BizTalk.Component.Interop.IComponent, 
      IBaseComponent, IPersistPropertyBag, IComponentUI
   {
      private static readonly Dictionary<int, string> _encodingList;
      private Encoding _encoding;
      private ResourceManager _resourceManager =
         new ResourceManager("Winterdom.BizTalk.Samples.FixEncoding.SetEncodingComponent", 
         Assembly.GetExecutingAssembly());


      /// <summary>
      /// Codepage of the encoding to use. We use this for editing
      /// of the property, since the name of the encoding is hard to get.
      /// </summary>
      [Editor(typeof(EncodingTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
      [TypeConverter(typeof(EncodingTypeConverter))]
      public Encoding Encoding
      {
         get { return _encoding; }
         set { _encoding = value; }
      }

      public SetEncodingComponent()
      {
         _encoding = global::System.Text.Encoding.UTF8;
      }

      static SetEncodingComponent()
      {
         EncodingInfo[] encodings = Encoding.GetEncodings();
         _encodingList = new Dictionary<int, string>();
         foreach ( EncodingInfo ei in encodings )
         {
            _encodingList.Add(ei.CodePage, ei.Name);
         }
      }

      #region IBaseComponent members
      /// <summary>
      /// Name of the component
      /// </summary>
      [Browsable(false)]
      public string Name
      {
         get {return _resourceManager.GetString("COMPONENTNAME", CultureInfo.InvariantCulture); }
      }

      /// <summary>
      /// Version of the component
      /// </summary>
      [Browsable(false)]
      public string Version
      {
         get { return _resourceManager.GetString("COMPONENTVERSION", CultureInfo.InvariantCulture); }
      }

      /// <summary>
      /// Description of the component
      /// </summary>
      [Browsable(false)]
      public string Description
      {
         get { return _resourceManager.GetString("COMPONENTDESCRIPTION", CultureInfo.InvariantCulture); }
      }
      #endregion

      #region IPersistPropertyBag members
      /// <summary>
      /// Gets class ID of component for usage from unmanaged code.
      /// </summary>
      /// <param name="classid">
      /// Class ID of the component
      /// </param>
      public void GetClassID(out System.Guid classid)
      {
         classid = new System.Guid("3ce651a3-9c9c-4cfd-96c2-5fee7cdc9455");
      }

      /// <summary>
      /// not implemented
      /// </summary>
      public void InitNew()
      {
      }

      /// <summary>
      /// Loads configuration properties for the component
      /// </summary>
      /// <param name="pb">Configuration property bag</param>
      /// <param name="errlog">Error status</param>
      public virtual void Load(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, int errlog)
      {
         object val = null;
         val = ReadPropertyBag(pb, "Encoding");
         if ( (val != null) )
         {
            Encoding = global::System.Text.Encoding.GetEncoding((int)val);
         }
      }

      /// <summary>
      /// Saves the current component configuration into the property bag
      /// </summary>
      /// <param name="pb">Configuration property bag</param>
      /// <param name="fClearDirty">not used</param>
      /// <param name="fSaveAllProperties">not used</param>
      public virtual void Save(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties)
      {
         this.WritePropertyBag(pb, "Encoding", Encoding.CodePage);
      }

      #region utility functionality
      /// <summary>
      /// Reads property value from property bag
      /// </summary>
      /// <param name="pb">Property bag</param>
      /// <param name="propName">Name of property</param>
      /// <returns>Value of the property</returns>
      private object ReadPropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName)
      {
         object val = null;
         try
         {
            pb.Read(propName, out val, 0);
         } catch ( System.ArgumentException )
         {
            return val;
         } catch ( System.Exception e )
         {
            throw new System.ApplicationException(e.Message);
         }
         return val;
      }

      /// <summary>
      /// Writes property values into a property bag.
      /// </summary>
      /// <param name="pb">Property bag.</param>
      /// <param name="propName">Name of property.</param>
      /// <param name="val">Value of property.</param>
      private void WritePropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName, object val)
      {
         try
         {
            pb.Write(propName, ref val);
         } catch ( System.Exception e )
         {
            throw new System.ApplicationException(e.Message);
         }
      }
      #endregion
      #endregion

      #region IComponentUI members
      /// <summary>
      /// Component icon to use in BizTalk Editor
      /// </summary>
      [Browsable(false)]
      public IntPtr Icon
      {
         get { return ((System.Drawing.Bitmap)(this._resourceManager.GetObject("COMPONENTICON", CultureInfo.InvariantCulture))).GetHicon(); }
      }

      /// <summary>
      /// The Validate method is called by the BizTalk Editor during the build 
      /// of a BizTalk project.
      /// </summary>
      /// <param name="obj">An Object containing the configuration properties.</param>
      /// <returns>The IEnumerator enables the caller to enumerate through a collection of strings containing error messages. These error messages appear as compiler error messages. To report successful property validation, the method should return an empty enumerator.</returns>
      public System.Collections.IEnumerator Validate(object obj)
      {
         // example implementation:
         // ArrayList errorList = new ArrayList();
         // errorList.Add("This is a compiler error");
         // return errorList.GetEnumerator();
         return null;
      }
      #endregion

      #region IComponent members
      /// <summary>
      /// Implements IComponent.Execute method.
      /// </summary>
      /// <param name="pc">Pipeline context</param>
      /// <param name="inmsg">Input message</param>
      /// <returns>Original input message</returns>
      /// <remarks>
      /// IComponent.Execute method is used to initiate
      /// the processing of the message in this pipeline component.
      /// </remarks>
      public IBaseMessage Execute(IPipelineContext pc, IBaseMessage inmsg)
      {
         TargetCharset targetCharset = new TargetCharset();
         inmsg.Context.Write ( 
            targetCharset.Name.Name, 
            targetCharset.Name.Namespace, 
            _encodingList[Encoding.CodePage]
         );
         return inmsg;
      }
      #endregion

   } // class SetEncodingComponent

} // namespace Winterdom.BizTalk.Samples.FixEncoding
