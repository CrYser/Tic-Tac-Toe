using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {
        bool turn = false; //false = Player X, true = Player O
        int turn_count = 0;
        static string player1, player2;
        bool contraPC = false;
        static bool hardAI = true;

        public Form1()
        {
            InitializeComponent();
        }

        public static void SetPlayerNames(string p1, string p2)
        {
            player1 = p1;
            player2 = p2;
        }

        public static void CheckAI(bool result)
        {
            hardAI = (result == true) ? true : false;
        }

        private void SairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feito por João Salvador\n\n2021","Tic Tac Toe");
        }

        private void Button_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (turn)
            {
                b.Text = "O";
                b.BackColor = Color.LightBlue;
            }
            else
            {
                b.Text = "X";
                b.BackColor = Color.LightSalmon;
            }

            turn = !turn;
            b.Enabled = false;
            turn_count++;
            CheckForWinner();

            if (contraPC && turn && hardAI)
                MinimaxMove();
            else if (contraPC && turn && !hardAI)
                PC_Move();
        }

        private Boolean MinimaxisMovesLeft()
        {
            foreach (var b in this.Controls.OfType<Button>())
                if (b.Text == "")
                    return true;
            return false;
        }

        private int Minimaxevaluate()
        {
            // Horizontal
            if ((A1.Text == A2.Text) && (A2.Text == A3.Text))
            {
                if (A1.Text == "X")
                    return -10;
                else if (A1.Text == "O")
                    return +10;
            }
            if ((B1.Text == B2.Text) && (B2.Text == B3.Text))
            {
                if (B1.Text == "X")
                    return -10;
                else if (B1.Text == "O")
                    return +10;
            }
            if ((C1.Text == C2.Text) && (C2.Text == C3.Text))
            {
                if (C1.Text == "X")
                    return -10;
                else if (C1.Text == "O")
                    return +10;
            }

            // Vertical
            if ((A1.Text == B1.Text) && (B1.Text == C1.Text))
            {
                if (A1.Text == "X")
                    return -10;
                else if (A1.Text == "O")
                    return +10;
            }
            if ((A2.Text == B2.Text) && (B2.Text == C2.Text))
            {
                if (A2.Text == "X")
                    return -10;
                else if (A2.Text == "O")
                    return +10;
            }
            if ((A3.Text == B3.Text) && (B3.Text == C3.Text))
            {
                if (A3.Text == "X")
                    return -10;
                else if (A3.Text == "O")
                    return +10;
            }

            // Diagonal
            if ((A1.Text == B2.Text) && (B2.Text == C3.Text))
            {
                if (A1.Text == "X")
                    return -10;
                else if (A1.Text == "O")
                    return +10;
            }
            if ((A3.Text == B2.Text) && (B2.Text == C1.Text))
            {
                if (A3.Text == "X")
                    return -10;
                else if (A3.Text == "O")
                    return +10;
            }

            return 0;
        }

        private int Minimax(int depth, Boolean isPC, int alpha, int beta)
        {
            int score = Minimaxevaluate();

            if (score == 10)
                return score - depth;

            if (score == -10)
                return score + depth;

            if (MinimaxisMovesLeft() == false)
                return 0;

            if (isPC)
            {
                int best = -1000;

                foreach (var b in this.Controls.OfType<Button>())
                {
                    if (b.Text == "")
                    {
                        b.Text = "O";
                        int val = Minimax(depth + 1, !isPC, alpha, beta);
                        b.Text = "";
                        best = Math.Max(best, val);
                        alpha = Math.Max(alpha, best);
                    }
                    if (beta <= alpha)
                        break;
                }
                return best;
            }
            else
            {
                int best = 1000;

                foreach (var b in this.Controls.OfType<Button>())
                {
                    if (b.Text == "")
                    {
                        b.Text = "X";
                        int val = Minimax(depth + 1, !isPC, alpha, beta);
                        b.Text = "";
                        best = Math.Min(best, val);
                        beta = Math.Min(beta, best);
                    }
                    if (beta <= alpha)
                        break;
                }
                return best;
            }
        }

        private Button MinimaxfindBestMove()
        {
            int bestVal = -1000;

            Button bestMove = null;

            foreach (var b in this.Controls.OfType<Button>())
            {
                if (b.Text == "")
                {
                    b.Text = "O";
                    int moveVal = Minimax(0, false, -1000, 1000);
                    b.Text = "";

                    if (moveVal > bestVal)
                    {
                        bestMove = b;
                        bestVal = moveVal;
                    }
                }
            }
            return bestMove;
        }

        private void MinimaxMove()
        {
            MinimaxfindBestMove().PerformClick();
        }

        private void PC_Move()
        {
            Button move;

            move = Look_for_win_or_block("O"); //look for win
            if (move == null)
            {
                move = Look_for_win_or_block("X"); //look for block
                if (move == null)
                {
                    move = Look_for_corner();
                    if (move == null)
                    {
                        move = Look_for_open_space();
                    }
                }
            }
            move.PerformClick();
        }

        private Button Look_for_win_or_block(string mark)
        {
            //HORIZONTAL TESTS
            if ((A1.Text == mark) && (A2.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A2.Text == mark) && (A3.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (A3.Text == mark) && (A2.Text == ""))
                return A2;

            if ((B1.Text == mark) && (B2.Text == mark) && (B3.Text == ""))
                return B3;
            if ((B2.Text == mark) && (B3.Text == mark) && (B1.Text == ""))
                return B1;
            if ((B1.Text == mark) && (B3.Text == mark) && (B2.Text == ""))
                return B2;

            if ((C1.Text == mark) && (C2.Text == mark) && (C3.Text == ""))
                return C3;
            if ((C2.Text == mark) && (C3.Text == mark) && (C1.Text == ""))
                return C1;
            if ((C1.Text == mark) && (C3.Text == mark) && (C2.Text == ""))
                return C2;

            //VERTICAL TESTS
            if ((A1.Text == mark) && (B1.Text == mark) && (C1.Text == ""))
                return C1;
            if ((B1.Text == mark) && (C1.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (C1.Text == mark) && (B1.Text == ""))
                return B1;

            if ((A2.Text == mark) && (B2.Text == mark) && (C2.Text == ""))
                return C2;
            if ((B2.Text == mark) && (C2.Text == mark) && (A2.Text == ""))
                return A2;
            if ((A2.Text == mark) && (C2.Text == mark) && (B2.Text == ""))
                return B2;

            if ((A3.Text == mark) && (B3.Text == mark) && (C3.Text == ""))
                return C3;
            if ((B3.Text == mark) && (C3.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A3.Text == mark) && (C3.Text == mark) && (B3.Text == ""))
                return B3;

            //DIAGONAL TESTS
            if ((A1.Text == mark) && (B2.Text == mark) && (C3.Text == ""))
                return C3;
            if ((B2.Text == mark) && (C3.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (C3.Text == mark) && (B2.Text == ""))
                return B2;

            if ((A3.Text == mark) && (B2.Text == mark) && (C1.Text == ""))
                return C1;
            if ((B2.Text == mark) && (C1.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A3.Text == mark) && (C1.Text == mark) && (B2.Text == ""))
                return B2;

            return null;
        }

        private Button Look_for_corner()
        {
            if (A1.Text == "O")
            {
                if (A3.Text == "")
                    return A3;
                if (C3.Text == "")
                    return C3;
                if (C1.Text == "")
                    return C1;
            }

            if (A3.Text == "O")
            {
                if (A1.Text == "")
                    return A1;
                if (C3.Text == "")
                    return C3;
                if (C1.Text == "")
                    return C1;
            }

            if (C3.Text == "O")
            {
                if (A1.Text == "")
                    return A3;
                if (A3.Text == "")
                    return A3;
                if (C1.Text == "")
                    return C1;
            }

            if (C1.Text == "O")
            {
                if (A1.Text == "")
                    return A3;
                if (A3.Text == "")
                    return A3;
                if (C3.Text == "")
                    return C3;
            }

            if (A1.Text == "")
                return A1;
            if (A3.Text == "")
                return A3;
            if (C1.Text == "")
                return C1;
            if (C3.Text == "")
                return C3;

            return null;
        }

        private Button Look_for_open_space()
        {
            foreach (var b in this.Controls.OfType<Button>())
            {
                if (b.Text == "")
                    return b;
            }
            return null;
        }

        private void CheckForWinner()
        {
            bool checkwinner = false;

            //Horizontal
            if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled))
                checkwinner = true;
            if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled))
                checkwinner = true;
            if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled))
                checkwinner = true;

            //Vertical
            if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled))
                checkwinner = true;
            if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled))
                checkwinner = true;
            if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled))
                checkwinner = true;

            //Diagonal
            if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!A1.Enabled))
                checkwinner = true;
            if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!A3.Enabled))
                checkwinner = true;

            if (checkwinner)
            {
                DesativarBotoes();

                if (turn)
                {
                    p1_count.Text = (Int32.Parse(p1_count.Text) + 1).ToString();
                    MessageBox.Show(player1 + " ganhou!", "Vitoria!");
                    AtivarBotoes();
                    turn_count = 0;
                }
                else
                {
                    p2_count.Text = (Int32.Parse(p2_count.Text) + 1).ToString();
                    MessageBox.Show(player2 + " ganhou!", "Vitoria!");
                    AtivarBotoes();
                    turn_count = 0;
                }   
            }
            else if (turn_count == 9)
            {
                draw_count.Text = (Int32.Parse(draw_count.Text) + 1).ToString();
                MessageBox.Show("Empate!", "Bolas!");
                AtivarBotoes();
                turn_count = 0;
            }
        }

        private void DesativarBotoes()
        {
            foreach (var b in this.Controls.OfType<Button>())
                b.Enabled = false;
        }

        private void AtivarBotoes()
        {
            foreach (var b in this.Controls.OfType<Button>())
            {
                b.Enabled = true;
                b.Text = "";
                b.BackColor = Color.LightGray;
            }
        }

        private void Entrada_rato(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (b.Enabled)
            {
                if (turn)
                {
                    b.Text = "O";
                    b.ForeColor = Color.Gray;
                }
                else
                {
                    b.Text = "X";
                    b.ForeColor = Color.Gray;
                }
            }
        }

        private void Saida_rato(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (b.Enabled)
                b.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IniciarForm2();
        }

        private void IniciarForm2()
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            label1.Text = player1;
            label3.Text = player2;
            if (label3.Text == "PC")
                contraPC = true;
        }

        private void MudarJogadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IniciarForm2();
            AtivarBotoes();
            turn_count = 0;
            turn = false;
            p1_count.Text = "0";
            draw_count.Text = "0";
            p2_count.Text = "0";
        }
    }
}
