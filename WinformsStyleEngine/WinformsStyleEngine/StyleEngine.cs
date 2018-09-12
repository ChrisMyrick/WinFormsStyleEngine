using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;

namespace WinformsStyleEngine
{
    public class StyleEngine : IStyleEngine
    {
        private Theme _theme { get; set; }            

        public StyleEngine(Theme theme)
        {
            _theme = theme; 
        }

        /// <summary>
        /// Read settings from user preferences and update theme properties
        /// </summary>
        public void ApplyStyleDefaults(Form form)
        {
            // select theme object based on user preference
            //Type program = typeof(Program);
            //FieldInfo[] programFields = program.GetFields();
            //_theme = (Theme)programFields.FirstOrDefault(f => f.Name == Properties.Settings.Default.ColorScheme).GetValue(null);
            
            // select background image based on user preference
            var backgroundImages = typeof(Theme.Backgrounds); // Get type pointer
            var fields = backgroundImages.GetFields(); // Obtain all fields      
            //_theme.FormBackgroundImage = (Bitmap)fields.FirstOrDefault(f => f.Name == Properties.Resources.Settings.Default.FormBackgroundImage)?.GetValue(null);

            // assign MDI parent theme overrides
            //_theme.FormBorderStyle = "classic"; // ensure MdiParent is a sizable form regadless of child form user settings

            // apply theme to MDI parent
            ApplyStyle(form);

            // configure theme defaults for mdi child forms
            //foreach (var child in MainMdiForm.MdiChildren)
            //{
            //    child.Close();
            //}
            //_theme.FormBorderStyle = Properties.Settings.Default.FormBorderStyle; // border style of child forms
            //_svmicLightTheme.FormBackgroundImage = null; // only apply the background image to the MdiParent
        }

        public void ApplyStyle(Form formView)
        {
            if (!(formView is Form form)) 
            {
                return;
            }

            // set backgorund color of all forms
            form.BackColor = _theme.FormBackColor;

            // set background image of mdiParent
            if (_theme.FormBackgroundImage != null)
            {
                form.BackgroundImage = _theme.FormBackgroundImage;
                form.BackgroundImageLayout = ImageLayout.Stretch;
            }

            var type = form.GetType();

            // set form border style of all forms
            if (type.Name != "FormMain" && _theme.FormBorderStyle == "web")
            {
                form.FormBorderStyle = FormBorderStyle.None;
            }
            else if (_theme.FormBorderStyle == "classic")
            {
                form.FormBorderStyle = FormBorderStyle.Sizable;
            }

            // handle the OnPaint event for forms
            form.Paint += Form_Paint;


            // apply theme to all child controls
            foreach (var mdi in GetAllChildControls(form).OfType<MdiClient>())
                ApplyTheme(mdi);

            foreach (var tbx in GetAllChildControls(form).OfType<TextBox>())
                ApplyTheme(tbx);

            foreach (var label in GetAllChildControls(form).OfType<Label>())
                ApplyTheme(label);

            foreach (var toolstrip in GetAllChildControls(form).OfType<ToolStrip>())
                ApplyTheme(toolstrip);

            foreach (var button in GetAllChildControls(form).OfType<Button>())
                ApplyTheme(button);

            foreach (var cbx in GetAllChildControls(form).OfType<ComboBox>())
                ApplyTheme(cbx);

            foreach (var dtp in GetAllChildControls(form).OfType<DateTimePicker>())
                ApplyTheme(dtp);

            foreach (var dgv in GetAllChildControls(form).OfType<DataGridView>())
                ApplyTheme(dgv);

            foreach (var tabControl in GetAllChildControls(form).OfType<TabControl>())
                ApplyTheme(tabControl);

            foreach (var gbx in GetAllChildControls(form).OfType<GroupBox>())
                ApplyTheme(gbx);

            foreach (var pnl in GetAllChildControls(form).OfType<Panel>())
                pnl.Paint += Panel_Paint;
        }

        #region"Theme Form"
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            var form = (Form)sender;

            // draw border around form
            ControlPaint.DrawBorder(e.Graphics, form.ClientRectangle, SystemColors.ControlDarkDark, ButtonBorderStyle.Solid);

            // use Form's OnPaint method to apply border to textbox since textbox does not inherit OnPaint event
            foreach (var tbx in GetAllChildControls(form).OfType<TextBox>())
            {
                ApplyTextboxBorder(tbx, e);
            }

