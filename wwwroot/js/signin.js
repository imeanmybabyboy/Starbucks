
class Base64 {
    static #textEncoder = new TextEncoder();
    static #textDecoder = new TextDecoder();

    // https://datatracker.ietf.org/doc/html/rfc4648#section-4
    static encode = (str) => btoa(String.fromCharCode(...Base64.#textEncoder.encode(str)));
    static decode = (str) => Base64.#textDecoder.decode(Uint8Array.from(atob(str), c => c.charCodeAt(0)));

    // https://datatracker.ietf.org/doc/html/rfc4648#section-5
    static encodeUrl = (str) => this.encode(str).replace(/\+/g, '-').replace(/\//g, '_');
    static decodeUrl = (str) => this.decode(str.replace(/\-/g, '+').replace(/\_/g, '/'));

    static jwtEncodeBody = (header, payload) => this.encodeUrl(JSON.stringify(header)) + '.' + this.encodeUrl(JSON.stringify(payload));
    static jwtDecodePayload = (jwt) => JSON.parse(this.decodeUrl(jwt.split('.')[1]));
}

///////////////// SIGN IN FORM
// login validation
let loginInput = document.getElementById("floatingLogin");
let loginLabel = document.getElementById("floatingLoginLabel");
let loginFirstFocus = true;

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
    if (input.value.length === 0) {
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
    let value = input.value;
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

loginInput.addEventListener("focusin", () => {
    loginInput.focused = true;
    checkInputFocusIn(loginInput, loginLabel, loginFirstFocus);
});
loginInput.addEventListener("focusout", () => {
    loginFirstFocus = false;
    checkInputFocusOut(loginInput, loginLabel);
});
loginInput.addEventListener('input', (e) => {
    checkInputTextChange(e.target, loginLabel)
});


// password validation
let passwordInput = document.getElementById("floatingPassword")
let passwordLabel = document.getElementById("floatingPasswordLabel")
let passwordFirstFocus = true;

passwordInput.addEventListener("focusin", () => {
    passwordInput.focused = true;
    checkInputFocusIn(passwordInput, passwordLabel, passwordFirstFocus)
});
passwordInput.addEventListener("focusout", () => {
    passwordFirstFocus = false;
    checkInputFocusOut(passwordInput, passwordLabel)
})
passwordInput.addEventListener('input', (e) => {
    checkInputTextChange(e.target, passwordLabel)
})


// DOMContentLoadedEvent
document.addEventListener("DOMContentLoaded", () => {
    handleFilledInput(loginInput, loginLabel)
    handleFilledInput(passwordInput, passwordLabel)
})


// submiting form

document.addEventListener("submit", (e) => {
    const form = e.target;

    if (form && form["id"] == "auth-form") {
        e.preventDefault();
        const formData = new FormData(form);
        const login = formData.get("user-name-email");
        const password = formData.get("user-password");

        // login and password sumbit validation
        let loginInvalid = login.trim().length === 0;
        let passwordInvalid = password.length === 0;

        if (loginInvalid) {
            loginInput.classList.add("is-invalid");
            loginInput.classList.remove("is-valid");
            loginLabel.style.color = "#C82014";
            loginInput.focus();
            loginInput.style.border = "2px solid #C82014";
        }
        else if (passwordInvalid) {
            passwordInput.classList.add("is-invalid");
            passwordInput.classList.remove("is-valid");
            passwordLabel.style.color = "#C82014";
            passwordInput.focus();
            passwordInput.style.border = "2px solid #C82014";
        }
        else {
            loginInput.classList.add("is-valid")
            passwordInput.classList.add("is-valid")
            const userPass = login + ":" + password;
            const basicCredentials = Base64.encode(userPass);
            const header = "Authorization: Basic " + basicCredentials;
            const submitBtn = document.getElementById("submit-btn");

            const spinner = `<div class="spinner-border text-light" style="width: 35px; height: 35px" role="status">
                                 <span class="visually-hidden">Loading...</span>
                             </div>`
            if (!submitBtn.classList.contains("loading")) {
                submitBtn.innerHTML = spinner;
                submitBtn.classList.add("loading")
            }

            // Clear invalid sign in feedback
            handleInvalidSignin();

            setTimeout(async () => {
                fetch("/User/Authenticate", {
                    method: "GET",
                    headers: {
                        Authorization: "Basic " + basicCredentials
                    }
                }).then(r => r.json())
                    .then(j => {
                        submitBtn.innerHTML = "Sign in";
                        submitBtn.classList.remove("loading");

                        if (j.status === "Ok") {
                            console.log(j);
                            //window.location.href = "/"
                        }
                        else {
                            handleInvalidSignin(j.error, "danger");
                        }
                    })
            }, 500)
        }
    }
})

function handleInvalidSignin(message = null, type = null) {
    const alertPlaceholder = document.getElementById('live-alert-placeholder');
    alertPlaceholder.innerHTML = '';

    if (message !== null && type !== null) {
        const appendAlert = (message, type) => {
            const wrapper = document.createElement('div');
            wrapper.innerHTML = [
                `<div class="alert alert-${type} alert-dismissible text-dark p-3" role="alert" style="border-color: #C82014; background-color: #FDF6F6">`,
                `   <div class="d-flex align-items-center justify-content-between">`,
                `       <h3>Sign is unsuccessfull.</h3>`,
                `       <i class="bi bi-exclamation-circle-fill text-danger"></i>`,
                `   </div>`,
                `   <div>${message}</div>`,
                '</div>'
            ].join('');
            alertPlaceholder.append(wrapper);
        }

        appendAlert(message, type)
    }
}

const submitBtn = document.getElementById("submit-btn");
submitBtn.addEventListener("mousedown", (e) => {
    const btn = e.target.closest("button");
    btn.classList.remove("shadow");
    btn.classList.add("shadow-sm");
    btn.style.transform = "translateY(5px)"
})

submitBtn.addEventListener("mouseup", (e) => {
    const btn = e.target.closest("button");
    btn.classList.remove("shadow-sm");
    btn.classList.add("shadow");
    btn.style.transform = "translateY(0)"
})

submitBtn.addEventListener("mouseleave", (e) => {
    const btn = e.target.closest("button");
    btn.classList.remove("shadow-sm");
    btn.classList.add("shadow");
    btn.style.transform = "translateY(0)"
})

const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]')
const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl))

const popoverDetails = new bootstrap.Popover('#details', {
    container: 'body'
})

popoverDetails.setContent({
    ".popover-body": "Checking this box will reduce the number of times you’re asked to sign in. To keep your account secure, use this option only on your personal devices.",
})

const popoverForgotUsername = new bootstrap.Popover('#forgot-username', {
    container: 'body'
})

popoverForgotUsername.setContent({
    ".popover-body": "You can now use your email instead of a username.",
})