﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAPINT;
using SAPINT.Function;
using SAPINT.Function.Meta;
using System.IO;

namespace SAPINT.Gui.Functions
{
    public partial class FormFunctionMetaEx : DockWindow
    {
        string _funcName = "";  //当前的函数名
        private string _systemName;//连接的SAP系统的配置名称
        private FunctionField selectedField = null;
        SAPFunctionEx function = null;
        public FormFunctionMetaEx()
        {
            InitializeComponent();
            new DgvFilterPopup.DgvFilterManager(this.dgvDetail);
            new DgvFilterPopup.DgvFilterManager(this.dgvTableContent);
            this.cbx_SystemList.DataSource = ConfigFileTool.SAPGlobalSettings.GetSAPClientList();
            this.cbx_SystemList.Text = ConfigFileTool.SAPGlobalSettings.GetDefaultSapCient();

            CDataGridViewUtils.CopyPasteDataGridView(this.dgvTableContent);
            CDataGridViewUtils.CopyPasteDataGridView(this.dgvImport);
            CDataGridViewUtils.CopyPasteDataGridView(this.dgvExport);
            CDataGridViewUtils.CopyPasteDataGridView(this.dgvChanging);
            CDataGridViewUtils.CopyPasteDataGridView(this.dgvException);
            CDataGridViewUtils.CopyPasteDataGridView(this.dgvTables);
            CDataGridViewUtils.CopyPasteDataGridView(this.dgvDetail);
        }

