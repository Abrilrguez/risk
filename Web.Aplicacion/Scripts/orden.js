/*$(document).ready(function () {
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
}*/




function add() {
    
}

function guardar() {

    var idOrden = 2;
    //Paquete.
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
            url: baseUrl + "Orden/Guardar",
            type: "POST",
            data: {
                idOrden: idOrden, idPaquete: idPaquete, paquetePeso: paquetePeso, paqueteTamanio: paqueteTamanio, paqueteContenido: paqueteContenido, paqueteDescripcion: paqueteDescripcion,
                idCliente: idCliente, clienteNombre: clienteNombre, clienteTelefono: clienteTelefono, clienteCorreo: clienteCorreo, clienteRfc: clienteRfc, clienteDomicilio: clienteDomicilio,
                idDestinatario: idDestinatario, destinatarioNombre: destinatarioNombre, destinatarioTelefono: destinatarioTelefono, destinatarioCorreo: destinatarioCorreo, destinatarioPersona: destinatarioPersona,
                    destinatarioCalle: destinatarioCalle, destinatarioNumero: destinatarioNumero, destinatarioAvenida: destinatarioAvenida, destinatarioColonia: destinatarioColonia, destinatarioCp: destinatarioCp,
                    destinatarioCiudad: destinatarioCiudad, destinatarioEstado: destinatarioEstado, destinatarioReferencia: destinatarioReferencia
            },
            cache: false,
            tradicional: true,
            success: function (data) {
                if (data === "true") {
                    var modal = $("#mdMain");
                    modal.modal("hide");
                    activarRenglon();
                }
            },
            error: function (xhr, exception) {

            }
        });
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