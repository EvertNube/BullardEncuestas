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

    $('.btnEditSeccion').click(function () {
        var id = $(this).data('idseccion');
        var nombre = $(this).data('nombre');
        var orden = $(this).data('orden');
        var essocio = $(this).data('essocio');
        var estado = $(this).data('estado');
        var count = $(this).data('count');
        $('#IdSeccion').val(id);
        $('#Nombre').val(nombre);
        $('#EsSocio').prop("checked", (essocio.toLowerCase() == 'true'));
        $('#Estado').prop("checked", (estado.toLowerCase() == 'true'));
        //$('#estadoSec').prop("checked", true); $('#chx i').addClass("checked");
        setOrden($('#Orden'), count, orden);
        $('#modal-Seccionform').modal('show');
    });

    $('.btnEditPregunta').click(function () {
        var id = $(this).data('id');
        var texto = $(this).data('texto');
        var descrip = $(this).data('descrip');
        var orden = $(this).data('orden');
        var tiporpta = $(this).data('tiporpta');
        var estado = $(this).data('estado');
        var count = $(this).data('count');
        getTiposRespuesta(tiporpta);
        setOrden($('#OrdenPregunta'), count, orden);
        $('#IdPregunta').val(id);
        $('#Texto').val(texto);
        $('#Descripcion').val(descrip);
        $('#EstadoPregunta').prop("checked", (estado.toLowerCase() == 'true'));
        $('#modal-Preguntaform').modal('show');
    });

    $('.btnEditEncuesta').click(function () {
        var id = $(this).data('id');
        var nombre = $(this).data('nombre');
        var instrucciones = $(this).data('instr');
        var leyenda = $(this).data('leyenda');
        var periodo = $(this).data('periodo');
        var gevaluado = $(this).data('gevaluado');
        var gevaluador = $(this).data('gevaluador');
        var estado = $(this).data('estado');
        var count = $(this).data('count');
        getPeriodos(periodo);
        getGruposEvaluados(gevaluado);
        getGruposEvaluadores(gevaluador);
        //getGruposEvaluadoresSel();
        $('#IdEncuesta').val(id);
        $('#NombreEncuesta').val(nombre);
        $('#Instrucciones').val(instrucciones);
        $('#Leyenda').val(leyenda);
        $('#EstadoEncuesta').prop("checked", (estado.toLowerCase() == 'true'));
        $('#modal-Encuestaform').modal('show');
    });

    $("#brnSaveEncuesta").click(function () {
        var values = [];
        $('#GrupoEvaluador option').each(function () {
            values.push($(this).val());
        });
        $("#GrupoEvaluador").val(values);//(["2", "3"]);
    });

    $("#add-grupoEvaluado").click(function () {
        var existe = false;
        var val = $('#IdGrupoEvaluador :selected').val();
        if (val > 0) {
            var text = $('#IdGrupoEvaluador :selected').text();
            $('#GrupoEvaluador option').each(function () {
                if ($(this).val() == val) existe = true;
            });
            if (!existe) {
                $('#GrupoEvaluador').append($('<option>', {
                    value: val,
                    text: text
                }));
                $('#IdGrupoEvaluador :selected').remove();
            } else {
                alert("No se puede agregar el grupo " + text + ", porque ya se encuentra agregado.");
            }
        } else {
            alert("Por favor, seleccione un grupo válido.");
        }
    });
    $("#remove-grupoEvaluado").click(function () {
        var val = $('#GrupoEvaluador :selected').val();
        if (val) {
            var text = $('#GrupoEvaluador :selected').text();
            $('#IdGrupoEvaluador').append($('<option>', {
                value: val,
                text: text
            }));
            $("select#IdGrupoEvaluador").html($("select#IdGrupoEvaluador option").sort(function (a, b) {
                return a.text == b.text ? 0 : a.value > b.value ? 1 : -1;
            }));
            $('#GrupoEvaluador :selected').remove();
        }
    });

    $('.dd').on('change', function () {
        /* on change event */
        console.log('event');
    });

});

function setOrden(name, length, selected) {
    var select = name;
    select.empty();
    for (var i = 1; i <= length; i++) {
        select.append($('<option />', {
            value: i,
            text: i
        }));
    }
    select.val(selected);
}
function getTiposRespuesta(id) {
    $.ajax({
        url: "/Admin/GetTiposRespuesta",
        type: "GET",
        cache: false,
        data: { AsSelectList : true },
        dataType: "json"
    }).done(function (data) {
        var select = $("#IdTipoRespuesta");
        select.empty();
        $.each(data, function (index, item) {
            select.append($('<option />', {
                value: item.IdTipoRespuesta,
                text: item.Nombre
            }));
        });
        select.val(id);
    }).fail(function () {
        alert('Error al intentar obtener el listado de Tipos de Respuesta. Por favor, actualice la página o presione F5.');
    });
}
function getPeriodos(id) {
    $.ajax({
        url: "/Admin/GetPeriodos",
        type: "GET",
        cache: false,
        data: { AsSelectList: true },
        dataType: "json"
    }).done(function (data) {
        var select = $("#IdPeriodo");
        select.empty();
        $.each(data, function (index, item) {
            select.append($('<option />', {
                value: item.IdPeriodo,
                text: item.Descripcion
            }));
        });
        select.val(id);
    }).fail(function () {
        alert('Error al intentar obtener el listado de Periodos. Por favor, actualice la página o presione F5.');
    });
}
function getGruposEvaluados(id) {
    $.ajax({
        url: "/Admin/GetGruposEvaluados",
        type: "GET",
        cache: false,
        data: { AsSelectList: true },
        dataType: "json"
    }).done(function (data) {
        var select = $("#IdGrupoEvaluado");
        select.empty();
        $.each(data, function (index, item) {
            select.append($('<option />', {
                value: item.IdGrupoTrabajo,
                text: item.Nombre
            }));
        });
        select.val(id);
    }).fail(function () {
        alert('Error al intentar obtener el listado de Grupos de Evaluados. Por favor, actualice la página o presione F5.');
    });
}
function getGruposEvaluadores(ids) {
    $.ajax({
        url: "/Admin/GetGruposEvaluados",
        type: "GET",
        cache: false,
        data: { AsSelectList: true },
        dataType: "json"
    }).done(function (data) {
        var select = $("#IdGrupoEvaluador");
        var select2 = $("#GrupoEvaluador");
        select.empty(); select2.empty();
        for (var i = 0; i < data.length; i++) {
            if (ids.indexOf(data[i].IdGrupoTrabajo) == -1)
            {
                select.append($('<option />', {
                    value: data[i].IdGrupoTrabajo,
                    text: data[i].Nombre
                }));
            } else {
                select2.append($('<option />', {
                    value: data[i].IdGrupoTrabajo,
                    text: data[i].Nombre
                }));
            }
        }
        //select.val(id);
    }).fail(function () {
        alert('Error al intentar obtener el listado de Grupos de Evaluados. Por favor, actualice la página o presione F5.');
    });
}