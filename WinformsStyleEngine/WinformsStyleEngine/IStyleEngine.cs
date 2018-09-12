//using SVMIC.Policy.Winforms.Views;
using System.Windows.Forms;

namespace WinformsStyleEngine
{
    public interface IStyleEngine
    {
        void ApplyStyle(Form form);

        /// <summary>
        /// Read settings from user preferences and update theme properties
        /// </summary>
        void ApplyStyleDefaults(Form form);
    }
}