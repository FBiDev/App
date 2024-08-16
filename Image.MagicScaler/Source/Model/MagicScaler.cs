using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhotoSauce.MagicScaler;
using App.Core;
using App.Core.Desktop;

namespace App.Image.MagicScaler
{
    public class MagicScaler
    {
        #region main
        public enum EncoderOptions
        {
            Png = 0,
            PngIndexed = 1,
            Jpg = 2,
            Gif = 3,
            Tiff = 4
        }

        public enum Sizes
        {
            Auto,
            NESHalf_128x112,
            Cover_320x320,
            Cover_360x512,
            Cover_512x512,
            Cover_420x600,
            Cover_600x600
        }

        public enum Interpolation
        {
            Average,
            CatmullRom,
            Cubic,
            CubicSmoother,
            Hermite,
            Lanczos,
            Linear,
            Mitchell,
            NearestNeighbor,
            Quadratic,
            Spline36,
        }

        EncoderOptions EncoderSelected;
        Sizes SizeSelected;
        public ProcessImageSettings Settings;
        public event Action Resized = delegate { };

        PngFilter PngFilter = PngFilter.Unspecified;
        bool PngInterlace;

        int JpgQuality = 98;
        ChromaSubsampleMode JpgChromaSubsample = ChromaSubsampleMode.Subsample444;

        string ImageFormats = "Image Formats (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";

        public MagicScaler()
        {
            Settings = new ProcessImageSettings
            {
                Width = 64,
                Height = 64
                //ResizeMode = CropScaleMode.Stretch,
                //EncoderOptions = new PngEncoderOptions(PngFilter.Unspecified, false),
                //EncoderOptions = new JpegEncoderOptions(98, ChromaSubsampleMode.Subsample444, true),
                //Interpolation = InterpolationSettings.Average,
                //Anchor = CropAnchor.Center,
                //BlendingMode = GammaMode.Linear,
                //HybridMode = HybridScaleMode.FavorQuality,
            };

            SetEncoderOptions();

            dlgOrigin = new OpenFileDialog
            {
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = "",
                Filter = ImageFormats
            };

            dlgDestinationCustom = new SaveFileDialog
            {
                Filter = ImageFormats
            };

            CustomName = true;
            OriginFile = string.Empty;
        }

        public async Task<bool> Resize()
        {
            if (IsInvalidInputs())
                return false;

            if (SizeSelected == Sizes.Auto)
            {
                var imageOrigin = BitmapExtension.SuperFastLoad(OriginPath);
                Settings.Width = imageOrigin.Width;
                Settings.Height = imageOrigin.Height;
            }

            await Task.Run(() =>
            {
                MagicImageProcessor.ProcessImage(OriginPath, DestinationPath, Settings);
            });

            Resized();
            return true;
        }
        #endregion

        #region LoadCombos
        public event Action EncoderChanged = delegate { };
        public delegate void BoolAction(bool enable);
        public event BoolAction EnableAnchor = delegate { };

        public void LoadEncoders(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(EncoderOptions));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                var innerCombo = s as FlatComboBoxNew;
                EncoderSelected = (EncoderOptions)innerCombo.SelectedItem;

                SetEncoderOptions();

                dlgDestinationCustom.FileName = SetExtension("", dlgOrigin.FileName);

                if (string.IsNullOrWhiteSpace(DestinationPath)) return;

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

                Settings.ResizeMode = cboItem;
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
                SizeSelected = (Sizes)innerCombo.SelectedItem;

