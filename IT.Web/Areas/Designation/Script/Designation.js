$('#createNewDesignation').click(function () {

    $('#updateDesignation').hide();
    $('#saveDesignation').show();

    $('#DesignationModel').modal('show');
});



$('#saveDesignation').click(function () {

    var Data = JSON.stringify({

        Designation: $('#Designation').val()
    })

    ajaxRequest("POST", "/Designation-Create", Data, "json").then(function (result) {

        if (result != "Failed") {
            $('#DesignationModel').modal('hide');
            $('#DesignationTable').DataTable().ajax.reload();
            sucessAdd();   
           
        }
        else {
            alert('Opration Failed');
        }
    });

});