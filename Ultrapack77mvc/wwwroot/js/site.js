$('#isMasterCategory').on('click', function () {
    if ($(this).prop('checked') === true) {
        $('#ismaster').css('display', 'none');
        $('#masterCheckValue').attr('name', 'True')
    }
    else {
        $('#ismaster').css('display', 'block');
        $('#masterCheckValue').attr('name','False')
    }
})