        /// <summary>
        /// //从服务器加载函数的具体信息,包括每个参数的名称，数据类型
        /// 如果是结构体，还显示它的字段列表。
        /// </summary>
        private void LoadFunctionMetaData()
        {
            if (!check())
            {
                return;
            }
            else
            {
                try
                {
                    function = new SAPFunctionEx(_systemName, _funcName);
                    if (function.FunctionMeta == null)
                    {
                        MessageBox.Show("无法找到函数信息！！");
                        return;
                    }
                    ParseMetaData(function);
                    if (function.Is_rfc == true)
                    {
                        this.button2.Enabled = true;
                        this.button2.Text = "RFC函数，可执行";
                    }
                    else
                    {
                        this.button2.Text = "非RFC函数，不可执行";
                        this.button2.Enabled = false;
                    }

                    this.Text = "函数:" + _funcName;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            CleanAll();
            LoadFunctionMetaData();
        }
        private bool check()
        {
            this._systemName = this.cbx_SystemList.Text.ToUpper().Trim();
            this._funcName = this.txtFunctionName.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(_systemName))
            {
                MessageBox.Show("请选择系统");
                return false;
            }
            if (string.IsNullOrEmpty(this._funcName))
            {
                MessageBox.Show("请指定表名");
                return false;
            }
            return true;
        }
        private void CleanAll()
        {
            dgvImport.DataSource = null;
            dgvExport.DataSource = null;
            dgvChanging.DataSource = null;
            dgvTables.DataSource = null;
            dgvException.DataSource = null;
        }
        private void ParseMetaData(SAPFunctionEx function)
        {
            dgvImport.DataSource = function.FunctionMeta.Import;
            dgvExport.DataSource = function.FunctionMeta.Export;
            dgvChanging.DataSource = function.FunctionMeta.Changing;
            dgvTables.DataSource = function.FunctionMeta.Tables;
            dgvException.DataSource = function.FunctionMeta.Exception;

            dgvImport.AutoResizeColumns();
            dgvExport.AutoResizeColumns();
            dgvChanging.AutoResizeColumns();
            dgvTables.AutoResizeColumns();
            tabPage2.BringToFront();
        }
        /// <summary>
        /// //选择字段时，显示它们的具体信息
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="e"></param>
        private void SetDgvSource(ref DataGridView dgv, DataGridViewCellEventArgs e)
        {
            dgvDetail.DataSource = null;
            dgvTableContent.DataSource = null;
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            String name = dgv.Rows[e.RowIndex].Cells[FuncFieldText.NAME].Value.ToString();
            String dataType = dgv.Rows[e.RowIndex].Cells[FuncFieldText.DATATYPE].Value.ToString();
            String dataTypeName = dgv.Rows[e.RowIndex].Cells[FuncFieldText.DATATYPENAME].Value.ToString();
            String defaultValue = dgv.Rows[e.RowIndex].Cells[FuncFieldText.DEFAULTVALUE].Value.ToString();
            selectedField = new FunctionField(name, dataType, dataTypeName, defaultValue);
            if (String.IsNullOrEmpty(selectedField.Name))
            {
                MessageBox.Show("点击选择参数");
                return;
            }
            if (dataType == SAPDataType.STRUCTURE.ToString() || dataType == SAPDataType.TABLE.ToString())
            {
                if (!function.FunctionMeta.StructureDetail.Keys.Contains(dataTypeName))
                {
                    return;
                }
                else
                {
                    DataTable dt = function.FunctionMeta.StructureDetail[dataTypeName];
                    dgvDetail.DataSource = dt;
                    dgvDetail.AutoResizeColumns();
                    if (function.TableValueList.Keys.Contains(selectedField.Name))
                    {
                        DataTable dtResult = function.TableValueList[selectedField.Name];
                        if (dtResult != null)
                        {
                            dgvTableContent.DataSource = dtResult;
                            dgvTableContent.AutoResizeColumns();
                        }
                    }
                }

            }
        }
        private void dgvExport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetDgvSource(ref dgvExport, e);
        }
        private void dgvChanging_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetDgvSource(ref dgvChanging, e);
        }
        private void dgvTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetDgvSource(ref dgvTables, e);
        }
        private void dgvImport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetDgvSource(ref dgvImport, e);
        }
        //填充结构或表数据
        private void InputSomethingIntoTable()
        {
            if (selectedField == null)
            {
                return;
            }
            if (String.IsNullOrEmpty(selectedField.Name))
            {
                return;
            }
            DataTable dtInput = null;
            if (function.TableValueList.Keys.Contains(selectedField.Name))
            {
                dtInput = function.TableValueList[selectedField.Name];
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(selectedField.DataTypeName))
                {
                    dtInput = function.TableValueList[selectedField.DataTypeName];
                }
            }
            if (dtInput == null)
            {
                MessageBox.Show("无法创建数据输入视图!");
                return;
            }
            FormTableInput formInput = new FormTableInput();
            if (selectedField.DataType == SAPDataType.STRUCTURE.ToString())
            {
                formInput.DataType = SAPDataType.STRUCTURE.ToString();
            }
            else if (selectedField.DataType == SAPDataType.TABLE.ToString())
            {
                formInput.DataType = SAPDataType.TABLE.ToString();
            }
            formInput.DgvSource = dtInput;
            formInput.InitializeDataSource();
            formInput.ShowDialog();
            function.TableValueList[selectedField.Name] = formInput.DgvSource;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            InputSomethingIntoTable();
        }
        private void doSomethingWhenSuccess()
        {
            //根据返回的结果处理控件
            foreach (var item in function.TableValueList)
            {
                //if (item.Value.Rows.Count > 0)
                //{
                foreach (DataGridViewRow row in dgvTables.Rows)
                {
                    if (row.Cells[FuncFieldText.NAME].Value.ToString() == item.Key)
                    {
                        if (item.Value.Rows.Count > 0)
                        {
                            row.Cells[FuncFieldText.DEFAULTVALUE].Style.BackColor = Color.Green;
                            row.Cells[FuncFieldText.DEFAULTVALUE].Value = "一共有" + item.Value.Rows.Count + "行数据";
                        }
                        else
                        {
                            row.Cells[FuncFieldText.DEFAULTVALUE].Style.BackColor = Color.Transparent;
                            row.Cells[FuncFieldText.DEFAULTVALUE].Value = string.Empty;
                        }
                        break;
                    }
                }

                foreach (DataGridViewRow row in this.dgvExport.Rows)
                {
                    if (row.Cells[FuncFieldText.NAME].Value.ToString() == item.Key)
                    {
                        if (item.Value.Rows.Count > 0)
                        {
                            row.Cells[FuncFieldText.DEFAULTVALUE].Style.BackColor = Color.Green;
                            row.Cells[FuncFieldText.DEFAULTVALUE].Value = "一共有" + item.Value.Rows.Count + "行数据";
                        }
                        else
                        {
                            row.Cells[FuncFieldText.DEFAULTVALUE].Style.BackColor = Color.Transparent;
                            row.Cells[FuncFieldText.DEFAULTVALUE].Value = string.Empty;
                        }
                        break;
                    }
                }
                foreach (DataGridViewRow row in this.dgvChanging.Rows)
                {
                    if (row.Cells[FuncFieldText.NAME].Value.ToString() == item.Key)
                    {
                        if (item.Value.Rows.Count > 0)
                        {
                            row.Cells[FuncFieldText.DEFAULTVALUE].Style.BackColor = Color.Green;
                            row.Cells[FuncFieldText.DEFAULTVALUE].Value = "一共有" + item.Value.Rows.Count + "行数据";
                        }
                        else
                        {
                            row.Cells[FuncFieldText.DEFAULTVALUE].Style.BackColor = Color.Transparent;
                            row.Cells[FuncFieldText.DEFAULTVALUE].Value = string.Empty;
                        }
                        break;
                    }
                }
                //}
            }
        }
        /// <summary>
        /// 执行函数
        /// </summary>
        private void ExcuteFunction()
        {
            if (function.FunctionMeta == null)
            {
                MessageBox.Show("请先获取函数信息！！");
                return;
            }
            try
            {
                function.Excute();
                doSomethingWhenSuccess();
                MessageBox.Show("成功调用！！！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ExcuteFunction();
        }

        private void btnSaveToDb_Click(object sender, EventArgs e)
        {
            if (this.selectedField == null)
            {
                return;
            }
            if (dgvTableContent.DataSource != null)
            {
                var dt = dgvTableContent.DataSource as DataTable;
                if (dt != null)
                {
                    SAPINT.Gui.DataBase.FormSaveDataTable formSaveDt = new DataBase.FormSaveDataTable();
                    formSaveDt.Dt = dt;
                    formSaveDt.SapSystemName = this._systemName;
                    formSaveDt.SapTableName = selectedField.Name;
                    formSaveDt.SapStrutureName = selectedField.DataTypeName;

                    formSaveDt.Show();
                }
            }

        }

        private void btnDisplay1_Click(object sender, EventArgs e)
        {

        }

        private void loadDataToColumnOne()
        {
            try
            {
                var dt = dgvTableContent.DataSource as DataTable;
                if (dt == null)
                {
                    MessageBox.Show("请先选择表");
                    return;
                }
                else if (dt.Columns.Count == 0)
                {
                    MessageBox.Show("DT没有列");
                    return;
                }
                OpenFileDialog openFile = new OpenFileDialog();

                openFile.InitialDirectory = Application.ExecutablePath;
                openFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFile.FilterIndex = 1;
                openFile.RestoreDirectory = true;
                openFile.Multiselect = false;

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    string fullName = openFile.FileName;


                    FileInfo filein = new FileInfo(fullName);
                    if (!filein.Exists)
                    {
                        MessageBox.Show("没有文件");
                        return;
                    }
                    // FileStream file = File.OpenRead(fullName);

                    //StreamReader sr = new StreamReader(fullName, Encoding.Default);
                    //String s = sr.ReadToEnd();
                    //string[] xx = s.Split(new string[] { "\r\n" }, StringSplitOptions.None);


                    //foreach (var item in xx)
                    //{
                    //    dt.Rows.Add(item);

                    //}
                    StreamReader sr = new StreamReader(fullName, Encoding.Default);
                    string strLine = null;
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        dt.Rows.Add(strLine);
                        //MessageBox.Show(strLine);
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnLoadFile_Click(object sender, EventArgs e)
        {

            loadDataToColumnOne();
        }

    }
}
