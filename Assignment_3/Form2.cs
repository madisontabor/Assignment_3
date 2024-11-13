using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_3
{
    public partial class DeckForm : Form
    {
        private readonly Deck deck;
        private readonly ImageList imageList;
        private ListBox cardListBox;
        private PictureBox cardPictureBox;

        public DeckForm(Deck deck, ImageList imageList)
        {
            InitializeComponent();
            this.deck = deck;
            this.imageList = imageList;

            this.Text = "Deck";
            this.FormBorderStyle = FormBorderStyle.FixedDialog; 
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Load += DeckForm_Load;

            InitializeControls();
        }

        private void InitializeControls()
        {
            Label cardsLabel = new Label
            {
                Text = "&Cards",
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true
            };
            this.Controls.Add(cardsLabel);

            cardListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(250, 300),
                DisplayMember = "Id" 
            };
            cardListBox.SelectedIndexChanged += CardListBox_SelectedIndexChanged;
            this.Controls.Add(cardListBox);

            cardPictureBox = new PictureBox
            {
                Location = new System.Drawing.Point(270, 40),
                Size = new System.Drawing.Size(100, 150),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(cardPictureBox);
        }
        public void UpdateDeck()
        {
            cardListBox.Items.Clear();

            for (int i = 0; i < deck.Count; i++)
            {
                cardListBox.Items.Add(deck.GetCard(i));
            }
        }
        private void CardListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Card selectedCard = cardListBox.SelectedItem as Card;
            if(selectedCard != null)
            {
                cardPictureBox.Image = imageList.Images[selectedCard.Id];
            }
        }
        private void DeckForm_Load(object sender, EventArgs e)
        {
            UpdateDeck();
        }
    }
}