            foreach (var cbx in GetAllChildControls(form).OfType<ComboBox>())
            {
                ApplyComboBoxBorder(cbx, e);
            }

            foreach (var dtp in GetAllChildControls(form).OfType<DateTimePicker>())
            {
                ApplyDateTimeBorder(dtp, e);
            }
        }
        #endregion

        #region"Theme Panel"
        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.Black)), panel.DisplayRectangle);

            Point[] points = new Point[4];
            points[0] = new Point(0, 0);
            points[1] = new Point(0, panel.Height);
            points[2] = new Point(panel.Width, panel.Height);
            points[3] = new Point(panel.Width, 0);

            Brush brush = new SolidBrush(_theme.PanelBackColor);
            e.Graphics.FillPolygon(brush, points);


            Brush borderBrush = new SolidBrush(_theme.PanelBorderColor);
            Pen borderPen = new Pen(borderBrush, 2);
            Rectangle rect = new Rectangle(panel.ClientRectangle.X,
              panel.ClientRectangle.Y,
              panel.ClientRectangle.Width - 1,
              panel.ClientRectangle.Height - 1);

            // Draw Borders      
            e.Graphics.DrawRectangle(borderPen, rect);
            borderPen.Dispose();

            // use Panel's OnPaint method to apply border to textbox since textbox does not inherit OnPaint event
            //foreach (var tbx in GetAllChildControls(p).OfType<TextBox>())
            //{
            //    ApplyTextboxBorder(tbx, e);
            //}
        }
        #endregion

        #region"Theme MdiClient"
        void ApplyTheme(MdiClient mdiClient)
        {
            mdiClient.BackColor = _theme.MdiClientBackColor;
            mdiClient.Paint += new PaintEventHandler(MdiClient_OnPaint);
        }

        private void MdiClient_OnPaint(object sender, PaintEventArgs e)
        {
            // apply color bands to toolstrip navigation bar
            if (_theme.ToolStripColorBands == true)
            {
                MdiClient mdiClient = (MdiClient)sender;
                Brush borderBrush1 = new SolidBrush(_theme.MdiClientBackAccentColor1);
                Brush borderBrush2 = new SolidBrush(_theme.MdiClientBackAccentColor2);
                Pen borderPen = new Pen(borderBrush1, 2);
                Rectangle rect = new Rectangle(mdiClient.ClientRectangle.X,
                                               mdiClient.ClientRectangle.Y + 1,
                                               mdiClient.ClientRectangle.Width,
                                               mdiClient.ClientRectangle.Height);

                // Draw two stripes of color at the top of the mdiClient
                e.Graphics.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y));
                borderPen.Brush = borderBrush2;
                e.Graphics.DrawLine(borderPen, new Point(rect.X, rect.Y + 2), new Point(rect.X + rect.Width, rect.Y + 2));
            }
        }
        #endregion

        #region "Theme Textbox"
        void ApplyTheme(TextBox tbx)
        {
            // Note: the textbox border color must be set in its parents' paint event (generally Form or Groupbox)
            // because textboxes do not implement the OnPaint themselves by design.
            tbx.Font = _theme.TextBoxFont;
            tbx.BackColor = _theme.TextBoxBackColor;
            tbx.ForeColor = _theme.TextBoxTextColor;
        }
        void ApplyTextboxBorder(TextBox tbx, PaintEventArgs e)
        {
            var test = tbx.Parent.Name;
            Rectangle rectangle;
            //if (tbx.Parent.Name == "panel" || tbx.Parent.Name == "tabControl")
            //{
            //    rectangle = new Rectangle(tbx.Location.X + tbx.Parent.Location.X, tbx.Location.Y + tbx.Parent.Location.Y, tbx.ClientSize.Width, tbx.ClientSize.Height);
            //}
            //rectangle  = new Rectangle(tbx.Location.X, tbx.Location.Y, tbx.ClientSize.Width, tbx.ClientSize.Height);

            //var location = tbx.PointToScreen(Point.Empty);
            //rectangle = new Rectangle(location.X, location.Y, tbx.ClientSize.Width, tbx.ClientSize.Height);

            //Point locationOnForm = tbx.FindForm().PointToClient(tbx.Parent.PointToScreen(tbx.Location));
            //rectangle = new Rectangle(locationOnForm.X, locationOnForm.Y, tbx.ClientSize.Width, tbx.ClientSize.Height);

            if(tbx.Parent is Form)
            {
                rectangle = new Rectangle(tbx.Location.X, tbx.Location.Y, tbx.ClientSize.Width, tbx.ClientSize.Height);

                // Adding 1 pixel to either side, then adding 3 pixels up top, plus 3 down below. 
                // This will add the necessary padding to mimic the border of a standard textbox.
                rectangle.Inflate(2, 2);

                tbx.BorderStyle = BorderStyle.None;
                ControlPaint.DrawBorder(e.Graphics, rectangle, _theme.TextBoxBorderColor, ButtonBorderStyle.Solid);
            }
            //var location = LocationOnClient(tbx);         
        }
        #endregion

        #region "Theme Button"
        void ApplyTheme(Button btn)
        {
            //btn.Font = _theme.ButtonFont;
            btn.BackColor = _theme.ButtonBackColor;
            btn.ForeColor = _theme.ButtonTextColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.NotifyDefault(false); // disable the focus highlight around the button
            btn.Paint += new PaintEventHandler(Button_Paint);
            btn.MouseEnter += new EventHandler(OnMouseEnter_Button);
            btn.MouseLeave += new EventHandler(OnMouseLeave_Button);
        }
        private void Button_Paint(object sender, PaintEventArgs e)
        {
            Button button = (Button)sender;
            Brush borderBrush = new SolidBrush(_theme.ButtonBorderColor);
            Pen borderPen = new Pen(borderBrush, 2);
            Rectangle rect = new Rectangle(button.ClientRectangle.X,
              button.ClientRectangle.Y,
              button.ClientRectangle.Width - 1,
              button.ClientRectangle.Height - 1);

            // Draw Borders      
            e.Graphics.DrawRectangle(borderPen, rect);
            borderPen.Dispose();
        }
        private void OnMouseEnter_Button(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = _theme.ButtonHoverBackColor;
            button.ForeColor = _theme.ButtonHoverTextColor;
        }
        private void OnMouseLeave_Button(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = _theme.ButtonBackColor;
            button.ForeColor = _theme.ButtonTextColor;
        }
        #endregion

        #region"Theme ComboBox"

        void ApplyTheme(ComboBox cbo)
        {
            cbo.FlatStyle = FlatStyle.Flat;
            cbo.Font = _theme.ComboBoxFont;
            cbo.ForeColor = _theme.ComboBoxTextColor;
            cbo.BackColor = _theme.ComboBoxBackColor;            
        }
        void ApplyComboBoxBorder(ComboBox cbo, PaintEventArgs e)
        {
            Rectangle rectangle;
            if (cbo.Parent is Form)
            {
                rectangle = new Rectangle(cbo.Location.X, cbo.Location.Y, cbo.ClientSize.Width, cbo.ClientSize.Height);

                // Adding 1 pixel to either side, then adding 3 pixels up top, plus 3 down below. 
                // This will add the necessary padding to mimic the border of a standard textbox.
                rectangle.Inflate(2, 1);                
                ControlPaint.DrawBorder(e.Graphics, rectangle, _theme.ComboBoxBorderColor, ButtonBorderStyle.Solid);
            }  
        }
        #endregion

        #region"Theme DateTimePicker"

        void ApplyTheme(DateTimePicker dtp)
        {
            dtp.Font = _theme.DateTimePickerFont;
            dtp.ForeColor = _theme.DateTimePickerTextColor;
            dtp.BackColor = _theme.DateTimePickerBackColor;            
        }
        void ApplyDateTimeBorder(DateTimePicker dtp, PaintEventArgs e)
        {
            Rectangle rectangle;
            if (dtp.Parent is Form)
            {
                rectangle = new Rectangle(dtp.Location.X, dtp.Location.Y, dtp.ClientSize.Width, dtp.ClientSize.Height);

                // Adding 1 pixel to either side, then adding 3 pixels up top, plus 3 down below. 
                // This will add the necessary padding to mimic the border of a standard textbox.
                rectangle.Inflate(1, 1);
                ControlPaint.DrawBorder(e.Graphics, rectangle, _theme.DateTimePickerBorderColor, ButtonBorderStyle.Solid);
            }
        }
        #endregion

        #region "Theme Label"
        void ApplyTheme(Label c)
        {
            c.Font = _theme.LabelFont;
            c.ForeColor = _theme.LabelTextColor;
        }

        #endregion

        #region "Theme GroupBox"
        private void ApplyTheme(GroupBox c)
        {
            c.Font = _theme.GroupBoxTitleFont;
            c.ForeColor = _theme.GroupBoxTitleTextColor;
            c.BackColor = _theme.GroupBoxBackColor;

            // paint the title text and border colors
            c.Paint += new PaintEventHandler(GroupBox_Paint);
        }

        private void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox grpBox = (GroupBox)sender;
            Brush textBrush = new SolidBrush(_theme.GroupBoxTitleTextColor);
            Brush borderBrush = new SolidBrush(_theme.GroupBoxBorderColor);
            Pen borderPen = new Pen(borderBrush);
            SizeF strSize = e.Graphics.MeasureString(grpBox.Text, grpBox.Font);
            Rectangle rect = new Rectangle(grpBox.ClientRectangle.X,
                                           grpBox.ClientRectangle.Y + (int)(strSize.Height / 2),
                                           grpBox.ClientRectangle.Width - 1,
                                           grpBox.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);


            // Clear text and border
            e.Graphics.Clear(grpBox.BackColor);

            // Draw text
            e.Graphics.DrawString(grpBox.Text, _theme.GroupBoxTitleFont, textBrush, grpBox.Padding.Left, 0);

            // Drawing Borders
            //Left
            e.Graphics.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
            //Right
            e.Graphics.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            //Bottom
            e.Graphics.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            //Top1
            e.Graphics.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + grpBox.Padding.Left, rect.Y));
            //Top2
            e.Graphics.DrawLine(borderPen, new Point(rect.X + grpBox.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));

            // apply theme to textboxes that are children of the groupbox
            foreach (var tbx in GetAllChildControls(grpBox).OfType<TextBox>())
            {
                ApplyTextboxBorder(tbx, e);
            }
        }
        #endregion

        #region "Theme ToolStrip"
        private void ApplyTheme(ToolStrip c)
        {
            c.BackColor = _theme.ToolStripBackColor;
            c.ForeColor = _theme.ToolStripTextColor;
            c.Paint += new PaintEventHandler(ToolStrip_OnPaint);

            foreach (ToolStripDropDownButton toolStripButton in c.Items.OfType<ToolStripDropDownButton>())
            {
                toolStripButton.BackColor = _theme.ToolStripBackColor;
                toolStripButton.DropDown.BackColor = _theme.ToolStripBackColor;
            }
            foreach (ToolStripButton toolStripButton in c.Items.OfType<ToolStripButton>())
            {
                toolStripButton.BackColor = _theme.ToolStripButtonBackColor;
                toolStripButton.ForeColor = _theme.ToolStripTextColor;
            }
            foreach (ToolStripTextBox toolStripTextBox in c.Items.OfType<ToolStripTextBox>())
            {
                toolStripTextBox.Font = _theme.ToolStripTextBoxFont;
                toolStripTextBox.BackColor = _theme.ToolStripTextBoxBackColor;
                toolStripTextBox.ForeColor = _theme.ToolStripTextBoxFontColor;
                toolStripTextBox.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        private void ToolStrip_OnPaint(object sender, PaintEventArgs e)
        {
            ToolStrip toolStrip = (ToolStrip)sender;

            // Draw Texture
            if (_theme.ToolStripBackgroundTexture == true)
            {
                HatchBrush brush = new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Gainsboro, toolStrip.BackColor);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // makes the drawing cleaner
                e.Graphics.FillRectangle(brush, toolStrip.ClientRectangle);
            }
        }

        #endregion

        #region"Theme TabControl"
        private void ApplyTheme(TabControl c)
        {
            c.Appearance = TabAppearance.FlatButtons;
            c.BackColor = Color.White;
            foreach (TabPage p in c.TabPages)
            {
                p.BackColor = Color.White;
            }
        }
        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabC = (TabControl)sender;
            TabPage page = tabC.TabPages[e.Index];
            e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);

            Rectangle paddedBounds = e.Bounds;
            int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);
            TextRenderer.DrawText(e.Graphics, page.Text, Theme.Fonts.H7, paddedBounds, page.ForeColor);
        }

        #endregion

        #region"Theme DataGridView"
        private void ApplyTheme(DataGridView c)
        {
            // appearance     
            c.BackgroundColor = _theme.DataGridViewBackgroundColor;
            c.ColumnHeadersDefaultCellStyle.ForeColor = _theme.DataGridViewColumnHeaderForeColor;
            c.ColumnHeadersDefaultCellStyle.BackColor = _theme.DataGridViewColumnHeaderBackColor;
            c.ColumnHeadersDefaultCellStyle.Font = _theme.DataGridViewColumnHeaderFont;
            c.DefaultCellStyle.SelectionBackColor = _theme.DataGridViewSelectedRowBackColor;
            c.DefaultCellStyle.SelectionForeColor = _theme.DataGridViewSelectedRowForeColor;
            c.AlternatingRowsDefaultCellStyle.BackColor = _theme.DataGridViewAlternateRowColor;
            c.BorderStyle = BorderStyle.FixedSingle; // border must be fixed for onPaint to work correctly
            c.Paint += new PaintEventHandler(DataGridView_OnPaint); // paint the border

            // features
            c.AllowUserToResizeColumns = true;
            c.AllowUserToOrderColumns = true;
            c.AllowUserToAddRows = false;
            c.AllowUserToDeleteRows = false;
            c.ReadOnly = true;
            c.SelectionMode = DataGridViewSelectionMode.CellSelect;
            c.MultiSelect = false;
            c.EnableHeadersVisualStyles = false; // must be done to allow theming of column headers      
            c.RowHeadersVisible = false; // remove the default empty column on the left side of the DataGridView

            // remove the flickering that occurs while scrolling datagridView controls
            // by setting DoubleBuffering = true.
            RenderHelper.SetDoubleBuffered(c);
        }

        private void DataGridView_OnPaint(object sender, PaintEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            Brush borderBrush = new SolidBrush(_theme.DataGridViewBorderColor);
            Pen borderPen = new Pen(borderBrush, 1);
            Rectangle rect = new Rectangle(grid.ClientRectangle.X,
              grid.ClientRectangle.Y,
              grid.ClientRectangle.Width - 1,
              grid.ClientRectangle.Height - 1);

            // Draw Borders      
            e.Graphics.DrawRectangle(borderPen, rect);

            borderPen.Dispose();
        }

        #endregion

        private IEnumerable<Control> GetAllChildControls(Control parentControl)
        {
            var stack = new Stack<Control>();
            stack.Push(parentControl);

            while (stack.Any())
            {
                var control = stack.Pop();
                foreach (Control childControl in control.Controls)
                {
                    stack.Push(childControl);
                }

                // yield will return the control and if it matches the type specified, then
                // it will apply theme. If type does not match, then resume.
                yield return control;
            }
        }

        #region "Location Methods"
        public Point GetPositionInForm(Control ctrl)
        {
            Point p = ctrl.Location;
            Control parent = ctrl.Parent;
            while (!(parent is Form))
            {
                p.Offset(parent.Location.X, parent.Location.Y);
                parent = parent.Parent;
            }
            return p;
        }

        private Point LocationOnClient(Control c)
        {
            Point retval = new Point(0, 0);
            for (; c.Parent != null; c = c.Parent)
            { retval.Offset(c.Location); }
            return retval;
        }

        private static Point FindLocation(Control ctrl)
        {
            Point p;
            for (p = ctrl.Location; ctrl.Parent != null; ctrl = ctrl.Parent)
                p.Offset(ctrl.Parent.Location);
            return p;
        }
        #endregion
    }
}


