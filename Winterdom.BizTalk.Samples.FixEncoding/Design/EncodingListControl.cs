
//
// EncodingListControl.cs
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
using System.Windows.Forms;

namespace Winterdom.BizTalk.Samples.FixEncoding.Design
{
   class EncodingListControl : ListBox
   {
      
      public int SelectedCodePage
      {
         get { return ((EncodingInfo)SelectedItem).CodePage; }
         set { SetSelectedCodePage(value); }
      }

      public EncodingListControl()
      {
         SelectionMode = SelectionMode.One;
         Dock = DockStyle.Fill;
         BorderStyle = BorderStyle.None;
         IntegralHeight = false;

         List<EncodingInfo> encodingsList = new List<EncodingInfo>();
         encodingsList.AddRange(Encoding.GetEncodings());
         encodingsList.Sort(new EncodingInfoComparer());
         DisplayMember = "DisplayName";
         ValueMember = "CodePage";

         foreach ( EncodingInfo ei in encodingsList )
         {
            Items.Add(ei);
         }
      }
      public void SetSelectedCodePage(int codepage)
      {
         for ( int i = 0; i < Items.Count; i++ )
         {
            if ( ((EncodingInfo)Items[i]).CodePage == codepage )
            {
               SelectedIndex = i;
               break;
            }
         }
      }

      public class EncodingInfoComparer : IComparer<EncodingInfo>
      {
         #region IComparer<EncodingInfo> Members

         public int Compare(EncodingInfo x, EncodingInfo y)
         {
            return x.DisplayName.CompareTo(y.DisplayName);
         }

         #endregion
      }

   } // class EncodingListControl

} // namespace Winterdom.BizTalk.Samples.FixEncoding.Design

