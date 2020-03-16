$(function () {

    var notifications = $.connection.adminHub;

    notifications.client.updateMessages = function () {
        GetOrderNotification();
    }

    notifications.client.updateDeliveryMessages = function () {
        GetInfoOnDelivery();
    }


    $.connection.hub.start().done(function () {
        // GetAcceptedOrderInfo(Id);       
        //  OrderDeliveryMessage(Id);
    }).fail(function (e) {
        alert(e);
    });

});