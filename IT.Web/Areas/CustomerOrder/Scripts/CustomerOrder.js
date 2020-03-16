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


$('#SaveOrder').click(function () {

    if (IsOrderValid()) {

        var Data = JSON.stringify({
            DriverId: $('#DriverId').val(),
            VehicleId: $('#VehicleId').val(),
            OrderQuantity: $('#OrderQuantity').val(),
            Comments: $('#Comments').val()
        })
        ajaxRequest("POST", "/Order-Create", Data, "json").then(function (result) {
            if (result == "success");
            {
                sucessAdd();
                $('#CustomerOrderTable').DataTable().ajax.reload();
                $('#CustomerOrderModel').modal('hide');
                ClearOrder();
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
        ajaxRequest("POST", "/Vehicle-Edit", Data, "json").then(function (result) {
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


function ClearOrder() {
    $('#OrderQuantity').css('border-color', 'lightgrey').val('');
    $('#VehicleId').css('border-color', 'lightgrey').val(0);
    $('#DriverId').css('border-color', 'lightgrey').val(0);
}

function IsOrderValid() {
    var isValid = true;
    if ($('#DriverId').val() == 0) {
        alert('Please select Driver');
        $('#DriverId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#DriverId').css('border-color', 'lightgrey');
    }

    if ($('#VehicleId').val() == 0) {
        $('#VehicleId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#VehicleId').css('border-color', 'lightgrey');
    }

    if ($('#OrderQuantity').val().trim() == "") {
        $('#OrderQuantity').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#OrderQuantity').css('border-color', 'lightgrey');
    }

    return isValid;
}