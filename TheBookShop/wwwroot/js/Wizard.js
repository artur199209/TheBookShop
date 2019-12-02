$(document).ready(function ()
{
    var currentFs, nextFs, previousFs;
    var opacity;

    $(".next").click(function () {
        str1 = "next1";
        str2 = "next2";

        if (!str1.localeCompare($(this).attr('id')) && validatePersonData() === true) {
            val1 = true;
        } else {
            val1 = false;
        }

        if (!str2.localeCompare($(this).attr('id')) && validate2(0) === true) {
            val2 = true;
        }
        else {
            val2 = false;
        }

        if ((!str1.localeCompare($(this).attr('id')) && val1 === true) || (!str2.localeCompare($(this).attr('id')) && val2 === true)) {
            currentFs = $(this).parent();
            nextFs = $(this).parent().next();
            var nextFs2 = $(this).parent().next().next();
           
            $("#progressbar li").eq($("fieldset").index(nextFs2)).addClass("active");
            nextFs.show();
            currentFs.animate({ opacity: 0 },
                {
                    step: function (now) {
                        opacity = 1 - now;

                        currentFs.css({
                            'display': 'none',
                            'position': 'relative'
                        });
                        nextFs.css({ 'opacity': opacity });
                    },
                    duration: 600
                });
        }
    });
    
    $(".previous").click(function ()
    {
        currentFs = $(this).parent();
        var currentFs2 = $(this).parent().next();
        previousFs = $(this).parent().prev();
        
        $("#progressbar li").eq($("fieldset").index(currentFs2)).removeClass("active");
        
        previousFs.show();
        
        currentFs.animate({ opacity: 0 }, {
            step: function (now) {
                opacity = 1 - now;

                currentFs.css({
                    'display': 'none',
                    'position': 'relative'
                });
                previousFs.css({ 'opacity': opacity });
            },
            duration: 600
        });
    });

    $('.radio-group .radio').click(function () {
        $(this).parent().find('.radio').removeClass('selected');
        $(this).addClass('selected');
    });

    $(".submit").click(function() {
        return false;
    });
});

function validatePersonData() {
    var fieldset1 = document.getElementById("fieldset1");
    var inputItems = fieldset1.getElementsByTagName("input");
    var itemsCount = inputItems.length;
    var flags = [];

    for (var i = 0; i < itemsCount; i++) {
        if (inputItems[i].value === "") {
            inputItems[i].style.borderColor = "red";
            flags[i] = false;
        }
        else {
            inputItems[i].style.borderColor = "lightgray";
            flags[i] = true;
        }
    }

    return allTrue(flags);
}

function validate2() {
    return true;
}

function hideAllPaymentMethodsDivs() {
    var items = document.getElementsByName("Order.DeliveryMethod.DeliveryMethodId");

    for (i = 0; i < items.length; i++) {
        var item = document.getElementById(items[i].value);
        item.style.display = 'None';
    }
}

function getit() {
    var clickedItem = document.querySelector('input[name="Order.DeliveryMethod.DeliveryMethodId"]:checked').value;
    hideAllPaymentMethodsDivs();
    var addressDiv = document.getElementById("address");

    if (clickedItem === '1') {
        address.style.display = 'None';
    } else {
        address.style.display = 'Block';
    }

    var item = document.getElementById(clickedItem);
    
    var allInputs = item.getElementsByTagName('input');
    allInputs[0].checked = true;
    
    item.style.display = 'Block';
    console.log(clickedItem);
}

function allTrue(obj) {
    for (var k = 0; k < obj.length; k++) {
        if (!obj[k]) {return false;}
    }

    return true;
}
