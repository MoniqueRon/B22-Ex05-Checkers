using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersUI
{
    public class InitializeGameForm : Form
    {
        private Label m_BoardSizeLabel = new Label();
        private RadioButton m_BoardSize6x6RadioButton = new RadioButton();
        private RadioButton m_BoardSize8x8RadioButton = new RadioButton();
        private RadioButton m_BoardSize10x10RadioButton = new RadioButton();
        private Label m_PlayersLabel = new Label();
        private Label m_Player1Label = new Label();
        private TextBox m_Player1NameTextBox = new TextBox();
        private Label m_Player2Label = new Label();
        private CheckBox m_EnablePlayer2CheckBox = new CheckBox();
        private TextBox m_Player2NameTextBox = new TextBox();
        private Button m_DoneButton = new Button();

        public InitializeGameForm()
        {
            Text = "Game Settings:";
            Size = new Size(300, 250);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initializeContols();
        }

        private void initializeContols()
        {
            m_BoardSizeLabel.Text = "Board Size:";
            m_BoardSizeLabel.Location = new Point(25, 20);
            m_BoardSize6x6RadioButton.Text = "6 x 6";
            m_BoardSize6x6RadioButton.Location = new Point(m_BoardSizeLabel.Left, m_BoardSizeLabel.Height + 15);
            m_BoardSize8x8RadioButton.Text = "8 x 8";
            m_BoardSize8x8RadioButton.Location = new Point(m_BoardSize6x6RadioButton.Right, m_BoardSizeLabel.Height + 15);
            m_BoardSize6x6RadioButton.Checked = true;
            m_BoardSize10x10RadioButton.Text = "10 x 10";
            m_BoardSize10x10RadioButton.Location = new Point(m_BoardSize8x8RadioButton.Right, m_BoardSizeLabel.Height + 15);
            m_PlayersLabel.Text = "Players:";
            m_PlayersLabel.Location = new Point(m_BoardSizeLabel.Left, m_BoardSize6x6RadioButton.Height + 40);
            m_Player1Label.Text = "Player1:";
            m_Player1Label.Location = new Point(m_PlayersLabel.Left + 10, m_PlayersLabel.Height + 65);
            m_Player1NameTextBox.Location = new Point(m_Player1Label.Right, m_PlayersLabel.Height + 65);
            m_EnablePlayer2CheckBox.Checked = false;
            m_EnablePlayer2CheckBox.Location = new Point(m_Player1Label.Left, m_Player1Label.Height + 90);
            m_EnablePlayer2CheckBox.Size = new Size(20, 20);
            m_Player2Label.Text = "Player2:";
            m_Player2Label.Location = new Point(m_EnablePlayer2CheckBox.Right, m_Player1Label.Height + 90);
            m_Player2Label.Size = new Size(60, 20);
            m_Player2NameTextBox.Location = new Point(m_Player1NameTextBox.Left, m_Player1Label.Height + 90);
            m_Player2NameTextBox.Enabled = false;
            m_Player2NameTextBox.Text = "[Computer]";
            m_Player1NameTextBox.MaxLength = 8;
            m_Player2NameTextBox.MaxLength = 8;
            m_DoneButton.Location = new Point(this.ClientSize.Width - 100, this.ClientSize.Height - 50);
            m_DoneButton.Text = "Done";
            Controls.AddRange(
                new Control[]
                {
                m_BoardSizeLabel,
                m_BoardSize6x6RadioButton,
                m_BoardSize8x8RadioButton,
                m_BoardSize10x10RadioButton,
                m_PlayersLabel,
                m_Player1Label,
                m_Player1NameTextBox,
                m_EnablePlayer2CheckBox,
                m_Player2Label,
                m_Player2NameTextBox,
                m_DoneButton
            });
            m_DoneButton.Click += new EventHandler(m_DoneButton_Click);
            m_EnablePlayer2CheckBox.Click += new EventHandler(m_EnablePlayer2CheckBox_Click);
        }


        private void m_DoneButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void m_EnablePlayer2CheckBox_Click(object sender, EventArgs e)
        {
            m_Player2NameTextBox.Enabled = true;
            m_Player2NameTextBox.Text = string.Empty;
        }

        public string Player1Name
        {
            get { return m_Player1NameTextBox.Text; }
            set { m_Player1NameTextBox.Text = value; }
        }

        public string Player2Name
        {
            get { return m_Player2NameTextBox.Text;}
            set { m_Player2NameTextBox.Text = value; }
        }

        public bool BoardSize6X6IsChecked
        {
            get { return m_BoardSize6x6RadioButton.Checked; }
        }

        public bool BoardSize8X8IsChecked
        {
            get { return m_BoardSize8x8RadioButton.Checked; }
        }

        public bool BoardSize10X10IsChecked
        {
            get { return m_BoardSize10x10RadioButton.Checked; }
        }

        public bool Player2CheckBoxIsChecked
        {
            get { return m_EnablePlayer2CheckBox.Checked; }
        }
    }
}
