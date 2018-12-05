function rastrear()
{
    var id = $('#numero-rastreo').val();
    alert(id);
    $.ajax({
        url: baseUrl + "Orden/ObtenerPorFolio/"+id,
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            
            var modalC = $("#info");
            $('#info').modal();
        }
    });
}