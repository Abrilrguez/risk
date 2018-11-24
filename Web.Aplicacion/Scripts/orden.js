function add() {
    
}

function guardar() {

    
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
            data: {
                id: id, nombre: nombre, direccion: direccion, telefono: telefono, cuenta: cuenta,
                rol: rol, password: password
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
}

