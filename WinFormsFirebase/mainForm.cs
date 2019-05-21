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
        }

        private async void BtnInsert_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                ID = txtID.Text,
                Name = txtName.Text,
                Email = txtEmail.Text
            };

            SetResponse response = await client.SetTaskAsync("Information/" + txtID.Text, data);
            Data result = response.ResultAs<Data>();
            MessageBox.Show("Inserido "+result.ID+" com sucesso!");
        }

        private async void BtnGet_Click(object sender, EventArgs e)
        {
            //FirebaseResponse response = await client.GetTaskAsync("Information/"+txtID.Text);
            //Data obj = response.ResultAs<Data>
        }
    }
}
