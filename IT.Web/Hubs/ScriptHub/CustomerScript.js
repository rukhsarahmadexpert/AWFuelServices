$(function () {

    var Id = $("#CompanyId").val();
    var notifications = $.connection.myHub;

    notifications.client.sendNotifyMasseges = function () {
        GetAcceptedOrderInfo(Id);
    };

    notifications.client.sendDeliveryNotifyMasseges = function () {
        OrderDeliveryMessage(Id);
    };


    $.connection.hub.start().done(function () {
          GetAcceptedOrderInfo(Id);      
          OrderDeliveryMessage(Id);
    }).fail(function (e) {
        alert(e);
    });
});