$(document).ready(function () {
    cargarTabla();
});

function cargarTabla() {
    var table = $('#table-historiales').DataTable();
    table.destroy();
    $('#table-historiales').DataTable({
        "autoWidth": true,
        "processing": true,
        "ajax": baseUrl + "Historial/ObtenerTodos",
        "columns": [
            { "data": "Id", visible: false, searchable: false },
            { "data": "Fecha" },
            { "data": "Descripcion" },
            { "data": "Ciudad" },
            { "data": "Estado" }
        ]
    });

    activarRenglon();
}


function add() {
    var modalC = $("#mdContent");
    $('#mdMain').modal();
    modalC.load(baseUrl + 'Historial/Add/0', {});
}


function obtenerId() {
    var table = $('#table-historiales').DataTable();
    var id = 0;
    if (table.$('tr.info')[0] != undefined) {
        var selectedIndex = table.$('tr.info')[0]._DT_RowIndex
        var row = table.row(selectedIndex).data();
        id = row.Id;
    }
    return id;
}

function activarRenglon() {
    var singleSelect = $('.datatable-selection-single').DataTable();
    $('.datatable-selection-single tbody').on('click', 'tr', function () {
        if ($(this).hasClass('info')) {
            $(this).removeClass('info');
        }
        else {
            singleSelect.$('tr.info').removeClass('info');
            $(this).addClass('info');
        }
    });
}


function cargarDatos() {
    var id = $.trim($("#historial-id").val());
    if (id != "" && id != 0) {
        $.ajax({
            url: baseUrl + "Historial/ObtenerPorId",
            data: { id: id },
            type: 'GET',
            dataType: 'json',
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#fecha").val(data.Nombre);
                $("#descripcion").val(data.Direccion);
                $("#municipio").val(data.Telefono);
                $("#estado").val(data.Cuenta);
            }
        });
    }
    return false;
}



function guardar() {
    var id = $.trim($("#historial-id").val());
    var fecha = $.trim($("#fecha").val());
    var descripcion = $.trim($("#descripcion").val());
    var ciudad = $.trim($("#municipio").val());
    var estado = $.trim($("#estado").val());

    obtenerLatLng();
    
        $.ajax({
            url: baseUrl + "Historial/Guardar/",
            data: {
                id: id, fecha: fecha, descripcion: descripcion, ciudad: ciudad, estado:estado
            },
            cache: false,
            tradicional: true,
            success: function (data) {
                if (data == "true") {
                    var modal = $("#mdMain");
                    modal.modal("hide");
                    activarRenglon();
                }
            },
            error: function (xhr, exception) {

            }
        });
        if (id != 0) {
            swal("Listo", "Se ha actualizado el historial", "success");
        } else {
            swal("Listo", "Se ha guardado el historial", "success");
        }
        cargarTabla();
}


function edit() {
    var modalC = $("#mdContent");
    var id = obtenerId();
    if (id != 0) {
        $('#mdMain').modal();
        modalC.load(baseUrl + 'Historial/Add/' + id, function () {
            cargarDatos();
        });
    } else {
        swal("Seleccione un registro");
        return false;
    }
}

function del() {
    var id = obtenerId();
    if (id != 0) {
        swal({
            title: "¿Estas seguro?",
            text: "Una vez eliminado, no podrás recuperar este historial",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        url: baseUrl + "Historial/Borrar/",
                        data: {
                            id: id
                        },
                        cache: false,
                        tradicional: true,
                        success: function (data) {
                            if (data == "true") {
                                cargarTabla();
                            }
                        },
                        error: function (xhr, exception) {

                        }
                    });
                    swal("Poof! Haz eliminado el historial", {
                        icon: "success",
                    });
                } else {
                    swal("Se ha cancelado la operación");
                    cargarTabla();
                }
            });
    } else {
        swal("Seleccione un registro");
        return false;
    }
    cargarTabla();
}