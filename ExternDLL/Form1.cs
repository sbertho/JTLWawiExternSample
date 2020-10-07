using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExternDLL
{
    public partial class Form1 : Form
    {
        private Worker _worker;

        public Form1()
        {
            InitializeComponent();
            this._worker = new Worker();
            this.txtDatabase.Text = "eazybusiness";
            this.txtServer.Text = "JTL-SBERTHO";
            this.txtUsername.Text = "sa";
            this.txtPassword.Text = "jtlgmbh";
        }


        private void btnOpenDialog_Click(object sender, EventArgs e)
        {
            this.txtXmlFilePath.Text = this._worker.GetXmlFilePath();
        }



        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            this._worker.TestConnection(this.txtServer.Text, this.txtDatabase.Text, this.txtUsername.Text, this.txtPassword.Text);
            TestAdapter.Instance.Reload(this.txtServer.Text, this.txtDatabase.Text, this.txtUsername.Text, this.txtPassword.Text);
            this.comboBox1.DataSource = TestAdapter.Instance.AlleBenutzer;
            this.comboBox1.DisplayMember = "cName";
            this.comboBox1.ValueMember = "kBenutzer";

            this.comboBox2.DataSource = TestAdapter.Instance.AlleArtikel;
            this.comboBox2.DisplayMember = "cName";
            this.comboBox2.ValueMember = "kArtikel";

            this.comboBox3.DataSource = TestAdapter.Instance.AlleWarenlagerplätze;
            this.comboBox3.DisplayMember = "cName";
            this.comboBox3.ValueMember = "kWarenlagerplatz";
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            //todo
            Worker import = new Worker();
            import.Import(this.txtServer.Text, this.txtDatabase.Text, this.txtUsername.Text, this.txtPassword.Text, 1, this.txtXmlFilePath.Text);
        }

        private void btnExecuteWorkflow_Click(object sender, EventArgs e)
        {
            //todo
            this._worker.WorkFlow(this.txtServer.Text, this.txtDatabase.Text, this.txtUsername.Text, this.txtPassword.Text, Convert.ToInt32(this.txtUserId.Text), 
                Convert.ToInt32(this.txtKey.Text), Convert.ToInt32(this.txtEvendId.Text));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var menge = this.numericUpDown1.Value;
            if (menge == 0)
            {
                return;
            }

            this._worker.Warenbuchung(this.txtServer.Text, this.txtDatabase.Text, this.txtUsername.Text, this.txtPassword.Text, (int)this.comboBox1.SelectedValue, (int)this.comboBox2.SelectedValue, (int)this.comboBox3.SelectedValue, (double)this.numericUpDown1.Value);
        }
    }
}
