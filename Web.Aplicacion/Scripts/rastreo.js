function rastrear()
{
    var folio = $('#numero-rastreo').val();
    alert(folio);
    $.ajax({
        url: baseUrl + "Orden/ObtenerPorFolio/"+folio,
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            alert(data.Fecha);
        }
    });
}