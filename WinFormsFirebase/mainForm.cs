using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace WinFormsFirebase
{
    public partial class mainForm : Form
    {
        DataTable dataTable = new DataTable();
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret= "0vHkKXJUSytcCXiHTBmPTBO49HWCxKcNqkVlXE6t",
            BasePath= "https://fir-csharp-f4422.firebaseio.com/"
        };
        public mainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            if (client!=null)
            {
                MessageBox.Show("Conexão estabelecida com o servidor!");
            }

            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Micro");
            dataTable.Columns.Add("Status");

            gridView.DataSource = dataTable;

        }
        private async void BtnInsert_Click(object sender, EventArgs e)
        {

            FirebaseResponse resp = await client.GetTaskAsync("Counter/node");
            Counter_Class get = resp.ResultAs<Counter_Class>();

            MessageBox.Show(get.cnt);

            var data = new Data
            {
                ID = Convert.ToInt32(get.cnt)+1,
                MicroName = txtMicroName.Text,
                Status = txtStatus.Text,
                Command = txtCommand.Text,
                Complement = txtComplement.Text
            };

            SetResponse response = await client.SetTaskAsync("Micro/" + data.ID, data);
            Data result = response.ResultAs<Data>();

            var obj = new Counter_Class
            {
                cnt = data.ID.ToString()
            };

            SetResponse response1 = await client.SetTaskAsync("Counter/node", obj);
            MessageBox.Show("Inserido " + result.MicroName + " com sucesso!");
        }
        private async void BtnGet_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetTaskAsync("Micro/" + txtMicroName.Text);
            Data obj = response.ResultAs<Data>();
            txtMicroName.Text = obj.MicroName;
            txtStatus.Text = obj.Status;
            txtCommand.Text = obj.Complement;
            txtComplement.Text = obj.Complement;


            MessageBox.Show("Success!");
        }
        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                MicroName = txtMicroName.Text,
                Status = txtStatus.Text,
                Command = txtCommand.Text,
                Complement = txtComplement.Text
            };

            FirebaseResponse response = await client.UpdateTaskAsync("Micro/" + txtMicroName.Text, data);
            Data result = response.ResultAs<Data>();
            MessageBox.Show("Dados atualizados via ID: "+result.MicroName);
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Micro/" + txtMicroName.Text);
            MessageBox.Show("Deletado com sucesso!");
        }

        private async void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Micro");
            MessageBox.Show("Todos Deletados com sucesso!");
        }

        private void BntGridView_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        async void ExportData()
        {
            int i = 0;
            FirebaseResponse resp1 = await client.GetTaskAsync("Counter/node");
            Counter_Class counter = resp1.ResultAs<Counter_Class>();
            int cnt = Convert.ToInt32(counter.cnt);

            while (true)
            {
                if (i == cnt)
                {
                    break;
                }
                i++;
                try
                {
                    FirebaseResponse resp2 = await client.GetTaskAsync("Micro/"+i);
                    Data obj = resp2.ResultAs<Data>();

                    DataRow row = dataTable.NewRow();
                    row["ID"] = obj.ID;
                    row["Micro"] = obj.MicroName;
                    row["Status"] = obj.Status;

                    dataTable.Rows.Add(row);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
