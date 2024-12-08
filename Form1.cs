using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SD = System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MediaConverterApp
{
    public partial class Form1 : Form
    {
        private string saveFilePath = string.Empty;
        private ComboBox comboBoxFormats;
        private ComboBox comboBoxExtensions;
        private FlowLayoutPanel flowPanelImages;
        private Button btnSelectFiles;
        private Button btnClearImages;
        private Button btnChooseSaveLocation;
        private Button btnSave;
        private Label lblTitle;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Conversor de Mídia Profissional";
            this.Width = 1000;
            this.Height = 850;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimizeBox = true;
            this.MaximizeBox = true;
            this.BackColor = SD.Color.FromArgb(35, 35, 35);

            Panel panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = SD.Color.FromArgb(45, 45, 45),
                Padding = new Padding(20, 15, 0, 0)
            };

            lblTitle = new Label
            {
                Text = "Conversor de Mídia",
                ForeColor = SD.Color.White,
                Font = new SD.Font("Segoe UI Semibold", 24F, SD.FontStyle.Bold),
                AutoSize = true
            };
            panelTop.Controls.Add(lblTitle);

            Panel panelControls = new Panel
            {
                Dock = DockStyle.Top,
                Height = 180,
                Padding = new Padding(20),
                BackColor = SD.Color.FromArgb(50, 50, 50)
            };

            btnSelectFiles = new Button
            {
                Text = "Selecionar Arquivos",
                Width = 200,
                Height = 40,
                BackColor = SD.Color.FromArgb(70, 70, 70),
                ForeColor = SD.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new SD.Font("Segoe UI", 10F, SD.FontStyle.Bold)
            };
            btnSelectFiles.FlatAppearance.BorderSize = 0;
            btnSelectFiles.Click += BtnSelectFiles_Click;

            btnClearImages = new Button
            {
                Text = "Remover Tudo",
                Width = 180,
                Height = 40,
                BackColor = SD.Color.FromArgb(70, 70, 70),
                ForeColor = SD.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new SD.Font("Segoe UI", 10F, SD.FontStyle.Bold)
            };
            btnClearImages.FlatAppearance.BorderSize = 0;
            btnClearImages.Click += BtnClearImages_Click;

            comboBoxFormats = new ComboBox
            {
                Width = 280,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new SD.Font("Segoe UI", 10F),
                BackColor = SD.Color.FromArgb(60,60,60),
                ForeColor = SD.Color.White
            };
            comboBoxFormats.Items.AddRange(new object[]
            {
                "Instagram Reels (1080x1920)",
                "YouTube Reels (1080x1920)",
                "TikTok (1080x1920)",
                "YouTube Video Padrão (1920x1080)"
            });
            comboBoxFormats.SelectedIndex = 0;

            comboBoxExtensions = new ComboBox
            {
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new SD.Font("Segoe UI", 10F),
                BackColor = SD.Color.FromArgb(60,60,60),
                ForeColor = SD.Color.White
            };
            comboBoxExtensions.Items.AddRange(new object[]
            {
                ".jpg", ".png", ".bmp", ".gif", ".tiff", ".webp"
            });
            comboBoxExtensions.SelectedIndex = 0;

            btnChooseSaveLocation = new Button
            {
                Text = "Escolher Diretório...",
                Width = 240,
                Height = 40,
                BackColor = SD.Color.FromArgb(70, 70, 70),
                ForeColor = SD.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new SD.Font("Segoe UI", 10F, SD.FontStyle.Bold)
            };
            btnChooseSaveLocation.FlatAppearance.BorderSize = 0;
            btnChooseSaveLocation.Click += BtnChooseSaveLocation_Click;

            btnSave = new Button
            {
                Text = "Converter e Salvar",
                Width = 220,
                Height = 45,
                BackColor = SD.Color.FromArgb(0, 150, 255),
                ForeColor = SD.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new SD.Font("Segoe UI", 10F, SD.FontStyle.Bold)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            Label formatLabel = new Label
            {
                Text = "Formato:",
                ForeColor = SD.Color.White,
                Font = new SD.Font("Segoe UI", 10F, SD.FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(20, 12, 5, 0)
            };

            Label extensionLabel = new Label
            {
                Text = "Extensão:",
                ForeColor = SD.Color.White,
                Font = new SD.Font("Segoe UI", 10F, SD.FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(20, 12, 5, 0)
            };

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = false,
                AutoScroll = false,
                Padding = new Padding(0),
                BackColor = SD.Color.Transparent
            };

            layout.Controls.Add(btnSelectFiles);
            layout.Controls.Add(btnClearImages);
            layout.Controls.Add(formatLabel);
            layout.Controls.Add(comboBoxFormats);
            layout.Controls.Add(extensionLabel);
            layout.Controls.Add(comboBoxExtensions);
            layout.Controls.Add(btnChooseSaveLocation);
            layout.Controls.Add(btnSave);
            panelControls.Controls.Add(layout);

            flowPanelImages = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = SD.Color.FromArgb(40,40,40),
                AllowDrop = true,
                Padding = new Padding(10)
            };
            flowPanelImages.DragEnter += FlowPanelImages_DragEnter;
            flowPanelImages.DragDrop += FlowPanelImages_DragDrop;

            this.Controls.Add(flowPanelImages);
            this.Controls.Add(panelControls);
            this.Controls.Add(panelTop);
        }

        private async void BtnSelectFiles_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Imagens|*.jpg;*.png;*.bmp;*.gif;*.tiff;*.webp",
                Title = "Selecione Imagens",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                await LoadImagesAsync(openFileDialog.FileNames);
            }
        }

        private async Task LoadImagesAsync(string[] files)
        {
            foreach (string file in files)
            {
                await AddImageToFlowPanelAsync(file);
            }
        }

        private void FlowPanelImages_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private async void FlowPanelImages_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                await LoadImagesAsync(files);
            }
        }

        private async Task AddImageToFlowPanelAsync(string filePath)
        {
            try
            {
                var thumbnailImage = await Task.Run(() => CreateThumbnail(filePath));
                if (thumbnailImage == null) return;
                Panel imagePanel = CreateImagePanel(thumbnailImage, filePath);
                flowPanelImages.Controls.Add(imagePanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar a imagem: {filePath}\n{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SD.Image CreateThumbnail(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            const int thumbWidth = 100;
            const int thumbHeight = 100;
            if (extension == ".webp")
            {
                using (var image = SixLabors.ImageSharp.Image.Load(filePath))
                {
                    image.Mutate(x => x.Resize(new SixLabors.ImageSharp.Size(thumbWidth, thumbHeight)));
                    using var ms = new MemoryStream();
                    image.SaveAsJpeg(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return SD.Image.FromStream(ms);
                }
            }
            else
            {
                using var original = SD.Image.FromFile(filePath);
                return original.GetThumbnailImage(thumbWidth, thumbHeight, () => false, IntPtr.Zero);
            }
        }

        private Panel CreateImagePanel(SD.Image image, string filePath)
        {
            var panel = new Panel
            {
                Width = 140,
                Height = 170,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.None,
                BackColor = SD.Color.FromArgb(60,60,60)
            };

            var pictureBox = new PictureBox
            {
                Image = image,
                Width = 100,
                Height = 100,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Tag = filePath,
                Margin = new Padding(10),
                Top = 10,
                Left = (140 - 100) / 2
            };

            var extension = Path.GetExtension(filePath).ToLower();
            var label = new Label
            {
                Text = extension,
                AutoSize = false,
                Height = 20,
                Width = 100,
                Font = new SD.Font("Segoe UI", 8F, SD.FontStyle.Bold),
                ForeColor = SD.Color.White,
                TextAlign = SD.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                BackColor = SD.Color.FromArgb(45,45,45)
            };

            pictureBox.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    flowPanelImages.Controls.Remove(panel);
                    pictureBox.Dispose();
                    image.Dispose();
                    label.Dispose();
                    panel.Dispose();
                }
            };

            panel.Controls.Add(label);
            panel.Controls.Add(pictureBox);

            return panel;
        }

        private void BtnClearImages_Click(object sender, EventArgs e)
        {
            foreach (Control c in flowPanelImages.Controls)
            {
                if (c is Panel p)
                {
                    foreach (Control child in p.Controls)
                    {
                        if (child is PictureBox pb)
                        {
                            pb.Image?.Dispose();
                        }
                    }
                }
            }
            flowPanelImages.Controls.Clear();
        }

        private void BtnChooseSaveLocation_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderDialog = new FolderBrowserDialog
            {
                Description = "Escolha o Diretório para Salvar os Arquivos Convertidos"
            };
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                saveFilePath = folderDialog.SelectedPath;
                MessageBox.Show($"Diretório selecionado: {saveFilePath}", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (flowPanelImages.Controls.Count == 0)
            {
                MessageBox.Show("Por favor, adicione pelo menos uma imagem.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(saveFilePath))
            {
                MessageBox.Show("Por favor, escolha um diretório para salvar os arquivos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                await Task.Run(() => ConvertAndSaveImages());
                MessageBox.Show("Arquivos convertidos e salvos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar os arquivos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConvertAndSaveImages()
        {
            foreach (Control control in flowPanelImages.Controls)
            {
                if (control is Panel p)
                {
                    foreach (Control child in p.Controls)
                    {
                        if (child is PictureBox pictureBox && pictureBox.Tag is string filePath)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(filePath);
                            string selectedExtension = comboBoxExtensions.SelectedItem.ToString();
                            string outputFilePath = Path.Combine(saveFilePath, fileName + selectedExtension);
                            using var image = SixLabors.ImageSharp.Image.Load(filePath);
                            (int width, int height) = GetSelectedSize();
                            if (width > 0 && height > 0)
                            {
                                image.Mutate(x => x.Resize(width, height));
                            }
                            switch (selectedExtension)
                            {
                                case ".jpg":
                                    image.SaveAsJpeg(outputFilePath);
                                    break;
                                case ".png":
                                    image.SaveAsPng(outputFilePath);
                                    break;
                                case ".bmp":
                                    image.SaveAsBmp(outputFilePath);
                                    break;
                                case ".gif":
                                    image.SaveAsGif(outputFilePath);
                                    break;
                                case ".tiff":
                                    image.SaveAsTiff(outputFilePath);
                                    break;
                                case ".webp":
                                    image.SaveAsWebp(outputFilePath);
                                    break;
                                default:
                                    throw new NotSupportedException($"Extensão {selectedExtension} não suportada.");
                            }
                        }
                    }
                }
            }
        }

        private (int, int) GetSelectedSize()
        {
            switch (comboBoxFormats.SelectedItem.ToString())
            {
                case "Instagram Reels (1080x1920)":
                case "YouTube Reels (1080x1920)":
                case "TikTok (1080x1920)":
                    return (1080, 1920);
                case "YouTube Video Padrão (1920x1080)":
                    return (1920, 1080);
                default:
                    return (0, 0);
            }
        }
    }
}
