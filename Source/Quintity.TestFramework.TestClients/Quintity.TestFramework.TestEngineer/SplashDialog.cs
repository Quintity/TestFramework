using System.Reflection;
using System.Windows.Forms;
//using Microsoft.VisualBasic.ApplicationServices;
//using System.Reflection;


namespace Quintity.TestFramework.TestEngineer
{
    /// <summary>
    /// Summary description for SplashDlg.
    /// </summary>
    public partial class SplashDialog : System.Windows.Forms.Form
    {
        private Timer m_timerSplash;
        private Label m_copyright;
        private PictureBox m_pbSplash;
        private Label m_version;

        public SplashDialog(bool bSetTimer)
        {
            // Initialize setup.
            InitializeComponent();

            // Get assembly copyright and version to display
            var assembly = Assembly.GetExecutingAssembly();

            // Get version
            this.m_version.Text = $"Version {assembly.GetName().Version}";
            // Get copyrights
            var copyRight = assembly.GetCustomAttribute(typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
            this.m_copyright.Text = copyRight.Copyright;

            // Set timer for splash screen.
            this.m_timerSplash.Enabled = bSetTimer;
        }

        #region Windows Form Designer generated code

        #endregion

        private void m_timerSplash_Tick(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void m_pbSplash_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
