using System;
using System.Data;
using System.Windows.Forms;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestEngineer
{
    public partial class TestPropertiesEditorDialog : Form
    {
        #region Data members

        TestPropertyCollection m_testProperties;
        public TestPropertyCollection TestProperties
        { get { return m_testProperties; } }

        #endregion
        public TestPropertiesEditorDialog(TestPropertyCollection testProperties)
        {
            m_testProperties = testProperties;

            InitializeComponent();

            InitializeDataSet();
        }

        #region Private methods


        private void InitializeDataSet()
        {
            // Create a DataSet with two tables and one relation.
            CreateDataSet();

            // Populate initial table.
            populateDataTable();

            // Bind the DataGrid to the DataSet.
            m_dataGrid.SetDataBinding(m_dataSet, "TestProperties");
        }

        private DataTable m_dataTable;
        private DataSet m_dataSet;

        private void populateDataTable()
        {
            //this.m_dataTable.Clear();

            //foreach (KeyValuePair<string, object> keyValue in m_testProperties)
            //{
            //    DataRow dataRow = m_dataTable.NewRow();
            //    dataRow["Key"] = keyValue.Key;
            //    dataRow["Value"] = keyValue.Value;
            //    m_dataTable.Rows.Add(dataRow);
            //}
        }

        // Create a DataSet with two tables and populate it.
        private void CreateDataSet()
        {
            // Create a DataSet.
            m_dataSet = new DataSet("m_dataSet");

            // Create TestProperties table.
            m_dataTable = new DataTable("TestProperties");
            DataColumn keyCol = new DataColumn("Key");
            DataColumn valCol = new DataColumn("Value");
            valCol.DataType = typeof(string);

            m_dataTable.Columns.Add(keyCol);
            m_dataTable.Columns.Add(valCol);
            m_dataSet.Tables.Add(m_dataTable);
        }

        #region Event handlers

        private void m_btnReset_Click(object sender, EventArgs e)
        {
            // Repopulate with original values.
            populateDataTable();
        }

        private void m_btnOK_Click(object sender, EventArgs e)
        {
            //TestPropertyCollection properties = new TestPropertyCollection();
            m_testProperties = new TestPropertyCollection();

            foreach (DataRow row in m_dataTable.Rows)
            {
                //m_testProperties.Add((string)row[0], row[1]);
            }
        }

        private void m_btnCancel_Click(object sender, EventArgs e)
        {
            // Do nothing
        }

        #endregion

        #endregion
    }
}
