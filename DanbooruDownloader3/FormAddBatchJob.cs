﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Entity;

namespace DanbooruDownloader3
{
    public partial class FormAddBatchJob : Form
    {
        public DanbooruBatchJob Job { get; set; }

        private List<CheckBox> chkList;
        private List<DanbooruProvider> providerList;
        
        public FormAddBatchJob()
        {
            InitializeComponent();
            
            //Auto populate Rating
            cbxRating.DataSource = new BindingSource(Constants.Rating, null);
            cbxRating.DisplayMember = "Key";
            cbxRating.ValueMember = "Value";
            cbxRating.SelectedIndex = 0;

            chkList = new List<CheckBox>();
        }

        private void FillProvider()
        {
            DanbooruProviderDao dao = new DanbooruProviderDao();
            providerList = dao.Read();

            foreach (DanbooruProvider p in providerList)
            {
                var controls = pnlProvider.Controls.Find(p.Name, true);
                if (controls.Length == 0)
                {
                    CheckBox chk = new CheckBox();
                    chk.Name = p.Name;
                    chk.Text = p.Name;
                    chk.AutoSize = true;
                    chkList.Add(chk);
                }
            }

            foreach (CheckBox c in chkList)
            {
                this.pnlProvider.Controls.Add(c);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Job = new DanbooruBatchJob();
            
            try
            {
                if (!string.IsNullOrWhiteSpace(txtLimit.Text)) Job.Limit = Convert.ToInt32(txtLimit.Text);
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error at Limit." + Environment.NewLine + ex.Message);
                txtLimit.Focus();
                txtLimit.SelectAll();
                return;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(txtPage.Text)) Job.Page = Convert.ToInt16(txtPage.Text);
                else Job.Page = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error at Page." + Environment.NewLine + ex.Message);
                txtPage.Focus();
                txtPage.SelectAll();
                return;
            }

            if (cbxRating.SelectedValue != null && chkNotRating.Checked) Job.Rating = "-" + cbxRating.SelectedValue;
            else Job.Rating = (string) cbxRating.SelectedValue;

            Job.TagQuery = txtTagQuery.Text.Replace(" ","_");

            if (string.IsNullOrWhiteSpace(txtSave.Text))
            {
                MessageBox.Show("Save destination is empty!");
                txtSave.Focus();
                return;
            }
            Job.SaveFolder = txtSave.Text;

            Job.ProviderList = new List<DanbooruProvider>();
            bool providerFlag = false;
            foreach (CheckBox c in chkList)
            {
                if (c.Checked)
                {
                    foreach (DanbooruProvider p in providerList)
                    {
                        if(c.Text.Equals(p.Name))
                        {
                            Job.ProviderList.Add(p);
                            providerFlag = true;
                            break;
                        }
                    }
                }
            }
            if (!providerFlag)
            {
                MessageBox.Show("Please select at least 1 provider.");
                pnlProvider.Focus();
                this.DialogResult = DialogResult.None;
                //this.Job = null;
                //this.btnCancel_Click(sender, e);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Job = null;
            //this.Close();
            this.Hide();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (CheckBox chk in chkList)
            {
                chk.Checked = chk.Checked == true ? false : true;
            }
        }

        private void FormAddBatchJob_Load(object sender, EventArgs e)
        {
            FillProvider();
        }

        private void pnlProvider_ControlAdded(object sender, ControlEventArgs e)
        {
            if (txtSave.Top < pnlProvider.Top + pnlProvider.Height)
            {
                this.Height = this.Height + pnlProvider.Top + pnlProvider.Height - txtSave.Top;
            }
        }
    }
}
