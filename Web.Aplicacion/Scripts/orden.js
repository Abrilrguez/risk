function add() {
    
}

function guardar() {
    var id = $.trim($("#usuario-id").val());
    var nombre = $.trim($("#nombre").val());
    var direccion = $.trim($("#direccion").val());
    var telefono = $.trim($("#telefono").val());
    var cuenta = $.trim($("#cuenta").val());
    var rol = $.trim($("#rol").val());
    var password = $.trim($("#password").val());
    var passwordValidar = $.trim($("#passwordValidar").val());
        $.ajax({
            url: baseUrl + "Usuario/Guardar/",
            data: {
                id: id, nombre: nombre, direccion: direccion, telefono: telefono, cuenta: cuenta, rol: rol, password: password
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

