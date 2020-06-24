$(document).ready(function () {
    $("select").select2();

    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });


    
    $('#TipoProveedor_Id').change(function () {
       
        var tipoProveedor_Id = $(this).val();
        if (!tipoProveedor_Id || tipoProveedor_Id == 0)
            return;
        setCuentas(tipoProveedor_Id);
        setRequisitos(tipoProveedor_Id);
    
      

    });
});

function setRequisitos(tipoProveedor_Id) {

    var t = $('table#tblDetails2 tbody');
    var rowIndex = $(t).find('tr.detail_data').length;
    var empty_row = $(t).find('tr.grid-empty-text');

    $.get('/Proveedor/getRowRequisitos/', {
        tipoProveedor: tipoProveedor_Id
    }, function (response) {
        $(t).append(response);
        $(empty_row).remove();
        var row = $(t).find('tr#' + rowIndex);
        $(".datepicker").datepicker();
    }, "html");

    return false;
}



function setCuentas(tipoProveedor_Id) {
    $.get('/Proveedor/GetCuentasPorTipoProveedor', {
        tipoProveedor_id: tipoProveedor_Id
    }, function (cuentas) {
        // check for errors
        if (cuentas.error) {
            alert(cuentas.error);
            return;
        }

        // set combos values
        $('#lblCuentasPagar').text(cuentas.Cuenta_Pagar);
        $('#lblCuentasCobrar').text(cuentas.Cuenta_Cobrar);
       

        // mostramos msj al usuario informando el cambio
        $.jGrowl("<i class='icon16 i-info'></i>Se han seleccionado las cuentas en el Tab de 'Contabilidad' y los 'Requisitos' considerando el tipo de proveedor seleccionado.", {
            group: 'info',
            closeTemplate: '<i class="icon16 i-close-2"></i>',
            animateOpen: {
                width: 'show',
                height: 'show'
            }
        });
    }, 'json');
}

$(document).ready(function () {
    $('#linkToPost').click(function () {
        var container = $(this).parent().parent();
        if (!validateDetalle(container)) {
            return;
        }

        var tipoContacto = $(container).find('select#IdTipoContacto').val();
        var nombre = $(container).find('#txtNombre').val();
        var cargo = $(container).find('#txtCargo').val();
        var telefono = $(container).find('#txtTelefono').val();
        var telefonoPBX = $(container).find('#txtTelefonoPBX').val();
        var celular = $(container).find('#txtCelular').val();
        var correo = $(container).find('#txtCorreo').val();
        var referencia = $(container).find('#txtReferencia').val();
        var t = $('table#tblDetails tbody');
        var rowIndex = $(t).find('tr.detail_data').length;
        var empty_row = $(t).find('tr.grid-empty-text');

        if ($(empty_row).length > 0) {
            $(empty_row).find('td').text("Cargando primera fila...");
        }

        $.get('/Proveedor/getRowDetail/', {
            tipoContacto: tipoContacto,
            nombre: nombre,
            cargo: cargo,
            telefono: telefono,
            telefonoPBX: telefonoPBX,
            celular: celular,
            correo: correo,
            referencia: referencia,
            rowIndex: rowIndex
        }, function (response) {
            $(t).append(response);
            $(empty_row).remove();
            var row = $(t).find('tr#' + rowIndex);
            estilizar_combos(row);
            limpiar_form();
        }, "html");

        return false;
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
        //var t_centrocosto = $(container).find('select.comboCentroCosto');
        //var t_cargoOperacional = $(container).find('select.comboCargoOperacional');

        $(container).find('.showing').hide();
        $(container).find('.edition').show();
        return false;
    });

    $(document).on('click', '.edit_row_detalle', function () {
        var container = $(this).parent().parent();
        //var t_centrocosto = $(container).find('select.comboCentroCosto');
        //var t_cargoOperacional = $(container).find('select.comboCargoOperacional');

        $(container).find('.showing').hide();
        $(container).find('.edition').show();
        return false;
    });

    // row remove handler
    $(document).on('click', '.remove_row_detalle', function () {
        if (!confirm("¿Está seguro que desea eliminar el contacto?"))
            return;
        var container = $(this).parent().parent();
        var hdn_deleted = $(container).find('.hdn_deleted');
      
        $(hdn_deleted).val("True");
        $(container).hide();
      
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

function finalizar_edicion_fila(row) {
    if (!validateDetalle(row)) {
        return;
    }
    $(row).find('.showing').show();
    $(row).find('.edition').hide();
   
    return false;
}


function validateDetalle(container) {
    // get controls
    var TipoContacto = $(container).find('#IdTipoContacto');
    var Nombre = $(container).find('#txtNombre');
    var Cargo = $(container).find('#txtCargo');
    var Telefono = $(container).find('#txtTelefono');
    var Correo = $(container).find('#txtCorreo');
 

  

    
    if ($(TipoContacto).val() == 0) {
        alert('Debe especificar el Tipo De Contacto');
        $(TipoContacto).focus();
        return false;
    } else if ($.trim($(Nombre).val()) == '') {
        alert('Debe especificar el Nombre');
        $(Nombre).focus();
        return false;
    } else if ($.trim($(Cargo).val()) == '') {
        alert("Debe especificar el Cargo");
        $(Cargo).focus();
        return;
    } else if ($.trim($(Telefono).val()) == '') {
        alert("Debe especificar el Telefono");
        $(cargoOperacional).focus();
        return;
    } else if ($.trim($(Correo).val()) == '') {
        alert('Debe especificar el Correo');
        $(control_valorDebito).focus();
        return false;
    }

    return true;
}

function limpiar_form() {
    var container = $('tr#controls_container');
    $(container).find('select').val(0);
    $(container).find('input#txtNombre').val('');
    $(container).find('input#txtCargo').val('');
    $(container).find('input#txtTelefono').val('');
    $(container).find('input#txtTelefonoPBX').val('');
    $(container).find('input#txtCelular').val('');
    $(container).find('input#txtCorreo').val('');
    $(container).find('input#txtReferencia').val('');

    $(container).find('select').each(function (i, s) {
        var t = $(s).find('option:selected').text();
        $(container).find('div.select2-container a span').text(t);
    });
}

