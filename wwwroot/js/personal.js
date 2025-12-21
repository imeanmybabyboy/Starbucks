function checkInputFocusIn(input, label, isRequied, firstFocus = null) {
    if (isRequied) {
        if (input.value.length === 0) {
            if (firstFocus) {
                label.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
                input.style.outline = "2px solid #00754A";
                input.style.outlineOffset = "-2px";
                firstFocus = false;
            } else {
                label.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
                input.style.outline = "2px solid #C82014";
                input.style.outlineOffset = "-2px";
                label.style.color = "#C82014";
            }
        }
        else {
            input.style.outline = "2px solid #00754A";
            input.style.outlineOffset = "-2px";
            label.style.color = "#00754A";
        }
    }
    else {
        label.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
        input.style.outline = "2px solid #00754A";
        input.style.outlineOffset = "-2px";
        label.style.color = "#00754A";
    }
}
function checkInputFocusOut(input, label, isRequired) {
    if (isRequired) {
        if (input.value.trim().length === 0) {
            label.style.transform = "scale(1) translateY(0) translateX(0)";
            input.style.outline = "1px solid #C82014";
            input.style.outlineOffset = "-1px";
            input.classList.add("is-invalid");
            label.style.color = "#C82014";
        }
        else {
            input.style.outline = "1px solid #00754A";
            input.style.outlineOffset = "-1px";
        }
    }
    else {
        if (input.value.length > 0) {
            input.style.outline = "1px solid #00754A";
            input.style.outlineOffset = "-1px";
        }
        else {
            label.style.transform = "scale(1) translateY(0) translateX(0)";
            input.style.outline = "1px solid #DEE2E6";
            input.style.outlineOffset = "-1px";
            label.style.color = "grey";
        }
    }
}
function checkInputTextChange(input, label, isRequired) {
    if (isRequired) {
        let value = input.value.trim();
        let validationCont = input.parentNode.querySelector("#validation-container");
        if (value.length === 0) {
            if (input.focused) {
                input.style.outline = "2px solid #C82014";
                input.style.outlineOffset = "-2px";
            }
            validationCont.style.display = "flex";
            input.classList.add("is-invalid");
            input.classList.remove("is-valid");
            label.style.color = "#C82014";
        }
        else {
            if (input.focused) {
                input.style.outline = "2px solid #00754A";
                input.style.outlineOffset = "-2px";
            }
            validationCont.style.display = "none";
            input.classList.remove("is-invalid");
            input.classList.add("is-valid");
            label.style.color = "#00754A";
        }
    }
}
function handleFilledInput(input, label, isRequired) {
    if (isRequired) {
        if (input.value.length > 0) {
            label.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
            input.style.outline = "1px solid #00754A";
            input.style.outlineOffset = "-1px";
            label.style.color = "#00754A";
            input.classList.add("is-valid")
        }
        else {
            label.style.transform = "scale(1) translateY(0) translateX(0)";
            input.style.outline = "1px solid #C82014";
            input.style.outlineOffset = "-1px";
            input.classList.add("is-invalid");
            label.style.color = "#C82014";
        }
    }
    else {
        if (input.value.length > 0) {
            label.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
            input.style.outline = "1px solid #00754A";
            input.style.outlineOffset = "-1px";
            label.style.color = "#00754A";
        }
        else {
            label.style.transform = "scale(1) translateY(0) translateX(0)";
            input.style.outline = "1px solid #DEE2E6";
            input.style.outlineOffset = "-1px";
        }
    }
}


//////////////////////////////////// Editing personal info form

// first name input check
let firstNameInput = document.getElementById("floatingFirstName");
let firstNameLabel = document.getElementById("floatingFirstNameLabel");
let firstNameFirstFocus = true;

firstNameInput.addEventListener("focusin", () => {
    firstNameInput.focused = true;
    checkInputFocusIn(firstNameInput, firstNameLabel, true, firstNameFirstFocus);
});
firstNameInput.addEventListener("focusout", () => {
    firstNameFirstFocus = false;
    checkInputFocusOut(firstNameInput, firstNameLabel, true);
})
firstNameInput.addEventListener("input", (e) => {
    checkInputTextChange(e.target, firstNameLabel, true);
})


// last name input check
let lastNameInput = document.getElementById("floatingLastName");
let lastNameLabel = document.getElementById("floatingLastNameLabel");
let lastNameFirstFocus = true;

