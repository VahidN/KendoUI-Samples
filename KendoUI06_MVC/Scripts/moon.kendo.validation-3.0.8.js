/*!
** ASP.NET MVC and KendoUI validation extensions.
** Copyright © 2012+ Dusan Janosik. MIT license.
*/
(function($, window) {
    if (!window.moon) {
        window.moon = {
            formSelector: 'form[data-val!="false"]',
            inputSelector: ':input:not(:button,[type=submit],[type=reset],[disabled])'
        };
    }

    if (!window.moon.utils) {
        window.moon.utils = {};
    }

    moon.utils.testPattern = function(value, pattern) {
        if (typeof pattern === "string") {
            pattern = new RegExp('^(?:' + pattern + ')$');
        }
        return pattern.test(value);
    };

    moon.utils.splitAndTrim = function(value) {
        return value.replace(/^\s+|\s+$/g, "").split(/\s*,\s*/g);
    };

    moon.utils.getModelPrefix = function(fieldName) {
        return fieldName.substr(0, fieldName.lastIndexOf(".") + 1);
    };

    moon.utils.appendModelPrefix = function(value, prefix) {
        if (value.indexOf("*.") === 0) {
            value = value.replace("*.", prefix);
        }
        return value;
    };

    moon.utils.escapeValue = function(value) {
        return value.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1");
    };

    moon.utils.isNullish = function(value) {
        return value == null || value == "";
    };

    moon.utils.isBool = function(value) {
        return value === true || value === false || value === "true" || value === "false";
    };
}(jQuery, window));
/*
** Common implementation of Moon validators.
*/
(function($) {
    moon.validators = {
        requiredIf: function(thisArg, input, value, params) {
            var otherPropertyId = '#' + params['other'];

            // get the target value
            var targetValue = params['targetvalue'];
            targetValue = (targetValue == null ? '' : targetValue).toString();

            // get the actual value of the other control
            var otherControl = $(otherPropertyId);
            var controlType = otherControl.prop('type');
            var otherValue = otherControl.val();

            if (controlType === 'checkbox') {
                otherValue = otherControl.prop("checked").toString();
            }

            if (targetValue === otherValue) {
                return moon.validators.forceRequired(thisArg, input, value, params);
            }

            return true;
        },
        requiredIfNotEmpty: function(thisArg, input, value, params) {
            var otherPropertyId = '#' + params['other'];

            // get the actual value of the other control
            var otherControl = $(otherPropertyId);
            var controlType = otherControl.prop('type');
            var otherValue = otherControl.val();

            if (controlType === 'checkbox') {
                otherValue = otherControl.prop("checked").toString();
            }

            if ($.trim(otherValue).length > 0) {
                return moon.validators.forceRequired(thisArg, input, value, params);
            }

            return true;
        },
        requiredIfEmpty: function(thisArg, input, value, params) {
            var otherPropertyId = '#' + params['other'];

            // get the actual value of the other control
            var otherControl = $(otherPropertyId);
            var controlType = otherControl.prop('type');
            var otherValue = otherControl.val();

            if (controlType === 'checkbox') {
                otherValue = otherControl.prop("checked").toString();
            }

            if ($.trim(otherValue).length == 0) {
                return moon.validators.forceRequired(thisArg, input, value, params);
            }

            return true;
        },
        forceRequired: function(thisArg, input, value, params) {
            $(input).attr('data-val-required', 'force');
            var result = moon.callValidator(thisArg, "required", input, value, params);
            $(input).removeAttr('data-val-required');
            return result;
        }
    };
}(jQuery));
/*
** Initializes MvcValidator. It holds list of ASP.NET MVC validation rules.
*/
if (!moon.kendo) {
    moon.kendo = {};
}
if (!moon.kendo.validator) {
    moon.kendo.validator = {};
}
moon.kendo.mvcValidator = new MvcValidator();
moon.kendo.validator.allRules = {};

