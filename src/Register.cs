﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using WindowsFormsApplication4;

namespace Personal_Diary
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }
        private bool validateInput()
        {
            string msg = "";
            if (firstnameTxtBox.Text.Length < 3)
                msg = "Firstname must be minimum of 3 characters";
            else if (lastnameTxtBox.Text.Length < 3)
                msg = "Lastname must be minimum of 3 characters";
            else if (usernameTxtBox.Text.Length < 5)
                msg = "username must be minimum of 5 characters";
            else if (passwordTxtBox.Text.Length < 4)
                msg = "Password must be minimum of 4 characters";

            Globals.conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = Globals.conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "getUser";
            cmd.Parameters.Add("name", usernameTxtBox.Text);
            cmd.Parameters.Add("user", OracleDbType.Varchar2, 40, null, ParameterDirection.Output);
            cmd.ExecuteNonQuery();

            if (!((OracleString)cmd.Parameters["user"].Value).IsNull)
                msg = "Username Already Exists";
            Globals.conn.Close();

            if (msg != "")
            {
                MessageBox.Show(msg, "Registeration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!validateInput())
                return;
            Globals.conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = Globals.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO DiaryUser VALUES (:username, :password, :firstname, :lastname)";
            cmd.Parameters.Add("username", usernameTxtBox.Text);
            cmd.Parameters.Add("password", passwordTxtBox.Text);
            cmd.Parameters.Add("firstname", firstnameTxtBox.Text);
            cmd.Parameters.Add("lastname", lastnameTxtBox.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Registered Successfully");
            Close();
            Globals.conn.Close();
        }
    }
}