using System.Windows.Navigation;

namespace ProfilesRNS_Manager
{
    public class WizardPageFunction : PageFunction<WizardResult>
    {
        private WizardData _context;

        public WizardData Context
        {
            get { return _context; }
            set
            {
                _context = value;
                DataContext = value;
            }
        }
    }
}