lastNameInput.addEventListener("focusin", () => {
    lastNameInput.focused = true;
    checkInputFocusIn(lastNameInput, lastNameLabel, true, lastNameFirstFocus);
});
lastNameInput.addEventListener("focusout", () => {
    lastNameFirstFocus = false;
    checkInputFocusOut(lastNameInput, lastNameLabel, true);
})
lastNameInput.addEventListener("input", (e) => {
    checkInputTextChange(e.target, lastNameLabel, true);
})


// phone input check
let phoneInput = document.getElementById("floatingPhone");
let phoneLabel = document.getElementById("floatingPhoneLabel");

phoneInput.addEventListener("focusin", () => {
    checkInputFocusIn(phoneInput, phoneLabel, false)
})
phoneInput.addEventListener("focusout", () => {
    checkInputFocusOut(phoneInput, phoneLabel, false)
})
phoneInput.addEventListener("input", () => {
    checkInputTextChange(phoneInput, phoneLabel, false)
})


// address line 1 check
let address1Input = document.getElementById("floatingAddress1");
let address1Label = document.getElementById("floatingAddress1Label");

address1Input.addEventListener("focusin", () => {
    checkInputFocusIn(address1Input, address1Label, false);
})
address1Input.addEventListener("focusout", () => {
    checkInputFocusOut(address1Input, address1Label, false);
})
address1Input.addEventListener("input", () => {
    checkInputTextChange(address1Input, address1Label, false);
})


// address line 2 check
let address2Input = document.getElementById("floatingAddress2");
let address2Label = document.getElementById("floatingAddress2Label");

address2Input.addEventListener("focusin", () => {
    checkInputFocusIn(address2Input, address2Label, false);
})
address2Input.addEventListener("focusout", () => {
    checkInputFocusOut(address2Input, address2Label, false);
})
address2Input.addEventListener("input", () => {
    checkInputTextChange(address2Input, address2Label, false);
})


// city check
let cityInput = document.getElementById("floatingCity");
let cityLabel = document.getElementById("floatingCityLabel");

cityInput.addEventListener("focusin", () => {
    checkInputFocusIn(cityInput, cityLabel, false);
})
cityInput.addEventListener("focusout", () => {
    checkInputFocusOut(cityInput, cityLabel, false);
})
cityInput.addEventListener("input", () => {
    checkInputTextChange(cityInput, cityLabel, false);
})


// zip code check
let zipInput = document.getElementById("floatingZip");
let zipLabel = document.getElementById("floatingZipLabel");

zipInput.addEventListener("focusin", () => {
    checkInputFocusIn(zipInput, zipLabel, false);
})
zipInput.addEventListener("focusout", () => {
    checkInputFocusOut(zipInput, zipLabel, false);
})
zipInput.addEventListener("input", () => {
    checkInputTextChange(zipInput, zipLabel, false);
})

function checkSelectFocusIn(select) {
    select.style.outline = "2px solid #00754A";
    select.style.outlineOffset = "-2px";
}
function checkSelectFocusOut(select) {
    select.style.outline = "1px solid #DEE2E6";
    select.style.outlineOffset = "-1px";
}
function checkSelectValueChange(select) {
    if (select.value !== "NULL") {
        select.style.outline = "1px solid #00754A";
        select.style.outlineOffset = "-1px";
    }
    else {
        select.style.outline = "1px solid #DEE2E6";
        select.style.outlineOffset = "-1px";
    }
}
function handleFilledSelect(select) {
    if (select.value !== "NULL") {
        select.style.outline = "1px solid #00754A";
        select.style.outlineOffset = "-1px";
    }
    else {
        select.style.outline = "1px solid #DEE2E6";
        select.style.outlineOffset = "-1px";
    }
}

// SAVE button click effects
let saveBtn = document.getElementById("btn-profile-edit")
saveBtn.addEventListener("mousedown", () => {
    saveBtn.classList.remove("shadow");
    saveBtn.classList.add("shadow-sm");
    saveBtn.style.setProperty("transform", "translateY(calc(-50% + 5px))", "important");
})
saveBtn.addEventListener("mouseup", () => {
    saveBtn.classList.remove("shadow-sm");
    saveBtn.classList.add("shadow");
    saveBtn.style.setProperty("transform", "translateY(-50%)", "important");
})
saveBtn.addEventListener("mouseleave", () => {
    saveBtn.classList.remove("shadow-sm");
    saveBtn.classList.add("shadow");
    saveBtn.style.setProperty("transform", "translateY(-50%)", "important");
})

