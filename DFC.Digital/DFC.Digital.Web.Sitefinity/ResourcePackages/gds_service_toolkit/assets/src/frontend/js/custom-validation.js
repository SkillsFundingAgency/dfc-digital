
$(document).ready(function () {
    
    $("button[type='submit']").click(function () {
      
        $('#error-validation-summary .govuk-error-summary__body ul').empty();

        var validator = $('form').validate();
        if ($('form').valid()) {
            $("form").submit(e);
        } else {
            $('#error-validation-summary').show();


            for (var i = 0; i < validator.errorList.length; i++) {
                var linkElement = validator.errorList[i].element.name.split('.').join('_');
                $('#error-validation-summary .govuk-error-summary__body ul').append('<li><a href="#' +
                    linkElement +
                    '">' +
                    validator.errorList[i].message +
                    '</a></li>');
            }

            $('html,body').animate({
                    scrollTop: $("#error-validation-summary").offset().top
                },
                2000);

            return false;
        }
    });

    $("#DateOfBirthDay").change(function () {
        PopulateDateOfBirth();
    });

    $("#DateOfBirthMonth").change(function () {
        PopulateDateOfBirth();
    });

    $("#DateOfBirthYear").change(function () {
        PopulateDateOfBirth();
    });


    var validator = $("form").validate();
    validator.settings.ignore = [];
});


$.validator.setDefaults({
    highlight: function (element) {
        if ($(element).attr('id') === "DateOfBirthDay" ||
            $(element).attr('id') === "DateOfBirthMonth" ||
            $(element).attr('id') === "DateOfBirth" ||
            $(element).attr('id') === "DateOfBirthYear")
        {
            $('#dobDiv').addClass('govuk-form-group--error');
        }
        else
        {
            $(element).closest(".govuk-form-group").addClass("govuk-form-group--error");
        }
    },
    unhighlight: function (element) {
        if ($(element).attr('id') === "DateOfBirthDay" ||
            $(element).attr('id') === "DateOfBirthMonth" ||
            $(element).attr('id') === "DateOfBirth" ||
            $(element).attr('id') === "DateOfBirthYear") {

            var otherValidationErrors = $("#dobDiv").find(".field-validation-error");

            if (otherValidationErrors.length === 0) {
                $('#dobDiv').removeClass('govuk-form-group--error');
            }
        } else {
            $(element).closest(".govuk-form-group").removeClass("govuk-form-group--error");
        }
    }
});

//// *** Date of Birth Validation ***


function PopulateDateOfBirth() {
    var dobDay = $('#DateOfBirthDay').val();
    var dobMonth = $('#DateOfBirthMonth').val();
    var dobDayYear = $('#DateOfBirthYear').val();

    if (dobDay !== "" && dobMonth !== "" && dobDayYear !== "") {
        var dateOfBirth = "";
        if (dobDay || dobMonth || dobDayYear) {
            dateOfBirth = dobDay + '/' + dobMonth + '/' + dobDayYear;
        }

        $('#DateOfBirth').val(dateOfBirth);
        var validator = $("form").validate();
        validator.element("#DateOfBirth");
    }
}



//// *** Custom Attributes area ***
jQuery.validator.addMethod("enforcetrue", function (value, element, param) {
    return element.checked;
});

jQuery.validator.unobtrusive.adapters.addBool("enforcetrue");

