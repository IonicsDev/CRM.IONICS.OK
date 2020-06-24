var _taxType = {
    RETENCION_IR: 1,
    RETENCION_IVA: 2,
    IMPUESTO: 3
};

$(document).ready(function () {
    $('input[type=checkbox]#remove_image').click(function () {
        // get controls
        var checked = $(this).is(':checked');
        var container = $('#image_container');
        var field = $('input[type=hidden]#QuitarImagen');

        // handle controls
        $(field).val(checked);
        if (checked) {
            $(container).hide();
        } else {
            $(container).show();
        }
    });

    $('select#TipoProducto_Id').change(function () {
        var tipoProducto_id = $(this).val();
        if (!tipoProducto_id || tipoProducto_id == 0)
            return;
        setCuentas(tipoProducto_id);
    });

    $('#impuestos_table tr td select').change(function () {
        var id = $(this).val();
        switch ($(this).attr('id')) {
            case "RetencionIR_Id":
                loadTax_Data(_taxType.RETENCION_IR, id);
                break;
            case "RetencionIVA_Id":
                loadTax_Data(_taxType.RETENCION_IVA, id);
                break;
            case "Impuesto_Id":
                loadTax_Data(_taxType.IMPUESTO, id);
                break;
            default:
                return;
        }
    });

    $('#impuestos_table tr td select').change();
});



function loadTax_Data(taxType, taxId) {
    if (taxId < 1) {
        showTaxData(taxType, "", "", "");
        return;
    }

    $.get('/Item/GetTaxData', {
        taxType: taxType,
        taxId: taxId
    }, function (data) {
        // check for errors
        if (data.error) {
            alert(cuentas.error);
            return;
        }

        // show data
        showTaxData(taxType, data.Porcentaje, data.CuentaCompra, data.CuentaVenta);
    }, 'json');
}

function showTaxData(taxType, porcentaje, cuentaCompra, cuentaVenta) {
    // show data
    switch (taxType) {
        case _taxType.RETENCION_IR:
            $('#rentecion_ir_porcentaje').text(porcentaje);
            $('#rentecion_ir_cuenta_compras').text(cuentaCompra);
            $('#rentecion_ir_cuenta_ventas').text(cuentaVenta);
            break;
        case _taxType.RETENCION_IVA:
            $('#rentecion_iva_porcentaje').text(porcentaje);
            $('#rentecion_iva_cuenta_compras').text(cuentaCompra);
            $('#rentecion_iva_cuenta_ventas').text(cuentaVenta);
            break;
        case _taxType.IMPUESTO:
            $('#impuestos_porcentaje').text(porcentaje);
            $('#impuestos_cuenta_compras').text(cuentaCompra);
            $('#impuestos_cuenta_ventas').text(cuentaVenta);
            break;
    }
}

function setCuentas(tipoProducto_id) {
    $.get('/Item/GetCuentasPorTipoProducto', {
        tipoProducto_id: tipoProducto_id
    }, function (cuentas) {
        // check for errors
        if (cuentas.error) {
            alert(cuentas.error);
            return;
        }

        // set combos values
        $('select#Cuenta_Compra_Id').val(cuentas.Cuenta_Compra_Id).change();
        $('select#Cuenta_Venta_Id').val(cuentas.Cuenta_Venta_Id).change();
        $('select#Cuenta_CostoVenta_Id').val(cuentas.Cuenta_CostoVenta_Id).change();

        // mostramos msj al usuario informando el cambio
        $.jGrowl("<i class='icon16 i-info'></i>Se han seleccionado las cuentas en el Tab de 'Contabilidad' considerando el tipo de produccto seleccionado.", {
            group: 'info',
            closeTemplate: '<i class="icon16 i-close-2"></i>',
            animateOpen: {
                width: 'show',
                height: 'show'
            }
        });
    }, 'json');
}