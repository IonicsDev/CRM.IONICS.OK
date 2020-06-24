
    $(function () {
        $(".datepickerTop").datepicker({ format: 'dd/mm/yyyy', autoclose: true, orientation: "top auto" });
        $(".datepicker").datepicker({ format: 'dd/mm/yyyy', autoclose: true });

        $(".multiselect").multiselect();

        $('.datepicker').on('keypress', function (e) {
            var thisVal = $(this).val();
            var leng = thisVal.length;

            if (window.event) {
                code = e.keyCode;
            } else {
                code = e.which;
            };

            var allowedCharacters = [49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 47];
            var isValidInput = false;

            for (var i = allowedCharacters.length - 1; i >= 0; i--) {
                if (allowedCharacters[i] === code) {
                    isValidInput = true;
                }
            };

            if (isValidInput === false ||/* Can only input 1,2,3,4,5,6,7,8,9 or - */
              (code === 45 && (leng < 2 || leng > 5 || leng === 3 || leng === 4)) ||
              ((leng === 2 || leng === 5) && code !== 47) || /* only can hit a - for 3rd pos. */
              leng === 10) /* only want 10 characters "12-45-7890" */ {

                event.preventDefault();
                return;
            }

        });

        function h(e) {
            $(e).css({ 'height': 'auto', 'overflow-y': 'hidden' }).height(e.scrollHeight);
        }
        $('textarea').each(function () {
            h(this);
        }).on('input', function () {
            h(this);
        });
    });