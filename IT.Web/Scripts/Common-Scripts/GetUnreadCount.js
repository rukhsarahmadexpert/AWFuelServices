$(function () {

    
    // Declare a proxy to reference the hub.
    var notifications = $.connection.myHub;   
    //debugger;
    // Create a function that the hub can call to broadcast messages.
    notifications.client.updateMessages = function () {
        getAllMessages()
    };

    notifications.client.sendDeliveryNotifyMasseges = function () {
        MessagesesDriverAdmin();
    };

    //notifications.client.sendDeliveryNotifyMassegestoAdmin = function () {
    //    MessagesesDriverAdmin();
    //};

   // $.connection.hub.start().done();

    $.connection.hub.start().done(function () {
       // alert('connection started')
        getAllMessages();
    }).fail(function (e) {
        alert(e);
    });

});


//function MessagesesDriverAdmin() {   
//    $.ajax({
//        url: '/IHome/DeliveryMessagesesToAdmin',
//        contentType: 'application/html ; charset:utf-8',
//        type: 'GET',
//        dataType: 'html',
//        success: function (result) {
//            alert(result);
//            //if (parseInt(result) > 0) {
//            //    alert(result);
//            //}
//        }
//    });
//}



function getAllMessages() {
    //var tbl = $('#messagesTable');
    $.ajax({
        url: '/IHome/GetMessageses',
        contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (result) {
            //var a2 = JSON.parse(result);
            //tbl.empty();
            //$.each(a2, function (key, value) {
            //    tbl.append('<tr>' + '<td>' + value.OrderId + '</td>' + '<td>' + value.OrderProgress + '</td>' + '</tr>');
            //});
                       
            $('#MassageNotification').text(result);
        }
    });
}


$('#NotificationBill').click(function () {

    ajaxRequest("POST", "/CustomerOrderNote", "", "json").then(function (result) {
        if (result != "") {
            $('#NotificationBody').empty();
            for (i = 0; i < 5; i++) {
                $('#NotificationBody').append('<div class="hd-message-info"><a href = "#" ><div class="hd-message-sn"><div class="hd-message-img"><img src="../../img/post/1.jpg" alt="" /></div><div class="hd-mg-ctn"><h3>' + result[i].Company + '</h3><p>' + result[i].statment + '</p></div></div></a ></div >')
            }
        }
    });
});