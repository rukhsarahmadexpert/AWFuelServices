function Edit(Id) {
    ajaxRequest("GET", "/Driver-Edit/" + Id, "", "JSON").then(function (result) {

        $('select').chosen();
        $('#DriverId').val(Id),
        $('#Name').val(result.Name);
        $('#Contact').val(result.Contact);
        $('#Email').val(result.Email);
        $('#Facebook').val(result.Facebook);
        $('#Comments').val(result.Comments);
        $('#Nationality').val(result.Nationality);
        $('#LicenseExpiry').val(result.LicenseExpiry);
        var arlene1 = [parseInt(result.LicenseType), parseInt(result.LicenseType2), parseInt(result.LicenseType3)];
        $('#LicenseType').val(arlene1);
        $('select').trigger("chosen:updated");
        $('#saveDriver').hide();
        $('#updateDriver').show();
        $('#DriverModel').modal('show');
        $('#left-panel').html(result);
    });
}

$('#updateDriver').click(function () {   

    var Data = JSON.stringify({

        Id: $('#DriverId').val(),
        Name: $('#Name').val(),
        Contact: $('#Contact').val(),
        Email: $('#Email').val(),
        Facebook: $('#Facebook').val(),
        Comments: $('#Comments').val(),
        Nationality: $('#Nationality').val(),
        LicenseExpiry: $('#LicenseExpiry').val(),
        LicenseType: $('#LicenseType').val(),
        LicenseTypes: $('#LicenseType').val(),

    })
    ajaxRequest("POST", "/AWFDriver-Update", Data, "json").then(function (result) {
        if (result);
        {
            ClearDriver();
            sucessUpdate();

            $('#DriverModel').modal('hide');
            $('#MyTable').DataTable().ajax.reload();
        }
        $('#left-panel').html(result);
    })
});



$('#CancelDriver').click(function () {

    ClearDriver();

});


$('#saveDriver').click(function () {

    if (IsValidDriver()) {

        var Data = JSON.stringify({

            Id: $('#DriverId').val(),
            Name: $('#Name').val(),
            Contact: $('#Contact').val(),
            Email: $('#Email').val(),
            Facebook: $('#Facebook').val(),
            Comments: $('#Comments').val(),
            Nationality: $('#Nationality').val(),
            DrivingLicenseExpiryDate: $('#LicenseExpiry').val(),
            LicenseType: $('#LicenseType').val(),
            LicenseTypes: $('#LicenseType').val(),

        })
        ajaxRequest("POST", "/AWFDriver-Create", Data, "json").then(function (result) {
            if (result);
            {
                ClearDriver();

                $('#DriverModel').modal('hide');
                sucessAdd();
                $('#MyTable').DataTable().ajax.reload();

            }
            $('#left-panel').html(result);
        })
    }
});


function View(Id) {
    window.location.href = '/AWFDriver-Details/' + Id;
}


function ClearDriver() {
    $('select').chosen();
    var arlene1 = [];
    $('#Name').css('border-color', 'lightgrey').val('');
    $('#Contact').css('border-color', 'lightgrey').val('');
    $('#Email').css('border-color', 'lightgrey').val('');
    $('#Facebook').css('border-color', 'lightgrey').val('');
    $('#Comments').css('border-color', 'lightgrey').val('');
    $('#Nationality').css('border-color', 'lightgrey').val('');
    $('#LicenseExpiry').css('border-color', 'lightgrey').val('03/19/2018');
    $('#LicenseType').css('border-color', 'lightgrey').val(arlene1);
    $('select').trigger("chosen:updated");
}



function IsValidDriver() {
    var isValid = true;
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }

    if ($('#LicenseExpiry').val().trim() == "03/19/2018") {
        $('#LicenseExpiry').css('border-color', 'Red');
        isValid = false
    }
    else {
        $('#LicenseExpiry').css('border-color', 'lightgrey');
    }

    if ($('#Nationality').val() == "") {
        $('#Nationality').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Nationality').css('border-color', 'lightgrey');
    }

    if ($('#Contact').val().trim() == "") {
        $('#Contact').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Contact').css('border-color', 'lightgrey');
    }


    return isValid;
}