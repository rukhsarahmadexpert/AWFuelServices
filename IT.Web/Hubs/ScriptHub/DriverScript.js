

$(function () {
    var Id = localStorage.getItem('DriverId');

    var notifications = $.connection.driverHub;

    notifications.client.SendNotification = function () {

        alert();
        GetMessagesesDriver(Id)
    };

    $.connection.hub.start().done(function () {
               
        GetMessagesesDriver(Id);      
        //  OrderDeliveryMessage(Id);
    }).fail(function (e) {
        alert(e);
    });
});

