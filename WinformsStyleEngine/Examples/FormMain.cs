using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinformsStyleEngine;

namespace Examples
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnShowChildForm_Click(object sender, EventArgs e)
        {
            Form childForm = new FormChild();
            Program.StyleEngine.ApplyStyle(childForm);
            childForm.StartPosition = FormStartPosition.CenterScreen;
            childForm.Show();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
