function checkInputFocusIn(input, label, firstFocus) {
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
function checkInputFocusOut(input, label) {
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
function checkInputTextChange(input, label) {
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
function handleFilledInput(input, label) {
    if (input.value.length > 0) {
        label.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
        input.style.outline = "1px solid #00754A";
        input.style.outlineOffset = "-1px";
        label.style.color = "#00754A";
        input.classList.add("is-valid")
    }
}


//////////////////////////////////// Editing personal info form

// first name input check
let firstNameInput = document.getElementById("floatingFirstName");
let firstNameLabel = document.getElementById("floatingFirstNameLabel");
let firstNameFirstFocus = true;

firstNameInput.addEventListener("focusin", () => {
    firstNameInput.focused = true;
    checkInputFocusIn(firstNameInput, firstNameLabel, firstNameFirstFocus);
});
firstNameInput.addEventListener("focusout", () => {
    firstNameFirstFocus = false;
    checkInputFocusOut(firstNameInput, firstNameLabel);
})
firstNameInput.addEventListener("input", (e) => {
    checkInputTextChange(e.target, firstNameLabel);
})


// last name input check
let lastNameInput = document.getElementById("floatingLastName");
let lastNameLabel = document.getElementById("floatingLastNameLabel");
let lastNameFirstFocus = true;

lastNameInput.addEventListener("focusin", () => {
    lastNameInput.focused = true;
    checkInputFocusIn(lastNameInput, lastNameLabel, lastNameFirstFocus);
});
lastNameInput.addEventListener("focusout", () => {
    lastNameFirstFocus = false;
    checkInputFocusOut(lastNameInput, lastNameLabel);
})
lastNameInput.addEventListener("input", (e) => {
    checkInputTextChange(e.target, lastNameLabel);
})

document.addEventListener("DOMContentLoaded", () => {
    handleFilledInput(firstNameInput, firstNameLabel)
    handleFilledInput(lastNameInput, lastNameLabel)
})