
//
// EncodingTypeEditor.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms.Design;

namespace Winterdom.BizTalk.Samples.FixEncoding.Design
{
   /// <summary>
   /// Simple type editor for the Encoding property
   /// of our component, so that we can show a UI
   /// with the list of possible encodings 
   /// supported
   /// </summary>
   public class EncodingTypeEditor : UITypeEditor
   {
      /// <summary>
      /// please allows us to be resized, otherwise 
      /// we won't be able to read anything :)
      /// </summary>
      public override bool IsDropDownResizable
      {
         get { return true; }
      }
      /// <summary>
      /// Make sure we tell the property grid we want a drop down 
      /// control
      /// </summary>
      /// <param name="context"></param>
      /// <returns></returns>
      public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
      {
         if ( context == null || context.Instance == null )
            return base.GetEditStyle(context);
         return UITypeEditorEditStyle.DropDown;
      }

      /// <summary>
      /// Popup the drop down with the encoding list
      /// </summary>
      /// <param name="context"></param>
      /// <param name="provider"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
      {
         IWindowsFormsEditorService editorService;

         if ( context == null || context.Instance == null || provider == null )
            return value;

         try
         {
            // get the editor service, just like in windows forms
            editorService = (IWindowsFormsEditorService)
               provider.GetService(typeof(IWindowsFormsEditorService));

            EncodingListControl control = new EncodingListControl();
            Encoding selectedEncoding = (Encoding)value;
            control.SelectedCodePage = selectedEncoding.CodePage;
            control.Click += delegate(object sender, EventArgs e)
            {
               selectedEncoding = Encoding.GetEncoding(control.SelectedCodePage);
               editorService.CloseDropDown();
            };

            editorService.DropDownControl(control);
            return selectedEncoding;
         }  
         finally
         {
            editorService = null;
         } 
      }

   } // class EncodingTypeEditor

} // namespace Winterdom.BizTalk.Samples.FixEncoding.Design
