using Depman.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Depman
{
    public partial class MainForm : Form
    {
        TableLayoutPanel activePanel;
        Button activeButton;
        string activeIcon;

        DepmanContext ctx;

        public MainForm()
        {
            InitializeComponent();
            ActivePanel(tlpProjects, btnProjects, "icons8_group_of_projects_25");
            ctx = new DepmanContext();
            
        }

        private void ActivePanel(TableLayoutPanel panel, Button button, string icon)
        {

            // Make unvisible current panel if activePanel is not null
            if (activePanel != null)
            {
                if (panel.Name == activePanel.Name) return; // if clicked button is already clicked then do nothing;
                var img = (Bitmap)Properties.Resources.ResourceManager.GetObject($"{activeIcon}");
                activePanel.Visible = false;
                activeButton.Image = img ?? throw new Exception("image not found"); // throw an exception if image not found
                activeButton.ForeColor = Color.FromArgb(173, 179, 201);
            }
            activePanel = panel; // Change active panel with selected panel
            activeButton = button;
            activeIcon = icon;
            activePanel.Dock = DockStyle.Fill;
            activePanel.Visible = true; // Make visible selected panel
            activeButton.ForeColor = Color.White;
            activeButton.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject($"{icon}_white");
        }

        /**
         * Add placeholder feature to the search textbox by checking value of textbox when entering and leaving from textbox.
         */
        private void MakePlaceholder(TextBox textBox, string placeholder, string eventName)
        {
            // developer needs to input eventName as lowercase
            if (eventName.ToLower() == "enter" && eventName != "enter") throw new Exception("eventName param needs to be lowercase");

            string val = textBox.Text.Trim().ToLower();
            if (eventName == "enter" && val == placeholder.ToLower())
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
                return;
            }
            else if (eventName == "leave" && string.IsNullOrWhiteSpace(val))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = Color.FromArgb(173, 179, 201);
                return;
            }
        }

        private void BtnProjects_Click(object sender, EventArgs e)
        {
            ActivePanel(tlpProjects, btnProjects, "icons8_group_of_projects_25");
        }

        private void BtnDepartments_Click(object sender, EventArgs e)
        {
            ActivePanel(tlpDepartments, btnDepartments, "icons8_organization_chart_people_25");
        }


        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            MakePlaceholder(txtSearch, "Ara", "enter");
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            MakePlaceholder(txtSearch, "Ara", "leave");
        }

        private void TxtAddDeparment_Enter(object sender, EventArgs e)
        {
            MakePlaceholder(txtAddDeparment, "Yeni Birim Ekle (Eklemek için Enter'a basınız)", "enter");
        }

        private void TxtAddDeparment_Leave(object sender, EventArgs e)
        {
            MakePlaceholder(txtAddDeparment, "Yeni Birim Ekle (Eklemek için Enter'a basınız)", "leave");
        }

        private void TxtAddQuestion_Enter(object sender, EventArgs e)
        {
            MakePlaceholder(txtAddQuestion, "Yeni Soru Ekle (Eklemek için Enter'a basınız)", "enter");
        }

        private void TxtAddQuestion_Leave(object sender, EventArgs e)
        {
            MakePlaceholder(txtAddQuestion, "Yeni Soru Ekle (Eklemek için Enter'a basınız)", "leave");
        }

        private void BtnQuestions_Click(object sender, EventArgs e)
        {
            ActivePanel(tlpQuestions, btnQuestions, "icons8_questions_25");
        }

        private void BtnReports_Click(object sender, EventArgs e)
        {
            ActivePanel(tlpReports, btnReports, "icons8_business_report_25");
            GetReports();
        }

        private void GetReports()
        {
            var sorgu = ctx.Report.Join(ctx.Employee, report => report.EmployeeFK, employee => employee.EmployeeID, (report, employee) => new {
                employee.EmployeeID,
                employee.EmployeeFirstName,
                employee.EmployeeLastName,
                employee.EmployeeImgPath,
                employee.Salary,
                employee.HireDate,
                report.Rating,
                report.ReportID,
                report.ReportDescription
            }).ToList();

            flpReports.Controls.Clear();

            foreach (var item in sorgu)
            {
                Panel newPanel = panel5.Clone();
                panel5.Visible = false;

                Button newButton = button13.Clone();
                newButton.FlatAppearance.BorderSize = 0;

                Button newButton2 = button12.Clone();
                
                newButton2.FlatAppearance.BorderSize = 0;
                newButton2.Text = $"{item.EmployeeFirstName} {item.EmployeeLastName}";

                label30.Text = $"{item.HireDate}";
                label28.Text = $"{item.Salary}";
                label26.Text = $"{item.Rating}";

                Button newButton3 = btnDetailReport.Clone();
                newButton3.Click += (senders, events) =>
                {
                    var f = new Report(item.EmployeeFirstName,item.EmployeeLastName,item.ReportDescription);
                    f.FormClosed += (sender, eventh) => GetReports(); // when form closed get new projects;
                    f.Show();
                };
                Label newLabel = label32.Clone();
                Label newLabel1 = label31.Clone();
                Label newLabel2 = label30.Clone();//Tarih-hiredate
                Label newLabel3 = label29.Clone();
                Label newLabel4 = label28.Clone();//maas-salary
                Label newLabel5 = label27.Clone();
                Label newLabel6 = label26.Clone();//puan-rating

                PictureBox newpictureBox = pictureBox2.Clone();



                flpReports.Controls.Add(newPanel);
                newPanel.Controls.Add(newButton);
                newPanel.Controls.Add(newButton2);
                newPanel.Controls.Add(newButton3);
                newPanel.Controls.Add(newLabel);
                newPanel.Controls.Add(newLabel1);
                newPanel.Controls.Add(newLabel2);
                newPanel.Controls.Add(newLabel3);
                newPanel.Controls.Add(newLabel4);
                newPanel.Controls.Add(newLabel5);

                newPanel.Controls.Add(newpictureBox);

                newPanel.Show();
                newButton.Show();
                newButton2.Show();
                newButton3.Show();
                newLabel.Show();
                newLabel1.Show();
                newLabel2.Show();
                newLabel3.Show();
                newLabel4.Show();
                newLabel5.Show();
                newLabel6.Show();
                newpictureBox.Show();
                
            }
        }

        private void BtnEmployees_Click(object sender, EventArgs e)
        {
            ActivePanel(tlpEmployees, btnEmployees, "icons8_people_working_together_25");

        }

        private void BtnAddEmployeeForm_Click(object sender, EventArgs e)
        {
            var f = new AddEmployeeForm();
            f.Show();
        }

        private void TxtAddQuestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ctx.Question.Add(new Question { QuestionTitle = txtAddQuestion.Text });
                ctx.SaveChanges();
                dgvQuestions.DataSource = ctx.Question.ToList();
            }
        }

        private void btnAddProjectForm_Click(object sender, EventArgs e)
        {
            var f = new AddProjectForm();
            f.Show();
        }

        private void btnProjectDetailForm_Click(object sender, EventArgs e)
        {

            var f = new DetailProjectForm();
            f.Show();

            string department = txtAddDeparment.Text.Trim();

            
        }

        private void SaveAndGetDepartments(bool save = false)
        {
            if (save) ctx.SaveChanges();
            dgvDepartments.DataSource = ctx.Department.Select(x => new
            {
                x.DepartmentID,
                x.DepartmentName,
                EmployeesOfDepartment = x.Employees.Count
            }).ToList();
        }

        private void DgvDepartments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvDepartments.SelectedRows.Count > 0)
            {
                long id = (long)dgvDepartments.SelectedRows[0].Cells[0].Value;
                Department department = ctx.Department.Find(id);
                ctx.Department.Remove(department);
                SaveAndGetDepartments(true);
            }
        }

       

        
        private void BtnDetailReport_Click(object sender, EventArgs e)
        {
          
        }
       

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void Button13_Click(object sender, EventArgs e)
        {

        }
    }

    public static class ControlExtensions
    {
        public static T Clone<T>(this T controlToClone)
            where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }

            return instance;

        }
    }
}
