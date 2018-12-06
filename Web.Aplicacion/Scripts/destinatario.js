$(document).ready(function () {
    cargarTabla();
});


function obtenerId() {
    var table = $('#table-destinatarios').DataTable();
    var id = 0;
    if (table.$('tr.info')[0] != undefined) {
        var selectedIndex = table.$('tr.info')[0]._DT_RowIndex
        var row = table.row(selectedIndex).data();
        id = row.Id;
    }
    return id;
}

function cargarTabla() {
    var table = $('#table-destinarios').DataTable();
    table.destroy();
    $('#table-destinatarios').DataTable({
        "autoWidth": true,
        "processing": true,
        "ajax": baseUrl + "Destinatario/ObtenerTodos",
        "bDestroy": true,
        "columns": [
            { "data": "Id", visible: false, searchable: false },
            { "data": "Nombre" },
            { "data": "Calle" },
            { "data": "Numero" },
            { "data": "Avenida" },
            { "data": "Colonia" },
            { "data": "Cp" },
            { "data": "Ciudad" },
            { "data": "Estado" },
            { "data": "Referencia" },
            { "data": "Telefono" },
            { "data": "Correo" },
            { "data": "Persona" }
        ]
    });

    activarRenglon();
}

function add() {
    var modalC = $("#mdContent");
    $('#mdMain').modal();
    modalC.load(baseUrl + 'Destinatario/Add/0', {});
}

function edit() {
    var modalC = $("#mdContent");
    var id = obtenerId();
    if (id != 0) {
        $("#mdMain").modal();
        modalC.load(baseUrl + "Destinatario/Add/" + id, function () {
            cargarDatos();

        });
    } else {
        swal("Seleccione un registro");
        return false;
    }
}

function del() {
    var id = obtenerId();
    alert(id);
    if (id != 0) {
        swal({
            title: "¿Estas seguro?",
            text: "Una vez eliminado, no podrás recuperar este destinatario",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        url: baseUrl + "Destinatario/Borrar/",
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
                    swal("Poof! Haz eliminado el destinatario", {
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
    var id = $.trim($("#destinatario-id").val());
    if (id != "" && id != 0) {
        $.ajax({
            url: baseUrl + "Destinatario/ObtenerPorId",
            data: { id: id },
            type: 'GET',
            dataType: 'json',
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#nombre").val(data.Nombre);
                
                $("#calle").val(data.Calle);
                $("#numero").val(data.Numero);
                $("#avenida").val(data.Avenida);
                $("#colonia").val(data.Colonia);
                $("#cp").val(data.Cp);
                $("#municipio").val(data.Ciudad);
               
                $("#estado").val(data.Estado);
                $("#referencia").val(data.Referencia);
                $("#telefono1").val(data.Telefono);
                $("#correo").val(data.Correo);
                $("#persona").val(data.Persona);
            }
        });
    } 
    return false;
}




function guardar() {
    var id = $.trim($("#destinatario-id").val());
    var nombre = $.trim($("#nombre").val());
    var calle = $.trim($("#calle").val());
    var numero = $.trim($("#numero").val());
    var avenida = $.trim($("#avenida").val());
    var colonia = $.trim($("#colonia").val());
    var cp = $.trim($("#cp").val());
    var ciudad = $.trim($("#municipio").val());
    var estado = $.trim($("#estado").val());
    var referencia = $.trim($("#referencia").val());
    var telefono1 = $.trim($("#telefono1").val());
    var telefono2 = $.trim($("#telefono2").val());
    var telefono3 = $.trim($("#telefono3").val());
    var telefono = telefono1 + "," + telefono2 + "," + telefono3;
    var correo = $.trim($("#correo").val());
    var persona = $.trim($("#persona").val());

    if (nombre !== "" && calle !== "" && avenida !== "" && colonia !== "" && cp !== "" && ciudad !== "" && estado !== "" && referencia !== "" &&
        telefono1 !== "" && telefono2 !== "" && telefono3 !== "" && correo !== "" && persona !== "") {
        $.ajax({
            url: baseUrl + "Destinatario/Guardar/",
            data: {
                id: id, nombre: nombre, calle: calle, numero: numero, avenida: avenida, colonia: colonia, cp: cp,
                ciudad: ciudad, estado: estado, referencia: referencia, telefono: telefono, correo: correo, persona: persona
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
            swal("Listo", "Se ha actualizado el destinatario", "success");
        } else {
            swal("Listo", "Se ha guardado el destinatario", "success");
        }
        cargarTabla();
    } else {
        swal("Error.", "Llene los campos correctamente.", "error");
    }
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