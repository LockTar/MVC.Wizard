using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MVC.Wizard.ModelBinders
{
    public class WizardModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            // Get the type out of the binding context of the posted viewmodel
            var stepTypeValue = bindingContext.ValueProvider.GetValue("WizardType");

            // Search for the type in the loaded assemblies of this application
            var stepType = GetTypeByTypeName((string)stepTypeValue.ConvertTo(typeof(string)));

            // Create an instance of the type
            var step = Activator.CreateInstance(stepType);

            // Bind the model data
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => step, stepType);
                        
            return step;
        }

        public static Type GetTypeByTypeName(String typeName)
        {
            foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = currentassembly.GetType(typeName, false, true);
                if (t != null) { return t; }
            }

            throw new ArgumentException("Can't find the specified type in the loaded assemblies.", typeName);
        }
    }
}
