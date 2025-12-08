using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Core;
using App.Core.Desktop;
using PhotoSauce.MagicScaler;

namespace App.Image.MagicScaler
{
    public class MagicScaler
    {
        #region main
        private ProcessImageSettings settings;

        private EncoderOptions encoderSelected;

        private PngFilter pngFilter = PngFilter.Unspecified;

        private bool pngInterlace;

        private int jpgQuality = 98;

        private ChromaSubsampleMode jpgChromaSubsample = ChromaSubsampleMode.Subsample444;

        private Sizes sizeSelected;

        private string imgFormats = "All Formats (*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tiff)|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tiff|PNG (*.png)|*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|TIFF (*.tiff)|*.tiff";

        private string outFormats = "All Formats (*.png;*.jpg;*.gif;*.tiff)|*.png;*.jpg;*.gif;*.tiff";

        private string _imgPath;

        private string _outPath;

        public MagicScaler()
        {
            settings = new ProcessImageSettings
            {
                Width = 64,
                Height = 64
                ////ResizeMode = CropScaleMode.Stretch,
                ////EncoderOptions = new PngEncoderOptions(PngFilter.Unspecified, false),
                ////EncoderOptions = new JpegEncoderOptions(98, ChromaSubsampleMode.Subsample444, true),
                ////Interpolation = InterpolationSettings.Average,
                ////Anchor = CropAnchor.Center,
                ////BlendingMode = GammaMode.Linear,
                ////HybridMode = HybridScaleMode.FavorQuality,
            };

            SetEncoderOptions();

            ImgDialog = new OpenFileDialog
            {
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = string.Empty,
                Filter = imgFormats
            };

            OutDialog = new SaveFileDialog
            {
                Filter = outFormats
            };

            CustomName = true;
            ImgFile = string.Empty;

            SuccessMessage = "MagicScaler Executed!";
            ErrorMessage = "MagicScaler Failed!";
        }

        public event Action EncoderChanged = delegate { };

        public event BoolAction EnableAnchor = delegate { };

        public event Action Resized = delegate { };

        public event Action InvalidFile = delegate { };

        private enum EncoderOptions
        {
            Png = 0,
            PngIndexed = 1,
            Jpg = 2,
            Gif = 3,
            Tiff = 4
        }

        private enum Interpolation
        {
            Average,
            Hermite,
            NearestNeighbor,
            CatmullRom,
            Cubic,
            CubicSmoother,
            Lanczos,
            Linear,
            Mitchell,
            Quadratic,
            Spline36,
        }

        private enum Sizes
        {
            Same,
            _50_Percent,
            _128x112,
            _320x320,
            _360x512,
            _420x600,
            _512x512,
            _600x600
        }

        public string ErrorMessage { get; set; }

        public string SuccessMessage { get; set; }

        public string ImgFolder { get; set; }

        public string ImgFile { get; set; }

        public string OutFolder { get; set; }

        public string OutFile { get; set; }

        public string ImgPath
        {
            get
            {
                return _imgPath;
            }

            set
            {
                _imgPath = value;
                UpdateImg();
            }
        }

        public string OutPath
        {
            get
            {
                return _outPath;
            }

            set
            {
                _outPath = value;
                UpdateOut();
            }
        }

        private OpenFileDialog ImgDialog { get; set; }

        private SaveFileDialog OutDialog { get; set; }

        private bool CustomName { get; set; }

        public async Task<bool> Resize()
        {
            if (IsInvalidInputs())
            {
                return false;
            }

            var imgBitmap = BitmapExtension.SuperFastLoad(ImgPath);

            if (sizeSelected == Sizes.Same)
            {
                settings.Width = imgBitmap.Width;
                settings.Height = imgBitmap.Height;
            }
            else if (sizeSelected == Sizes._50_Percent)
            {
                settings.Width = imgBitmap.Width / 2;
                settings.Height = imgBitmap.Height / 2;
            }

            await Task.Run(() =>
            {
                MagicImageProcessor.ProcessImage(ImgPath, OutPath, settings);
            });

            Resized();
            return true;
        }
        #endregion

        #region LoadCombos
        public void LoadEncoders(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(EncoderOptions));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                var innerCombo = s as FlatComboBoxNew;
                encoderSelected = (EncoderOptions)innerCombo.SelectedItem;

                SetEncoderOptions();

                OutDialog.FileName = SetExtension(string.Empty, ImgDialog.FileName);

