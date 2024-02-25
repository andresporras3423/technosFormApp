using Microsoft.VisualBasic;
using System.Data.SqlClient;
using technosFormApp.classes;

namespace technosFormApp
{
    public partial class Form1 : Form
    {
        SQLConnection conn;
        public Form1()
        {
            conn = new SQLConnection();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = userText.Text;
            string pass = passText.Text;
            List<User> user = conn.selectUsers($"select * from users where email='{email}'");
            if(user.Count == 0)
            {
                MessageBox.Show("email not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                bool correctPass =Session.VerifyPassword(pass, user[0].password);
                if (correctPass)
                {
                    this.Hide();
                    Form2 form2_ = new Form2((int)user[0].id);
                    form2_.FormClosed += Form2_FormClosed; // Subscribe to the FormClosed event
                    form2_.Show();
                }
                else
                {
                    MessageBox.Show("incorrect password", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // This method is the event handler for Form2's FormClosed event
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}