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
    public partial class Form1 : Form
    {
        private const int Handsize = 5;
        private const string Path = "C:\\Users\\Madison\\Downloads\\";
        private readonly List<int> deckList = new List<int>();
        private readonly int[] handArray = new int[Handsize];
        private readonly ImageList imageList = new ImageList();

        private readonly PictureBox[] pictureBoxes = new PictureBox[Handsize];
        private readonly CheckBox[] checkBoxes = new CheckBox[Handsize];

        private Button dealButton, saveButton, loadButton;
        public Form1()
        {
            this.Text = "Card Game";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            InitializeComponent();

            InitializeControls();

            InitializeImageList();

            InitializeDeck();

            DealHand();
        }

        private void InitializeImageList()
        {
            imageList.ImageSize = new Size(100, 150);

            for (int i = 0; i < 52; i++)
            {
                string filePath = Path + $"p{i}.png";
                imageList.Images.Add(Image.FromFile(filePath));
            }

        }

        private void InitializeControls()
        {
            dealButton = new Button { Text = "&Deal Hand", Location = new Point(639, 71) };
            dealButton.Click += (s, e) => DealHand();

            saveButton = new Button { Text = "&Save Hand", Location = new Point(484, 71) };
            saveButton.Click += (s, e) => SaveHand();

            loadButton = new Button { Text = "&Load Hand", Location = new Point(40, 71) };
            loadButton.Click += (s, e) => LoadHand();

            this.Controls.Add(dealButton);
            this.Controls.Add(saveButton);
            this.Controls.Add(loadButton);



            for (int i = 0; i < Handsize; i++)
            {
                checkBoxes[i] = new CheckBox
                {

                    Text = $"&Keep {i +1}",
                    Location = new Point(40 + i * 160, 128),
                    TabIndex = i + 1,
                    Checked = i >= 0
                };

                int index = i;
                checkBoxes[i].CheckedChanged += (s, e) => ToggleKeep(index);
  
                this.Controls.Add(checkBoxes[i]);

                pictureBoxes[i] = new PictureBox
                {
                    Location = new Point(40 + i * 160, 168),
                    Size = new System.Drawing.Size(100, 150),
                    Image = null,
                    TabStop = false,
                    BorderStyle = BorderStyle.FixedSingle

                };
                this.Controls.Add(pictureBoxes[i]);     
            }
        }

        private void InitializeDeck()
        {
            for (int i = 0; i < 52; i++)
            {
                deckList.Add(i);
            }
            Shuffle(deckList);
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
                    if (deckList.Count > 0)
                    {
                        handArray[i] = deckList[0];
                        deckList.RemoveAt(0);
                    }
                    else
                    {
                        handArray[i] = -1;
                    }
                }
                    UpdateCardDisplay(i);
              
            }
}

            private void ToggleKeep(int index)
            {
               bool isChecked = checkBoxes[index].Checked;
            if(isChecked)   
            {
                handArray[index] = deckList.Count > 0 ? deckList[0] : -1;
            }
            else
            {
                handArray[index] = -1;
            }
                UpdateCardDisplay(index);
            }

            private void UpdateHand()
            {
                for (int i = 0; i < Handsize; i++)
                {
                    if (checkBoxes[i].Checked)
                    {
                        handArray[i] = handArray[i] >= 0 ? handArray[i] : -1;
                    }
                    else
                    {
                        handArray[i] = -1;
                    }
                    UpdateCardDisplay(i);
                }

            }

            private void UpdateCardDisplay(int index)
            {
                pictureBoxes[index].Image = handArray[index] >= 0 ? imageList.Images[handArray[index]] : null;
            }

            private void Shuffle(List<int> list)
            {
                Random random = new Random();
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = random.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        private void SaveHand()
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (int card in handArray)
                        {
                            file.WriteLine(card);
                        }
                    }
                }
            }

            private void LoadHand()
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog.FileName))
                    {
                        for (int i = 0; i < Handsize; i++)
                        {
                            handArray[i] = int.Parse(file.ReadLine());
                            UpdateCardDisplay(i);
                        }
                    }
                }
            }
        }
    }
