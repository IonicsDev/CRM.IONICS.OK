$(document).ready(function () {
    $('.i-print-2').click(function () {
        // vars
        var btn = $(this);
        var original_text = $(btn).text();
        var printer = $('<div></div>').attr("id", "printer_dialog");

        // ajax-state
        $(this).text("Imprimiendo...");

        // request to the server
        $.get('/Cuenta/GetCuentasToPrint', {
            idGrupoEmpresarial: $('select#IdGrupoEmpresarial').val(),
            idCompania: $('select#Compania_Id').val(),
            idClaseCuenta: $('select#claseCuenta_Id').val()
        }, function (response) {
            $(btn).text(original_text);
            if (!response) { 
                alert("No se han encontrado resultados para imprimir");
                return;
            }
            $(printer).html(response);
            $(printer).printArea();
        }, 'html');
    });

    $('.minimize').click(); // fix to bootstrap bug
});

