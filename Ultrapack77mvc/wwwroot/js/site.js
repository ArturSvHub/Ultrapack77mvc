/*var val = $('#isMasterCategory').find('option:selected').val(*/

$(documen.ready(function () {
    $('#isMasterCategory option').each(function () {
        if ($(this).prop('selected')==true)
        {
            $('#ismaster').css('display', 'none');
            console.log($(this).val());
        }
        else {
            $('#ismaster').css('display', 'block');
            console.log($(this).val());
        }

    });
}));