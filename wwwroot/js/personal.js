function checkInputFocusIn(input, label, firstFocus) {
    if (input.value.length === 0) {
        if (firstFocus) {
            label.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
            input.style.border = "2px solid #00754A";
            firstFocus = false;
        } else {
            label.style.transform = "scale(.85) translateY(-2rem) translateX(.15rem)";
            input.style.border = "2px solid #C82014";
            label.style.color = "#C82014";
        }
    }
    else {
        input.style.border = "2px solid #00754A";
        label.style.color = "#00754A";
    }
}
function checkInputFocusOut(input, label) {
    if (input.value.trim().length === 0) {
        label.style.transform = "scale(1) translateY(0) translateX(0)";
        input.style.border = "1px solid #C82014";
        input.classList.add("is-invalid");
        label.style.color = "#C82014";
    }
    else {
        input.style.border = "1px solid #00754A";
    }
}
function checkInputTextChange(input, label) {
    let value = input.value.trim();
    let validationCont = input.parentNode.querySelector("#validation-container");

    if (value.length === 0) {
        if (input.focused) {
            input.style.border = "2px solid #C82014";
        }
        validationCont.style.display = "flex";
        input.classList.add("is-invalid");
        input.classList.remove("is-valid");
        label.style.color = "#C82014";
    }
    else {
        if (input.focused) {
            input.style.border = "2px solid #00754A";
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
        input.style.border = "1px solid #00754A";
        label.style.color = "#00754A";
        input.classList.add("is-valid")
    }
}

//////////////////////////////////// Editing personal info form
let firstNameInput = document.getElementById("floatingFirstName");
let firstNameLabel = document.getElementById("floatingFirstNameLabel");
let firstNameFirstFocus = true;

firstNameInput.addEventListener("focusin", () => {
    firstNameFirstFocus.focused = true;
    checkInputFocusIn(firstNameInput, firstNameLabel, firstNameFirstFocus);
});
firstNameInput.addEventListener("focusout", () => {
    firstNameFirstFocus = false;
    checkInputFocusOut(firstNameInput, firstNameLabel);
})
firstNameInput.addEventListener("input", (e) => {
    checkInputTextChange(e.target, firstNameLabel);
})

document.addEventListener("DOMContentLoaded", () => {
    handleFilledInput(firstNameInput, firstNameLabel)
})