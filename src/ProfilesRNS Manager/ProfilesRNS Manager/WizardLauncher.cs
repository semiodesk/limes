using System.Windows.Navigation;

namespace ProfilesRNS_Manager
{
    public class WizardLauncher : PageFunction<WizardResult>
    {
        protected override void Start()
        {
            base.Start();

            NavigationService?.Navigate(new Page1(new WizardData()));
        }
    }
}