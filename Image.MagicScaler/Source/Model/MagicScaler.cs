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

        private string imageFormats = "Image Formats (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";

        private string _originPath;

        private string _destinationPath;

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

            OriginDialog = new OpenFileDialog
            {
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = string.Empty,
                Filter = imageFormats
            };

            DestinationDialog = new SaveFileDialog
            {
                Filter = imageFormats
            };

            CustomName = true;
            OriginFile = string.Empty;
        }

        public delegate void BoolAction(bool enable);

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
            _128x112,
            _320x320,
            _360x512,
            _420x600,
            _512x512,
            _600x600
        }

        public string OriginFolder { get; set; }

        public string OriginFile { get; set; }

        public string DestinationFolder { get; set; }

        public string DestinationFile { get; set; }

        public string OriginPath
        {
            get
            {
                return _originPath;
            }

            set
            {
                _originPath = value;
                UpdateOrigin();
            }
        }

        public string DestinationPath
        {
            get
            {
                return _destinationPath;
            }

            set
            {
                _destinationPath = value;
                UpdateDestination();
            }
        }

        private OpenFileDialog OriginDialog { get; set; }

        private SaveFileDialog DestinationDialog { get; set; }

        private bool CustomName { get; set; }

        public async Task<bool> Resize()
        {
            if (IsInvalidInputs())
            {
                return false;
            }

            if (sizeSelected == Sizes.Same)
            {
                var imageOrigin = BitmapExtension.SuperFastLoad(OriginPath);
                settings.Width = imageOrigin.Width;
                settings.Height = imageOrigin.Height;
            }

            await Task.Run(() =>
            {
                MagicImageProcessor.ProcessImage(OriginPath, DestinationPath, settings);
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

                DestinationDialog.FileName = SetExtension(string.Empty, OriginDialog.FileName);

                if (string.IsNullOrWhiteSpace(DestinationPath))
                {
                    return;
                }

                DestinationPath = SetExtension(DestinationFolder, DestinationPath);

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
                    settings.EncoderOptions = new GifEncoderOptions(16, null, DitherMode.Auto);
                    break;
                case EncoderOptions.Tiff:
                    settings.EncoderOptions = new TiffEncoderOptions(TiffCompression.Deflate);
                    break;
            }
        }
        #endregion

        #region OriginFileAndDestination
        public bool PickOrigin()
        {
            bool result;

            if (result = OriginDialog.ShowDialog() == DialogResult.OK)
            {
                OriginPath = OriginDialog.FileName.NormalizePath();
                UpdateDestinationFile();
            }

            return result;
        }

        public bool PickDestination()
        {
            if (CustomName)
            {
                DestinationDialog.InitialDirectory = DestinationFolder;
                if (DestinationDialog.InitialDirectory == null)
                {
                    DestinationDialog.InitialDirectory = OriginDialog.InitialDirectory;
                }

                if (DestinationDialog.ShowDialog() == DialogResult.OK)
                {
                    DestinationPath = DestinationDialog.FileName.NormalizePath();

                    DestinationPath = SetExtension(DestinationFolder, DestinationPath);

                    return true;
                }

                return false;
            }

            return false;
        }

        private void UpdateOrigin()
        {
            if (string.IsNullOrWhiteSpace(OriginPath))
            {
                return;
            }

            OriginDialog.InitialDirectory = Path.GetDirectoryName(OriginPath);
            OriginDialog.FileName = Path.GetFileName(OriginPath);

            OriginFolder = OriginDialog.InitialDirectory;
            OriginFile = OriginDialog.FileName;
        }

        private void UpdateDestination()
        {
            if (string.IsNullOrWhiteSpace(DestinationPath))
            {
                return;
            }

            if (CustomName)
            {
                DestinationDialog.InitialDirectory = Path.GetDirectoryName(DestinationPath);
                DestinationDialog.FileName = Path.GetFileName(DestinationPath);

                DestinationFolder = Path.GetDirectoryName(DestinationPath);
                DestinationFile = DestinationDialog.FileName;
                return;
            }

            DestinationFolder = Path.GetDirectoryName(DestinationPath);
            DestinationFile = Path.GetFileName(DestinationPath);
        }

        private void UpdateDestinationFile()
        {
            if (string.IsNullOrWhiteSpace(DestinationPath) == false)
            {
                DestinationPath = SetExtension(Path.GetDirectoryName(DestinationPath), OriginFile);
            }
            else if (CustomName)
            {
                DestinationDialog.FileName = SetExtension(string.Empty, OriginDialog.FileName);
            }
        }

        private string SetExtension(string folderBase, string baseFile)
        {
            var newDestination = Path.GetFileNameWithoutExtension(baseFile);
            if (encoderSelected == EncoderOptions.PngIndexed)
            {
                newDestination += "." + "Png".ToLower();
            }
            else
            {
                newDestination += "." + encoderSelected.ToString().ToLower();
            }

            newDestination = Path.Combine(folderBase, newDestination);

            return newDestination.NormalizePath();
        }

        private bool IsInvalidInputs()
        {
            if (string.IsNullOrWhiteSpace(OriginPath) || string.IsNullOrWhiteSpace(DestinationPath)
            || OriginPath == DestinationPath)
            {
                InvalidFile();
                return true;
            }

            return false;
        }
        #endregion
    }
}