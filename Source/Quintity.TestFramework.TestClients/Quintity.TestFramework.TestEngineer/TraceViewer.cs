/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System;
using System.Windows.Forms;

namespace Quintity.TestFramework.TestEngineer
{
	/// <summary>
	/// Summary description for TestOutputViewer.
	/// </summary>
	public class TraceViewer : System.Windows.Forms.RichTextBox
	{
		private System.Windows.Forms.ContextMenu m_contextMenu;
		private System.Windows.Forms.MenuItem m_miClear;
        private System.Windows.Forms.MenuItem m_miCopy;
        private System.Windows.Forms.MenuItem m_miSelectAll;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TraceViewer()
		{
            InitializeComponent();

            ReadOnly = true;
		}

		private void InitializeComponent()
		{
			this.m_contextMenu = new System.Windows.Forms.ContextMenu();
            this.m_miSelectAll = new System.Windows.Forms.MenuItem();
            this.m_miCopy = new System.Windows.Forms.MenuItem();
            this.m_miClear = new System.Windows.Forms.MenuItem();

			// 
			// m_contextMenu
			// 
			this.m_contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this.m_miSelectAll,
																						  this.m_miCopy, this.m_miClear});
			this.m_contextMenu.Popup += new System.EventHandler(this.m_contextMenu_Popup);
			// 
			// m_miClear
			// 
			this.m_miClear.Index = 0;
			this.m_miClear.Text = "&Clear Contents";
			this.m_miClear.Click += new System.EventHandler(this.m_miClear_Click);
            // 
            // m_miCopy
            // 
            this.m_miCopy.Index = 0;
            this.m_miCopy.Text = "&Copy";
            this.m_miCopy.Click += new System.EventHandler(this.m_miCopy_Click);
            // 
            // m_miSelectAll
            // 
            this.m_miSelectAll.Index = 0;
            this.m_miSelectAll.Text = "&Select All";
            this.m_miSelectAll.Click += new System.EventHandler(this.m_m_miSelectAll_Click);
			// 
			// TestOutputViewer
			// 
			this.ContextMenu = this.m_contextMenu;
		}

		/// <summary>
		/// Message handler for Clear popup menu selection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void m_miClear_Click(object sender, System.EventArgs e)
		{
			this.Clear();
		}

        private void m_miCopy_Click(object sender, System.EventArgs e)
        {
            this.Copy();
        }

        private void m_m_miSelectAll_Click(object sender, System.EventArgs e)
        {
            this.SelectAll();
        }

		private void m_contextMenu_Popup(object sender, System.EventArgs e)
		{
            this.m_miCopy.Enabled = this.SelectedText != "" ? true : false;
			this.m_contextMenu.MenuItems.Add(0, this.m_miClear );
            this.m_contextMenu.MenuItems.Add(1, this.m_miCopy);
		}
	}
}
