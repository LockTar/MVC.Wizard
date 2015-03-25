MVC Wizard Readme

Use these steps below to create your wizard or download the NuGet sample package: MVC.Wizard.Sample

1.  Add a MVC Controller to your project and let it inherit from "MVC.Wizard.Controllers.WizardController"
2.  Add a ViewModel to your project and inherit it from "MVC.Wizard.ViewModels.WizardViewModel"
3.  Add for each step in your wizard a class and add a public property (with the name Step1, Step2, Step3 etc.) to your ViewModel to expose the step properties. In this way, each step has it's own validation. 
    Create in the constructor of your viewmodel a new instance of each step.
4.  Create an MVC Action in your controller and construct a new instance of your ViewModel. Send this viewmodel to your view.
5.  Create a view for your action
6.  Add a form element with an id attribute
8.  Add a div with an id attribute. Place inside the div an ul and multiple li elements inside for each step.
8.  Use this code for the buttons to navigate. You can change the text or add css classes for the buttons if you like.
<input data-bind="event: { click: Previous }" type="submit" value="Previous" />
<input data-bind="event: { click: Next.bind($data, $element) }" type="submit" value="Next" />

9.  Create textboxes for each property in the right step in the right li of your lu as your normally would do in MVC. Also add a knockoutjs binding to this textbox. 

Sample with Step1 as property in your inherited WizardViewModel and InitialWizardValue as a string property inside your Step1 class:
<div class="form-group">
    @Html.LabelFor(model => model.Step1.InitialWizardValue, new { @class = "control-label col-md-2" })
    <div class="col-md-10">
        @Html.TextBoxFor(model => model.Step1.InitialWizardValue, new { data_bind = "value: Model.Step1.InitialWizardValue" })
        @Html.ValidationMessageFor(model => model.Step1.InitialWizardValue)
    </div>
</div>

10.  Construct the wizard on document ready:
WizardContainer: The id of the div container around the ul.
YourUniqueWizardForm: The id of the form for posting your wizard to the server.

<script type="text/javascript">
    $(function () {    
        $("#WizardContainer").Wizard({
            formId: "YourUniqueWizardForm",
            model: @(Html.Raw(Json.Encode(Model))),
            url: "/@HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()/",
            mapping: {
                create: function (options)
                {
                    var m = ko.mapping.fromJS(options.data);          
                    return m;
                }
            }
        });
    });
</script>

11. Add a validation summary for general messages by placing a div inside your "#WizardContainer". 
General messages are messages that are not for a specific property. This is the same as an ModelState error that has an empty MemberName in MVC.
It is possible to place multiple summaries on the page as long they are in the wizard container.
You can throw a custom error message in the ProcessToNext method in your MVC controller by throwing a System.ComponentModel.DataAnnotations.ValidationException. 
<div class="wizard-errors-summary"></div>