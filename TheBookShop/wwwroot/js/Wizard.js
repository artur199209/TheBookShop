$(document).ready(function ()
{
    var currentFs, nextFs, previousFs;
    var opacity;
    selectFirstRadioButtonInDeliveryMethodRow();

    $(".next").click(function () {
        str1 = "next1";
        str2 = "next2";

        if (!str1.localeCompare($(this).attr('id')) && validatePersonDataAndPrepareSummary() === true) {
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

        if ((!str1.localeCompare($(this).attr('id')) && val1 === true)) {// || (!str2.localeCompare($(this).attr('id')) && val2 === true)) {
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

function formatNumberToCurrency(item) {
    return item.toLocaleString('pl-PL', {
        style: 'currency',
        currency: 'PLN'
    });
}

function displayDeliveryCost() {
    var deliveryCostDiv = document.getElementById("deliveryCost");
    var deliveryCost = parseInt(getDeliveryPrice());
    deliveryCostDiv.innerHTML = formatNumberToCurrency(deliveryCost);
}

function calculateTotalCost() {
    var cartCostDiv = document.getElementById("cartCost");
    var cartCost = cartCostDiv.innerHTML.trim().split(' ')[0];
    var deliveryPrice = getDeliveryPrice();
    var totalCostDiv = document.getElementById("totalCost");
    var totalCostBankTransfer = document.getElementById("totalCostBankTransfer");
    var totalCost = parseInt(deliveryPrice) + parseInt(cartCost);
    totalCostDiv.innerHTML = formatNumberToCurrency(totalCost);
    totalCostBankTransfer.innerHTML = formatNumberToCurrency(totalCost);
}

function displayDeliveryAndMethodPaymentsInSummary() {
    var delMethod = $('input[name="DeliveryMethod.DeliveryMethodId"]:checked').parent('label').text().trim();
    var payMethod = $('input[name="PaymentMethod.PaymentMethodId"]:checked').parent('label').text().trim();

    var deliveryMethodSummary = document.getElementById("deliveryMethodSummary");
    var paymentMethodSummary = document.getElementById("paymentMethodSummary");

    deliveryMethodSummary.innerHTML = delMethod;
    paymentMethodSummary.innerHTML = payMethod;
}

function displayGiftWrapIsChoosen() {
    var giftWrap = document.querySelector('input[name="GiftWrap"]');
    var giftWrapCheck = document.getElementById("giftWrapCheck");
    giftWrapCheck.checked = giftWrap.checked;
}

function prepareSummaryInfo() {
    calculateTotalCost();
    displayDeliveryCost();
    displayGiftWrapIsChoosen();
    displayCustomerAndAddressEnteredData();
    displayDeliveryAndMethodPaymentsInSummary();
}

function validatePersonDataAndPrepareSummary() {
    var inputs;

    if (personalPickUpIsChoosen()) {
        var personData = document.getElementById("personData");
        inputs = personData.getElementsByTagName("input");
       
    } else {
        var fieldset1 = document.getElementById("fieldset1");
        inputs = fieldset1.getElementsByTagName("input");
    }
    
    var itemsCount = inputs.length;
    var flags = [];
   
    for (var i = 0; i < itemsCount; i++) {
        if (inputs[i].value === "") {
            inputs[i].style.borderColor = "red";
            flags[i] = false;
        }
        else {
            inputs[i].style.borderColor = "lightgray";
            flags[i] = true;
        }
    }

    prepareSummaryInfo();

    return allTrue(flags);
}

function validate2() {
    return true;
}

function selectFirstRadioButtonInDeliveryMethodRow() {
    var allInputs = document.getElementsByName("DeliveryMethod.DeliveryMethodId");
    allInputs[0].checked = true;
}

function selectFirstRadioButtonInRow(item) {
    var allInputs = item.getElementsByTagName('input');
    allInputs[0].checked = true;
}

function hideAllPaymentMethodsDivs() {
    var items = document.getElementsByName("DeliveryMethod.DeliveryMethodId");
   
    for (i = 0; i < items.length; i++) {
        var item = document.getElementById(items[i].value);
        item.style.display = 'None';
    }
}

function getCheckedItem() {
    var clickedItem = document.querySelector('input[name="DeliveryMethod.DeliveryMethodId"]:checked').value;
    return clickedItem;
}

function personalPickUpIsChoosen() {
    var item = getCheckedItem();
    if (item ==='1') {
        return true;
    }
    return false;
}

function getDeliveryPrice() {
    var deliveryLabelValue = $('input[name="PaymentMethod.PaymentMethodId"]:checked').parent('label').text().trim();
    
    re = /\((.*)\)/;
    var price = deliveryLabelValue.match(re)[1]//.replace(/[^0-9]/g, '');
    return price;
}

function hideOrShowPaymentMethodsAndAddressForm() {
    hideAllPaymentMethodsDivs();
    var clickedItem = document.querySelector('input[name="DeliveryMethod.DeliveryMethodId"]:checked').value;
    var item = document.getElementById(clickedItem);
    item.style.display = 'Block';
    selectFirstRadioButtonInRow(item);

    var addressDiv = document.getElementById("address");

    if (clickedItem === '1') {
        addressDiv.style.display = 'None';
    } else {
        addressDiv.style.display = 'Block';
    }
}

function displayCustomerAndAddressEnteredData() {
    var customerName = document.getElementById("Customer_Name");
    var customerSurname = document.getElementById("Customer_Surname");
    var customerEmail = document.getElementById("Customer_Email");
    var customerPhoneNumber = document.getElementById("Customer_PhoneNumber");

    var deliveryAddressCountry = document.getElementById("DeliveryAddress_Country");
    var deliveryAddressCity = document.getElementById("DeliveryAddress_City");
    var deliveryAddressHomeNumber = document.getElementById("DeliveryAddress_HomeNumber");
    var deliveryAddressZipCode = document.getElementById("DeliveryAddress_ZipCode");
    
    var personName = document.getElementById("personName");
    personName.innerHTML = customerName.value;

    var personSurname = document.getElementById("personSurname");
    personSurname.innerHTML = customerSurname.value;

    var personEmail = document.getElementById("personEmail");
    personEmail.innerHTML = customerEmail.value;

    var personPhoneNumber = document.getElementById("personPhoneNumber");
    personPhoneNumber.innerHTML = customerPhoneNumber.value;

    var addressCountry = document.getElementById("addressCountry");
    addressCountry.innerHTML = deliveryAddressCountry.value;

    var addressCity = document.getElementById("addressCity");
    addressCity.innerHTML = deliveryAddressCity.value;

    var addressHomeNumber = document.getElementById("addressHomeNumer");
    addressHomeNumber.innerHTML = deliveryAddressHomeNumber.value;

    var addressZipCode = document.getElementById("addressZipCode");
    addressZipCode.innerHTML = deliveryAddressZipCode.value;
}

function allTrue(obj) {
    for (var k = 0; k < obj.length; k++) {
        if (!obj[k]) {return false;}
    }

    return true;
}