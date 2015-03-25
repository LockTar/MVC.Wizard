using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Wizard.ViewModels
{
    [Serializable]
    public class WizardViewModel
    {
        internal int stepIndex = 1;
        private readonly int numberOfSteps = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardViewModel"/> class.
        /// </summary>
        public WizardViewModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardViewModel"/> class.
        /// </summary>
        /// <param name="numberOfSteps">The number of steps in this wizard.</param>
        public WizardViewModel(int numberOfSteps)
        {
            this.numberOfSteps = numberOfSteps;
        }

        /// <summary>
        /// Gets the step index of the wizard.
        /// </summary>
        /// <value>The index of the step.</value>
        public int StepIndex
        {
            get
            {
                return stepIndex;
            }
        }

        /// <summary>
        /// Gets the number of steps in the wizard.
        /// </summary>
        /// <value>The number of steps.</value>
        public int NumberOfSteps
        {
            get
            {
                return numberOfSteps;
            }
        }

        /// <summary>
        /// Gets the type of the wizard.
        /// </summary>
        /// <value>The type of the wizard so the custom modelbinding knows what kind of object there is posted to the server.
        /// Just return the Type of the class of your ViewModel like this: return GetType().ToString();
        /// </value>
        public virtual string WizardType
        {
            get { return GetType().ToString(); }
        }

        public string Sender { get; set; }

        /// <summary>
        /// Gets the errors of the current step.
        /// </summary>
        /// <value>The errors.</value>
        public List<WizardValidationResult> Errors { get; internal set; }

        /// <summary>
        /// Sets the step index of the stap.
        /// </summary>
        /// <param name="index">The index to set.</param>
        internal void SetStepIndex(int index)
        {
            stepIndex = index;
        }

    }
}