function MvcValidator() {
    this.rules = {};
    var that = this;

    function createMessage(ruleName) {
        return function(input) {
            return input.attr("data-val-" + ruleName);
        };
    }

    function createRule(ruleName) {
        return function(input) {
            if (input.filter("[data-val-" + ruleName + "]").length) {
                return that.rules[ruleName](input, extractParams(input, ruleName));
            }
            return true;
        };
    }

    function extractParams(input, ruleName) {
        var params = {}, index, data = input.data(),
            length = ruleName.length, rule, key;

        for (key in data) {
            rule = key.toLowerCase();
            index = rule.indexOf(ruleName);
            if (index > -1) {
                rule = rule.substring(index + length, key.length);
                if (rule) {
                    params[rule] = data[key];
                }
            }
        }
        return params;
    }

    function getValidatorOptions() {
        return {
            rules: that.getValidatorRules(),
            messages: that.getValidatorMessage(),
            errorTemplate: '<span>${message}</span>'
        };
    }

    that.getValidatorRules = function() {
        var name, result = {};

        for (name in that.rules) {
            result["mvc" + name] = createRule(name);
        }
        return result;
    };

    that.getValidatorMessage = function() {
        var name, result = {};

        for (name in that.rules) {
            result["mvc" + name] = createMessage(name);
        }
        return result;
    };

    that.init = function(formSelector) {
        var options = getValidatorOptions();
        var validators = $(formSelector).kendoValidator(options).data('kendoValidator');

        $(validators).each(function() {
            $.extend(moon.kendo.validator.allRules, this.options.rules);
        });
    };
}
/*
** Registers ASP.NET MVC and Moon validators for KendoUI.
*/
(function ($) {
    $.extend(true, moon.kendo.mvcValidator, {
        rules: {
            required: function (input) {
                var value = input.val(),
                    checkbox = input.filter('[type="checkbox"]'),
                    select = input.filter('select');

                if (checkbox.length) {
                    var hidden = checkbox.next("input:hidden[name=" + checkbox[0].name + "]");
                    if (hidden.length) {
                        value = hidden.val();
                    } else {
                        value = input.attr("checked") === "checked";
                    }
                } else if (select.length) {
                    value = value && value.length > 0;
                }

                return !($.trim(value) === "" || !value);
            },
            number: function (input) {
                var value = input.val();
                return value === "" || (kendo.parseFloat(value) !== null && !isNaN(Number(value.replace(',', '.'))));
            },
            digits: function (input) {
                return input.val() === "" || this.regex(input, { pattern: /^\d+$/ });
            },
            integer: function (input) {
                return input.val() === "" || this.regex(input, { pattern: /^-?\d+$/ });
            },
            regex: function (input, params) {
                return moon.utils.testPattern(input.val(), params.pattern);
            },
            range: function (input, params) {
                return (!params.min || this.min(input, params)) &&
                    (!params.max || this.max(input, params));
            },
            min: function (input, params) {
                var value = input.val();

                var min = parseFloat(params.min) || 0,
                    num = parseFloat(value);

                return value === "" || min <= num;
            },
            max: function (input, params) {
                var value = input.val();

                var max = parseFloat(params.max) || 0,
                    num = parseFloat(value);

                return value === "" || num <= max;
            },
            length: function (input, params) {
                return (!params.min || this.minlength(input, params)) &&
                    (!params.max || this.maxlength(input, params));
            },
            minlength: function (input, params) {
                var len = $.trim(input.val()).length;
                return len == 0 || len >= params.min;
            },
            maxlength: function (input, params) {
                var len = $.trim(input.val()).length;
                return len <= params.max;
            },
            email: function (input) {
                var emailPattern = /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i;
                return input.val() === "" || this.regex(input, { pattern: emailPattern });
            },
            url: function (input) {
                var urlPattern = /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i;
                return input.val() === "" || this.regex(input, { pattern: urlPattern });
            },
            date: function (input) {
                return input.val() === "" || kendo.parseDate(input.val(), input.attr(kendo.attr("format"))) !== null;
            },
            requiredif: function (input, params) {
                return moon.validators.requiredIf(null, input, input.val(), params);
            },
            requiredifnotempty: function (input, params) {
                return moon.validators.requiredIfNotEmpty(null, input, input.val(), params);
            },
            requiredifempty: function (input, params) {
                return moon.validators.requiredIfEmpty(null, input, input.val(), params);
            },
            compare: function (input, params) {
                var otherPropertyId = '#' + params['other'];
                var operator = params["operator"].toLowerCase();

                // get values to compare
                var value = input.val();
                var otherValue = $(otherPropertyId).val();

                // return true if one of values is empty
                if (value === "" || otherValue === "") {
                    return true;
                }

                // try to parse values
                var dateFormat = input.attr(kendo.attr("format"));
                var dateValue = kendo.parseDate(value, dateFormat);
                var numericValue = kendo.parseFloat(value);

                if (dateValue !== null) {
                    value = dateValue;
                    otherValue = kendo.parseDate(otherValue, dateFormat);
                } else if (numericValue !== null) {
                    value = numericValue;
                    otherValue = kendo.parseFloat(otherValue);
                }

                // compare values
                switch (operator) {
                    case "greaterthan":
                        if (value > otherValue) return true;
                        break;
                    case "greaterthanorequal":
                        if (value >= otherValue) return true;
                        break;
                    case "lessthan":
                        if (value < otherValue) return true;
                        break;
                    case "lessthanorequal":
                        if (value <= otherValue) return true;
                        break;
                    case "equal":
                        if (value == otherValue) return true;
                        break;
                }

                return false;
            }
        }
    });
})(jQuery);
/*
** Registers ASP.NET MVC Remote validator for KendoUI.
*/
(function($) {
    $.extend(true, moon.kendo.mvcValidator, {
        rules: {
            remote: function(input, params) {
                var inputName = input.attr("name"),
                    value = input.val(),
                    prefix = moon.utils.getModelPrefix(inputName),
                    form = input.parents(moon.formSelector),
                    data = {};

                input.attr('data-val-remote-original', input.attr('data-val-remote'));

                if ($.isArray(value)) {
                    value = value.join();
                }

                var remoteValidator = form.data('remoteValidator');
                if (!remoteValidator) {
                    remoteValidator = new RemoteValidator();
                    form.data("remoteValidator", remoteValidator);
                }

                if (remoteValidator.isPending(inputName)) {
                    return "pending";
                }

                var result = remoteValidator.getResult(inputName, value);
                if (result != undefined) {
                    if (!result) {
                        input.attr('data-val-remote', remoteValidator.getMessage(inputName, value));
                    }
                    return result;
                }

                var validator = form.data('kendoValidator');
                if (!validator) {
                    return true;
                }

                $.each(moon.utils.splitAndTrim(params.additionalfields || inputName), function(i, fieldName) {
                    var paramName = moon.utils.appendModelPrefix(fieldName, prefix);
                    data[paramName] = function() {
                        var inputValue = form.find(':input[name="' + moon.utils.escapeValue(paramName) + '"]').val();
                        if ($.isArray(inputValue)) {
                            inputValue = inputValue.join();
                        }
                        return inputValue;
                    };
                });

                delete params.additionalfields;

                remoteValidator.startRequest(inputName);
                data[inputName] = value;

                $.ajax({
                    url: params.url,
                    mode: 'abort',
                    port: 'validate' + inputName,
                    dataType: "json",
                    data: data,
                    success: function(response) {
                        if (response === true || response === 'true') {
                            remoteValidator.setResult(inputName, value, true);
                        } else if (response === false || response === 'false') {
                            remoteValidator.setResult(inputName, value, false, input.attr('data-val-remote-original'));
                        } else {
                            remoteValidator.setResult(inputName, value, false, response);
                        }

                        remoteValidator.stopRequest(inputName);
                        validator.validateInput(input);
                    }
                });

                return "pending";
            }
        }
    });
})(jQuery);