/// <summary>
/// This class will remove the flicker that occurs when scrolling a DataGridView
/// </summary>
public static class RenderHelper
{
    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
    private const int WM_SETREDRAW = 11;

    /// <summary>
    /// A workaround to allow setting the DoubleBuffered property on a DataGridView
    /// </summary>
    /// <param name="control">The Control on which to set DoubleBuffered to true.</param>
    public static void SetDoubleBuffered(Control control)
    {
        // set instance non-public property with name "DoubleBuffered" to true
        typeof(Control).InvokeMember("DoubleBuffered",
          System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance |
          System.Reflection.BindingFlags.NonPublic,
          null, control, new object[] { true });
    }

    /// <summary>
    /// Suspend drawing updates for the specified control. After the control has been updated
    /// call DrawingControl.ResumeDrawing(Control control).
    /// </summary>
    /// <param name="control">The control to suspend draw updates on.</param>
    public static void SuspendDrawing(Control control)
    {
        SendMessage(control.Handle, WM_SETREDRAW, false, 0);
    }

    /// <summary>
    /// Resume drawing updates for the specified control.
    /// </summary>
    /// <param name="control">The control to resume draw updates on.</param>
    public static void ResumeDrawing(Control control)
    {
        SendMessage(control.Handle, WM_SETREDRAW, true, 0);
        control.Refresh();
    }
}

