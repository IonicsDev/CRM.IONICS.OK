$(document).ready(function () {
    $('#linkToPost').click(function () {
        var container = $(this).parent().parent();
        if (!validateDetalle(container)) {
            return;
        }

        var cuentaId = $(container).find('select#Cuenta_Id').val();
        var glosa = $(container).find('#txtGlosa').val();
        var centroCostoId = $(container).find('select#CentroCosto_Id').val();;
        var cargoOperacionalId = $(container).find('select#CargoOperacional_Id').val();;
        var control_valorDebito = $(container).find('#txtDebe');
        var control_valorCredito = $(container).find('#txtHaber')
        var valorDebito = $(control_valorDebito).decimal_value();
        var valorCredito = $(control_valorCredito).decimal_value();
        var t = $('table#tblDetails tbody');
        var rowIndex = $(t).find('tr.detail_data').length;
        var empty_row = $(t).find('tr.grid-empty-text');

        if ($(empty_row).length > 0) {
            $(empty_row).find('td').text("Cargando primera fila...");
        }

        $.get('/AsientoDiario/getRowDetail/', {
            cuentaId: cuentaId,
            glosa: glosa,
            centroCostoId: centroCostoId,
            cargoOperacionalId: cargoOperacionalId,
            valorDebito: valorDebito,
            valorCredito: valorCredito,
            rowIndex: rowIndex
        }, function (response) {
            $(t).append(response);
            $(empty_row).remove();
            var row = $(t).find('tr#' + rowIndex);
            estilizar_combos(row);
            actualizar_totales();
            limpiar_form();
        }, "html");

        return false;
    });

    // quick validation
    $('#_submit_asentar').click(function () {
        var suma_debito = $('#_suma_detalle_debito').decimal_value();
        var suma_credito = $('#_suma_detalle_credito').decimal_value();

        if (!(suma_debito > 0 && suma_credito > 0)) {
            alert("El asiento está descuadrado. No se puede asentar.");
            return false;
        }
        if ((suma_debito != suma_credito)) {
            alert("El asiento está descuadrado. No se puede asentar.");
            return false;
        }
    });

    // avoid the enter submit form
    $(document).on('keypress', 'tr.detail_data input[type=text]', function (e) {
        var code = e.keyCode || e.which;
        var row = $(this).parent().parent().parent();
        if (code == 13) {
            finalizar_edicion_fila(row);
            return false;
        }
    });

    // editing row-text events
    $(document).on('blur', 'tr.detail_data input[type=text]', function (e) {
        var container = $(this).parent().parent();
        var shower = $(container).find('.showing');
        var text = $(this).val();
        $(shower).text(text);
        return false;
    });

    // editing select#CuentaId events
    $(document).on('change', 'tr.detail_data td select#Cuenta_Id', function () {
        var row = $($(this).parents()[3]);
        var comboCentrosCosto = $(row).find('select.comboCentroCosto');
        var comboCargosOperacionales = $(row).find('select.comboCargoOperacional');
        var cuentaId = $(this).val();
        relacionar_combos(cuentaId, comboCentrosCosto, comboCargosOperacionales);
    });

    // editing select#CuentaId events
    $(document).on('change', 'tr#controls_container td select#Cuenta_Id', function () {
        var row = $(this).parent().parent();
        var comboCentrosCosto = $(row).find('select#CentroCosto_Id');
        var comboCargosOperacionales = $(row).find('select#CargoOperacional_Id');
        var cuentaId = $(this).val();
        relacionar_combos(cuentaId, comboCentrosCosto, comboCargosOperacionales);
    });


    // editing row-select events
    $(document).on('change', 'tr.detail_data td', function () {
        var select = $(this).find('select');
        var shower = $(this).find('.showing');
        if ($(select).val() == 0) {
            $(shower).text('');
            return;
        }
        var text = $(select).find('option:selected').text();
        $(shower).text(text);
    });

    // row edit handler
    $(document).on('click', '.edit_row_detalle', function () {
        var container = $(this).parent().parent();
        $(container).find('.showing').hide();
        $(container).find('.edition').show();
        return false;
    });

    // row remove handler
    $(document).on('click', '.remove_row_detalle', function () {
        if (!confirm("¿Está seguro que desea eliminar el detalle?"))
            return;
        var container = $(this).parent().parent();
        var hdn_deleted = $(container).find('.hdn_deleted');
        var valorDebito = $(container).find('#txtDebe').decimal_value();
        var valorCredito = $(container).find('#txtHaber').decimal_value();
        $(hdn_deleted).val("True");
        $(container).hide();
        actualizar_totales();
    });

    // row cancel handler
    $(document).on('click', '.finish_row_edition', function () {
        var row = $(this).parent().parent();
        finalizar_edicion_fila(row);
    });

    $(document).on('keypress', '#tblDetails tfoot tr td input[type=text]', function (e) {
        var code = e.keyCode ? e.keyCode : e.which;
        var row = $($(this).parents()[1]);
        if (code == 13) {
            $(row).find('#linkToPost').click();
            return false;
        }
    });

    estilizar_combos($('tr#controls_container'));
});