function RemoteValidator() {
    var results = {};
    var pendingRequests = 0;
    var pending = {};

    this.getResult = function(inputName, value) {
        var item = results[inputName];

        if (!item || item.value != value) {
            delete results[inputName];
            return undefined;
        }

        return item.result;
    };

    this.getMessage = function(inputName, value) {
        var item = results[inputName];

        if (!item || item.value != value) {
            delete results[inputName];
            return undefined;
        }

        return item.message;
    };

    this.setResult = function(inputName, value, result, message) {
        results[inputName] = { value: value, result: result, message: message };
    };

    this.isPending = function(inputName) {
        if (pendingRequests <= 0) {
            this.stopRequest(inputName);
            return false;
        }
        return pending[inputName] != undefined;
    };

    this.startRequest = function(inputName) {
        if (!this.isPending(inputName)) {
            pendingRequests++;
            pending[inputName] = true;
        }
    };

    this.stopRequest = function(inputName) {
        pendingRequests--;
        if (pendingRequests < 0) {
            pendingRequests = 0;
        }
        delete pending[inputName];
    };
}
/*
** Configures and initializes KendoUI Validator.
*/
(function($) {
    if (!moon.kendo) {
        moon.kendo = {};
    }
    moon.kendo.errorImages = {};
    moon.kendo.inputs = {};

    var kendo = window.kendo,
        Validator = kendo.ui.Validator,
        oldValidateInput = Validator.fn.validateInput;

    Validator.fn.validateInput = function(input) {
        input = $(input);

        var parent = input.closest('.input, td'),
            invalidInputs = parent.data('invalidInputs') || [],
            isValid = $.proxy(oldValidateInput, this)(input),
            inputElement = input[0],
            inputIndex;

        if (isValid) {
            inputIndex = invalidInputs.indexOf(inputElement);
            if (inputIndex >= 0) {
                invalidInputs.splice(inputIndex, 1);
            }
        }
        else {
            inputIndex = invalidInputs.indexOf(inputElement);
            if (inputIndex < 0) {
                invalidInputs.push(inputElement);
            }
        }

        parent.toggleClass('k-invalid', invalidInputs.length > 0);
        parent.data('invalidInputs', invalidInputs);
        return isValid;
    };

    moon.callValidator = function(thisArg, ruleName, input) {
        var rules = moon.kendo.validator.allRules;
        var rule = rules['mvc' + ruleName] || rules[ruleName];
        if (rule) {
            return rule(input);
        }
        return true;
    };

    $.extend(true, kendo.ui.validator, {
        messageLocators: {
            mvcLocator: {
                locate: function(element, fieldName) {
                    var placeHolder = locateMessagePlaceHolder(element, fieldName);
                    moon.kendo.errorImages[fieldName] = placeHolder.attr('data-valmsg-image');
                    moon.kendo.inputs[fieldName] = locateInput(element, fieldName);
                    return placeHolder;
                },
                decorate: function(message, fieldName) {
                    var errorMessage = $.trim(message.text()),
                        imageFile = moon.kendo.errorImages[fieldName],
                        input = moon.kendo.inputs[fieldName];

                    input.prop('title', errorMessage);
                    input.closest('.input, td').prop('title', errorMessage);

                    if (imageFile) {
                        message.empty()
                            .attr('data-valmsg-image', imageFile)
                            .append($('<img />')
                                .attr('src', imageFile)
                                .attr('title', errorMessage));
                    }

                    message
                        .addClass("field-validation-error")
                        .attr("data-valmsg-for", fieldName || "");
                }
            }
        }
    });

    function locateMessagePlaceHolder(element, fieldName) {
        var attributeSlector = '[data-valmsg-for="' + moon.utils.escapeValue(fieldName) + '"]';
        return element.find('.field-validation-valid' + attributeSlector + ', .field-validation-error' + attributeSlector);
    }

    function locateInput(element, fieldName) {
        return element.find(':input[name=' + moon.utils.escapeValue(fieldName) + ']');
    }

    $(function() {
        moon.kendo.mvcValidator.init(moon.formSelector);
    });
}(jQuery));
