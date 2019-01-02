/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestEngineer
{
    /// <summary>
    /// Summary description for TestQueueTreeView.
    /// </summary>
    public class ResultsViewer : System.Windows.Forms.RichTextBox
    {
        private System.Windows.Forms.ContextMenu m_contextMenu;
        private System.Windows.Forms.MenuItem m_miCopy;
        private System.Windows.Forms.MenuItem m_miSelectAll;

        public ResultsViewer()
        {
            InitializeComponent();

            ReadOnly = true;
        }

        private void InitializeComponent()
        {
            this.m_contextMenu = new System.Windows.Forms.ContextMenu();
            this.m_miCopy = new System.Windows.Forms.MenuItem();
            this.m_miSelectAll = new System.Windows.Forms.MenuItem();

            this.m_contextMenu = new System.Windows.Forms.ContextMenu();
            this.m_contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.m_miCopy, this.m_miSelectAll });
            //this.m_contextMenu.Popup += new System.EventHandler(this.m_contextMenu_Popup);
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

        private void m_m_miSelectAll_Click(object sender, System.EventArgs e)
        {
            this.SelectAll();
        }

        private void m_miCopy_Click(object sender, System.EventArgs e)
        {
            this.Copy();
        }
    }
}