                if (string.IsNullOrWhiteSpace(OutPath))
                {
                    return;
                }

                OutPath = SetExtension(OutFolder, OutPath);

                EncoderChanged();
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadResizeModes(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(CropScaleMode));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                var cboItem = (CropScaleMode)cbo.SelectedItem;

                settings.ResizeMode = cboItem;
                EnableAnchor(cboItem == CropScaleMode.Crop);
            };

            cbo.SelectedIndex = 2;
        }

        public void LoadSizes(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(Sizes));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                var innerCombo = s as FlatComboBoxNew;
                sizeSelected = (Sizes)innerCombo.SelectedItem;

                switch (sizeSelected)
                {
                    case Sizes._128x112:
                        settings.Width = 128;
                        settings.Height = 112;
                        break;
                    case Sizes._320x320:
                        settings.Width = 320;
                        settings.Height = 320;
                        break;
                    case Sizes._360x512:
                        settings.Width = 360;
                        settings.Height = 512;
                        break;
                    case Sizes._420x600:
                        settings.Width = 420;
                        settings.Height = 600;
                        break;
                    case Sizes._512x512:
                        settings.Width = 512;
                        settings.Height = 512;
                        break;
                    case Sizes._600x600:
                        settings.Width = 600;
                        settings.Height = 600;
                        break;
                }
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadAnchors(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(CropAnchor));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                settings.Anchor = (CropAnchor)cbo.SelectedItem;
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadMatteColors(FlatComboBox cbo)
        {
            var colors = new ListBind<KnownColor>
            {
                KnownColor.Transparent, KnownColor.Magenta, KnownColor.Black, KnownColor.White
            };

            cbo.DataSource = colors;

            cbo.SelectedIndexChanged += (s, e) =>
            {
                settings.MatteColor = Color.FromKnownColor((KnownColor)cbo.SelectedItem);
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadInterpolations(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(Interpolation));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                switch ((Interpolation)cbo.SelectedItem)
                {
                    case Interpolation.Average: settings.Interpolation = InterpolationSettings.Average; break;
                    case Interpolation.CatmullRom: settings.Interpolation = InterpolationSettings.CatmullRom; break;
                    case Interpolation.Cubic: settings.Interpolation = InterpolationSettings.Cubic; break;
                    case Interpolation.CubicSmoother: settings.Interpolation = InterpolationSettings.CubicSmoother; break;
                    case Interpolation.Hermite: settings.Interpolation = InterpolationSettings.Hermite; break;
                    case Interpolation.Lanczos: settings.Interpolation = InterpolationSettings.Lanczos; break;
                    case Interpolation.Linear: settings.Interpolation = InterpolationSettings.Linear; break;
                    case Interpolation.Mitchell: settings.Interpolation = InterpolationSettings.Mitchell; break;
                    case Interpolation.NearestNeighbor: settings.Interpolation = InterpolationSettings.NearestNeighbor; break;
                    case Interpolation.Quadratic: settings.Interpolation = InterpolationSettings.Quadratic; break;
                    case Interpolation.Spline36: settings.Interpolation = InterpolationSettings.Spline36; break;
                }
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadBlendingModes(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(GammaMode));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                settings.BlendingMode = (GammaMode)cbo.SelectedItem;
            };

            cbo.SelectedIndex = 1;
        }

        public void LoadSharpen(FlatComboBox cbo)
        {
            cbo.DataSource = new ListBind<bool> { false, true };

            cbo.SelectedIndexChanged += (s, e) =>
            {
                settings.Sharpen = (bool)cbo.SelectedItem;
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadColorProfiles(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(ColorProfileMode));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                settings.ColorProfileMode = (ColorProfileMode)cbo.SelectedItem;
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadHybridModes(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(HybridScaleMode));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                settings.HybridMode = (HybridScaleMode)cbo.SelectedItem;
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadPngFilters(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(PngFilter));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                pngFilter = (PngFilter)cbo.SelectedItem;
                SetEncoderOptions();
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadPngInterlaces(FlatComboBox cbo)
        {
            cbo.DataSource = new ListBind<bool> { false, true };

            cbo.SelectedIndexChanged += (s, e) =>
            {
                pngInterlace = (bool)cbo.SelectedItem;
                SetEncoderOptions();
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadJpgQuality(FlatComboBox cbo)
        {
            cbo.DataSource = Enumerable.Range(0, 101).ToList();

            cbo.SelectedIndexChanged += (s, e) =>
            {
                jpgQuality = (int)cbo.SelectedItem;
                SetEncoderOptions();
            };

            cbo.SelectedIndex = 98;
        }

        public void LoadJpgChromaSubsample(FlatComboBox cbo)
        {
            var list = Enum.GetValues(typeof(ChromaSubsampleMode)).Cast<ChromaSubsampleMode>().ToList();
            list.RemoveAt(list.Count - 1);

            cbo.DataSource = list;

            cbo.SelectedIndexChanged += (s, e) =>
            {
                jpgChromaSubsample = (ChromaSubsampleMode)cbo.SelectedItem;
                SetEncoderOptions();
            };

            cbo.SelectedIndex = 0;
        }

        public void SetEncoderOptions()
        {
            switch (encoderSelected)
            {
                case EncoderOptions.Png:
                    settings.EncoderOptions = new PngEncoderOptions(pngFilter, pngInterlace);
                    break;
                case EncoderOptions.PngIndexed:
                    settings.EncoderOptions = new PngIndexedEncoderOptions(256, null, DitherMode.Auto, pngFilter, pngInterlace);
                    break;
                case EncoderOptions.Jpg:
                    settings.EncoderOptions = new JpegEncoderOptions(jpgQuality, jpgChromaSubsample);
                    break;
                case EncoderOptions.Gif:
                    settings.EncoderOptions = new GifEncoderOptions(256, null, DitherMode.Auto);
                    break;
                case EncoderOptions.Tiff:
                    settings.EncoderOptions = new TiffEncoderOptions(TiffCompression.Deflate);
                    break;
            }
        }
        #endregion

        #region ImgAndOutFile
        public bool PickImg()
        {
            bool result;

            if (result = ImgDialog.ShowDialog() == DialogResult.OK)
            {
                ImgPath = ImgDialog.FileName.NormalizePath();
                UpdateOutFile();
            }

            return result;
        }

        public bool PickOut()
        {
            if (CustomName)
            {
                OutDialog.InitialDirectory = OutFolder;
                if (OutDialog.InitialDirectory == null)
                {
                    OutDialog.InitialDirectory = ImgDialog.InitialDirectory;
                }

                if (OutDialog.ShowDialog() == DialogResult.OK)
                {
                    OutPath = OutDialog.FileName.NormalizePath();

                    OutPath = SetExtension(OutFolder, OutPath);

                    return true;
                }

                return false;
            }

            return false;
        }

        private void UpdateImg()
        {
            if (string.IsNullOrWhiteSpace(ImgPath))
            {
                return;
            }

            ImgDialog.InitialDirectory = Path.GetDirectoryName(ImgPath);
            ImgDialog.FileName = Path.GetFileName(ImgPath);

            ImgFolder = ImgDialog.InitialDirectory;
            ImgFile = ImgDialog.FileName;
        }

        private void UpdateOut()
        {
            if (string.IsNullOrWhiteSpace(OutPath))
            {
                return;
            }

            if (CustomName)
            {
                OutDialog.InitialDirectory = Path.GetDirectoryName(OutPath);
                OutDialog.FileName = Path.GetFileName(OutPath);

                OutFolder = Path.GetDirectoryName(OutPath);
                OutFile = OutDialog.FileName;
                return;
            }

            OutFolder = Path.GetDirectoryName(OutPath);
            OutFile = Path.GetFileName(OutPath);
        }

        private void UpdateOutFile()
        {
            if (string.IsNullOrWhiteSpace(OutPath) == false)
            {
                OutPath = SetExtension(Path.GetDirectoryName(OutPath), ImgFile);
            }
            else if (CustomName)
            {
                OutDialog.FileName = SetExtension(string.Empty, ImgDialog.FileName);
            }
        }

        private string SetExtension(string folderBase, string baseFile)
        {
            var newOutFilename = Path.GetFileNameWithoutExtension(baseFile);
            if (encoderSelected == EncoderOptions.PngIndexed)
            {
                newOutFilename += "." + "Png".ToLower();
            }
            else
            {
                newOutFilename += "." + encoderSelected.ToString().ToLower();
            }

            newOutFilename = Path.Combine(folderBase, newOutFilename);

            return newOutFilename.NormalizePath();
        }

        private bool IsInvalidInputs()
        {
            if (string.IsNullOrWhiteSpace(ImgPath) || string.IsNullOrWhiteSpace(OutPath)
            || ImgPath == OutPath)
            {
                InvalidFile();
                return true;
            }

            return false;
        }
        #endregion
    }
}