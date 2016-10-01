// Authors: Dominik Ramsaier, Andreas Schwyrz
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Text;

namespace OSV_Import_Digest_Tool
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        DataTable dt1 = new DataTable();
        bool is_minimized = true;
        int datagrid_width_max = 0;
        int datagrid_width_min = 0;
        int minimized_form_amount = 292;
        String standort = "Standort";
        String rufnummer = "Rufnummer";
        String kuerzel = "Kürzel";
        String strasse = "Straße";
        String emea = "EM EA";
        String command_mand = "Command Mand.";
        String dls_mand = "DLS Mand.";
        String onsite_serv = "Onsite Service";
        String realm = "Realm";
        String passwort = "Passwort";
        String validcharacters_password_numbers = "1234567890";
        String validcharacters_password_letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        String validcharacters_password_special = "#+'-_.,:;><§$%&()=?~€{[]}/";
        String std_valid_char_passw = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Int16 std_count_passw = 20;
        Random rnd = new Random();
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            this.setup_datatable();

            TransferCSVToTable(this.dt1, "digesttool.csv");
            this.dataGridView1.DataSource = this.dt1;
            dataGridView1.AutoResizeColumn(0);//standort
            dataGridView1.AutoResizeColumn(1);//rufnummer
            dataGridView1.AutoResizeColumn(2);//
            dataGridView1.AutoResizeColumn(3);//straße
            dataGridView1.AutoResizeColumn(4);//emea
            dataGridView1.AutoResizeColumn(5);//command mandant
            dataGridView1.AutoResizeColumn(6);// dls mandant
            dataGridView1.AutoResizeColumn(7);//onsite service
            dataGridView1.Columns[8].Width = 30;	//leer	
            dataGridView1.Columns[9].Visible = false;//.Width = 0;
            dataGridView1.Columns[10].Visible = false;//.Width = 0;
            for (int i = 5; i < 8; i++)
            {
                this.datagrid_width_max = this.datagrid_width_max + dataGridView1.Columns[i].Width;
            }
            this.datagrid_width_max += 10;
            for (int i = 0; i < 5; i++)
            {
                datagrid_width_min = datagrid_width_min + dataGridView1.Columns[i].Width;
            }
            datagrid_width_min = datagrid_width_min + 209;
            this.Width = datagrid_width_min;
            this.button7.Visible = false;

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
        public void TransferCSVToTable(DataTable dt, string filePath)
        {
            string[] csvRows = System.IO.File.ReadAllLines(filePath, System.Text.Encoding.Default);
            string[] fields = null;
            foreach (string csvRow in csvRows)
            {
                fields = csvRow.Split(';');
                DataRow row = dt.NewRow();
                row.ItemArray = fields;
                dt.Rows.Add(row);
            }
        }
        public void setup_datatable()
        {
            dt1.Columns.Add(this.standort, System.Type.GetType("System.String"));
            dt1.Columns.Add(this.rufnummer, System.Type.GetType("System.String"));
            dt1.Columns.Add(this.kuerzel, System.Type.GetType("System.String"));
            dt1.Columns.Add(this.strasse, System.Type.GetType("System.String"));
            dt1.Columns.Add(this.emea, System.Type.GetType("System.String"));
            dt1.Columns.Add(this.command_mand, System.Type.GetType("System.String"));
            dt1.Columns.Add(this.dls_mand, System.Type.GetType("System.String"));
            dt1.Columns.Add(this.onsite_serv, System.Type.GetType("System.String"));
            dt1.Columns.Add(" ", System.Type.GetType("System.String"));//Füllspalte am rechten Rand ohne Funktion
            dt1.Columns.Add(this.realm, System.Type.GetType("System.String"));
            dt1.Columns.Add(this.passwort, System.Type.GetType("System.String"));
        }
        private void btnSuchen_Click(object sender, EventArgs e)
        {
            this.start_search();
        }
        public void start_search()
        {
            string expression = " LIKE " + "'*" + this.textBox1.Text + "*'"; ;
            expression = expression = this.standort + expression + " or " + this.rufnummer + expression + " or " + this.kuerzel + expression + " or " + this.strasse + expression;
            this.dt1.DefaultView.RowFilter = expression;
        }
        private void btnZufall_Click(object sender, EventArgs e)
        {
            String validcharacters_password = "";
            Int16 laenge = 0;
            StringBuilder res = new StringBuilder();
            if ((this.checkBox4.Visible))
            {
                laenge = Convert.ToInt16(this.numericUpDown1.Value);
                if (this.checkBox4.Checked)
                {
                    validcharacters_password = validcharacters_password + this.validcharacters_password_numbers;
                }
                if (this.checkBox2.Checked)
                {
                    validcharacters_password = validcharacters_password + this.validcharacters_password_letters;
                }
                if (this.checkBox3.Checked)
                {
                    validcharacters_password = validcharacters_password + this.validcharacters_password_special;
                }
                if (validcharacters_password == "")
                {
                    validcharacters_password = "1234567890";
                    this.checkBox4.Checked = true;
                }
                while (0 < laenge--)
                {
                    res.Append(validcharacters_password[this.rnd.Next(validcharacters_password.Length)]);
                }
                Clipboard.SetDataObject(res.ToString());
            }
            else
            {
                laenge = this.std_count_passw;
                while (0 < laenge--)
                {
                    res.Append(std_valid_char_passw[this.rnd.Next(this.std_valid_char_passw.Length)]);
                }
                Clipboard.SetDataObject(res.ToString());
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.start_search();
            }
            if (this.textBox1.Text == "")
            {
                this.textbox_chgd_rstt();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.label2.Focus();
            this.textBox1.Text = "";
            this.start_search();
            this.textbox_chgd_rstt();
            this.textBox1.Select();
        }
        private void textbox_chgd_rstt()
        {
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1];
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.SelectedRows[0].Index;
            dataGridView1.ClearSelection();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.radRealm.Checked)
                {
                    Clipboard.SetDataObject(((DataRowView)this.dataGridView1.SelectedRows[0].DataBoundItem).Row["Realm"].ToString());
                }
                if (this.radPasswort.Checked)
                {
                    Clipboard.SetDataObject(((DataRowView)this.dataGridView1.SelectedRows[0].DataBoundItem).Row["Passwort"].ToString());
                }
            }
            catch
            {
            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox5.Checked)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.checkBox4.Visible)
            {
                this.checkBox4.Visible = false;
                this.checkBox3.Visible = false;
                this.checkBox2.Visible = false;
                this.numericUpDown1.Visible = false;
                this.label2.Visible = false;
            }
            else
            {
                this.checkBox4.Visible = true;
                this.checkBox3.Visible = true;
                this.checkBox2.Visible = true;
                this.numericUpDown1.Visible = true;
                this.label2.Visible = true;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.is_minimized)
            {
                this.button7.Visible = true;

                this.Width = this.datagrid_width_min + this.datagrid_width_max;
                this.is_minimized = false;
                this.button4.Text = "Kompakt";
            }
            else
            {
                this.button7.Visible = false;
                this.Width = datagrid_width_min;
                this.is_minimized = true;
                this.button4.Text = "Erweitert";
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(((DataRowView)this.dataGridView1.SelectedRows[0].DataBoundItem).Row["Onsite Service"].ToString());
            }
            catch
            {
            }
        }
        void BeendenToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        void BeendenToolStripMenuItem1Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
