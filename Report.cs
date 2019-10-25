using Depman.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Depman
{
    public partial class Report : Form
    {
        DepmanContext ctx = new DepmanContext();
        string EmplooyeFirsName;
        string EmplooyeLastName;
        string ReportDescription;
        public Report(string EmplooyeFirstName,string EmplooyeLastName,string ReportDescription)
        {
            this.EmplooyeFirsName = EmplooyeFirstName;
            this.EmplooyeLastName = EmplooyeLastName;
            this.ReportDescription = ReportDescription;

            InitializeComponent();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Report_Load(object sender, EventArgs e)
        {
            txtReportName.Text = EmplooyeFirsName;
            txtReportLastName.Text = EmplooyeLastName;





            IQueryable<Question> sorgu = ctx.Question;
            dgvReportSoru.AutoGenerateColumns = false;
            dgvReportSoru.DataSource = sorgu.ToList();

            cboDepartment.DataSource = ctx.Department.ToList();
            cboDepartment.DisplayMember = "DepartmentName";
            cboDepartment.ValueMember = "DepartmentID";

            cboEmplooyeFK.DataSource = ctx.Employee.ToList();
            cboEmplooyeFK.DisplayMember = "EmployeeFirstName";
            cboEmplooyeFK.ValueMember = "EmployeeID";
        }

        private void BtnAddProject_Click(object sender, EventArgs e)
        {
           
            try
            {
                MessageBox.Show("Kaydedildi");
            var report = new Models.Report {
                ReportDescription = txtReportDescription.Text,
                EmployeeFK = (long)cboEmplooyeFK.SelectedValue
            };
                ctx.Report.Add(report);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Kayıt işlemi başarısız");
            }


            

           
            ctx.SaveChanges();
        }

        private void BtnGoruntule_Click(object sender, EventArgs e)
        {
            DetailReportForm f = new DetailReportForm();
            f.Show();
        }

        private void TxtRating_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                
                
                ctx.SaveChanges();
            }
        }
    }
}
