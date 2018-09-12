using System.Drawing;
namespace WinformsStyleEngine
{
    public class Theme
    {
        public static class Fonts
        {
            public static Font H1 = new Font("Arial", 16);
            public static Font H2 = new Font("Arial", 14);
            public static Font H3 = new Font("Arial", 12);
            public static Font H4 = new Font("Arial", 11);
            public static Font H5 = new Font("Arial", 10);
            public static Font H6 = new Font("Arial", 9);
            public static Font H7 = new Font("Arial", 8);
            public static Font H1Bold = new Font("Arial", 16, FontStyle.Bold);
            public static Font H2Bold = new Font("Arial", 14, FontStyle.Bold);
            public static Font H3Bold = new Font("Arial", 12, FontStyle.Bold);
            public static Font H4Bold = new Font("Arial", 11, FontStyle.Bold);
            public static Font H5Bold = new Font("Arial", 10, FontStyle.Bold);
            public static Font H6Bold = new Font("Arial", 9, FontStyle.Bold);
            public static Font H7Bold = new Font("Arial", 8, FontStyle.Bold);
            public static Font H1Italic = new Font("Arial", 16, FontStyle.Italic);
            public static Font H2Italic = new Font("Arial", 14, FontStyle.Italic);
            public static Font H3Italic = new Font("Arial", 12, FontStyle.Italic);
            public static Font H4Italic = new Font("Arial", 11, FontStyle.Italic);
            public static Font H5Italic = new Font("Arial", 10, FontStyle.Italic);
            public static Font H6Italic = new Font("Arial", 9, FontStyle.Italic);
            public static Font H7Italic = new Font("Arial", 8, FontStyle.Italic);
        }

        public static class BrandColors
        {
            public static Color PrimaryPurple = Color.FromArgb(117, 59, 189);
            public static Color PrimaryGreen = Color.FromArgb(0, 177, 64);
            public static Color PrimaryBlack = Color.FromArgb(30, 30, 30);

            public static Color SecondaryYellow = Color.FromArgb(246, 190, 0);
            public static Color SecondaryOrange = Color.FromArgb(246, 141, 46);
            public static Color SecondaryRedOrange = Color.FromArgb(225, 82, 61);
            public static Color SecondaryDarkPurple = Color.FromArgb(51, 0, 114);

            public static Color TertiaryLightGreen = Color.FromArgb(151, 215, 0);
            public static Color TertiaryDarkGreen = Color.FromArgb(0, 106, 82);
            public static Color TertiaryDarkBlue = Color.FromArgb(0, 47, 108);
            public static Color TertiaryLightBlue = Color.FromArgb(155, 203, 235);

            public static Color NeutralWarmGrey = Color.FromArgb(215, 210, 203);
            public static Color NeutralWarmGreyBrown = Color.FromArgb(209, 204, 189);
            public static Color NeutralWarmLightBrown = Color.FromArgb(183, 169, 154);
            public static Color NeutralWarmDarkBrown = Color.FromArgb(116, 102, 97);

            public static Color NeutralCoolLightGrey = Color.FromArgb(208, 211, 212);
            public static Color NeutralCoolMediumGrey = Color.FromArgb(162, 170, 173);
            public static Color NeutralCoolLightBlue = Color.FromArgb(164, 188, 194);
            public static Color NeutralCoolDarkBlue = Color.FromArgb(66, 85, 99);
        }

        public static class Backgrounds
        {
            public static Image LightBlueStripes = Properties.Resources.Background_LightBlueStripes;
            //public static Image Geometric = //Properties.Resources.Background_Geometric;
            //public static Image SimpleGrey = //Properties.Resources.Background_SimpleGrey;
        }

        #region "Properties"

        // button
        public Font ButtonFont { get; set; } = Theme.Fonts.H6;
        public Color ButtonTextColor { get; set; } = Theme.BrandColors.PrimaryBlack;
        public Color ButtonBackColor { get; set; } = Color.White;
        public Color ButtonBorderColor { get; set; } = Theme.BrandColors.PrimaryPurple;
        public Color ButtonHoverBackColor { get; set; } = Theme.BrandColors.SecondaryOrange;
        public Color ButtonHoverTextColor { get; set; } = Color.White;

