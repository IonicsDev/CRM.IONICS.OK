$(document).ready(function () {

    var filterOn = $('#filterOn').val() == "True";
    if (!filterOn) 
        $('.minimize').click(); // fix to bootstrap bug

});