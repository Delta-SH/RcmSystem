$(document).ready(function () {
    $('.captcha').bind('click', function () {
        $(this).attr('src', '/Account/GetCaptcha?time=' + (new Date()).getTime());
    });

    $('#login-form').validate({
        errorElement: 'span',
        highlight: function (element) {
            var pt = $(element).closest('.input-group');
            if (!pt.hasClass('has-error'))
                pt.addClass('has-error');
        },
        errorPlacement: function (error, element) {
            error.appendTo(element.parent().next());
        },
        success: function (label, element) {
            var pt = $(element).closest('.input-group');
            if (pt.hasClass('has-error'))
                pt.removeClass('has-error');
        }
    });

    $('.brand').hover(function () {
        $(this).toggleClass('brand-hover');
    });
});