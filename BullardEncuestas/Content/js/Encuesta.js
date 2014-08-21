$(document).ready(function () {

    // activate Nestable for list 1
    $('#nestable1').nestable({
        group: 1
    });

    // activate Nestable for list 2
    $('#nestable2').nestable({
        group: 1
    });

    var $expand = false;
    $('#nestable-menu').on('click', function (e) {
        if ($expand) {
            $expand = false;
            $('.dd').nestable('expandAll');
        } else {
            $expand = true;
            $('.dd').nestable('collapseAll');
        }
    });

    $('#nestable3').nestable();

    $('.btnEditar').click(function () {
        var id = $(this).data('idseccion');
        var nombre = $(this).data('nombre');
        var orden = $(this).data('orden');
        var essocio = $(this).data('essocio');
        var estado = $(this).data('estado');
        var count = $(this).data('count');
        $('#ide').val(id);
        $('#nombre').val(nombre);
        $('#esSocio').prop("checked", (essocio.toLowerCase() == 'true'));
        $('#estado').prop("checked", (estado.toLowerCase() == 'true'));
        //$('#estadoSec').prop("checked", true); $('#chx i').addClass("checked");
        setOrden(count, orden);
        $('#modal-form').modal('show');
    });

    $('.dd').on('change', function () {
        /* on change event */
        console.log('event');
    });

});

function setOrden(length, selected) {
    var select = $('#orden');
    select.empty();
    for (var i = 1; i <= length; i++) {
        select.append($('<option />', {
            value: i,
            text: i
        }));
    }
    select.val(selected);
}

function setEstado() {
    $.ajax({
        url: "/Admin/GetEstado",
        type: "GET",
        cache: false,
        data: { NombreTabla: "TIPOPERSONA" },
        dataType: "json"
    }).done(function (data) {
        var select = $("#tipoPersona");
        select.empty();
        $.each(data, function (index, itemData) {
            select.append($('<option />', {
                value: itemData.IdGenerica,
                text: itemData.NombreCampo
            }));
        });
        select.trigger("click");
    }).fail(function () {
        alert('Error al intentar obtener el listado de Tipo de Personas. Por favor, actualice la página o presione F5.');
    });
}