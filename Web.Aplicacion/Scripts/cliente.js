$(document).ready(function () {
    cargarTabla();
});

function cargarTabla() {
    var table = $('#table-clientes').DataTable();
    table.destroy();
    $('#table-clientes').DataTable({
        "autoWidth": true,
        "processing": true,
        "ajax": baseUrl + "Cliente/ObtenerTodos",
        "columns": [
            { "data": "Id", visible: false, searchable: false },
            { "data": "Nombre" },
            { "data": "Domicilio" },
            { "data": "Telefono" },
            { "data": "Correo" },
            { "data": "Rfc" }
        ]
    });

    activarRenglon();
}

function add() {
    var modalC = $("#mdContent");
    $('#mdMain').modal();
    modalC.load(baseUrl + 'Cliente/Add/0', {});
}

function edit() {
    var modalC = $("#mdContent");
    var id = obtenerId();
    if (id != 0) {
        $("#mdMain").modal();
        modalC.load(baseUrl + "Cliente/Edit/" + id, function () {
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
            text: "Una vez eliminado, no podrás recuperar este cliente",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        url: baseUrl + "Cliente/Borrar/",
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
                    swal("Poof! Haz eliminado el cliente", {
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

function cargarDatos() {
    var id = $.trim($("#cliente-id").val());
    if (id != "" && id != 0) {
        $.ajax({
            url: baseUrl + "Cliente/ObtenerPorId",
            data: { id: id },
            type: 'GET',
            dataType: 'json',
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#nombre").val(data.Nombre);
                $("#domicilio").val(data.Direccion);
                $("#telefono").val(data.Telefono);
                $("#correo").val(data.Cuenta);
                $("#rfc").val(data.Rol);
            }
        });
    }
    return false;
}

function guardar() {
    var id = $.trim($("#cliente-id").val());
    var nombre = $.trim($("#nombre").val());
    var domicilio = $.trim($("#domicilio").val());
    var telefono = $.trim($("#telefono").val());
    var correo = $.trim($("#correo").val());
    var rfc = $.trim($("#rfc").val());

    $.ajax({
        url: baseUrl + "Cliente/Guardar/",
        data: {
            id: id, nombre: nombre, domicilio: domicilio, telefono: telefono, correo: correo, rfc: rfc
        },
        cache: false,
        tradicional: true,
        success: function (data) {
            if (data == "true") {
                var modal = $("#mdMain");
                modal.modal("hide");
                cargarTabla();
            }
        },
        error: function (xhr, exception) {

        }
    });
    if (id != 0) {
        swal("Listo", "Se ha actualizado el cliente", "success");
    } else {
        swal("Listo", "Se ha guardado el cliente", "success");
    }
    cargarTabla();
}

function obtenerId() {
    var table = $('#table-clientes').DataTable();
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