// make console.log safe to use
window.console||(console={log:function(){}});

$(document).ready(function() {

    conditionizr({
        ie8: { 
            customScript: "js/excanvas.min.js" 
        }         
    });
    //Init the genyxAdmin plugin
    $.genyxAdmin({
        fixedWidth: false,// make true if you want to use fixed widht instead of fluid version.
        customScroll: false,// Custom scroll for page. true or false 
        responsiveTablesCustomScroll: true,// custom scroll for responsive tables. true or false
        backToTop: true,//show back to top , true or false
        navigation: {
            useNavMore: true, //use arrow for hint. ture or false
            navMoreIconDown: 'i-arrow-down-2', //icon for down state.
            navMoreIconUp: 'i-arrow-up-2',//icon for up state
            rotateIcon: true//rotate icon on hover , true or false
        },
        setCurrent: {
            absoluteUrl: false, //put true if use absolute path links. example http://www.host.com/dashboard instead of /dashboard
            subDir: '' //if you put template in sub dir you need to fill here. example '/html'
        },
        collapseNavIcon: 'i-arrow-left-7', //icon for collapse navigation button
        collapseNavRestoreIcon: 'i-arrow-right-8', //icon for restore navigation button
        rememberNavState: true, //remember if menu is collapsed 
        remeberExpandedSub: false, //remeber expanded sub menu by user
        hoverDropDown: false, //set false if not want to show dropdown on hover ( click instead)
        accordionIconShow: 'i-arrow-down-2',//icon for accordion expand
        accordionIconHide: 'i-arrow-up-2',//icon for accordion hide
        showThemer: false
    });

    //Disable certain links
    $('a[href^="#"]').click(function (e) {
        e.preventDefault()
    })

    //------------- Prettify code  -------------//
    window.prettyPrint && prettyPrint();

    //------------- Bootstrap tooltips -------------//
    $("[data-toggle=tooltip]").tooltip ({});
    $(".tip").tooltip ({placement: 'top', container: 'body'});
    $(".tipR").tooltip ({placement: 'right', container: 'body'});
    $(".tipB").tooltip ({placement: 'bottom', container: 'body'});
    $(".tipL").tooltip ({placement: 'left', container: 'body'});
    //--------------- Popovers ------------------//
    //using data-placement trigger
    $("a[data-toggle=popover]")
      .popover()
      .click(function(e) {
        e.preventDefault()
    });

    $('#fixedwidth').click(function() {
        $.genyxAdmin({fixedWidth: true});
    });

    //init this last don`t change
    //------------- Uniform  -------------//
    //add class .nostyle if not want uniform to style field
    //$("input, textarea, select").not('.nostyle').uniform();
    $("[type='checkbox'], [type='radio'], [type='file'], select").not('.toggle, .select2, .multiselect').uniform();

    $(".select2.form-select").select2("destroy").select2({
        placeholder: "Seleccione"
    });

    $(document).on('click', '.confirm', function (event) {
        var $this = this;
        event.preventDefault();
        BootstrapDialog.show({
            title: 'Eliminar',
            message: '¿Está seguro que desea realizar esta acción?',
            cssClass: 'custom-bootstrap-dialog',
            buttons: [{
                label: 'Cancelar',
                cssClass: 'btn-default',
                action: function (dialog) {
                    dialog.close();
                }
            }, {
                label: 'Eliminar',
                cssClass: 'btn-danger',
                action: function (dialog) {
                    window.location = $this.href;
                }
            }]
        });
    });

    $(document).on('keypress', '.cellphone', (function (event) {
        if (event.which < 40 || event.which >= 58 || event.which === 42
            || event.which === 43 || event.which === 44 || event.which === 46) {
            event.preventDefault();
        }
    }));

    $(document).on('keypress', '.numeric', (function (event) {
        if (event.which < 40 || event.which >= 58 || event.which === 47) {
            event.preventDefault();
        }
    }));

    $(".select2").select2({
        placeholder: "Seleccione"
    });
});

function createSuccessMsg(msg) {
    var div = '<div class="alertMsg alert alert-success" style="margin-bottom: 0px;"><button type="button" class="close" data-dismiss="alert">&times;</button>'
            + '<strong><i class="icon24 i-checkmark-circle"></i> CORRECTO!</strong> ' + msg + '</div>';

    if ($("div.alertMsg").length > 0) {
        $("div.alertMsg").replaceWith(div);
    }
    else {
        $("div#content").before(div);
    }
}

function createErrorMsg(msg) {
    var div = '<div class="alertMsg alert alert-error" style="margin-bottom: 0px;"><button type="button" class="close" data-dismiss="alert">&times;</button>'
        + '<strong><i class="icon24 i-close-4"></i> ERROR!</strong> ' + msg + '</div>';

    if ($("div.alertMsg").length > 0) {
        $("div.alertMsg").replaceWith(div);
    }
    else {
        $("div#content").before(div);
    }
}
