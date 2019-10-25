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
    public partial class DetailReportForm : Form
    {
        DepmanContext ctx = new DepmanContext();
        
        

        public DetailReportForm()
        {
            InitializeComponent();
            
            
        }

        private void DetailReportForm_Load(object sender, EventArgs e)
        {


            IQueryable<Question> soru = ctx.Question;
            dgvReportDetail.DataSource = soru.ToList();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ctx.SaveChanges();
            MessageBox.Show("Kaydedildi");
            SaveAndGetQuestions();
        }
        private void SaveAndGetQuestions(bool save = false)
        {
            if (save) ctx.SaveChanges();
            dgvReportDetail.DataSource = ctx.Question.ToList();
        }
    }
}
