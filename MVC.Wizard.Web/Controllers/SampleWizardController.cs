using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVC.Wizard.Controllers;
using MVC.Wizard.Web.ViewModels;

namespace MVC.Wizard.Web.Controllers
{
    public class SampleWizardController : WizardController
    {

        public ActionResult Sample()
        {
            InitializeWizard();
            var vm = new SampleWizardViewModel("This is just some demo text to indicate that you can use startup data.");

            return View(vm);
        }

        protected override void ProcessToNext(Wizard.ViewModels.WizardViewModel model)
        {
            // Do here some custom things if you navigate to the next step.

            if (model.GetType() == typeof(SampleWizardViewModel))
            {
                // Check the type so you could use multiple wizards in one controller.
            }
        }

        protected override void ProcessToPrevious(Wizard.ViewModels.WizardViewModel model)
        {
            // Do here some custom things if you navigate to the next step.

            if (model.GetType() == typeof(SampleWizardViewModel))
            {
                // Check the type so you could use multiple wizards in one controller.
            }
        }

    }
}