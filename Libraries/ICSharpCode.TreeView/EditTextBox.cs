// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows;

namespace ICSharpCode.TreeView
{
	public class EditTextBox : TextBox
	{
		static EditTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(EditTextBox),
				new FrameworkPropertyMetadata(typeof(EditTextBox)));
		}

		public EditTextBox()
		{
			Loaded += delegate { Init(); };
		}

		public SharpTreeViewItem Item { get; set; }

		public SharpTreeNode Node {
			get { return Item.Node; }
		}

		void Init()
		{
			if (Node != null)
				Text = Node.LoadEditText();
			Focus();
			SelectAll();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Enter) {
				Commit();
			} else if (e.Key == Key.Escape) {
				if (Node != null)
					Node.IsEditing = false;
			}
		}

		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			if (Node != null && Node.IsEditing) {
				Commit();
			}
		}

		bool committing;

		void Commit()
		{
			if (!committing) {
				committing = true;

				if (Node != null) {
					Node.IsEditing = false;
					if (!Node.SaveEditText(Text)) {
						Item.Focus();
					}
					Node.RaisePropertyChanged("Text");
				}

				//if (Node.SaveEditText(Text)) {
				//    Node.IsEditing = false;
				//    Node.RaisePropertyChanged("Text");
				//}
				//else {
				//    Init();
				//}

				committing = false;
			}
		}
	}
}