function relacionar_combos(cuentaId, comboCentrosCosto, comboCargosOperacionales, centroCosto_id, cargoOperacional_id) {
    // limpiamos los combos
    $(comboCentrosCosto).html("");
    $(comboCargosOperacionales).html("");
    centroCosto_id = (centroCosto_id && centroCosto_id > 0) ? centroCosto_id : 0;
    cargoOperacional_id = (cargoOperacional_id && cargoOperacional_id) ? cargoOperacional_id : 0;

    // cargamos los combos
    $.get('/AsientoDiario/GetCentrosYCargos', { cuentaId: cuentaId }, function (data) {
        var op = $('<option></option>'); // generic control 
        var centrosCosto = data.centrosCosto;
        var cargosOperacionales = data.cargosOperacionales;

        if (centrosCosto.length < 1 || cargosOperacionales < 1) {
            $(comboCentrosCosto).removeAttr('validate');
            $(comboCargosOperacionales).removeAttr('validate');

            $(comboCentrosCosto).append($(op).clone().val(null)).change();
            $(comboCargosOperacionales).append($(op).clone().val(null)).change();
            return;
        }

        if (data.related) {
            $(comboCentrosCosto).attr('validate', true);
            $(comboCargosOperacionales).attr('validate', true);
        } else {
            $(comboCentrosCosto).removeAttr('validate');
            $(comboCargosOperacionales).removeAttr('validate');
        }

        // combo Centro de costo
        $(comboCentrosCosto).append($(op).clone().val(0).text("Seleccione..."));
        for (var i = 0; i < centrosCosto.length; i++) {
            var centroCosto = centrosCosto[i];
            var centroCosto_op = $(op).clone().val(centroCosto.Id).text(centroCosto.Descripcion);
            $(comboCentrosCosto).append(centroCosto_op);
        }

        // combo Cargo operacional
        $(comboCargosOperacionales).append($(op).clone().val(0).text("Seleccione..."));
        for (var i = 0; i < cargosOperacionales.length; i++) {
            var cargoOperacional = cargosOperacionales[i];
            var cargoOperacional_op = $(op).clone().val(cargoOperacional.Id).text(cargoOperacional.Descripcion);
            $(comboCargosOperacionales).append(cargoOperacional_op);
        }

        // selecciona valor por defecto
        $(comboCentrosCosto).val(centroCosto_id).change();
        $(comboCargosOperacionales).val(cargoOperacional_id).change();
    }, 'json');
}

function enfocar(td) {
    var element = null;

    element = $(td).find('input');
    if ($(element).length == 1) {
        $(element).focus();
        return;
    }

    element = $(td).find('select');
    if ($(element).length == 1) {
        $(element).focus();
        return;
    }
}

function estilizar_combos(container) {
    $(container).find("select").select2();
}

function limpiar_form() {
    var container = $('tr#controls_container');
    $(container).find('select').val(0);
    $(container).find('input#txtGlosa').val('');
    $(container).find('input#txtDebe, input#txtHaber').val(0);

    $(container).find('select').each(function (i, s) {
        var t = $(s).find('option:selected').text();
        $(container).find('div.select2-container a span').text(t);
    });
}

function finalizar_edicion_fila(row) {
    if (!validateDetalle(row)) {
        return;
    }
    $(row).find('.showing').show();
    $(row).find('.edition').hide();
    actualizar_totales();
    return false;
}

