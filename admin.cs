﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;



namespace Kindergarten
{
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
        }


        private void getclasses()
        {
            classes.Items.Clear();
            //classes.View = View.List;
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM classes", func.connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                //class1.Add((int)dataReader["classid"], (string)dataReader["name"]);
                //classes.Items.Add(dataReader["name"]);
                ListViewItem item = new ListViewItem();
                item.Tag = dataReader["classid"];
                item.Text = dataReader["name"].ToString();
                classes.Items.Add(item);
            }
            dataReader.Close();
        }

        private void fillteachergrid()
        {
            teachergrid.DataSource = null;
            teachergrid.Rows.Clear();
            teachergrid.Columns.Clear();
            //teachergrid.Refresh();
            using (MySqlDataAdapter a = new MySqlDataAdapter("SELECT accountid,fullname,email,username,classid,level,password FROM accounts where accounttype='teacher'", func.connection))
            {
                DataTable t = new DataTable();
                a.Fill(t);
                teachergrid.DataSource = t;
                teachergrid.Columns[0].Visible = false;
                teachergrid.Columns[6].Visible = false;
            }
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            teachergrid.Columns.Add(btn);
            btn.HeaderText = "Password";
            btn.Text = "Change";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
        }

        private void fillstudentgrid()
        {
            studentgrid.DataSource = null;
            studentgrid.Rows.Clear();
            studentgrid.Columns.Clear();
            using (MySqlDataAdapter b = new MySqlDataAdapter("SELECT accountid,fullname,email,username,birthdate,classid,level,parent_username,password FROM accounts where accounttype='student'", func.connection))
            {
                DataTable t = new DataTable();
                b.Fill(t);
                studentgrid.DataSource = t;
                studentgrid.Columns[0].Visible = false;
                studentgrid.Columns[8].Visible = false;
            }
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            studentgrid.Columns.Add(btn);
            btn.HeaderText = "Password";
            btn.Text = "Change";
            btn.Name = "btn1";
            btn.UseColumnTextForButtonValue = true;
        }

        private void fillparentgrid()
        {
            parentgrid.DataSource = null;
            parentgrid.Rows.Clear();
            parentgrid.Columns.Clear();

            using (MySqlDataAdapter a = new MySqlDataAdapter("SELECT accountid,fullname,email,username,telephone,address,password FROM accounts where accounttype='parent'", func.connection))
            {
                DataTable t = new DataTable();
                a.Fill(t);
                parentgrid.DataSource = t;
                parentgrid.Columns[0].Visible = false;
                parentgrid.Columns[6].Visible = false;
            }
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            parentgrid.Columns.Add(btn);
            btn.HeaderText = "Password";
            btn.Text = "Change";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
        }

        private void admin_Load(object sender, EventArgs e)
        {


            this.listView1.Items[0].Selected = true;

            MySqlCommand cmd = new MySqlCommand("SELECT username FROM accounts where accounttype='parent'", func.connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                comboBox1.Items.Add(dataReader["username"]);
            }
            dataReader.Close();
            comboBox1.SelectedIndex = 0;

            fillteachergrid();
            fillstudentgrid();
            fillparentgrid();        

            getclasses();
        }

        private void about_Click(object sender, EventArgs e)
        {
            func.about(this);
        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Teacher
        private void button1_Click(object sender, EventArgs e)
        {
            fillteachergrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlCommand comm = new MySqlCommand("delete from accounts where accounttype='teacher'", func.connection);
            comm.ExecuteNonQuery();
            DataTable t = new DataTable();
            //a.Fill(t);
            //Object a = teachergrid.Rows[0]["0"];
            for (int i = 0; i < teachergrid.Rows.Count - 1; i++)
            {
                if (string.IsNullOrEmpty(teachergrid.Rows[i].Cells["password"].Value.ToString()))
                    teachergrid.Rows[i].Cells["password"].Value = "12345";

                comm.CommandText = "INSERT INTO accounts (accounttype,accountid, fullname, email, username, password, level,classid) VALUES ('teacher', '"
                 + teachergrid.Rows[i].Cells["accountid"].Value + "','"
                 + teachergrid.Rows[i].Cells["fullname"].Value.ToString() + "','"
                 + teachergrid.Rows[i].Cells["email"].Value.ToString() + "','"
                 + teachergrid.Rows[i].Cells["username"].Value.ToString() + "','"
                 + teachergrid.Rows[i].Cells["password"].Value.ToString() + "','"
                 + teachergrid.Rows[i].Cells["level"].Value + "','"
                 + teachergrid.Rows[i].Cells["classid"].Value + "');";
                comm.ExecuteNonQuery();
            }
        }


        //student
        private void button4_Click(object sender, EventArgs e)
        {
            fillstudentgrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MySqlCommand comm = new MySqlCommand("delete from accounts where accounttype='student'", func.connection);
            comm.ExecuteNonQuery();
            DataTable t = new DataTable();
            DateTime dt = new DateTime();
            for (int i = 0; i < studentgrid.Rows.Count - 1; i++)
            {
               // studentgrid.Columns[5].DefaultCellStyle.Format = "yyyy-mm-dd";
                string tmp;
                tmp = studentgrid.Rows[i].Cells["birthdate"].Value.ToString();
                dt = DateTime.Parse(tmp);
                if (string.IsNullOrEmpty(studentgrid.Rows[i].Cells["password"].Value.ToString()))
                    studentgrid.Rows[i].Cells["password"].Value = "12345";
                comm.CommandText = "INSERT INTO accounts (accounttype,accountid,fullname,email,username,password,birthdate,level,classid,parent_username) VALUES ('student', '"
                 + studentgrid.Rows[i].Cells["accountid"].Value + "','"
                 + studentgrid.Rows[i].Cells["fullname"].Value.ToString() + "','"
                 + studentgrid.Rows[i].Cells["email"].Value.ToString() + "','"
                 + studentgrid.Rows[i].Cells["username"].Value.ToString() + "','"
                 + studentgrid.Rows[i].Cells["password"].Value.ToString() + "','"
                 + String.Format("{0:yyyy-M-dd}", dt) + "','"
                 + studentgrid.Rows[i].Cells["level"].Value + "','"
                 + studentgrid.Rows[i].Cells["classid"].Value + "','"
                 + studentgrid.Rows[i].Cells["parent_username"].Value.ToString() + "');";
                //System.Windows.Forms.MessageBox.Show(comm.CommandText);
                comm.ExecuteNonQuery();
            }
        }


        //parent
        private void button6_Click(object sender, EventArgs e)
        {
            fillparentgrid();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MySqlCommand comm = new MySqlCommand("delete from accounts where accounttype='parent'", func.connection);
            comm.ExecuteNonQuery();
            DataTable t = new DataTable();
            for (int i = 0; i < parentgrid.Rows.Count - 1; i++)
            {
                if (string.IsNullOrEmpty(parentgrid.Rows[i].Cells["password"].Value.ToString()))
                    parentgrid.Rows[i].Cells["password"].Value = "12345";
                comm.CommandText = "INSERT INTO accounts (accounttype,accountid,fullname,email,username,password,telephone,address) VALUES ('parent', '"
                 + parentgrid.Rows[i].Cells["accountid"].Value.ToString() + "','"                 
                 + parentgrid.Rows[i].Cells["fullname"].Value.ToString() + "','"
                 + parentgrid.Rows[i].Cells["email"].Value.ToString() + "','"
                 + parentgrid.Rows[i].Cells["username"].Value.ToString() + "','"
                 + parentgrid.Rows[i].Cells["password"].Value.ToString() + "','"
                 + parentgrid.Rows[i].Cells["telephone"].Value.ToString() + "','"
                 + parentgrid.Rows[i].Cells["address"].Value.ToString() + "');";
                //System.Windows.Forms.MessageBox.Show(comm.CommandText);
                comm.ExecuteNonQuery();
            }
        }

        private void admin_Shown(object sender, EventArgs e)
        {
            func.loginForm.Hide();
        }

        private void admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            func.loginForm.Show();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                this.tabControl1.SelectedIndex = this.listView1.SelectedItems[0].Index;
            }
            fillteachergrid();
            fillstudentgrid();
            fillparentgrid();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!comboBox1.Text.Equals("Empty") && !string.IsNullOrEmpty(comboBox1.Text) && comboBox1.Items.Count > 0)
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO messages (senderaccount, receiveraccount, text, messagedate) VALUES ('"
                + func.userid + "','"
                + comboBox1.SelectedItem.ToString() + "','"
                + richTextBox1.Text.ToString() + "',now())", func.connection);
                cmd.ExecuteNonQuery();
                System.Windows.Forms.MessageBox.Show("Message Send.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO classes (name) values ('" + textBox1.Text + "')",func.connection);
                cmd.ExecuteNonQuery();
                getclasses();
            }
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(classes.FocusedItem.Tag.ToString()))
            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM classes WHERE classid=" + classes.FocusedItem.Tag + "", func.connection);
                cmd.ExecuteNonQuery();
                getclasses();
            }
        }

        private void teachergrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex != teachergrid.Rows.Count -1 )
            {
                //System.Windows.Forms.MessageBox.Show(e.ColumnIndex + " Column " + e.RowIndex + " Row " +
                //"for this row Account id: " + teachergrid.Rows[e.RowIndex].Cells["accountid"].Value.ToString());
                changepass chng = new changepass(teachergrid.Rows[e.RowIndex].Cells["accountid"].Value);
                chng.ShowDialog();
                fillteachergrid();
            }
        }

        private void studentgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9 && e.RowIndex != studentgrid.Rows.Count - 1)
            {
                //System.Windows.Forms.MessageBox.Show(e.ColumnIndex + " Column " + e.RowIndex + " Row " +
                //"for this row Account id: " + studentgrid.Rows[e.RowIndex].Cells["accountid"].Value.ToString());
                changepass chng = new changepass(studentgrid.Rows[e.RowIndex].Cells["accountid"].Value);
                chng.ShowDialog();
                fillstudentgrid();
            }
        }

        private void parentgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex != parentgrid.Rows.Count - 1)
            {
                //System.Windows.Forms.MessageBox.Show(e.ColumnIndex + " Column " + e.RowIndex + " Row " +
                //"for this row Account id: " + studentgrid.Rows[e.RowIndex].Cells["accountid"].Value.ToString());
                changepass chng = new changepass(parentgrid.Rows[e.RowIndex].Cells["accountid"].Value);
                chng.ShowDialog();
                fillparentgrid();
            }
        }
    }
}