// Profile update
function btnProfileEditClick(e) {
    let changes = {};

    // button loading effect
    const btn = e.target.closest("button");
    const spinner = `<div class="spinner-border text-light" style="width: 25px; height: 25px" role="status">
                             <span class="visually-hidden">Loading...</span>
                     </div>`
    if (!btn.classList.contains("loading")) {
        btn.innerHTML = spinner;
        btn.classList.add("loading")
        btn.disabled = true;
    }

    for (let item of document.querySelectorAll("input")) {
        let oldText = item.defaultValue;
        let newText = item.value;

        if (oldText !== newText) {
            changes[item.getAttribute("name")] = newText;
        }
    }

    for (let item of document.querySelectorAll("select")) {
        let oldValue = item.options[item.selectedIndex].defaultSelected
            ? item.value : [...item.options].find(o => o.defaultSelected)?.value
        let newValue = item.value;

        if (oldValue !== newValue) {
            changes[item.getAttribute("name")] = newValue;
        }
    }

    setTimeout(() => {
        if (Object.keys(changes).length > 0) {
            let firstName = firstNameInput.value.trim();
            let lastName = lastNameInput.value.trim();

            let isFirstNameInvalid = firstName.length === 0;
            let isLastNameInvalid = lastName.length === 0;

            // if first name is invalid
            if (isFirstNameInvalid) {
                firstNameInput.classList.add("is-invalid");
                firstNameInput.classList.remove("is-valid");
                firstNameLabel.style.color = "#C82014";
                firstNameInput.focus();
                firstNameInput.style.border = "2px solid #C82014";
            }
            else if (isLastNameInvalid) {
                lastNameInput.classList.add("is-invalid");
                lastNameInput.classList.remove("is-valid");
                lastNameLabel.style.color = "#C82014";
                lastNameInput.focus();
                lastNameInput.style.border = "2px solid #C82014";
            }
            else {
                firstNameInput.classList.add("is-valid");
                lastNameInput.classList.add("is-valid");

                fetch("/User/Update", {
                    method: "PATCH",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(changes)
                })
                    .then(r => r.json())
                    .then(j => {
                        if (j.status.toLowerCase() === "ok") {
                            handleProfileEditAlert(j)
                        }
                        else {
                            handleProfileEditAlert(j)
                        }
                    })
            }
            btn.classList.remove("loading");
        }
        else {
            btn.classList.remove("loading");
            handleProfileEditAlert({ "status": "info", "message": "Nothing was changed" })
        }

        btn.innerHTML = "Save";
        btn.disabled = false;
    }, 500)
}

function handleProfileEditAlert(json) {
    const alertPlaceholder = document.getElementById('live-alert-placeholder')

    const appendAlert = (message, type) => {
        const wrapper = document.createElement('div')
        wrapper.innerHTML = [
            `<div class="alert alert-${type} alert-dismissible text-center fade show p-3 mx-auto" role="alert" style="transition: 1.5s !important; width: fit-content">`,
            `   <div>${message}</div>`,
            '</div>'
        ].join('')

        alertPlaceholder.append(wrapper)

        const alertElement = wrapper.querySelector(".alert")
        const addProductAlert = bootstrap.Alert.getOrCreateInstance(alertElement);
        window.setTimeout(() => {
            addProductAlert.close();
        }, 3000)
    }

    if (json.status.toLowerCase() === "ok") {
        appendAlert(json.message, 'success')
    }
    else if (json.status.toLowerCase() === "error") {
        appendAlert(json.message, 'danger')
    }
    else {
        appendAlert(json.message, 'warning')
    }
}

function handleFilledInputs() {
    handleFilledInput(firstNameInput, firstNameLabel, true)
    handleFilledInput(lastNameInput, lastNameLabel, true)
    handleFilledInput(phoneInput, phoneLabel, false);
    handleFilledInput(address1Input, address1Label, false);
    handleFilledInput(address2Input, address2Label, false);
    handleFilledInput(cityInput, cityLabel, false);
    handleFilledInput(zipInput, zipLabel, false);
}

document.addEventListener("DOMContentLoaded", () => {
    handleFilledInputs();

    let btn = document.getElementById("btn-profile-edit");
    if (btn) {
        btn.addEventListener('click', btnProfileEditClick);
    }
})
