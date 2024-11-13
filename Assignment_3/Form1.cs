using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Assignment_3
{
    public partial class Form1: Form
    {
        private const int Handsize = 5;
        private readonly Deck deck;
        private readonly Card[] handArray = new Card[Handsize];
        private readonly ImageList imageList = new ImageList();

        private readonly PictureBox[] pictureBoxes = new PictureBox[Handsize];
        private readonly CheckBox[] checkBoxes = new CheckBox[Handsize];

        private Button dealButton, saveButton, loadButton, showDeckButton;
        public Form1()
        {
            this.Text = "Card Game";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            InitializeComponent();

            InitializeControls();

            InitializeImageList();

            deck = new Deck(imageList);

            DealHand();
        }

        private void InitializeImageList()
        {
            imageList.ImageSize = new Size(100, 150);

            for (int i = 0; i < 52; i++)
            {
                string filePath = Path.Combine("C:\\Users\\Madison\\Downloads\\", $"p{i}.png");
                try
                {
                    imageList.Images.Add(Image.FromFile(filePath));
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show($"File not found: {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void InitializeControls()
        {
            dealButton = new Button { Text = "&Deal Hand", Location = new Point(639, 71) };
            dealButton.Click += (s, e) => DealHand();
           
            saveButton = new Button { Text= "&Save Hand", Location = new Point(484, 71) };
            saveButton.Click += (s, e) => SaveHand();

            loadButton = new Button { Text = "&Load Hand", Location = new Point(40, 71) };
            loadButton.Click += (s, e) => LoadHand();

            showDeckButton = new Button { Text = "&Show Deck", Location = new Point(252, 71) };
            showDeckButton.Click += (s, e) => ShowDeckForm();

            this.Controls.Add(dealButton);
            this.Controls.Add(saveButton);
            this.Controls.Add(loadButton);
            this.Controls.Add(showDeckButton);

            for (int i = 0; i < Handsize; i++)
            {
                checkBoxes[i] = new CheckBox
                {
                    Text = $"&Keep {i + 1}",
                    Location = new Point(40 + i * 160, 128),
                    TabIndex = i + 1,
                    Checked = false
                };
                int index = i;
                checkBoxes[i].CheckedChanged += (s, e) => ToggleKeep(index);

                this.Controls.Add(checkBoxes[i]);

                pictureBoxes[i] = new PictureBox
                {
                    Location = new Point(40 + i * 160, 168),
                    Size = new Size(100, 150),
                    Image = null,
                    TabStop = false,
                    BorderStyle = BorderStyle.FixedSingle
                };
                this.Controls.Add(pictureBoxes[i]);
            }
        }

        private void ShowDeckForm()
        {
            DeckForm deckForm = new DeckForm(deck, imageList);
            deckForm.ShowDialog();
        }   

        private readonly OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Card files (*.card)|*.card|All files (*.*)|*.*",
            FilterIndex = 1,
            RestoreDirectory = true
        };

        private readonly SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "Card files (*.card)|*.card|All files (*.*)|*.*",
            FilterIndex = 1,
            RestoreDirectory = true
        };

        private void DealHand()
        {

            for (int i = 0; i < Handsize; i++)
            {
                if (!checkBoxes[i].Checked)
                {
                    Card card = deck.DealCard();
                    handArray[i] = card;
                }
                UpdateCardDisplay(i);

            }

        }

        private void ToggleKeep(int index)
        {
            bool isChecked = checkBoxes[index].Checked;

            if (isChecked)
            {
                if (handArray[index] == null || handArray[index].Id == -1)
                {
                    handArray[index] = deck.DealCard();
                }
            }
            else
            {
                handArray[index] = Card.NoCard;
            }
            UpdateCardDisplay(index);
        }

        private void UpdateCardDisplay(int index)
        {
            if (index >= 0 && index < Handsize)
            {
                pictureBoxes[index].Image = handArray[index]?.Id >= 0 ? imageList.Images[handArray[index].Id] : null;
            }

        }

        private void SaveHand()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                deck.SaveHand(saveFileDialog.FileName, handArray);
            }
        }

        private void LoadHand()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (deck.LoadHand(openFileDialog.FileName, handArray))
                {
                    for (int i = 0; i < Handsize; i++)
                    {
                        UpdateCardDisplay(i);
                    }
                }
            }

        }
       
    }
}


      
