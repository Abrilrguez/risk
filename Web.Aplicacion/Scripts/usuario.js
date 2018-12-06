$(document).ready(function () {
    cargarTabla();
});

function cargarTabla() {
    var table = $('#table-usuarios').DataTable();
    table.destroy();
    $('#table-usuarios').DataTable({
        "autoWidth": true,
        "processing": true,
        "ajax": baseUrl + "Usuario/ObtenerTodos",
        "columns": [
            { "data": "Id", visible: false, searchable: false },
            { "data": "Nombre" },
            { "data": "Direccion" },
            { "data": "Telefono" },
            { "data": "Cuenta" }
        ]
    });

    activarRenglon();
}

function add() {
    var modalC = $("#mdContent");
    $('#mdMain').modal();
    modalC.load(baseUrl + 'Usuario/Add/0', {});
}

function edit() {
    var modalC = $("#mdContent");
    var id = obtenerId();
    if (id != 0) {
        $('#mdMain').modal();
        modalC.load(baseUrl + 'Usuario/Add/' + id, function () {
            cargarDatos();
            $('#password').attr("type", "hidden");
            $('#label_password').remove();
            $('#passwordValidar').attr("type", "hidden");
            $('#label_passwordValidar').remove();
        });
    } else {
        swal("Error.", "Seleccione un registro", "error");
        activarRenglon();
        return false;
    }
}

function del() {
    var id = obtenerId();
    if (id != 0) {
        swal({
            title: "¿Estas seguro?",
            text: "Una vez eliminado, no podrás recuperar este usuario",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: baseUrl + "Usuario/Borrar/",
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
                swal("Poof! Haz eliminado el usuario", {
                    icon: "success",
                });
            } else {
                swal("Se ha cancelado la operación");
                cargarTabla();
            }
            });
    } else {
        swal("Error.", "Seleccione un registro", "error");
        activarRenglon();
        return false;
    }
    cargarTabla();
}

function changePassword() {
    var modalC = $("#mdContent");
    var id = obtenerId();
    if (id != 0) {
        $('#mdMain').modal();
        modalC.load(baseUrl + 'Usuario/ChangePassword/' + id, {});
        $('#header-text').text("Cambiar contraseña");
    } else {
        swal("Error.", "Seleccione un registro", "error");
        activarRenglon();
        return false;
    }
}

function cargarDatos() {
    var id = $.trim($("#usuario-id").val());
    if (id != "" && id != 0) {
        $.ajax({
            url: baseUrl + "Usuario/ObtenerPorId",
            data: { id: id },
            type: 'GET',
            dataType: 'json',
            cache: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#nombre").val(data.Nombre);
                $("#direccion").val(data.Direccion);
                $("#telefono").val(data.Telefono);
                $("#cuenta").val(data.Cuenta);
                $("#rol").val(data.Rol);

                var rol = data.Rol;
                if (rol == 1) {
                    rol = "Administrador";
                } else {
                    rol = "Empleado";
                }
                $("#rol").val(rol);
            }
        });
    }
    return false;
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

    if (rol == "Administrador") {
        rol = 1;
    } else {
        rol = 2;
    }

    if (nombre !== "" && direccion !== "" && telefono !== "" && cuenta !== "" && rol !== "" || password !== "" || passwordValidar !== "") {
        
        if (id != 0) {
            $.ajax({
                url: baseUrl + "Usuario/Guardar/",
                data: {
                    id: id, nombre: nombre, direccion: direccion, telefono: telefono, cuenta: cuenta, rol: rol
                },
                cache: false,
                tradicional: true,
                success: function (data) {
                    if (data == "true") {
                        var modal = $("#mdMain");
                        modal.modal("hide");
                        activarRenglon();
                        
                        swal("Listo", "Se ha actualizado el usuario", "success");
                        
                    } else {
                        alert("Error");
                    }
                },
                error: function (xhr, exception) {

                }
            });
            
            cargarTabla();
        } else {
            if (password === passwordValidar) {
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

                            swal("Listo", "Se ha guardado el usuario", "success");

                        }
                    },
                    error: function (xhr, exception) {

                    }
                });

                cargarTabla();
            } else {
                swal("Error.", "Las contraseñas no coinciden.", "error");
            }
            activarRenglon();
        }
        activarRenglon();
    } else {
        swal("Error", "Llene los datos correctamente", "warning");
        activarRenglon();
    }
    activarRenglon();
}

function actualizarPassword() {
    var id = $.trim($("#usuario-id").val());
    var password = $.trim($("#password").val());
    var passwordValidar = $.trim($("#passwordValidar").val());
    var passwordNueva = $.trim($("#passwordNueva").val());

    if (passwordNueva == passwordValidar) {
        $.ajax({
            url: baseUrl + "Usuario/ActualizarPassword/",
            data: {
                id: id, password: password, passwordValidar: passwordValidar, passwordNueva: passwordNueva
            },
            cache: false,
            tradicional: true,
            success: function (data) {
                if (data == "true") {
                    var modal = $("#mdMain");
                    modal.modal("hide");
                    swal("Listo", "Se ha actualizado la contraseña del usuario", "success");
                    cargarTabla();
                }
            },
            error: function (xhr, exception) {
                swal("Error", "No se ha logrado actualizar la contraseña", "warning");
            }
        });
        
        
        cargarTabla();
    } else {
        swal("Error", "Las contraseñas no coinciden", "warning");
    }
}

function obtenerId() {
    var table = $('#table-usuarios').DataTable();
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