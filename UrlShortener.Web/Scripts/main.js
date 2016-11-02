$(document).ready(function () {

    jQuery.validator.addMethod("anydomain", function (value, element) {
        return this.optional(element) || /^(http|https)?:\/\/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/.test(value);
    }, "");

    jQuery.validator.addMethod("mydealdomain", function (value, element) {
        return this.optional(element) || /^(http|https):\/\/([a-z0-9]+[.])*mydeal.com.au/.test(value);
    }, "");

    $('#urlForm').validate({
        container: '#messages',
        rules: {
            textlongUrl: {
                required: true,
                anydomain:true,
                mydealdomain: true
            }
        },
        messages: {
            textlongUrl: {
                required: "Please enter a url",
                anydomain: "Please enter a valid url",
                mydealdomain: "Only urls from mydeal.com.au are allowed! "
            }
        },
        errorElement: 'div',
        errorLabelContainer: '#messages'
    });

    $('#textlongUrl').keypress(function (e) {
        
            var ini = $(this).val().substring(0, 4);
            if (ini === 'http') {
                $.noop();
            }
            else {
                var currentVal = $(this).val();
                $(this).val('http://' + currentVal);
            }
        
    });

    $("#btnShorten").click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        if ($('#urlForm').valid()) {

            processRequest($("#textlongUrl").val());
        }
    });
});

function processRequest(longUrl) {
    initSpinner();
    $.ajax({
        url: '/Home/ShortenUrl',
        dataType: "json",
        async: true,
        type: "POST",
        data: {
            url: longUrl
        },
        error: function(xhr, status, error) {
            $('#textShortUrl').append(error);
            spinner.spin(false);
            $(result).fadeIn();

        },
        success: function (data) {
            spinner.spin(false);
            $('#textShortUrl').text("");
            if (data.redirect == true) {
                $('#textShortUrl').append('<a href="' + data.redirectUrl + '" target="_blank">' + data.redirectUrl + '</a>');
            }
            else {
                $('#textShortUrl').append(data.message);
            }
            $(result).fadeIn();
        }});
}

var spinner;
function initSpinner() {
    var opts = {
        length: 11 // The length of each line
        , width: 6 // The line thickness
        , radius: 22 // The radius of the inner circle
        , color: 'black' // #rgb or #rrggbb or array of colors
        , opacity: 0.25 // Opacity of the lines
        , rotate: 0 // The rotation offset
        , direction: 1 // 1: clockwise, -1: counterclockwise
        , speed: 1 // Rounds per second
        , trail: 60 // Afterglow percentage
        , fps: 20 // Frames per second 
        , zIndex: 2e9 // The z-index 
        , className: 'spinner' // The CSS class to assign to the spinner
        , top: '40%' // Top position relative to parent
        , left: '50%' // Left position relative to parent
    };
    spinner = new Spinner(opts).spin();
    $("#loader").append(spinner.el);
}