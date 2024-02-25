using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using technosFormApp.classes;

namespace technosFormApp
{
    public partial class Form2 : Form
    {
        SQLConnection conn;
        List<Word> words;
        List<Techno> technos;
        Dictionary<String, int> technoDict = new Dictionary<string, int>();
        public int userId;
        public int wordId;
        public Form2(int nuserId)
        {
            InitializeComponent();
            userId = nuserId;
            conn = new SQLConnection();
            wordId = -1;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            searchComboBox.SelectedItem = "contains";
            refreshData();
            //dataGridView1.Refresh();
            // var x = dataGridView1.Rows;
        }

        private void refreshData()
        {
            technoDict = new Dictionary<string, int>();
            technoComboBox.Items.Clear();
            string wordQuery = $"select w.id, w.word, w.translation, w.techno_id, t.techno_name, t.user_id from words w inner join technos t on w.techno_id=t.id where t.user_id={userId} and t.techno_status=1";
            updateGridWords(wordQuery);
            technos = conn.selectTechnos($"select * from technos where techno_status=1 and user_id={userId}");
            technoComboBox.Items.Add("select techno...");
            technoDict["select techno..."] = -1;
            foreach (Techno techno_ in technos)
            {
                technoDict[techno_.techno_name] = (int)techno_.id;
                technoComboBox.Items.Add(techno_.techno_name);
            }
            technoComboBox.SelectedItem = "select techno...";
        }

        public void updateGridWords(string wordQuery)
        {
            dataGridView1.Rows.Clear();
            words = conn.selectWords(wordQuery);
            foreach (Word word_ in words)
            {
                dataGridView1.Rows.Add(new object[] { word_.word, word_.techno_name, word_.id, word_.translation, word_.techno_id });
            }
            dataGridView1.Refresh();
        }

        private void clearForm()
        {
            wordId = -1;
            wordText.Enabled = true;
            translationText.Enabled = true;
            technoComboBox.Enabled = true;
            wordText.Text = "";
            translationText.Text = "";
            technoComboBox.SelectedItem = "select techno...";
        }

        void edit()
        {
            wordText.Enabled = true;
            translationText.Enabled = true;
            technoComboBox.Enabled = true;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            wordText.Text = dataGridView1.Rows[e.RowIndex].Cells["word"].Value.ToString();
            translationText.Text = dataGridView1.Rows[e.RowIndex].Cells["translation"].Value.ToString();
            technoComboBox.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["techno_name"].Value.ToString();
            wordText.Enabled = false;
            translationText.Enabled = false;
            technoComboBox.Enabled = false;
            wordId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string nword = wordText.Text.Replace("'", "''");
            string ntranslation = translationText.Text.Replace("'", "''");
            int ntechno_id = technoDict[technoComboBox.SelectedItem.ToString()];
            int modifieds = -1;
            if (wordId==-1)
            {
                modifieds = conn.run_command($"insert into words (word, translation, user_id, techno_id) values ('{nword}','{ntranslation}',{userId},{ntechno_id})");
            }
            else
            {
                modifieds = conn.run_command($"update words set word='{nword}', translation='{ntranslation}', techno_id={ntechno_id}, user_id={userId} where id={wordId}");
            }
            clearForm();
            refreshData();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            edit();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (wordId != -1)
            {
                conn.run_command($"delete from words where id={wordId}");
                clearForm();
                refreshData();
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string nword = wordText.Text.Replace("'", "''");
            string ntranslation = translationText.Text.Replace("'", "''");
            int ntechno_id = technoDict[technoComboBox.SelectedItem.ToString()];
            string wordQuery = "";
            if (searchComboBox.SelectedItem == "contains")
            {
                wordQuery = $"select w.id, w.word, w.translation, w.techno_id, t.techno_name, t.user_id from words w inner join technos t on w.techno_id=t.id where t.user_id={userId} and t.techno_status=1 and (w.word like '%{nword}%' or ''='{nword}') and (w.translation like '%{ntranslation}%' or ''='{ntranslation}') and (w.techno_id={ntechno_id} or -1={ntechno_id})";
            }
            else if (searchComboBox.SelectedItem == "equals")
            {
                wordQuery = $"select w.id, w.word, w.translation, w.techno_id, t.techno_name, t.user_id from words w inner join technos t on w.techno_id=t.id where t.user_id={userId} and t.techno_status=1 and (w.word='{nword}' or ''='{nword}') and (w.translation='{ntranslation}' or ''='{ntranslation}') and (w.techno_id={ntechno_id} or -1={ntechno_id})";
            }
            else if (searchComboBox.SelectedItem == "starts with")
            {
                wordQuery = $"select w.id, w.word, w.translation, w.techno_id, t.techno_name, t.user_id from words w inner join technos t on w.techno_id=t.id where t.user_id={userId} and t.techno_status=1 and (w.word like '{nword}%' or ''='{nword}') and (w.translation like '{ntranslation}%' or ''='{ntranslation}') and (w.techno_id={ntechno_id} or -1={ntechno_id})";
            }
            else if (searchComboBox.SelectedItem == "ends with")
            {
                wordQuery = $"select w.id, w.word, w.translation, w.techno_id, t.techno_name, t.user_id from words w inner join technos t on w.techno_id=t.id where t.user_id={userId} and t.techno_status=1 and (w.word like '%{nword}' or ''='{nword}') and (w.translation like '%{ntranslation}' or ''='{ntranslation}') and (w.techno_id={ntechno_id} or -1={ntechno_id})";
            }
            updateGridWords(wordQuery);
        }
    }
}