jQuery.validator.addMethod("agerange", function (value, element, param) {
    if (value == "" || value == null || value == undefined) {
        return true;
    }

    var dates = param["dates"];
    var errormessages = param["errormessages"];
    var failedDatesErrorMessages = new Array();

    var minAge = parseInt(dates[0]);
    var maxAge = parseInt(dates[1]);

    var minAgeErrorMessage = errormessages[0];
    var maxAgeErrorMessage = errormessages[1];
    var invalidAgeErrorMessage = errormessages[2];

    var dateParts = value.split('/');
    var entryDay = parseInt(dateParts[0]);
    var entryMonth = parseInt(dateParts[1]) - 1;
    var entryYear = parseInt(dateParts[2]);
    var entryDate = new Date(entryYear, entryMonth, entryDay);

    if (entryDate.getFullYear() == entryYear && entryDate.getMonth() == entryMonth && entryDate.getDate() == entryDay) {

        if (Object.prototype.toString.call(entryDate) === "[object Date]") {
            // it is a date type
            if (isNaN(entryDate.getTime())) {  // entryDate.valueOf() could also work
                // date is not valid
                failedDatesErrorMessages[0] = invalidAgeErrorMessage;
            }
            else {
                // date is valid
                var minYear = entryYear + minAge;
                var minAgeDate = new Date(minYear, entryMonth, entryDay);

                var maxYear = entryYear + maxAge;
                var maxAgeDate = new Date(maxYear, entryMonth, entryDay);

                var today = new Date();
                today.setHours(0, 0, 0, 0);

                if (minAgeDate > today) {
                    failedDatesErrorMessages[0] = minAgeErrorMessage;
                }
                else if (maxAgeDate < today) {
                    failedDatesErrorMessages[0] = maxAgeErrorMessage;
                }
                else {
                    return true;
                }
            }
        }
        else {
            // not a date
            failedDatesErrorMessages[0] = invalidAgeErrorMessage;
        }
    }
    else {
        // not valid date - day, month, year don't match before and after Date parsing
        failedDatesErrorMessages[0] = invalidAgeErrorMessage;
    }
    if (failedDatesErrorMessages.length > 0) {

        $.validator.messages.agerange = failedDatesErrorMessages.toString();
        return false;
    }
});

$.validator.unobtrusive.adapters.add('agerange', ['dates', 'errormessages'], function (options) {
    options.rules['agerange'] = {
        dates: options.params['dates'].split(' '),
        errormessages: options.params['errormessages'].split(',')
    };
});

jQuery.validator.addMethod("daterange", function (value, element, param) {
    if (value == "" || value == null || value == undefined) {
        return true;
    }

    var dates = param["dates"];
    var errormessages = param["errormessages"];
    var failedDatesErrorMessages = new Array();

    var startDateDay = parseInt(dates[0]);
    var endDateDay = parseInt(dates[1]);

    var dateRangeErrorMessage = errormessages[0];
    var invalidErrorMessage = errormessages[1];

    var dateParts = value.split('-');
    var entryYear = parseInt(dateParts[0]);
    var entryMonth = parseInt(dateParts[1]) - 1;
    var entryDay = parseInt(dateParts[2]);
    var entryDate = new Date(entryYear, entryMonth, entryDay);

    if (entryDate.getFullYear() == entryYear && entryDate.getMonth() == entryMonth && entryDate.getDate() == entryDay) {

        if (Object.prototype.toString.call(entryDate) === "[object Date]") {
            // it is a date type
            if (isNaN(entryDate.getTime())) {  // entryDate.valueOf() could also work
                // date is not valid
                failedDatesErrorMessages[0] = invalidErrorMessage;
            }
            else {
                // date is valid
                var validStartDate = new Date();
                validStartDate.setDate(validStartDate.getDate() - startDateDay - 1);
                var validEndDate = new Date();
                validEndDate.setDate(validEndDate.getDate() + endDateDay);

                if (entryDate >= validStartDate && entryDate <= validEndDate) {
                    return true;
                }
                else {
                    failedDatesErrorMessages[0] = dateRangeErrorMessage;
                }
            }
        }
        else {
            // not a date
            failedDatesErrorMessages[0] = invalidErrorMessage;
        }
    }
    else {
        // not valid date - day, month, year don't match before and after Date parsing
        failedDatesErrorMessages[0] = invalidErrorMessage;
    }

    if (failedDatesErrorMessages.length > 0) {

        $.validator.messages.daterange = failedDatesErrorMessages.toString();
        return false;
    }
});

$.validator.unobtrusive.adapters.add('daterange', ['dates', 'errormessages'], function (options) {
    options.rules['daterange'] = {
        dates: options.params['dates'].split(' '),
        errormessages: options.params['errormessages'].split(',')
    };
});

