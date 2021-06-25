using System;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Form2 : Form
    {
        static bool result = false;

        public Form2()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(p1.Text == "" || p2.Text == "")
                MessageBox.Show("Tens que introduzir os nomes dos jogadores");
            else
            {
                if (p2.Text == "PC")
                {
                    var res = MessageBox.Show("Queres jogar contra o AI dificil?", "", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                        Form1.CheckAI(result = true);
                    else if(res == DialogResult.No)
                        Form1.CheckAI(result = false);
                }
                Form1.SetPlayerNames(p1.Text, p2.Text);
                this.Close();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
