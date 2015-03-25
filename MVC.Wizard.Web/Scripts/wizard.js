(function ($) {
    $.fn.Wizard = function (options) {

        // Append ul with error messages in li that are not for a specific property
        this.find(".wizard-errors-summary").append('<ul data-bind="foreach: Model.GeneralErrors, visible: Model.GeneralErrors().length > 0"><li data-bind="text: Message"></li></ul>');

        //Set the form of the current wizard
        var $form = $("#" + options.formId);

        //Disable the client validation for a specific form
        function DisableClientValidation($form) {
            ($form).validate().settings.ignore = "*";
        }

        //Enable the client validation for a specific form
        function EnableClientValidation($form) {
            ($form).validate().settings.ignore = ":hidden";
        }

        //Enable or disable client validation
        function SetClientValidation(enableClientValidation) {
            if (enableClientValidation)
                EnableClientValidation($form);
            else
                DisableClientValidation($form);
        }

        function SetServerErrors(errorsList) {
            // Check if client side validation is enabled and if we have errors
            if (errorsList) {
                // Get the validator from the form
                var validator = $form.validate();

                // Remove old errors
                validator.resetForm();

                // Create new errors
                var errors = {};
                for (var i = 0; i < errorsList.length; i++) {
                    if (errorsList[i].MemberName !== '') {
                        errors[errorsList[i].MemberName] = errorsList[i].Message;
                    }
                }

                // Show errors
                validator.showErrors(errors);
            }
        }

        var ViewModel = function (d, m) {
            var self = this;

            self.Model = ko.mapping.fromJS(d, m);

            self.Model.GeneralErrors = ko.computed(function () {
                return ko.utils.arrayFilter(self.Model.Errors(), function (item) {
                    return !!item.MemberName;
                });
            });

            self.Model.CurrentStep = ko.computed(function () {
                return eval("self.Model.Step" + self.Model.StepIndex());
            });

            self.Model.Update = function (element) {
                self.Update(element);
            }

            self.Next = function (element) {
                var validator = $(element).closest("form").validate();

                if ($(element).closest("form").valid()) {
                    self.RoundTrip("NextWizardStep");
                } else {
                    validator.focusInvalid();
                }
            }

            self.Previous = function () {
                self.RoundTrip("PreviousWizardStep");
            }

            self.Update = function (element) {
                var validator = $(element).closest("form").validate();

                if ($(element).closest("form").valid()) {
                    if (self.UpdateOnChange) {
                        self.RoundTrip("UpdateWizardStep", $(element).data("sender"));
                    }
                } else {
                    validator.focusInvalid();
                }
            }

            self.RoundTrip = function (action, sender) {
                self.Model.Sender(sender);

                $.ajax({
                    url: options.url + action,
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: ko.toJSON(self.Model),
                    success: function (data) {
                        self.UpdateOnChange = false;
                        ko.mapping.fromJS(data, self.Model);
                        self.UpdateOnChange = true;

                        //window.location.hash = self.Model.Hash();

                        SetServerErrors(data.Errors);
                        SetClientValidation(self.Model.CurrentStep().EnableClientValidation());
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                    }
                });
            }

            self.MemberHasErrors = function (memberName) {
                return !!self.FirstErrorForMember(memberName);
            }

            self.FirstErrorForMember = function (memberName) {
                if (self.Model.Errors()) {
                    var r = ko.utils.arrayFirst(self.Model.Errors(), function (error) {
                        return error.MemberName() == memberName;
                    });

                    return r ? r.Message() : null;
                }
            }
        }

        var vm = new ViewModel(options.model, options.mapping);

        ko.applyBindings(vm, this[0]);

        vm.UpdateOnChange = true;

        //Set the client validation for the first step because it could be disabled and default is enabled.
        SetClientValidation(vm.Model.CurrentStep().EnableClientValidation());

        return this;
    };
}(jQuery));