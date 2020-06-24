$(document).ready(function () {
    // form presubmit
    $('form').submit(function () {
        var nroFactura = $('input[type=text]#nroFactura_0').val() + "-" +
                         $('input[type=text]#nroFactura_1').val() + "-" +
                         $('input[type=text]#nroFactura_2').val();
        var nroComprobanteReferencia = $('input[type=text]#nroComprobanteReferencia_0').val() + "-" +
                                       $('input[type=text]#nroComprobanteReferencia_1').val() + "-" +
                                       $('input[type=text]#nroComprobanteReferencia_2').val();

        var nroComprobanteRetencion = $('input[type=text]#RetencionFacturaCompra_nroComprobante_0').val() + "-" +
                                      $('input[type=text]#RetencionFacturaCompra_nroComprobante_1').val() + "-" +
                                      $('input[type=text]#RetencionFacturaCompra_nroComprobante_2').val();

        $('input[type=hidden]#NroFactura').val(nroFactura);
        $('input[type=hidden]#NroComprobanteReferencia').val(nroComprobanteReferencia);
        $('input[type=hidden]#RetencionFacturaCompra_NroComprobante').val(nroComprobanteRetencion);
    });

    // ejercicio fiscal handler
    $('input[type=text]#RetencionFacturaCompra_FechaEmision').change(function () {
        obtener_ejercicio_fiscal();
    });

    // clone factura data handler
    $('#nroFactura_0, #nroFactura_1, #nroFactura_2').change(function () {
        var nroFactura = "";
        var nroFactura_0 = $.trim($('#nroFactura_0').val());
        var nroFactura_1 = $.trim($('#nroFactura_1').val());
        var nroFactura_2 = $.trim($('#nroFactura_2').val());

        if ($(this).is('#nroFactura_0'))
            nroFactura_0 = $(this).val();
        if ($(this).is('#nroFactura_1'))
            nroFactura_1 = $(this).val();
        if ($(this).is('#nroFactura_2'))
            nroFactura_2 = $(this).val();

        nroFactura = nroFactura_0 + "-" + nroFactura_1 + "-" + nroFactura_2;
        $('#_numero_comprobante_venta').text(nroFactura);
    });

    $('select#Proveedor_Id').change(function () {
        var proveedor = $(this).find('option:selected').text();
        $('#_nombre_proveedor').text(proveedor);
    });

    // get detail  handler
    $('#_add_detail').click(function () {
        var container = $(this).parent().parent();
        if (!validateDetalle(container)) {
            return;
        }

        var itemId = $(container).find('select#Item_Id').val();
        var unidadMedidaId = $(container).find('input[type=hidden].UnidadMedida_Id').val();
        var cantidad = $(container).find('input.txtCantidad').val();
        var precioUnitario = $(container).find('input.txtPrecioUnitario').decimal_value();
        var total = $(container).find('._total').decimal_value();
        var descuento = $(container).find('input.txtDescuento').decimal_value();
        var porcentajeIva = $(container).find('input[type=hidden].PorcentajeIva').decimal_value();
        var valorIva = $(container).find('._valorIva').decimal_value();
        var subtotal = $(container).find('._subtotal').decimal_value();

        var t = $('table#tblDetails tbody');
        var rowIndex = $(t).find('tr.detail_data').length;
        var empty_row = $(t).find('tr.grid-empty-text');

        $.get('/FacturaCompra/getRowDetail/', {
            itemId: itemId,
            unidadMedidaId: unidadMedidaId,
            cantidad: cantidad,
            precioUnitario: precioUnitario,
            descuento: descuento,
            porcentajeIva: porcentajeIva,
            valorIva: valorIva,
            subtotal: subtotal,
            total: total,
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

    // avoid the enter post for details
    $(document).on('keypress', 'input[type=text]', function (e) {
        var code = e.keyCode || e.which;
        var row = $(this).parent().parent().parent();
        if ($(this).is('tr.detail_data input[type=text]') && code == 13) {
            finalizar_edicion_fila(row);
            return false;
        } else if ($(this).is('#tblDetails tr#controls_container td input[type=text]') && code == 13) {
            $(row).find('#_add_detail').click();
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
        $(hdn_deleted).val("True");
        $(container).hide();
        actualizar_totales();
    });

    // row cancel handler
    $(document).on('click', '.finish_row_edition', function () {
        var row = $(this).parent().parent();
        finalizar_edicion_fila(row);
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

    // item handler
    $(document).on('change', '.comboItem', function () {
        // set container
        var container = $(this).parents()[1];
        if ($(container).is('td'))
            container = $(container).parent();

        // preconditions
        var itemId = $(this).val();
        if (!itemId) {
            $(container).find('._unidadMedida').text('');
            $(container).find('._porcentajeIva').text(0);
            $(container).find('input[type=hidden].UnidadMedida_Id').val(0);
            $(container).find('input[type=hidden].PorcentajeIva').val(0);
            return;
        }

        // get data from server
        $.get('/FacturaCompra/GetItemData/', {
            item_Id: itemId
        }, function (data) {
            $(container).find('._unidadMedida').text(data.unidadMedida.Simbolo);
            $(container).find('input[type=hidden].UnidadMedida_Id').val(data.unidadMedida.Id);
            $(container).find('._porcentajeIva').text(data.item.Impuesto.Porcentaje);
            $(container).find('input[type=hidden].PorcentajeIva').val(data.item.Impuesto.Porcentaje);

            $(container).find('input[type=hidden].hdn_retencion_ir_id').val(data.item.RetencionIR_Id);
            $(container).find('input[type=hidden].hdn_retencion_ir_nombre').val(data.item.RetencionIR.Nombre);
            $(container).find('input[type=hidden].hdn_retencion_ir_porcentaje').val(data.item.RetencionIR.Porcentaje);

            $(container).find('input[type=hidden].hdn_retencion_iva_id').val(data.item.RetencionIVA_Id);
            $(container).find('input[type=hidden].hdn_retencion_iva_nombre').val(data.item.RetencionIR.Nombre);
            $(container).find('input[type=hidden].hdn_retencion_iva_porcentaje').val(data.item.RetencionIVA.Porcentaje);

            calcular_fila(container);
        }, 'json');

        return false;
    });

    // descuento handler
    $('input[type=checkbox]#_chk_suma_descuento').click(function () {
        var checked = $(this).is(':checked');
        var container = $('#_suma_descuento_container');
        show_distribution_form(checked);
    });

    // confirmation
    $('#_confirm_distribution').click(function () {
        if (confirm("¿Esta seguro que desea distribuir el descuento?. \n\n Se modificarán los datos y cálculos de los detalles")) {

            // contar filas no eliminadas para determinar la cantidad a distribuir
            var count = 0;
            var not_deleted_rows = [];
            $('#tblDetails tr.detail_data').each(function () {
                var deleted = $(this).find('input[type=hidden].hdn_deleted').val() == "True";
                if (!deleted) {
                    count++;
                    not_deleted_rows.push($(this));
                }
            });

            // verificamos existencia de filas
            if (count < 0) {
                alert("No hay detalles cargados");
            } else {

                // validamos el valor a distribuir
                if (!$('#_txt_distribucion_descuento').validate_decimal) {
                    alert('Formato incorrecto. Ej: 324.23 (Hasta 3 decimales)');
                    $('_txt_distribucion_descuento').focus();
                    return;
                }

                // distribuimos y asignamos el valor
                var distribuido = parseFloat($('#_txt_distribucion_descuento').val() / count).toFixed(2);
                $(not_deleted_rows).each(function (i, row) {
                    $(row).find('._descuento').text(distribuido);
                    $(row).find('.txtDescuento').val(distribuido);
                    calcular_fila(row);
                });

                actualizar_totales();
            }
        }

        show_distribution_form(false);
    });

    // calcs handlers
    $(document).on('blur', '.txtCantidad, .txtPrecioUnitario, .txtDescuento', function () {
        var container = $(this).parents()[1];
        if ($(container).is('td'))
            container = $(container).parent();

        if ($(this).is('.txtCantidad') && !$(this).validate_integer()) {
            $(this).focus();
            alert('Debe ser un valor entero válido.')
            return;
        }

        calcular_fila(container);
    });


    // initialization
    $("select").select2();
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
});

function obtener_ejercicio_fiscal() {
    var fecha = $('input[type=text]#RetencionFacturaCompra_FechaEmision').val();
    $('#_ejercicio_fiscal_status').removeClass('validation-summary-errors').text(" Obteniendo ejercicio fiscal...");
    $('#RetencionFacturaCompra_EjercicioFiscalId').val('');
    $('#RetencionFacturaCompra_EjercicioFiscalDescripcion').val('');
    $('._descripcion_ejercicio_fiscal').text('');
    $('input[type=hidden]._ejercicio_mes_id').val('');

    $.get('/FacturaCompra/GetEjercicioFiscal/', {
        fecha: fecha
    }, function (ejercicioFiscal) {
        if (!ejercicioFiscal) {
            alert("No se ha encontrado el ejercicio fiscal.");
            return;
        } else if (ejercicioFiscal.error) {
            $('#_ejercicio_fiscal_status').addClass('validation-summary-errors').text(ejercicioFiscal.error);
            return;
        }

        // set data to controls
        $('#_ejercicio_fiscal_status').removeClass('validation-summary-errors').text("");
        $('#RetencionFacturaCompra_EjercicioFiscalId').val(ejercicioFiscal.Id);
        $('input[type=hidden]._ejercicio_mes_id').val(ejercicioFiscal.Id);
        $('#RetencionFacturaCompra_EjercicioFiscalDescripcion').val(ejercicioFiscal.Descripcion);
        $('._descripcion_ejercicio_fiscal').text(ejercicioFiscal.Descripcion);
    }, 'json');

    return false;
}

function calcular_fila(row) {
    // make calculations
    var cantidad = $(row).find('.txtCantidad').integer_value();
    var precioUnitario = $(row).find('.txtPrecioUnitario').decimal_value();
    var descuento = $(row).find('.txtDescuento').decimal_value();
    var porcentajeIva = parseFloat($(row).find('._porcentajeIva').decimal_value() / 100);

    var precioTotal = parseFloat(cantidad * precioUnitario).toFixed(2);
    var precioSubtotal = parseFloat(precioTotal - descuento).toFixed(2);
    var valorIva = parseFloat(precioSubtotal * porcentajeIva).toFixed(2);

    // update data controls
    $(row).find('._total').text(precioTotal);
    $(row).find('.Total').val(precioTotal);
    $(row).find('._subtotal').text(precioSubtotal);
    $(row).find('.Subtotal').val(precioSubtotal);
    $(row).find('._valorIva').text(valorIva);
    $(row).find('.ValorIva').val(valorIva);

    if (!$(row).is('#controls_container')) {
        $(row).find('._precioUnitario').val(precioUnitario);
        $(row).find('._descuento').val(descuento);
    }
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

function limpiar_form() {
    var container = $('tr#controls_container');
    $(container).find('select').val(0).change();
    $(container).find('input[type=text]').val(0);
    $(container).find('input[type=hiden].Total').val(0);
    $(container).find('input[type=hiden].ValorIva').val(0);
    $(container).find('input[type=hiden].Subtotal').val(0);
    $(container).find('._total').text(0);
    $(container).find('._valorIva').text(0);
    $(container).find('._subtotal').text(0);
}

function estilizar_combos(container) {
    $(container).find("select").select2();
}

function validateDetalle(container) {
    // get controls
    var comboItem = $(container).find('select.comboItem');
    var txtCantidad = $(container).find('input.txtCantidad');
    var txtDescuento = $(container).find('input.txtDescuento');
    var txtPrecioUnitario = $(container).find('input.txtPrecioUnitario');

    // get values
    var cantidad = $(txtCantidad).val();
    var descuento = $(txtDescuento).decimal_value();
    var precioUnitario = $(txtPrecioUnitario).decimal_value();

    // todo: parseInt centroCosto & cargoOperacional
    if ($(comboItem).val() == 0) {
        alert('Debe especificar el item');
        $(comboItem).focus();
        return false;
    } else if (!$(txtCantidad).validate_integer()) {
        alert('Debe ingresar un entero positivo.');
        $(txtCantidad).focus();
        return false;
    } else if (!$(txtDescuento).validate_decimal()) {
        alert('Formato incorrecto. Ej: 324.23 (Hasta 3 decimales)');
        $(txtDescuento).focus();
        return false;
    } else if (!$(txtPrecioUnitario).validate_decimal()) {
        alert('Formato incorrecto. Ej: 324.23 (Hasta 3 decimales)');
        $(txtPrecioUnitario).focus();
        return false;
    }

    return true;
}

function actualizar_totales() {
    var suma_valor_iva = 0;
    var suma_subtotal = 0;
    var suma_descuento = 0;

    var suma_base_12 = 0;
    var suma_base_0 = 0;
    var suma_valor_iva_12 = 0;
    var total_complemento = 0;

    // sum values
    $('#tblDetails tr.detail_data').each(function () {
        var deleted = $(this).find('input[type=hidden].hdn_deleted').val() == "True";
        if (!deleted) {
            var valor_iva = $(this).find('._valorIva').decimal_value();
            var subtotal = $(this).find('._subtotal').decimal_value();
            var descuento = $(this).find('.txtDescuento').decimal_value();
            var porcentaje_iva = $(this).find('._porcentajeIva').decimal_value();

            suma_valor_iva += valor_iva;
            suma_subtotal += subtotal;
            suma_descuento += descuento;

            if (porcentaje_iva == 12) {
                suma_base_12 += subtotal;
                suma_valor_iva_12 += valor_iva;
            } else if (porcentaje_iva == 0) {
                suma_base_0 += subtotal;
            } else {
                console.log("Porcentaje de iva no esperado. Los valores esperados eran (12 y 0)");
            }
        }
    });

    // set to controls
    $('#_suma_valor_iva').text(parseFloat(suma_valor_iva).toFixed(2));
    $('#_suma_subtotal').text(parseFloat(suma_subtotal).toFixed(2));
    $('#_suma_descuento').text(parseFloat(suma_descuento).toFixed(2));

    $('#_base_iva_12').text(parseFloat(suma_base_12).toFixed(2));
    $('._base_iva_12').val(parseFloat(suma_base_12).toFixed(2));
    $('#_base_iva_0').text(parseFloat(suma_base_0).toFixed(2));
    $('._base_iva_0').val(parseFloat(suma_base_0).toFixed(2));
    $('#_valor_iva_12').text(parseFloat(suma_valor_iva_12).toFixed(2));
    $('._valor_iva_12').val(parseFloat(suma_valor_iva_12).toFixed(2));
    $('#_total_complemento').text(parseFloat(suma_base_12 + suma_base_0 + suma_valor_iva_12).toFixed(2));

    calcular_retenciones();
}

function calcular_retenciones() {
    var active_rows = getActivesRows();
    var retencion_ir_ids = [];
    var retencion_iva_ids = [];
    var retenciones = [];

    $(active_rows).each(function (i, row) {
        // tomamos las retenciones(ir) de la fila
        var retencion_ir_id = $(row).find('.hdn_retencion_ir_id').val() * 1;
        var retencion_ir_nombre = $(row).find('.hdn_retencion_ir_nombre').val();
        var retencion_ir_porcentaje = $(row).find('.hdn_retencion_ir_porcentaje').val() * 1;

        // tomamos las retenciones(iva) de la fila
        var retencion_iva_id = $(row).find('.hdn_retencion_iva_id').val() * 1;
        var retencion_iva_nombre = $(row).find('.hdn_retencion_iva_nombre').val();
        var retencion_iva_porcentaje = $(row).find('.hdn_retencion_iva_porcentaje').val() * 1;

        // agregamos los datos de las retenciones ir
        if (retencion_ir_ids.indexOf(retencion_ir_id) < 0) {
            retencion_ir_ids.push(retencion_ir_id);
            retenciones.push({
                retencion_id: retencion_ir_id,
                retencion_nombre: retencion_ir_nombre,
                retencion_porcentaje: retencion_ir_porcentaje,
                suma_base_imponible: 0,
                suma_valor_retenido: 0
            });
        }

        // agregamos los datos de las retenciones iva
        if (retencion_iva_ids.indexOf(retencion_iva_id) < 0) {
            retencion_iva_ids.push(retencion_iva_id);
            retenciones.push({
                retencion_id: retencion_iva_id,
                retencion_nombre: retencion_iva_nombre,
                retencion_porcentaje: retencion_iva_porcentaje,
                suma_base_imponible: 0,
                suma_valor_retenido: 0
            });
        }
    });

    // calculamos las retenciones en cada fila
    $(active_rows).each(function (i, row) {
        // obtenemos variables iniciales
        var retencion_ir_id = $(row).find('.hdn_retencion_ir_id').val() * 1;
        var retencion_iva_id = $(row).find('.hdn_retencion_iva_id').val() * 1;
        var subtotal = $(row).find('._subtotal').decimal_value();
        var valor_iva = $(row).find('._valorIva').decimal_value();

        // obtenemos la retención ir
        var retencion_ir = retenciones.filter(function (ret) { return ret.retencion_id === retencion_ir_id; })[0];
        var valor_retenido = parseFloat(valor_iva * (retencion_ir.retencion_porcentaje / 100));
        retencion_ir.suma_base_imponible += subtotal;
        retencion_ir.suma_valor_retenido += valor_retenido;

        // obtenemos y calculamos la retención iva
        var retencion_iva = retenciones.filter(function (ret) { return ret.retencion_id === retencion_iva_id; })[0];
        var valor_retenido = parseFloat(valor_iva * (retencion_iva.retencion_porcentaje / 100));
        retencion_iva.suma_base_imponible += subtotal;
        retencion_iva.suma_valor_retenido += valor_retenido;
    });

    // armamos las filas de retenciones
    var t = $('#tblRetencionesDetails tbody').html('');
    var hdn_base = $('<input type=hidden />');
    var tr_base = $('<tr></tr>');
    var td_base = $('<td></td>');
    var ejercicio_fiscal_descripcion = $('#RetencionFacturaCompra_EjercicioFiscalDescripcion').val();
    var ejercicio_fiscal_id = $('#RetencionFacturaCompra_EjercicioFiscalId').val();
    if (ejercicio_fiscal_id == 0)
        ejercicio_fiscal_id = null;

    var suma_total_valores_retenidos = 0;
    for (var i = 0; i < retenciones.length; i++) {
        var retencion = retenciones[i];

        // set post data
        var name_structure = 'RetencionFacturaCompra.Detalles[' + i + ']';
        var hdn_ejercicio = $(hdn_base).clone().attr('name', name_structure + ".EjercicioMes_Id").val(ejercicio_fiscal_id).addClass('_ejercicio_mes_id');
        var hdn_retencion = $(hdn_base).clone().attr('name', name_structure + ".Retencion_Id").val(retencion.retencion_id);
        var hdn_base_imponible = $(hdn_base).clone().attr('name', name_structure + ".BaseImponible").val(retencion.suma_base_imponible);
        var hdn_valor_retenido = $(hdn_base).clone().attr('name', name_structure + ".ValorRetenido").val(retencion.suma_valor_retenido);

        // showing data
        var td_ejercicio = $(td_base).clone().append($('<b></b>').addClass('_descripcion_ejercicio_fiscal').text(ejercicio_fiscal_descripcion)).css({ "text-align": "center" })
            .append(hdn_ejercicio)
            .append(hdn_retencion)
            .append(hdn_base_imponible)
            .append(hdn_valor_retenido);
        var td_codigo_impuesto = $(td_base).clone().append(retencion.retencion_id).css({ "text-align": "center" });
        var td_nombre_impuesto = $(td_base).clone().append(retencion.retencion_nombre);
        var td_porcentaje_impuesto = $(td_base).clone().append(retencion.retencion_porcentaje.toFixed(2) + " %").css({ "text-align": "center" });
        var td_base_imponible = $(td_base).clone().append("$ " + retencion.suma_base_imponible.toFixed(2)).css({ "text-align": "right" });
        var td_valor_retenido = $(td_base).clone().append("$ " + retencion.suma_valor_retenido.toFixed(2)).css({ "text-align": "right" });

        // sumamos los valores retenidos
        suma_total_valores_retenidos += retencion.suma_valor_retenido;

        $(t).append((tr_base).clone()
            .append(td_ejercicio)
            .append(td_codigo_impuesto)
            .append(td_nombre_impuesto)
            .append(td_porcentaje_impuesto)
            .append(td_base_imponible)
            .append(td_valor_retenido)
        );
    }

    // seteamos la suma de los valores retenidos
    $('#_suma_total_valores_retenidos').text(suma_total_valores_retenidos.toFixed(2));
}

function get_server_decimal(value) {
    value = value.toString().replace('.', ',');
    return value;
}

function show_distribution_form(show) {
    if (show) {
        $('button#_confirm_distribution').show();
        $('input[type=text]#_txt_distribucion_descuento').show().focus();
        $('b#_suma_descuento').hide();
    } else {
        $('button#_confirm_distribution').hide();
        $('input[type=text]#_txt_distribucion_descuento').val('').hide();
        $('b#_suma_descuento').show();
        $('#_chk_suma_descuento').click();
    }
}

function getActivesRows() {
    var rows = [];

    $('#tblDetails tr.detail_data').each(function () {
        var deleted = $(this).find('input[type=hidden].hdn_deleted').val() == "True";
        if (!deleted) {
            rows.push($(this));
        }
    });

    return rows;
}