                switch (SizeSelected)
                {
                    case Sizes.NESHalf_128x112:
                        Settings.Width = 128;
                        Settings.Height = 112;
                        break;
                    case Sizes.Cover_320x320:
                        Settings.Width = 320;
                        Settings.Height = 320;
                        break;
                    case Sizes.Cover_360x512:
                        Settings.Width = 360;
                        Settings.Height = 512;
                        break;
                    case Sizes.Cover_420x600:
                        Settings.Width = 420;
                        Settings.Height = 600;
                        break;
                    case Sizes.Cover_512x512:
                        Settings.Width = 512;
                        Settings.Height = 512;
                        break;
                    case Sizes.Cover_600x600:
                        Settings.Width = 600;
                        Settings.Height = 600;
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
                Settings.Anchor = (CropAnchor)cbo.SelectedItem;
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
                Settings.MatteColor = Color.FromKnownColor((KnownColor)cbo.SelectedItem);
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
                    case Interpolation.Average: Settings.Interpolation = InterpolationSettings.Average; break;
                    case Interpolation.CatmullRom: Settings.Interpolation = InterpolationSettings.CatmullRom; break;
                    case Interpolation.Cubic: Settings.Interpolation = InterpolationSettings.Cubic; break;
                    case Interpolation.CubicSmoother: Settings.Interpolation = InterpolationSettings.CubicSmoother; break;
                    case Interpolation.Hermite: Settings.Interpolation = InterpolationSettings.Hermite; break;
                    case Interpolation.Lanczos: Settings.Interpolation = InterpolationSettings.Lanczos; break;
                    case Interpolation.Linear: Settings.Interpolation = InterpolationSettings.Linear; break;
                    case Interpolation.Mitchell: Settings.Interpolation = InterpolationSettings.Mitchell; break;
                    case Interpolation.NearestNeighbor: Settings.Interpolation = InterpolationSettings.NearestNeighbor; break;
                    case Interpolation.Quadratic: Settings.Interpolation = InterpolationSettings.Quadratic; break;
                    case Interpolation.Spline36: Settings.Interpolation = InterpolationSettings.Spline36; break;
                }
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadBlendingModes(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(GammaMode));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                Settings.BlendingMode = (GammaMode)cbo.SelectedItem;
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadSharpen(FlatComboBox cbo)
        {
            cbo.DataSource = new ListBind<bool> { true, false };

            cbo.SelectedIndexChanged += (s, e) =>
            {
                Settings.Sharpen = (bool)cbo.SelectedItem;
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadColorProfiles(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(ColorProfileMode));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                Settings.ColorProfileMode = (ColorProfileMode)cbo.SelectedItem;
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadHybridModes(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(HybridScaleMode));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                Settings.HybridMode = (HybridScaleMode)cbo.SelectedItem;
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadPngFilters(FlatComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(PngFilter));

            cbo.SelectedIndexChanged += (s, e) =>
            {
                PngFilter = (PngFilter)cbo.SelectedItem;
                SetEncoderOptions();
            };

            cbo.SelectedIndex = 0;
        }

        public void LoadPngInterlaces(FlatComboBox cbo)
        {
            cbo.DataSource = new ListBind<bool> { false, true };

            cbo.SelectedIndexChanged += (s, e) =>
            {
                PngInterlace = (bool)cbo.SelectedItem;

            };

            cbo.SelectedIndex = 0;
        }

        public void LoadJpgQuality(FlatComboBox cbo)
        {
            cbo.DataSource = Enumerable.Range(0, 101).ToList();

            cbo.SelectedIndexChanged += (s, e) =>
            {
                JpgQuality = (int)cbo.SelectedItem;
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
                JpgChromaSubsample = (ChromaSubsampleMode)cbo.SelectedItem;
                SetEncoderOptions();
            };

            cbo.SelectedIndex = 0;
        }

        public void SetEncoderOptions()
        {
            switch (EncoderSelected)
            {
                case EncoderOptions.Png: Settings.EncoderOptions = new PngEncoderOptions(PngFilter, PngInterlace); break;
                case EncoderOptions.PngIndexed: Settings.EncoderOptions = new PngIndexedEncoderOptions(256, null, DitherMode.Auto, PngFilter, PngInterlace); break;
                case EncoderOptions.Jpg: Settings.EncoderOptions = new JpegEncoderOptions(JpgQuality, JpgChromaSubsample); break;
                case EncoderOptions.Gif: Settings.EncoderOptions = new GifEncoderOptions(16, null, DitherMode.Auto); break;
                case EncoderOptions.Tiff: Settings.EncoderOptions = new TiffEncoderOptions(TiffCompression.Deflate); break;
            }
        }
        #endregion

        #region OriginFileAndDestination
        public event Action InvalidFile = delegate { };

        bool CustomName { get; set; }

        OpenFileDialog dlgOrigin;
        SaveFileDialog dlgDestinationCustom { get; set; }

        string _OriginPath;
        public string OriginPath
        {
            get { return _OriginPath; }
            set
            {
                _OriginPath = value;
                UpdateOrigin();
            }
        }
        public string OriginFolder { get; set; }
        public string OriginFile { get; set; }

        string _DestinationPath;
        public string DestinationPath
        {
            get { return _DestinationPath; }
            set
            {
                _DestinationPath = value;
                UpdateDestination();
            }
        }
        public string DestinationFolder { get; set; }
        public string DestinationFile { get; set; }

        void UpdateOrigin()
        {
            if (string.IsNullOrWhiteSpace(OriginPath)) return;

            dlgOrigin.InitialDirectory = Path.GetDirectoryName(OriginPath);
            dlgOrigin.FileName = Path.GetFileName(OriginPath);

            OriginFolder = dlgOrigin.InitialDirectory;
            OriginFile = dlgOrigin.FileName;
        }

        void UpdateDestination()
        {
            if (string.IsNullOrWhiteSpace(DestinationPath)) return;

            if (CustomName)
            {
                dlgDestinationCustom.InitialDirectory = Path.GetDirectoryName(DestinationPath);
                dlgDestinationCustom.FileName = Path.GetFileName(DestinationPath);

                DestinationFolder = Path.GetDirectoryName(DestinationPath);
                DestinationFile = dlgDestinationCustom.FileName;
                return;
            }

            DestinationFolder = Path.GetDirectoryName(DestinationPath);
            DestinationFile = Path.GetFileName(DestinationPath);
        }

        public bool PickOrigin()
        {
            bool result;

            if (result = dlgOrigin.ShowDialog() == DialogResult.OK)
            {
                OriginPath = dlgOrigin.FileName.NormalizePath();
                UpdateDestinationFile();
            }

            return result;
        }

        void UpdateDestinationFile()
        {
            if (string.IsNullOrWhiteSpace(DestinationPath) == false)
                DestinationPath = SetExtension(Path.GetDirectoryName(DestinationPath), OriginFile);
            else if (CustomName)
            {
                dlgDestinationCustom.FileName = SetExtension("", dlgOrigin.FileName);
            }
        }

        public bool PickDestination()
        {
            if (CustomName)
            {
                dlgDestinationCustom.InitialDirectory = DestinationFolder;
                if (dlgDestinationCustom.InitialDirectory == null)
                    dlgDestinationCustom.InitialDirectory = dlgOrigin.InitialDirectory;

                if (dlgDestinationCustom.ShowDialog() == DialogResult.OK)
                {
                    DestinationPath = dlgDestinationCustom.FileName.NormalizePath();

                    DestinationPath = SetExtension(DestinationFolder, DestinationPath);

                    return true;
                }
                return false;
            }
            return false;
        }

        string SetExtension(string folderBase, string baseFile)
        {
            var newDestination = Path.GetFileNameWithoutExtension(baseFile);
            if (EncoderSelected == EncoderOptions.PngIndexed)
                newDestination += "." + "Png".ToLower();
            else
                newDestination += "." + EncoderSelected.ToString().ToLower();

            newDestination = Path.Combine(folderBase, newDestination);

            return newDestination.NormalizePath();
        }

        bool IsInvalidInputs()
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