        // textBox
        public Font TextBoxFont { get; set; } = Theme.Fonts.H3;
        public Color TextBoxTextColor { get; set; } = Theme.BrandColors.PrimaryBlack;
        public Color TextBoxBackColor { get; set; } = Color.White;
        public Color TextBoxBorderColor { get; set; } = Theme.BrandColors.PrimaryPurple;

        // comboBox
        public Font ComboBoxFont { get; set; } = Theme.Fonts.H4;
        public Color ComboBoxTextColor { get; set; } = Theme.BrandColors.PrimaryBlack;
        public Color ComboBoxBackColor { get; set; } = Theme.BrandColors.NeutralCoolLightBlue;
        public Color ComboBoxBorderColor { get; set; } = Theme.BrandColors.PrimaryPurple;  

        // dateTimePicker
        public Font DateTimePickerFont { get; set; } = Theme.Fonts.H4;
        public Color DateTimePickerTextColor { get; set; } = Theme.BrandColors.PrimaryBlack;
        public Color DateTimePickerBackColor { get; set; } = Color.White;
        public Color DateTimePickerBorderColor { get; set; } = Theme.BrandColors.PrimaryPurple;

        // label
        public Font LabelFont { get; set; } = Theme.Fonts.H5;
        public Color LabelTextColor { get; set; } = Theme.BrandColors.PrimaryBlack;
        public Color LabelBackColor { get; set; } = Color.White;

        // groupBox
        public Font GroupBoxTitleFont { get; set; } = Theme.Fonts.H3;
        public Color GroupBoxTitleTextColor { get; set; } = Theme.BrandColors.PrimaryBlack;
        public Color GroupBoxBorderColor { get; set; } = Theme.BrandColors.PrimaryPurple;
        public Color GroupBoxBackColor { get; set; } = Color.White;

        // toolstrip
        public Color ToolStripBackColor { get; set; } = Color.White;
        public Color ToolStripButtonBackColor { get; set; } = Color.White;
        public Color ToolStripTextColor { get; set; } = Color.Black;
        public Color ToolStripTextBoxBackColor { get; set; } = Color.White;
        public Font ToolStripTextBoxFont { get; set; } = Theme.Fonts.H4Italic;
        public Color ToolStripTextBoxFontColor { get; set; } = SystemColors.ControlDarkDark;
        public bool ToolStripBackgroundTexture { get; set; } = true;
        public bool ToolStripColorBands { get; set; } = false;

        // form
        public Color FormBackColor { get; set; } = Color.White;
        public string FormBorderStyle { get; set; } = "classic"; // "web" //Properties.Settings.Default.FormBorderStyle;
        public Image FormBackgroundImage { get; set; } = Theme.Backgrounds.LightBlueStripes;

        // panel
        public Color PanelBackColor { get; set; } = Color.White;
        public Color PanelBorderColor { get; set; } = Theme.BrandColors.PrimaryPurple;
        //public string PanelBorderStyle { get; set; } = ;
        public Image PanelBackgroundImage { get; set; } = Theme.Backgrounds.LightBlueStripes;

        // mdi client control
        public Color MdiClientBackColor { get; set; } = SystemColors.ControlDark;
        public Color MdiClientBackAccentColor1 { get; set; } = Theme.BrandColors.PrimaryGreen;
        public Color MdiClientBackAccentColor2 { get; set; } = Theme.BrandColors.PrimaryPurple;
        public bool MdiClientBackgroundTexture { get; set; } = false;

        // dataGridView
        public Color DataGridViewBackgroundColor { get; set; } = Color.White;
        public Color DataGridViewColumnHeaderBackColor { get; set; } = BrandColors.SecondaryOrange;
        public Color DataGridViewColumnHeaderForeColor { get; set; } = Color.White;
        public Color DataGridViewSelectedRowBackColor { get; set; } = Color.LemonChiffon;
        public Color DataGridViewSelectedRowForeColor { get; set; } = BrandColors.PrimaryBlack;
        public Color DataGridViewAlternateRowColor { get; set; } = Color.Gainsboro;
        public Color DataGridViewBorderColor { get; set; } = BrandColors.PrimaryPurple;
        public Font DataGridViewColumnHeaderFont { get; set; } = Theme.Fonts.H5Bold;


        // Disabled control color
        public Color ControlDisabledColor { get; set; } = Theme.BrandColors.NeutralCoolMediumGrey;

        #endregion
    }
}
