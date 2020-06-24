$(document).ready(function () {
    $("select").select2();

    $('#LineaInventario_Id').change(function () {
        var lineaInventarioId = $(this).val();
        getTallasByLineaInventario(lineaInventarioId);
    });
});

function getTallasByLineaInventario(lineaInventarioId) {
    var op = $('<option></option>'); // generic option control 
    var combo_tallas = $('select#TallaCorrida_Id');
    $(combo_tallas).html("").append($(op).clone().val(0).text("Cargando tallas...")).change();
    $.get('/Talla/GetTallasByLineaInventario', { lineaInventarioId: lineaInventarioId }, function (tallas) {
        // clear combo
        $(combo_tallas).html("").append($(op).clone().val(null).text("Seleccione")).change();

        // check for errors
        if (tallas.error) {
            alert(tallas.error);
            return;
        } else if (tallas.length < 1) {
            $(combo_tallas).html("");
            $(combo_tallas).append($(op).clone().val(null).text("No se han encontrado tallas...")).change();
            return;
        }

        // load data
        for (var i = 0; i < tallas.length; i++) {
            var talla = tallas[i];
            var talla_op = $(op).clone().val(talla.Id).text(talla.Descripcion);
            $(combo_tallas).append(talla_op);
        }
    }, 'json');
}