function validateDetalle(container) {
    // get controls
    var cuenta = $(container).find('#Cuenta_Id');
    var glosa = $(container).find('#txtGlosa');
    var centroCosto = $(container).find('#CentroCosto_Id');
    var cargoOperacional = $(container).find('#CargoOperacional_Id');
    var control_valorDebito = $(container).find('#txtDebe');
    var control_valorCredito = $(container).find('#txtHaber');

    // get values
    var valorDebito = $(control_valorDebito).decimal_value();
    var valorCredito = $(control_valorCredito).decimal_value();
    var validateCentroCosto = $(centroCosto).attr('validate');
    var validatecargoOperacional = $(cargoOperacional).attr('validate');

    // todo: parseInt centroCosto & cargoOperacional
    if ($(cuenta).val() == 0) {
        alert('Debe especificar la cuenta');
        $(cuenta).focus();
        return false;
    } else if ($.trim($(glosa).val()) == '') {
        alert('Debe especificar la glosa');
        $(glosa).focus();
        return false;
    } else if (validateCentroCosto && $(centroCosto).val() < 1) {
        alert("Debe especificar el centro de costo");
        $(centroCosto).focus();
        return;
    } else if (validatecargoOperacional && $(cargoOperacional).val() < 1) {
        alert("Debe especificar el cargo operacional");
        $(cargoOperacional).focus();
        return;
    } else if (!$(control_valorDebito).validate_decimal()) {
        alert('Formato incorrecto. Ej: 324.23 (Hasta 3 decimales)');
        $(control_valorDebito).focus();
        return false;
    } else if (!$(control_valorCredito).validate_decimal()) {
        alert('Formato incorrecto. Ej: 324.23 (Hasta 3 decimales)');
        $(control_valorCredito).focus();
        return false;
    } else if (valorDebito == 0 && valorCredito == 0) {
        alert("Debe ingresar un valor en el debe o en el haber");
        return false;
    } else if (valorDebito > 0 && valorCredito > 0) {
        alert("Debe ingresar solamente un valor (En el debe o en el haber)");
        return false;
    }

    return true;
}

function actualizar_totales() {
    // define controls
    var lbl_numero_registros = $('#label_numero_registros');
    var lbl_suma_detalle_debito = $('#label_suma_detalle_debito');
    var lbl_suma_detalle_credito = $('#label_suma_detalle_credito');
    var hdn_numero_registros = $('#_numero_registros');
    var hdn_suma_detalle_debito = $('#_suma_detalle_debito');
    var hdn_suma_detalle_credito = $('#_suma_detalle_credito');
    var current_numero_registros = $(hdn_numero_registros).val();
    var current_suma_detalle_debito = 0;
    var current_suma_detalle_credito = 0;

    // suma credito & debito
    $('#tblDetails tr.detail_data').each(function () {
        var textBox_debito = $(this).find('input[type=text]#txtDebe');
        var textBox_credito = $(this).find('input[type=text]#txtHaber');
        var deleted = $(this).find('input[type=hidden].hdn_deleted').val() == "True";
        if (!deleted) {
            current_suma_detalle_debito += $(textBox_debito).decimal_value();
            current_suma_detalle_credito += $(textBox_credito).decimal_value();
        }
    });

    // prepare data to the user view
    var numeroRegistros = $('#tblDetails tr.detail_data .hdn_deleted[value=False]').length;
    var server_numero_registros = get_server_decimal(numeroRegistros);
    var server_suma_detalle_debito = get_server_decimal(current_suma_detalle_debito);
    var server_suma_detalle_credito = get_server_decimal(current_suma_detalle_credito);

    // pass data to user view
    $(hdn_numero_registros).val(server_numero_registros);
    $(hdn_suma_detalle_debito).val(server_suma_detalle_debito);
    $(hdn_suma_detalle_credito).val(server_suma_detalle_credito);
    $(lbl_numero_registros).text(server_numero_registros);
    $(lbl_suma_detalle_debito).text(server_suma_detalle_debito);
    $(lbl_suma_detalle_credito).text(server_suma_detalle_credito);
}

function get_server_decimal(value) {
    value = value.toString().replace('.', ',');
    return value;
}