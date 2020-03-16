function Edit(Id) {

    $('#saveEmployee').hide();
    $('#updateEmployee').show();

    ajaxRequest("GET", "/Employee-Edit/" + Id, "", "json").then(function (result) {
        if (result) {
          
            $('select').chosen();
            $('#EmployeeId').val(result.Id);
            $('#Name').val(result.Name);
            $('#Nationality').val(result.Nationality);
            $('#Contact').val(result.Contact);
            $('#Email').val(result.Email);
            $('#Designation').val(result.Designation);
            $('#BasicSalary').val(result.BasicSalary);
            $('#Facebook').val(result.Facebook);
            $('#Comments').val(result.Comments);
            $('select').trigger("chosen:updated");
            $('#EmployeeModel').modal('show');
        }
        else {
            $('#left-panel').html(result);
        }
    });
}


$('#saveEmployee').click(function () {


    if (ValidEmployee()) {

        var Data = JSON.stringify({

            Name: $('#Name').val(),
            Nationality: $('#Nationality').val(),
            Contact: $('#Contact').val(),
            Email: $('#Email').val(),
            Designation: $('#Designation').val(),
            BasicSalary: $('#BasicSalary').val(),
            Facebook: $('#Facebook').val(),
            Comments: $('#Comments').val(),
        })
        ajaxRequest("POST", "Employee-Create", Data, "json").then(function (result) {

            if (result == "Success") {
                sucessAdd();
                $('#EmployeeTable').DataTable().ajax.reload();
                $('#EmployeeModel').modal('hide');
                ClearEmployee();
            }

            else {
                $('#left-panel').html(result);
            }

        });
    }
});



$('#updateEmployee').click(function () {
    if (ValidEmployee) {

        var Data = JSON.stringify({

            Id: $('#EmployeeId').val(),
            Name: $('#Name').val(),
            Nationality: $('#Nationality').val(),
            Contact: $('#Contact').val(),
            Email: $('#Email').val(),
            Designation: $('#Designation').val(),
            BasicSalary: $('#BasicSalary').val(),
            Facebook: $('#Facebook').val(),
            Comments: $('#Comments').val(),
        })
        ajaxRequest("POST", "/Employee-Update", Data, "json").then(function (result) {

            if (result == "Success") {
                sucessUpdate();
                $('#EmployeeTable').DataTable().ajax.reload();
                $('#EmployeeModel').modal('hide');
                ClearEmployee();
            }
            else {
                $('#left-panel').html(result);
            }

        });

    }
});



function ClearEmployee() {
    $('#Name').css('border-color', 'lightgrey').val('');
    $('#Nationality').css('border-color', 'lightgrey').val(0);
    $('#Contact').css('border-color', 'lightgrey').val('');
    $('#Email').css('border-color', 'lightgrey').val('');
    $('#Designation').css('border-color', 'lightgrey').val('');
    $('#BasicSalary').css('border-color', 'lightgrey').val('');
    $('#Facebook').css('border-color', 'lightgrey').val('');
    $('#Comments').css('border-color', 'lightgrey').val('');
}



function ValidEmployee() {
    var isValid = true;
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }

    if ($('#Nationality').val() == 0) {
        $('#Nationality').css('border-color', 'Red');
        isValid = false;

        alert('select Nationality');
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

    if ($('#Designation').val() == 0) {
        $('#Designation').css('border-color', 'Red');
        isValid = false;
        alert('select Designation');
    }
    else {
        $('#Designation').css('border-color', 'lightgrey');
    }

    if ($('#BasicSalary').val().trim() == "") {
        $('#BasicSalary').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#BasicSalary').css('border-color', 'lightgrey');
    }

    return isValid;
}