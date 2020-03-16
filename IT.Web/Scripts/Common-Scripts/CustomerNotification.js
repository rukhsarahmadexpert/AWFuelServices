
//$(function () {

//    debugger;
//    var Id = $('#CompanyId').val();
//    // Declare a proxy to reference the hub.
//    var notifications = $.connection.myHub;
   
//    // Create a function that the hub can call to broadcast messages.
//     notifications.client.sendNotifyMasseges = function () {
//        GetAcceptedOrderInfo(Id);
//     };
    
//     notifications.client.sendDeliveryNotifyMasseges = function () {
//        MessagesesDriverAdmin();
//     };
//   // $.connection.hub.start().done();

//    $.connection.hub.start().done(function () {   
//       // alert('connection started');
//        GetAcceptedOrderInfo(Id);
//        MessagesesDriverAdmin();
//     }).fail(function (e) {
//        alert(e);
//     });
//});



$(function () {

    debugger;
    var Id = $('#CompanyId').val();
    // Declare a proxy to reference the hub.
    var notifications = $.connection.myHub;

    // Create a function that the hub can call to broadcast messages.
    notifications.client.sendNotifyMasseges = function () {

        alert();
        GetAcceptedOrderInfo(Id);
    };

    notifications.client.sendDeliveryNotifyMasseges = function () {
        //MessagesesDriverAdmin();
    };
    // $.connection.hub.start().done();

    $.connection.hub.start().done(function () {
        alert('connection started');
        //GetAcceptedOrderInfo(Id);
        //MessagesesDriverAdmin();
    }).fail(function (e) {
        alert(e);
    });
})



function GetAcceptedOrderInfo(Id) {  
    $.ajax({
        url: '/IHome/CustomerMassages/' + Id,
        contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (result) {   
            if (parseInt(result) > 0) {
                ViewDeliveryInfo(Id);
            }
        }
    });
}


function ViewDeliveryInfo(Id) {
    debugger;

    $('#CustomerResponseModel').modal('show');
    var Data = JSON.stringify({
        Id : Id
    })
    ajaxRequest("POST", "/ViewDeliveryInfo", Data, "json").then(function (result) {
      
        $('#DeliveryPerson').text(result.DeliverdDriverName);
        $('#DeliveryDriverContact').text(result.Contact);
        $('#DeliveryVehicleNumber').text(result.DeliverdVehicleNumber);
        $('#OrderIdNow').val(result.Id);
       
        $('#CustomerResponseModel').modal('show');
    });
}


$('#Ok').click(function () {

    var Dats = JSON.stringify({
        Id: $('#OrderIdNow').val()
    })
    ajaxRequest("POST", "/ViewedNotifyCustomer", Dats, "json").then(function (result) {

        if (result == "Success") {
            AlertCustomerOk();
            $('#CustomerResponseModel').modal('hide');
        }
    });
});



