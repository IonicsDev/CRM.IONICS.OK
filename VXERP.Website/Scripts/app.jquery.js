(function ($) {
    $.fn.validate_decimal = function () {
        var value = $.trim($(this).is('input') ? $(this).val() : $(this).text());
        var reg = /^\d+(\.\d{1,3})?$/;

        if (!reg.test(value)) {
            return false;
        }
        return true;
    };

    $.fn.decimal_value = function () {
        if ($(this).validate_decimal()) {
            var value = $.trim($(this).is('input') ? $(this).val() : $(this).text());
            value = value.replace(',', '.');
            return parseFloat(value);
        }

        return 0;
    };

    $.fn.validate_integer = function () {
        var value = $.trim($(this).is('input') ? $(this).val() : $(this).text());
        var reg = /^\d+$/;

        if (!reg.test(value)) {
            return false;
        }
        return true;
    }

    $.fn.integer_value = function () {
        if ($(this).validate_integer()) {
            var value = $.trim($(this).is('input') ? $(this).val() : $(this).text());
            return parseInt(value);
        }

        return 0;
    };
})(jQuery);