
var Id = localStorage.getItem('DriverId');

$(function () {
    // Declare a proxy to reference the hub.
    var notifications = $.connection.myHub;    
    //debugger;
    // Create a function that the hub can call to broadcast messages.
    notifications.client.sendMasseges = function () {  
        
        GetMessagesesDriver(Id);
    };

    $.connection.hub.start().done(function () {      
        //alert("connection started")
       // notify("bottom", "right", "undefined", "inverse", "undefined");
        GetMessagesesDriver(Id);
    }).fail(function (e) {
        alert(e);
    });
});


function GetMessagesesDriver(Id) {
    //var tbl = $('#messagesTable');
    
    $.ajax({
        url: '/IHome/GetMessagesesDriver/'+Id,
        contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (result) {
            //var a2 = JSON.parse(result);
            //tbl.empty();
            //$.each(a2, function (key, value) {
            //    tbl.append('<tr>' + '<td>' + value.OrderId + '</td>' + '<td>' + value.OrderProgress + '</td>' + '</tr>');
            //});
            //$('#MassageNotification').text(result);
            //alert(result + ' Driver');
            if (parseInt(result) > 0) {                  
                GetOrderData(Id);
            }
        }
    });   
}


function GetOrderData(id) {
    var Data = JSON.stringify({
        OrderId: id
    })
    ajaxRequest("POST", "/DriverViewOrder", Data, "json").then(function (result) {
        $('#CurrentOrderId').val(result.OrderId);
        $('#CompanyName').text(result.Company);
        $('#CompanyContact').text(result.Cell);
        $('#DriverName').text(result.DriverName);
        $('#DriverContact').text(result.Contact);
        $('#VehicleNumber').text(result.TraficPlateNumber);
        $('#Quantity').text(result.OrderQuantity + ' GALLON');
               
        $('#NotifyMeOrder').modal('show');
    });
}

$('#AcceptDriver').click(function () {

    var Data = JSON.stringify({
        OrderId: $('#CurrentOrderId').val()
    })
    ajaxRequest("POST", "/CustomerOrderAcceptDriver", Data, "json").then(function (result) {

        if (result == "Success") {
            $('#NotifyMeOrder').modal('hide');
            DriverAccept();
        }
    });
});
