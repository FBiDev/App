﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using App.Core.Properties;

namespace App.Core.Desktop
{
    public partial class DebugForm : BaseForm2
    {
        private bool enableFormLevelDoubleBuffering = true;
        private int originalExStyle = -1;

        public DebugForm()
        {
            InitializeComponent();

            Icon = Resources.ico_debug;

            Load += Form_Load;
            Shown += Form_Shown;
            HandleCreated += (sender, e) =>
            {
                if (DesignMode)
                {
                    return;
                }

                ResizeBegin += (s, ev) => { TurnOffFormLevelDoubleBuffering(); };
                ResizeEnd += (s, ev) => { TurnOnFormLevelDoubleBuffering(); };
            };

            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            dgvSQLSistema.Visible = false;
            dgvSQLSistema.RowsAdded += DataGridView_RowsAdded;
            dgvSQLSistema.MouseDown += (sender, e) => dgvSQLSistema.ShowContextMenu(e, mnuSqlSystem);

            mniSqlSystemCopyCommand.MouseDown += MniSqlSystemCopyCommand_MouseDown;
            mniSqlSystemCopyLog.MouseDown += MniSqlSystemCopyLog_MouseDown;

            dgvSQLBase.Visible = false;
            dgvSQLBase.RowsAdded += DataGridView_RowsAdded;
            dgvSQLBase.MouseDown += (sender, e) => dgvSQLBase.ShowContextMenu(e, mnuSqlBase);

            mniSqlBaseCopyCommand.MouseDown += MniSqlBaseCopyCommand_MouseDown;
            mniSqlBaseCopyLog.MouseDown += MniSqlBaseCopyLog_MouseDown;
        }

        #region Fix_Flickering_Controls
        protected override CreateParams CreateParams
        {
            get
            {
                if (originalExStyle == -1)
                {
                    originalExStyle = base.CreateParams.ExStyle;
                }

                CreateParams cp = base.CreateParams;
                if (enableFormLevelDoubleBuffering && DesignMode == false)
                {
                    cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                }
                else
                {
                    cp.ExStyle = originalExStyle;
                }

                return cp;
            }
        }
        #endregion

        public void TabSelectIndex(int index)
        {
            tabControl.SelectedIndex = index;
        }

        public void UpdateErrors()
        {
            UpdateErrorTitle();
            var errors = DebugManager.GetErrors();

            txtErrors.Text = string.Empty;
            foreach (KeyValuePair<string, int> item in errors)
            {
                txtErrors.Text += "[" + item.Value + "x] " + item.Key;
            }
        }

        public void UpdateErrorTitle()
        {
            var errors = DebugManager.GetErrors();
            string title = errors.Count + " Error";

            if (errors.Count == 0 || errors.Count > 1)
            {
                title += "s";
            }

            tabError.Text = title;
        }

        public void UpdateMessages()
        {
            UpdateMessageTitle();
            txtMessages.Text = DebugManager.GetMessages();
        }

        public void UpdateMessageTitle()
        {
            var messages = DebugManager.GetMessages();
            int total = messages.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Length - 1;
            string title = total + " Message";

            if (total == 0 || total > 1)
            {
                title += "s";
            }

            tabMessage.Text = title;
        }

        private void TurnOnFormLevelDoubleBuffering()
        {
            enableFormLevelDoubleBuffering = true;
            UpdateStyles();
        }

        private void TurnOffFormLevelDoubleBuffering()
        {
            enableFormLevelDoubleBuffering = false;
            UpdateStyles();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // TopMost = true;
            Top = Screen.PrimaryScreen.WorkingArea.Height - Height;
            Left = 0;

            dgvSQLSistema.AutoGenerateColumns = false;
            dgvSQLSistema.AddColumn<int>("Index", "Linha", align: ColumnAlign.Center, width: 64);
            dgvSQLSistema.AddColumn<DateTime>("Date", "Data", align: ColumnAlign.Center, width: 85);
            dgvSQLSistema.AddColumn<string>("Method", "Função");
            dgvSQLSistema.AddColumn<string>("Action", "Ação", width: 60);
            dgvSQLSistema.AddColumn<string>("Command", "Comando");
            dgvSQLSistema.Columns["Command"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSQLSistema.DataSource = DebugManager.LogSQLSistema;

            dgvSQLBase.AutoGenerateColumns = false;
            dgvSQLBase.AddColumn<int>("Index", "Linha", align: ColumnAlign.Center, width: 64);
            dgvSQLBase.AddColumn<DateTime>("Date", "Data", align: ColumnAlign.Center, width: 85);
            dgvSQLBase.AddColumn<string>("Method", "Função");
            dgvSQLBase.AddColumn<string>("Action", "Ação", width: 60);
            dgvSQLBase.AddColumn<string>("Command", "Comando");
            dgvSQLBase.Columns["Command"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSQLBase.DataSource = DebugManager.LogSQLBase;

            UpdateErrors();
            UpdateMessages();
        }

        private void Form_Shown(object sender, EventArgs e)
        {
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabSQLSistema)
            {
                dgvSQLSistema.Focus();
            }
            else if (tabControl.SelectedTab == tabSQLBase)
            {
                dgvSQLBase.Focus();
            }
        }

        private void DataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ((DataGridView)sender).Visible = true;
            if (((DataGridView)sender) == dgvSQLSistema)
            {
                lblDgv1NoRows.Visible = false;
            }
            else
            {
                lblDgv2NoRows.Visible = false;
            }

            if (((DataGridView)sender).RowCount > 0)
            {
                ((DataGridView)sender).FirstDisplayedScrollingRowIndex = 0;
            }
        }

        private void MniSqlSystemCopyCommand_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var log = dgvSQLSistema.GetCurrentRowObject<SqlLog>();
            ClipboardSafe.SetText(log.Command + Environment.NewLine);
        }

        private void MniSqlBaseCopyCommand_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var log = dgvSQLBase.GetCurrentRowObject<SqlLog>();
            ClipboardSafe.SetText(log.Command + Environment.NewLine);
        }

        private void MniSqlSystemCopyLog_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var log = dgvSQLSistema.GetCurrentRowValue(true);
            ClipboardSafe.SetText(log);
        }

        private void MniSqlBaseCopyLog_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var log = dgvSQLBase.GetCurrentRowValue(true);
            ClipboardSafe.SetText(log);
        }

        private void GarbageCollectOnClick(object sender, EventArgs e)
        {
            AppManager.CollectGarbage();
        }
    }
}