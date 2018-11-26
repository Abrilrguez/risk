$(document).ready(function () {
    cargarTabla();
});
function cargarTabla() {
    var table = $('#table-ordenes').DataTable();
    table.destroy();
    $('#table-ordenes').DataTable({
        "autoWidth": true,
        "processing": true,
        "ajax": baseUrl + "Orden/ObtenerTodos",
        "bDestroy": true,
        "columns": [
            { "data": "Id", visible: false, searchable: false },
            { "data": "Folio" },
            { "data": "NumeroRastreo" },
            { "data": "Fecha" },
            { "data": "Estado" },
        ]
    });

    activarRenglon();
}
function add() {
    var modalC = $("#mdContent");
    $('#mdMain').modal();
    modalC.load(baseUrl + 'Orden/Add/0/0/0/0', {});
    activarRenglon();
}
function guardar() {

    //Orden.
    var idOrden = $.trim($("#orden-id").val());
    var ordenFecha = $.trim($("#orden-fecha").val());
    var ordenFolio = $.trim($("#orden-folio").val());
    var ordenNumRastreo = $.trim($("#orden-numRastreo").val());
    var ordenPrecio = $.trim($("#orden-precio").val());
    var ordenEstado = $.trim($("#orden-estado").val());

    //Paquete.
    var idUsuario = 0;
    var idPaquete = $.trim($("#paquete-id").val());
    var paquetePeso = $.trim($("#paquete-peso").val());
    var paqueteTamanio = $.trim($("#paquete-tamanio").val());
    var paqueteContenido = $.trim($("#paquete-contenido").val());
    var paqueteDescripcion = $.trim($("#paquete-descripcion").val());

    //Cliente.
    var idCliente = $.trim($("#cliente-id").val());
    var clienteNombre = $.trim($("#cliente-nombre").val());
    var clienteTelefono = $.trim($("#cliente-telefono").val());
    var clienteCorreo = $.trim($("#cliente-correo").val());
    var clienteRfc = $.trim($("#cliente-rfc").val());
    var clienteDomicilio = $.trim($("#cliente-domicilio").val());

    //Destinatario.
    var idDestinatario = $.trim($("#destinatario-id").val());
    var destinatarioNombre = $.trim($("#destinatario-nombre").val());
    var destinatarioTelefono = $.trim($("#destinatario-telefono").val());
    var destinatarioCorreo = $.trim($("#destinatario-correo").val());
    var destinatarioPersona = $.trim($("#destinatario-persona").val());
    var destinatarioCalle = $.trim($("#destinatario-calle").val());
    var destinatarioNumero = $.trim($("#destinatario-numero").val());
    var destinatarioAvenida = $.trim($("#destinatario-avenida").val());
    var destinatarioColonia = $.trim($("#destinatario-colonia").val());
    var destinatarioCp = $.trim($("#destinatario-cp").val());
    var destinatarioCiudad = $.trim($("#destinatario-ciudad").val());
    var destinatarioEstado = $.trim($("#destinatario-estado").val());
    var destinatarioReferencia = $.trim($("#destinatario-referencia").val());

    $.ajax({
        url: baseUrl + "Orden/Guardar/",
        data: {
            idUsuario: idUsuario,
            idOrden: idOrden,
            ordenEstado: ordenEstado, ordenPrecio: ordenPrecio, ordenFolio: ordenFolio, ordenNumRastreo: ordenNumRastreo, ordenFecha: ordenFecha,
            idPaquete: idPaquete, paquetePeso: paquetePeso, paqueteTamanio: paqueteTamanio, paqueteContenido: paqueteContenido, paqueteDescripcion: paqueteDescripcion,
            idCliente: idCliente, clienteNombre: clienteNombre, clienteTelefono: clienteTelefono, clienteCorreo: clienteCorreo, clienteRfc: clienteRfc, clienteDomicilio: clienteDomicilio,
            idDestinatario: idDestinatario, destinatarioNombre: destinatarioNombre, destinatarioTelefono: destinatarioTelefono, destinatarioCorreo: destinatarioCorreo, destinatarioPersona: destinatarioPersona,
            destinatarioCalle: destinatarioCalle, destinatarioNumero: destinatarioNumero, destinatarioAvenida: destinatarioAvenida, destinatarioColonia: destinatarioColonia, destinatarioCp: destinatarioCp,
            destinatarioCiudad: destinatarioCiudad, destinatarioEstado: destinatarioEstado, destinatarioReferencia: destinatarioReferencia
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
            swal(exception);
        }

    });
    
    if (idOrden != 0) {
        swal("Listo", "Se ha actualizado el usuario", "success");
        activarRenglon();

    } else {
        swal("Listo", "Se ha guardado el usuario", "success");
        activarRenglon();
    }
    activarRenglon();
}

