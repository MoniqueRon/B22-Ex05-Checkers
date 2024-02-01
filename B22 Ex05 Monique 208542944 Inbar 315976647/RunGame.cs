using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersUI
{
    public class RunGame
    {
        public static void Run()
        {
            InitializeGameForm initializeGameForm = new InitializeGameForm();

            if (initializeGameForm.ShowDialog() == DialogResult.OK)
            {
                if (initializeGameForm.Player1Name.Length == 0 || (initializeGameForm.Player2CheckBoxIsChecked && initializeGameForm.Player2Name.Length == 0))
                {
                    if (MessageBox.Show("Please enter valid names.", "Invalid Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    {
                        Run();
                    }
                }

                else
                {
                    GameForm gameForm = new GameForm(initializeGameForm);
                    gameForm.ShowDialog();
                }
            }
        }
    }
}
