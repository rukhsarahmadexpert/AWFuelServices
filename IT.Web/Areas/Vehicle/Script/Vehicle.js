function Edit(Id) {
    $('#updateVehicle').show();
    $('#saveVehicle').hide();
    ajaxRequest("GET", "/Vehicle-Edit/" + Id, "", "json").then(function (result) {

        if (result) {
            $('select').chosen();
            $('#VehicleType').val(result.VehicleType);
            $('select').trigger("chosen:updated");
            $('#TraficPlateNumber').val(result.TraficPlateNumber);
            $('#TCNumber').val(result.TCNumber);
            $('#Model').val(result.Model);
            $('#Color').val(result.Color);
            $('#MulkiaExpiry').val(result.MulkiaExpiry);
            $('#InsuranceExpiry').val(result.InsuranceExpiry);
            $('#RegisteredRegion').val(result.RegisteredRegion);
            $('#Brand').val(result.Brand);
            $('#Comments').val(result.Comments);
            $('#VehicleId').val(result.Id);
            $('#VehicleModel').modal('show');
        }
        else {
            $('#left-panel').html(result);
        }
    });
}


$('#saveVehicle').click(function () {

    if (ValidVehicle()) {

        var Data = JSON.stringify({
            VehicleType: $('#VehicleType').val(),
            TraficPlateNumber: $('#TraficPlateNumber').val(),
            TCNumber: $('#TCNumber').val(),
            Model: $('#Model').val(),
            Color: $('#Color').val(),
            MulkiaExpiry: $('#MulkiaExpiry').val(),
            InsuranceExpiry: $('#InsuranceExpiry').val(),
            RegisteredRegion: $('#RegisteredRegion').val(),
            Brand: $('#Brand').val(),
            Comments: $('#Comments').val()
        })
        ajaxRequest("POST", "/Vehicle-Create", Data, "json").then(function (result) {
            if (result == "success");
            {
                sucessAdd();
                $('#vehicleTable').DataTable().ajax.reload();
                $('#VehicleModel').modal('hide');
            }
            $('#left-panel').html(result);
        })
    }
});



$('#updateVehicle').click(function () {

    if (ValidVehicle()) {

        var Data = JSON.stringify({
            Id: $('#VehicleId').val(),
            VehicleType: $('#VehicleType').val(),
            TraficPlateNumber: $('#TraficPlateNumber').val(),
            TCNumber: $('#TCNumber').val(),
            Model: $('#Model').val(),
            Color: $('#Color').val(),
            MulkiaExpiry: $('#MulkiaExpiry').val(),
            InsuranceExpiry: $('#InsuranceExpiry').val(),
            RegisteredRegion: $('#RegisteredRegion').val(),
            Brand: $('#Brand').val(),
            Comments: $('#Comments').val()
        })
        ajaxRequest("POST", "VehicleUpdate", Data, "json").then(function (result) {
            if (result == "success");
            {

                $('#vehicleTable').DataTable().ajax.reload();
                $('#VehicleModel').modal('hide');
                ClearVehicle();
                sucessUpdate();
            }
            $('#left-panel').html(result);
        })

    }
});

$('#CancelVehicle').click(function () {
    ClearVehicle();
});

function loadVehicleType() {
    $("#New").empty();
    $("#New").prepend("<option value=0>" + 'select Vehicle Type' + "</option>");
    $("#New").prepend("<option value='AddNewVehicleType'>" + 'Add New Vehicle type' + "</option>");
    $.ajax({
        url: "/GetVehicleTypeAll",
        type: "Get",
        async: false,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {

            $.each(data, function (key, item) {
                $('#New').append($('<option></option>').val(item.Id).html(item.VehicleType));
            });
        },
        Error: function (errormessage) {
            alert(errormessage);
        }
    });
}



function ClearVehicle() {
    $('#TraficPlateNumber').css('border-color', 'lightgrey').val('');
    $('#TCNumber').css('border-color', 'lightgrey').val('');
    $('#Model').css('border-color', 'lightgrey').val('');
    $('#Color').css('border-color', 'lightgrey').val('');
    $('#MulkiaExpiry').css('border-color', 'lightgrey').val('03/19/2018');
    $('#InsuranceExpiry').css('border-color', 'lightgrey').val('03/19/2018');
    $('#RegisteredRegion').css('border-color', 'lightgrey').val(0);
}

function ValidVehicle() {
    var isValid = true;
    if ($('#VehicleType').val() == 0) {
        alert('Please select vehicle type');
        $('#VehicleType').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#VehicleType').css('border-color', 'lightgrey');
    }

    if ($('#TraficPlateNumber').val().trim() == "") {
        $('#TraficPlateNumber').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#TraficPlateNumber').css('border-color', 'lightgrey');
    }

    if ($('#TCNumber').val().trim() == "") {
        $('#TCNumber').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#TCNumber').css('border-color', 'lightgrey');
    }

    if ($('#Model').val().trim() == "") {
        $('#Model').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Model').css('border-color', 'lightgrey');
    }

    if ($('#Color').val().trim() == "") {
        $('#Color').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Color').css('border-color', 'lightgrey');
    }

    if ($('#RegisteredRegion').val() == 'Select Place of Issue') {
        $('#RegisteredRegion').css('border-color', 'Red');
        isValid = false;

        alert($('#RegisteredRegion').val());
    }
    else {
        $('#RegisteredRegion').css('border-color', 'lightgrey');
    }

    if ($('#MulkiaExpiry').val() == '03/19/2018') {
        $('#MulkiaExpiry').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MulkiaExpiry').css('border-color', 'lightgrey');
    }

    if ($('#InsuranceExpiry').val() == '03/19/2018') {
        $('#InsuranceExpiry').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#InsuranceExpiry').css('border-color', 'lightgrey');
    }

    return isValid;
}