function del() {
    var id = obtenerId();
    swal({
        title: "¿Estás seguro?",
        text: "¡Una vez borrado no se podrá ver más tu registro!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: baseUrl + "Orden/Eliminar",
                    data: {
                        id: id
                    },
                    cache: false,
                    tradicional: true,
                    success: function (data) {
                        if (data == "true") {
                            swal("¡Genial!", "La acción se realizó con éxito", "success");
                            cargarTabla();
                        } else {
                            swal("¡Error!", "La acción no se realizó con éxito", "error");
                        }
                    },
                    error: function (xhr, exception) {
                        swal("¡Error!", "La acción no se realizó con éxito", "error");
                    }
                });
            } else {
                swal("Tu registro de orden no se elimino con éxito");
                cargarTabla();
            }
        });
    cargarTabla();
}
function obtenerId() {
    var table = $('#table-ordenes').DataTable();
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
    var id = $.trim($("#orden-id").val());
    if (id != "" && id != 0) {
        $.ajax({
            url: baseUrl + "Orden/ObtenerPorId",
            data: { id: id },
            type: 'GET',
            dataType: 'json',
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var idOrden = $.trim($("#orden-id").val());
                $.trim($("#orden-folio").val(data.Folio));
                $.trim($("#orden-numRastreo").val(data.NumeroRastreo));
                $.trim($("#orden-fecha").val(data.Fecha));
                $.trim($("#orden-estado").val(data.Estado));
                $.trim($("#orden-precio").val(data.Precio));
                //Paquete.
                $.trim($("#paquete-id").val(data.Paquete.Id));
                $.trim($("#paquete-peso").val(data.Paquete.Peso));
                $.trim($("#paquete-tamanio").val(data.Paquete.Tamanio));
                $.trim($("#paquete-contenido").val(data.Paquete.Contenido));
                $.trim($("#paquete-descripcion").val(data.Paquete.Descripcion));

                //Cliente.
                $.trim($("#cliente-id").val(data.Cliente.Id));
                $.trim($("#cliente-nombre").val(data.Cliente.Nombre));
                $.trim($("#cliente-telefono").val(data.Cliente.Telefono));
                $.trim($("#cliente-correo").val(data.Cliente.Correo));
                $.trim($("#cliente-rfc").val(data.Cliente.Rfc));
                $.trim($("#cliente-domicilio").val(data.Cliente.Domicilio));

                //Destinatario.
                $.trim($("#destinatario-id").val(data.Destinatario.Id));
                $.trim($("#destinatario-nombre").val(data.Destinatario.Nombre));
                $.trim($("#destinatario-telefono").val(data.Destinatario.Telefono));
                $.trim($("#destinatario-correo").val(data.Destinatario.Correo));
                $.trim($("#destinatario-persona").val(data.Destinatario.Persona));
                $.trim($("#destinatario-calle").val(data.Destinatario.Calle));
                $.trim($("#destinatario-numero").val(data.Destinatario.Numero));
                $.trim($("#destinatario-avenida").val(data.Destinatario.Avenida));
                $.trim($("#destinatario-colonia").val(data.Destinatario.Colonia));
                $.trim($("#destinatario-cp").val(data.Destinatario.Cp));
                $.trim($("#destinatario-ciudad").val(data.Destinatario.Ciudad));
                $.trim($("#destinatario-estado").val(data.Destinatario.Estado));
                $.trim($("#destinatario-referencia").val(data.Destinatario.Referencia));

                
            }
        });
    }
    return false;
}

function edit() {
    var modalC = $("#mdContent");
    var id = obtenerId();

    if (id != 0) {
        $('#mdMain').modal();
        modalC.load(baseUrl + 'Orden/Add/' + id +'/1/2/3',function () {
            cargarDatos();
        });
    } else {
        swal("Seleccione un registro");
        activarRenglon();
        return false;
    }
    activarRenglon();
}
function cargarDatosOrden()
{
    var id = $.trim($("#orden-id").val());
    if (id == 0) {
        $.ajax({
            url: baseUrl + "Orden/ObtenerDatosOrden",
            type: 'GET',
            dataType: 'json',
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                alert(data.Folio);
                $.trim($("#orden-folio").val(data.Folio));
                $.trim($("#orden-numRastreo").val(data.NumeroRastreo));
                $.trim($("#orden-fecha").val(data.Fecha));
                $.trim($("#orden-estado").val(data.Estado));
            }
        });
    }
}